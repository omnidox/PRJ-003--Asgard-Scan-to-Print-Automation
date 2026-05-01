using System;
using System.Linq;
using PX.Data;
using PX.BarcodeProcessing;
using PX.Objects.SO;
using PX.Objects.SO.WMS;
using PX.Objects.CS;
using PX.SM;

namespace PX.Objects.SO.WMS
{
    /// <summary>
    /// ========================================================================
    /// WMS Ship Mode - UCC Carrier Label Scan Integration
    /// ========================================================================
    /// 
    /// Purpose:
    /// When a warehouse user scans a package's custom UCC value (UsrTCUCC128)
    /// in WMS Ship Mode, this extension detects that barcode and automatically
    /// generates and prints the carrier label for only that package.
    /// 
    /// Hook Strategy:
    /// - Extends PickPackShip.CommandOrShipmentOnlyState (the Ship Mode scan handler)
    /// - Overrides Process(string barcode) to intercept UCC scans
    /// - After shipment is loaded (@ship or direct scan), non-command scans arrive here
    /// - Currently fails with "Only commands or a shipment number can be used to continue"
    /// - We intercept and check if barcode matches package.UsrTCUCC128
    /// - If match found, generate carrier label and return true (consume the scan)
    /// - If no match, fall through to base Process behavior
    /// 
    /// Why CommandOrShipmentOnlyState (not Logic):
    /// - Process(string barcode) is declared on CommandOrShipmentOnlyState itself
    /// - The Logic inner class contains only GetPromptForCommandOrShipmentOnly() and GetErrorForCommandOrShipmentOnly()
    /// - We must extend the class that owns the method we want to override
    /// 
    /// Workflow:
    /// User enters Ship Mode (@ship)
    /// → Scans/loads shipment number
    /// → Enters CommandOrShipmentOnlyState
    /// → User scans package UCC barcode
    /// → Process(barcode) is called
    /// → TryHandleUccCarrierLabelScan checks if barcode matches UsrTCUCC128
    /// → If match: generate label, return true (consume scan)
    /// → If no match: return base_Process(barcode) (original behavior)
    /// </summary>
    public class ShipMode_UccCarrierLabelScanExt : PXGraphExtension<PickPackShip.CommandOrShipmentOnlyState, PickPackShip.Host>
    {
        public static bool IsActive() => true;

        /// <summary>
        /// Override Process to intercept UCC carrier label scans in Ship Mode.
        /// This method is called for every non-command scan after shipment is loaded.
        /// </summary>
        public delegate bool ProcessDelegate(string barcode);

        [PXOverride]
        public virtual bool Process(string barcode, ProcessDelegate base_Process)
        {
            // Only process UCC scans in Ship Mode
            if (Basis.Header.Mode != PickPackShip.ShipMode.Value)
                return base_Process(barcode);

            PXTrace.WriteInformation("[UCC-SHIP-SCAN] Process called with barcode: '{0}'", barcode ?? "null");

            // Try to handle as UCC carrier label scan first
            if (TryHandleUccCarrierLabelScan(Basis, barcode))
            {
                PXTrace.WriteInformation("[UCC-SHIP-SCAN] ✅ Barcode handled as UCC carrier label scan");
                return true;
            }

            // Not a UCC match - fall through to base Process behavior
            PXTrace.WriteInformation("[UCC-SHIP-SCAN] Barcode not a UCC match, passing to base Process");
            return base_Process(barcode);
        }

        /// <summary>
        /// Main entry point for UCC barcode scan handling in Ship Mode.
        /// Normalizes barcode, finds matching package, validates conditions, queues generation.
        /// </summary>
        private bool TryHandleUccCarrierLabelScan(PickPackShip.Host basis, string rawBarcode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(rawBarcode))
                {
                    PXTrace.WriteInformation("[UCC-SHIP-SCAN] Barcode is empty, skipping UCC handling");
                    return false;
                }

                string normalizedBarcode = NormalizeBarcode(rawBarcode);
                PXTrace.WriteInformation("[UCC-SHIP-SCAN] Normalized barcode: '{0}'", normalizedBarcode);

                // Access shipment through Basis.RefNbr (the shipment loaded in Ship Mode)
                if (string.IsNullOrWhiteSpace(basis.RefNbr))
                {
                    PXTrace.WriteWarning("[UCC-SHIP-SCAN] No shipment is currently loaded");
                    return false;
                }

