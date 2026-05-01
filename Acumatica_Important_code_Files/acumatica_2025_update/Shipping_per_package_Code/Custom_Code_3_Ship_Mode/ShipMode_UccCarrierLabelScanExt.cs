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
    /// WMS Ship Mode - Generate Package Carrier Label Command
    /// ========================================================================
    /// 
    /// Command: *ship*label
    /// 
    /// Purpose:
    /// In Ship Mode, allows warehouse user to generate and print a carrier label
    /// for a single package by scanning the package's custom UCC value (UsrTCUCC128).
    /// 
    /// Workflow:
    /// User: @ship                          (Enter Ship Mode)
    /// User: scan shipment number           (Load shipment)
    /// User: scan *ship*label              (Activate this command)
    /// System: "Scan package UCC."         (Prompt user)
    /// User: scan package.UsrTCUCC128      (Provide package identifier)
    /// System: Generate and output label    (Single file only)
    /// 
    /// Design:
    /// - Command is only available in Ship Mode
    /// - Requires a shipment to be loaded (Basis.RefNbr)
    /// - Creates temporary scan state to capture the UCC barcode
    /// - Uses PackageCarrierLabelService for label generation
    /// - Performs duplicate protection before and after reload
    /// - Uses PXLongOperation with fresh SOShipmentEntry graph
    /// - Outputs only the generated file (not shipment-level print)
    /// </summary>
    public partial class PickPackShip : WMSBase
    {
        public sealed class ShipMode : ScanMode
        {
            #region Commands
            public sealed class ShipLabelCommand : ScanCommand
            {
                public override string Code => "SHIP*LABEL";
                public override string ButtonName => "scanShipLabel";
                public override string DisplayName => Msg.DisplayName;
                protected override bool IsEnabled => Basis.DocumentIsEditable && Basis.RefNbr != null;

                protected override bool Process() => Get().ProcessShipLabel();

                #region Logic
                public class Logic : ScanExtension
                {
                    /// <summary>
                    /// Process the *ship*label command.
                    /// Enters a temporary state that waits for the next scan to be a package UCC.
                    /// </summary>
                    public virtual bool ProcessShipLabel()
                    {
                        // Verify shipment is loaded
                        if (string.IsNullOrWhiteSpace(Basis.RefNbr))
                        {
                            Basis.ReportError(Msg.NoShipmentLoaded);
                            return false;
                        }

                        PXTrace.WriteInformation("[SHIP-LABEL-CMD] *ship*label command activated");
                        PXTrace.WriteInformation("[SHIP-LABEL-CMD] Entering UCC capture state");

                        // Enter temporary scan state to capture package UCC
                        Basis.SetScanState<UccCaptureState>();
                        Basis.ReportInfo(Msg.PromptUcc);

                        return true;
                    }
                }
                #endregion

                #region Messages
                [PXLocalizable]
                public abstract class Msg
                {
                    public const string DisplayName = "Ship Package Label";
                    public const string NoShipmentLoaded = "Scan a shipment before generating a package label.";
                    public const string PromptUcc = "Scan package UCC.";
                    public const string UccNotFound = "No package was found for the scanned UCC.";
                    public const string UccDuplicate = "Package already has a carrier label or tracking number.";
                    public const string GeneratingLabel = "Generating carrier label for package {0}...";
                    public const string LabelGenerated = "Carrier label generated and output successfully.";
                    public const string LabelGenerationFailed = "Carrier label generation failed. Check the trace log for details.";
                }
                #endregion
            }

            /// <summary>
            /// Temporary scan state that captures the package UCC barcode.
            /// This state is entered after *ship*label command is activated.
            /// </summary>
            public class UccCaptureState : ScanState
            {
                public override string Prompt => Msg.Prompt;

                public override bool Process(string barcode)
                {
                    PXTrace.WriteInformation("[SHIP-LABEL-UCC] Processing UCC barcode: '{0}'", barcode ?? "null");

                    if (string.IsNullOrWhiteSpace(barcode))
                    {
                        Basis.ReportError(Msg.BarcodeEmpty);
                        return false;
                    }

                    // Try to handle UCC carrier label scan
                    if (TryHandleUccCarrierLabelScan(Basis, barcode))
                    {
                        PXTrace.WriteInformation("[SHIP-LABEL-UCC] ✅ UCC handled successfully");
                        // Return to default state after processing
                        Basis.SetScanState();
                        return true;
                    }

                    // UCC not found on any package
                    Basis.ReportError(ShipLabelCommand.Logic.Msg.UccNotFound);
                    return false;
                }

                #region Messages
                [PXLocalizable]
                public abstract class Msg
                {
                    public const string Prompt = "Scan package UCC.";
                    public const string BarcodeEmpty = "Barcode cannot be empty.";
                }
                #endregion
            }

            /// <summary>
            /// Main entry point for UCC carrier label scan handling.
            /// Normalizes barcode, finds matching package, validates conditions, queues generation.
            /// </summary>
            private static bool TryHandleUccCarrierLabelScan(PickPackShip basis, string rawBarcode)
            {
                try
                {
                    string normalizedBarcode = NormalizeBarcode(rawBarcode);
                    PXTrace.WriteInformation("[SHIP-LABEL-UCC] Normalized barcode: '{0}'", normalizedBarcode);

                    // Access shipment through Basis.RefNbr (the shipment loaded in Ship Mode)
                    if (string.IsNullOrWhiteSpace(basis.RefNbr))
                    {
                        PXTrace.WriteWarning("[SHIP-LABEL-UCC] No shipment is currently loaded");
                        return false;
                    }

                    SOShipment shipment = SOShipment.PK.Find(basis.Graph, basis.RefNbr);
                    if (shipment == null)
                    {
                        PXTrace.WriteWarning("[SHIP-LABEL-UCC] Shipment {0} could not be found", basis.RefNbr);
                        return false;
                    }

                    PXTrace.WriteInformation("[SHIP-LABEL-UCC] Current shipment: {0}", shipment.ShipmentNbr);

                    // Use a fresh graph for the lookup (safe approach to avoid cross-graph contamination)
                    var lookupGraph = PXGraph.CreateInstance<SOShipmentEntry>();
                    var lookupShipment = SOShipment.PK.Find(lookupGraph, shipment.ShipmentNbr);
                    if (lookupShipment == null)
                    {
                        PXTrace.WriteWarning("[SHIP-LABEL-UCC] Shipment {0} could not be loaded on lookup graph", shipment.ShipmentNbr);
                        return false;
                    }
                    lookupGraph.Document.Current = lookupShipment;

                    // Find package matching this UCC in the current shipment
                    SOPackageDetailEx package = FindPackageByUcc(lookupGraph, shipment.ShipmentNbr, normalizedBarcode);
                    if (package == null)
                    {
                        PXTrace.WriteInformation("[SHIP-LABEL-UCC] No package found with UCC '{0}' in shipment {1}", normalizedBarcode, shipment.ShipmentNbr);
                        return false;
                    }

                    PXTrace.WriteInformation("[SHIP-LABEL-UCC] Found package line {0} with UCC '{1}'", package.LineNbr, normalizedBarcode);

                    // Check for existing tracking/label (duplicate protection before queuing)
                    if (HasExistingTrackingOrLabel(lookupGraph, package))
                    {
                        PXTrace.WriteWarning("[SHIP-LABEL-UCC] Package line {0} already has tracking or label attached, skipping generation", package.LineNbr);
                        basis.ReportInfo(ShipLabelCommand.Logic.Msg.UccDuplicate);
                        return true;
                    }

                    // Queue the carrier label generation in a long operation
                    QueueCarrierLabelGeneration(basis, shipment, package);
                    return true;
                }
                catch (Exception ex)
                {
                    PXTrace.WriteError("[SHIP-LABEL-UCC] Exception in TryHandleUccCarrierLabelScan: {0}", ex.Message);
                    PXTrace.WriteError("[SHIP-LABEL-UCC] Stack: {0}", ex.StackTrace);
                    return false;
                }
            }

            /// <summary>
            /// Normalize barcode by trimming whitespace and handling GS1 formatting.
            /// Enhanced to handle UCC-128 barcodes with GS1 separators (ASCII 0x1D).
            /// </summary>
            private static string NormalizeBarcode(string raw)
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
            private static SOPackageDetailEx FindPackageByUcc(SOShipmentEntry graph, string shipmentNbr, string normalizedUcc)
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
                    PXTrace.WriteWarning("[SHIP-LABEL-UCC] Exception during package lookup: {0}", ex.Message);
                    return null;
                }
            }

            /// <summary>
            /// Check if package already has a tracking number or carrier label file attached.
            /// Prevents duplicate carrier generation.
            /// </summary>
            private static bool HasExistingTrackingOrLabel(SOShipmentEntry graph, SOPackageDetailEx package)
            {
                if (package == null)
                    return false;

                // Check for existing tracking number
                if (!string.IsNullOrWhiteSpace(package.TrackNumber))
                {
                    PXTrace.WriteWarning("[SHIP-LABEL-UCC] Package line {0} already has TrackNumber: {1}", package.LineNbr, package.TrackNumber);
                    return true;
                }

                // Check for existing attached file
                FileInfo existingFile = TryGetExistingCarrierLabel(graph, package);
                if (existingFile != null)
                {
                    PXTrace.WriteWarning("[SHIP-LABEL-UCC] Package line {0} already has label file attached: {1}", package.LineNbr, existingFile.Name);
                    return true;
                }

                return false;
            }

            /// <summary>
            /// Check if a carrier label file is already attached to the package.
            /// Looks for .pdf, .zpl, .zplii, .epl files in package notes.
            /// </summary>
            private static FileInfo TryGetExistingCarrierLabel(SOShipmentEntry graph, SOPackageDetailEx package)
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
                    PXTrace.WriteWarning("[SHIP-LABEL-UCC] Error checking for existing label file: {0}", ex.Message);
                    return null;
                }
            }

            /// <summary>
            /// Queue a PXLongOperation to generate and print the carrier label for the package.
            /// Uses fresh graph to avoid state contamination from WMS context.
            /// </summary>
            private static void QueueCarrierLabelGeneration(PickPackShip basis, SOShipment shipment, SOPackageDetailEx package)
            {
                string shipmentNbr = shipment.ShipmentNbr;
                int? packageLineNbr = package.LineNbr;

                if (packageLineNbr == null)
                {
                    PXTrace.WriteError("[SHIP-LABEL-UCC] Package has no LineNbr, cannot queue generation");
                    return;
                }

                PXTrace.WriteInformation("[SHIP-LABEL-UCC] Queueing carrier label generation for package {0} in shipment {1}", packageLineNbr, shipmentNbr);
                basis.ReportInfo(ShipLabelCommand.Logic.Msg.GeneratingLabel, packageLineNbr);

                // Use basis.Graph as the context (following Acumatica WMS patterns)
                PXLongOperation.StartOperation(basis.Graph, delegate()
                {
                    PXTrace.WriteInformation("[SHIP-LABEL-UCC-LONGOP] Long operation started for package {0}", packageLineNbr);

                    try
                    {
                        // Create fresh graph inside long operation
                        SOShipmentEntry freshGraph = PXGraph.CreateInstance<SOShipmentEntry>();

                        // Reload shipment
                        SOShipment reloadedShipment = SOShipment.PK.Find(freshGraph, shipmentNbr);
                        if (reloadedShipment == null)
                        {
                            PXTrace.WriteError("[SHIP-LABEL-UCC-LONGOP] Shipment {0} not found after reload", shipmentNbr);
                            throw new PXException($"Shipment '{shipmentNbr}' could not be reloaded in the label generation operation.");
                        }

                        freshGraph.Document.Current = reloadedShipment;

                        // Reload package
                        SOPackageDetailEx reloadedPackage = ReloadPackage(freshGraph, shipmentNbr, packageLineNbr.Value);
                        if (reloadedPackage == null)
                        {
                            PXTrace.WriteError("[SHIP-LABEL-UCC-LONGOP] Package {0} not found after reload", packageLineNbr);
                            throw new PXException($"Package line {packageLineNbr} could not be reloaded in the label generation operation.");
                        }

                        PXTrace.WriteInformation("[SHIP-LABEL-UCC-LONGOP] Shipment and package reloaded successfully");

                        // DUPLICATE PROTECTION (second check after reload)
                        // Another user/process could have generated the label after scan but before operation ran
                        if (!string.IsNullOrWhiteSpace(reloadedPackage.TrackNumber))
                        {
                            PXTrace.WriteWarning("[SHIP-LABEL-UCC-LONGOP] Package {0} now has tracking number {1}, skipping generation", packageLineNbr, reloadedPackage.TrackNumber);
                            return;
                        }

                        if (TryGetExistingCarrierLabel(freshGraph, reloadedPackage) != null)
                        {
                            PXTrace.WriteWarning("[SHIP-LABEL-UCC-LONGOP] Package {0} now has label attached, skipping generation", packageLineNbr);
                            return;
                        }

                        // Generate carrier label using service
                        var service = new PackageCarrierLabelService(freshGraph);

                        PXTrace.WriteInformation("[SHIP-LABEL-UCC-LONGOP] Calling GenerateCarrierLabelForPackage for package {0}", packageLineNbr);

                        FileInfo generatedFile = service.GenerateCarrierLabelForPackage(reloadedShipment, reloadedPackage);

                        if (generatedFile == null)
                        {
                            PXTrace.WriteWarning("[SHIP-LABEL-UCC-LONGOP] GenerateCarrierLabelForPackage returned null");
                            return;
                        }

                        PXTrace.WriteInformation("[SHIP-LABEL-UCC-LONGOP] ✅ Carrier label generated: {0}", generatedFile.Name);

                        // Output/print the single generated label file
                        OutputSingleGeneratedLabelFile(generatedFile);

                        PXTrace.WriteInformation("[SHIP-LABEL-UCC-LONGOP] ✅ Label file output completed");
                    }
                    catch (Exception longOpEx)
                    {
                        PXTrace.WriteError("[SHIP-LABEL-UCC-LONGOP] Exception in label generation long operation: {0}", longOpEx.Message);
                        PXTrace.WriteError("[SHIP-LABEL-UCC-LONGOP] Stack: {0}", longOpEx.StackTrace);
                        throw;
                    }
                });

                PXTrace.WriteInformation("[SHIP-LABEL-UCC] ✅ Long operation queued for package {0}", packageLineNbr);
            }

            /// <summary>
            /// Reload a specific package from the database by shipment + line number.
            /// </summary>
            private static SOPackageDetailEx ReloadPackage(SOShipmentEntry graph, string shipmentNbr, int lineNbr)
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
                    PXTrace.WriteWarning("[SHIP-LABEL-UCC] Error reloading package {0}: {1}", lineNbr, ex.Message);
                    return null;
                }
            }

            /// <summary>
            /// Output (print/download) the single generated label file.
            /// Current implementation uses PXRedirectToFileException for browser download/viewing.
            /// NOTE: This is for testing (browser output).
            /// Future implementation may replace this with DeviceHub or print-job logic.
            /// </summary>
            private static void OutputSingleGeneratedLabelFile(FileInfo fileInfo)
            {
                if (fileInfo == null)
                    throw new PXException("No label file was generated for output.");

                PXTrace.WriteInformation("[SHIP-LABEL-UCC-LONGOP] Redirecting to generated label file: {0}", fileInfo.Name);

                throw new PXRedirectToFileException(fileInfo, true);
            }
            #endregion
        }
    }
}