                SOShipment shipment = SOShipment.PK.Find(basis.Graph, basis.RefNbr);
                if (shipment == null)
                {
                    PXTrace.WriteWarning("[UCC-SHIP-SCAN] Shipment {0} could not be found", basis.RefNbr);
                    return false;
                }

                PXTrace.WriteInformation("[UCC-SHIP-SCAN] Current shipment: {0}", shipment.ShipmentNbr);

                // Use a fresh graph for the lookup (safe approach to avoid cross-graph contamination)
                var lookupGraph = PXGraph.CreateInstance<SOShipmentEntry>();
                var lookupShipment = SOShipment.PK.Find(lookupGraph, shipment.ShipmentNbr);
                if (lookupShipment == null)
                {
                    PXTrace.WriteWarning("[UCC-SHIP-SCAN] Shipment {0} could not be loaded on lookup graph", shipment.ShipmentNbr);
                    return false;
                }
                lookupGraph.Document.Current = lookupShipment;

                // Find package matching this UCC in the current shipment
                SOPackageDetailEx package = FindPackageByUcc(lookupGraph, shipment.ShipmentNbr, normalizedBarcode);
                if (package == null)
                {
                    PXTrace.WriteInformation("[UCC-SHIP-SCAN] No package found with UCC '{0}' in shipment {1}", normalizedBarcode, shipment.ShipmentNbr);
                    return false;
                }

                PXTrace.WriteInformation("[UCC-SHIP-SCAN] Found package line {0} with UCC '{1}'", package.LineNbr, normalizedBarcode);

                // Check for existing tracking/label (duplicate protection before queuing)
                if (HasExistingTrackingOrLabel(lookupGraph, package))
                {
                    PXTrace.WriteWarning("[UCC-SHIP-SCAN] Package line {0} already has tracking or label attached, skipping generation", package.LineNbr);
                    
                    // Report to user that package already has label
                    basis.ReportInfo($"Package line {package.LineNbr} already has a carrier label or tracking number.");
                    return true;
                }

                // Queue the carrier label generation in a long operation
                QueueCarrierLabelGeneration(basis, shipment, package);
                return true;
            }
            catch (Exception ex)
            {
                PXTrace.WriteError("[UCC-SHIP-SCAN] Exception in TryHandleUccCarrierLabelScan: {0}", ex.Message);
                PXTrace.WriteError("[UCC-SHIP-SCAN] Stack: {0}", ex.StackTrace);
                return false;
            }
        }

        /// <summary>
        /// Normalize barcode by trimming whitespace and handling GS1 formatting.
        /// Enhanced to handle UCC-128 barcodes with GS1 separators (ASCII 0x1D).
        /// </summary>
        private string NormalizeBarcode(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return string.Empty;

            // Trim whitespace and remove GS1 group separator (FNC1 / 0x1D)
            return raw.Trim().Replace("\u001D", "");
        }

        /// <summary>
        /// Find a package in the current shipment by matching its UsrTCUCC128 field.
        /// Uses reflection on extension to match barcode to package UCC.
        /// Applies normalization to both scanned barcode and stored UCC value for safe comparison.
        /// </summary>
        private SOPackageDetailEx FindPackageByUcc(SOShipmentEntry graph, string shipmentNbr, string normalizedUcc)
        {
            if (string.IsNullOrWhiteSpace(shipmentNbr) || string.IsNullOrWhiteSpace(normalizedUcc))
                return null;

            try
            {
                // Query all packages for this shipment and match UCC via extension reflection
                var result = PXSelect<
                    SOPackageDetailEx,
                    Where<SOPackageDetailEx.shipmentNbr, Equal<Required<SOPackageDetailEx.shipmentNbr>>>>
                    .Select(graph, shipmentNbr)
                    .RowCast<SOPackageDetailEx>()
                    .FirstOrDefault(pkg =>
                    {
                        var ext = pkg.GetExtension<TCAddon.TCSOPackageDetailExt>();
                        if (ext == null)
                            return false;

                        // Normalize BOTH scanned barcode and stored UCC for safe comparison
                        string pkgUcc = NormalizeBarcode(ext.UsrTCUCC128);
                        
                        // Case-insensitive comparison for UCC-128 barcodes
                        return string.Equals(pkgUcc, normalizedUcc, StringComparison.OrdinalIgnoreCase);
                    });

                return result;
            }
            catch (Exception ex)
            {
                PXTrace.WriteWarning("[UCC-SHIP-SCAN] Exception during package lookup: {0}", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Check if package already has a tracking number or carrier label file attached.
        /// Prevents duplicate carrier generation.
        /// </summary>
        private bool HasExistingTrackingOrLabel(SOShipmentEntry graph, SOPackageDetailEx package)
        {
            if (package == null)
                return false;

            // Check for existing tracking number
            if (!string.IsNullOrWhiteSpace(package.TrackNumber))
            {
                PXTrace.WriteWarning("[UCC-SHIP-SCAN] Package line {0} already has TrackNumber: {1}", package.LineNbr, package.TrackNumber);
                return true;
            }

            // Check for existing attached file
            FileInfo existingFile = TryGetExistingCarrierLabel(graph, package);
            if (existingFile != null)
            {
                PXTrace.WriteWarning("[UCC-SHIP-SCAN] Package line {0} already has label file attached: {1}", package.LineNbr, existingFile.Name);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check if a carrier label file is already attached to the package.
        /// Looks for .pdf, .zpl, .zplii, .epl files in package notes.
        /// 
        /// Note: This is a conservative check. In production, you may want to add additional
        /// validation for carrier-label-specific naming patterns to avoid treating unrelated
        /// package files as carrier labels.
        /// </summary>
        private FileInfo TryGetExistingCarrierLabel(SOShipmentEntry graph, SOPackageDetailEx package)
        {
            if (package == null)
                return null;

            try
            {
                Guid[] fileNotes = PXNoteAttribute.GetFileNotes(graph.Packages.Cache, package);
                if (fileNotes == null || fileNotes.Length == 0)
                    return null;

                UploadFileMaintenance upload = PXGraph.CreateInstance<UploadFileMaintenance>();
                string[] allowed = { ".zpl", ".zplii", ".epl", ".pdf" };

                return fileNotes
                    .Select(id => upload.GetFile(id))
                    .Where(f => f != null && !string.IsNullOrEmpty(f.Name))
                    .FirstOrDefault(f => allowed.Any(ext => f.Name.EndsWith(ext, StringComparison.OrdinalIgnoreCase)));
            }
            catch (Exception ex)
            {
                PXTrace.WriteWarning("[UCC-SHIP-SCAN] Error checking for existing label file: {0}", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Queue a PXLongOperation to generate and print the carrier label for the package.
        /// Uses fresh graph to avoid state contamination from WMS context.
        /// Uses basis.Graph as the context for PXLongOperation.
        /// </summary>
        private void QueueCarrierLabelGeneration(PickPackShip.Host basis, SOShipment shipment, SOPackageDetailEx package)
        {
            string shipmentNbr = shipment.ShipmentNbr;
            int? packageLineNbr = package.LineNbr;

            if (packageLineNbr == null)
            {
                PXTrace.WriteError("[UCC-SHIP-SCAN] Package has no LineNbr, cannot queue generation");
                return;
            }

            PXTrace.WriteInformation("[UCC-SHIP-SCAN] Queueing carrier label generation for package {0} in shipment {1}", packageLineNbr, shipmentNbr);

            // Use basis.Graph as the context (following Acumatica WMS patterns)
            PXLongOperation.StartOperation(basis.Graph, delegate()
            {
                PXTrace.WriteInformation("[UCC-SHIP-SCAN-LONGOP] Long operation started for package {0}", packageLineNbr);

                try
                {
                    // Create fresh graph inside long operation
                    SOShipmentEntry freshGraph = PXGraph.CreateInstance<SOShipmentEntry>();

                    // Reload shipment
                    SOShipment reloadedShipment = SOShipment.PK.Find(freshGraph, shipmentNbr);
                    if (reloadedShipment == null)
                    {
                        PXTrace.WriteError("[UCC-SHIP-SCAN-LONGOP] Shipment {0} not found after reload", shipmentNbr);
                        throw new PXException($"Shipment '{shipmentNbr}' could not be reloaded in the label generation operation.");
                    }

                    freshGraph.Document.Current = reloadedShipment;

                    // Reload package
                    SOPackageDetailEx reloadedPackage = ReloadPackage(freshGraph, shipmentNbr, packageLineNbr.Value);
                    if (reloadedPackage == null)
                    {
                        PXTrace.WriteError("[UCC-SHIP-SCAN-LONGOP] Package {0} not found after reload", packageLineNbr);
                        throw new PXException($"Package line {packageLineNbr} could not be reloaded in the label generation operation.");
                    }

                    PXTrace.WriteInformation("[UCC-SHIP-SCAN-LONGOP] Shipment and package reloaded successfully");

                    // DUPLICATE PROTECTION (second check after reload)
                    // Another user/process could have generated the label after scan but before operation ran
                    if (!string.IsNullOrWhiteSpace(reloadedPackage.TrackNumber))
                    {
                        PXTrace.WriteWarning("[UCC-SHIP-SCAN-LONGOP] Package {0} now has tracking number {1}, skipping generation", packageLineNbr, reloadedPackage.TrackNumber);
                        return;
                    }

                    if (TryGetExistingCarrierLabel(freshGraph, reloadedPackage) != null)
                    {
                        PXTrace.WriteWarning("[UCC-SHIP-SCAN-LONGOP] Package {0} now has label attached, skipping generation", packageLineNbr);
                        return;
                    }

                    // Generate carrier label using service
                    var service = new PackageCarrierLabelService(freshGraph);

                    PXTrace.WriteInformation("[UCC-SHIP-SCAN-LONGOP] Calling GenerateCarrierLabelForPackage for package {0}", packageLineNbr);

                    FileInfo generatedFile = service.GenerateCarrierLabelForPackage(reloadedShipment, reloadedPackage);

                    if (generatedFile == null)
                    {
                        PXTrace.WriteWarning("[UCC-SHIP-SCAN-LONGOP] GenerateCarrierLabelForPackage returned null");
                        return;
                    }

                    PXTrace.WriteInformation("[UCC-SHIP-SCAN-LONGOP] ✅ Carrier label generated: {0}", generatedFile.Name);

                    // Output/print the single generated label file
                    OutputSingleGeneratedLabelFile(freshGraph, generatedFile);

                    PXTrace.WriteInformation("[UCC-SHIP-SCAN-LONGOP] ✅ Label file output completed");
                }
                catch (Exception longOpEx)
                {
                    PXTrace.WriteError("[UCC-SHIP-SCAN-LONGOP] Exception in label generation long operation: {0}", longOpEx.Message);
                    PXTrace.WriteError("[UCC-SHIP-SCAN-LONGOP] Stack: {0}", longOpEx.StackTrace);
                    throw;
                }
            });

            PXTrace.WriteInformation("[UCC-SHIP-SCAN] ✅ Long operation queued for package {0}", packageLineNbr);
        }

        /// <summary>
        /// Reload a specific package from the database by shipment + line number.
        /// </summary>
        private SOPackageDetailEx ReloadPackage(SOShipmentEntry graph, string shipmentNbr, int lineNbr)
        {
            try
            {
                SOPackageDetailEx package = PXSelect<
                    SOPackageDetailEx,
                    Where<
                        SOPackageDetailEx.shipmentNbr, Equal<Required<SOPackageDetailEx.shipmentNbr>>,
                        And<SOPackageDetailEx.lineNbr, Equal<Required<SOPackageDetailEx.lineNbr>>>>>
                    .Select(graph, shipmentNbr, lineNbr)
                    .FirstOrDefault() as SOPackageDetailEx;

                return package;
            }
            catch (Exception ex)
            {
                PXTrace.WriteWarning("[UCC-SHIP-SCAN] Error reloading package {0}: {1}", lineNbr, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Output (print/download) the single generated label file.
        /// 
        /// Current implementation uses PXRedirectToFileException for browser download/viewing.
        /// NOTE: This is for testing (browser output).
        /// Future implementation may replace this with DeviceHub or print-job logic.
        /// 
        /// This method signature accepts SOShipmentEntry graph so that
        /// it can be overridden to use alternative output methods (DeviceHub direct printing,
        /// print job queuing, etc.) without requiring changes to the core logic.
        /// </summary>
        protected virtual void OutputSingleGeneratedLabelFile(SOShipmentEntry graph, FileInfo fileInfo)
        {
            if (fileInfo == null)
                throw new PXException("No label file was generated for output.");

            PXTrace.WriteInformation("[UCC-SHIP-SCAN-LONGOP] Redirecting to generated label file: {0}", fileInfo.Name);

            throw new PXRedirectToFileException(fileInfo, true);
        }
    }
}
