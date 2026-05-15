using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PX.Data;
using PX.Objects.SO;
using PX.Objects.CS;
using PX.Objects.IN;
using PX.SM;
using PX.Objects.CR;

namespace PX.Objects.SO
{
    public class SOShipmentEntry_Ext : PXGraphExtension<SOShipmentEntry>
    {
        public static bool IsActive() => true;

        public PXAction<SOShipment> PrintSelectedPackageLabel;

        [PXButton(CommitChanges = true)]
        [PXUIField(DisplayName = "Generate/Print Selected Package Label", Visible = true, Enabled = true)]
        protected virtual IEnumerable printSelectedPackageLabel(PXAdapter adapter)
        {
            SOShipment shipment = Base.Document.Current;
            SOPackageDetailEx package = Base.Packages.Current;

            if (shipment == null || package == null)
            {
                throw new PXException("Shipment and package must be selected to generate labels.");
            }

            PXTrace.WriteInformation(
                "[MANUAL-PRINT] Starting manual package label generation. Shipment={0}, LineNbr={1}",
                shipment.ShipmentNbr,
                package.LineNbr);

            // ========================================================================
            // Variables to capture state from inside filter scope
            // These will be used OUTSIDE the scope to avoid context leakage
            // ========================================================================
            FileInfo queuedFile = null;
            Guid? queuedPrinterID = null;
            FileInfo fallbackFile = null;

            // ========================================================================
            // CRITICAL: Activate filter scope for manual button path
            // This ensures CarrierRates.GetPackages override filters packages
            // ========================================================================
            using (CarrierPackageFilterScope.Activate(shipment.ShipmentNbr, package.LineNbr))
            {
                PXTrace.WriteInformation(
                    "[MANUAL-PRINT] [CarrierPkgFilter] Activated filter scope. Shipment={0}, LineNbr={1}",
                    shipment.ShipmentNbr,
                    package.LineNbr);

                try
                {
                    var svc = new PackageCarrierLabelService(Base);
                    svc.ValidatePackageForGeneration(shipment, package);

                    FileInfo existingFile = svc.TryGetExistingCarrierLabel(package);
                    if (existingFile != null)
                    {
                        PXTrace.WriteInformation(
                            "[MANUAL-PRINT] Using existing label file: {0}",
                            existingFile.Name);
                        
                        // Capture existing file for use outside scope
                        if (existingFile.UID.HasValue)
                        {
                            queuedFile = existingFile;
                            queuedPrinterID = ResolveDeviceHubPrinter();
                            if (!queuedPrinterID.HasValue)
                            {
                                // No printer - use fallback download
                                fallbackFile = existingFile;
                                queuedFile = null;
                            }
                        }
                        else
                        {
                            // No UID - use fallback download
                            fallbackFile = existingFile;
                        }
                        
                        // Exit scope to use captured state
                        // IMPORTANT: This return happens INSIDE the using block, but values are captured
                        // actual queueing happens outside scope
                    }
                    else
                    {
                        FileInfo generatedFile = svc.GenerateCarrierLabelForPackage(shipment, package);
                        if (generatedFile != null)
                        {
                            PXTrace.WriteInformation(
                                "[MANUAL-PRINT] ✅ Label generated: {0}",
                                generatedFile.Name);

                            // ========================================================================
                            // Simplify refresh: just request refresh, avoid heavy cache clearing
                            // ========================================================================
                            PXTrace.WriteInformation(
                                "[MANUAL-PRINT] Requesting package grid refresh after generation");

                            Base.Packages.View.RequestRefresh();

                            // Capture generated file for use outside scope
                            if (generatedFile.UID.HasValue)
                            {
                                queuedFile = generatedFile;
                                queuedPrinterID = ResolveDeviceHubPrinter();
                                if (!queuedPrinterID.HasValue)
                                {
                                    // No printer - use fallback download
                                    fallbackFile = generatedFile;
                                    queuedFile = null;
                                }
                            }
                            else
                            {
                                throw new PXException("Generated label file does not have a valid UID for printing.");
                            }
                        }
                        else
                        {
                            throw new PXException($"No label could be found or generated for package line {package.LineNbr}.");
                        }
                    }
                }
                catch (PXException pxEx)
                {
                    PXTrace.WriteError(
                        "[MANUAL-PRINT] [CarrierPkgFilter-ERROR] PXException in manual print: {0}",
                        pxEx.Message);
                    PXTrace.WriteError(
                        "[MANUAL-PRINT] [CarrierPkgFilter-ERROR] Stack: {0}",
                        pxEx.StackTrace);
                    throw;
                }
                finally
                {
                    PXTrace.WriteInformation(
                        "[MANUAL-PRINT] [CarrierPkgFilter] Filter scope exiting");
                }
            }

            // ========================================================================
            // FIX #1: CRITICAL - Print job queueing happens OUTSIDE filter scope
            // This prevents CarrierPackageFilterScope context from leaking into async operation
            // ========================================================================
            if (queuedFile != null && queuedPrinterID.HasValue)
            {
                PXTrace.WriteInformation(
                    "[MANUAL-PRINT] Queueing DeviceHub print job for file {0} (OUTSIDE scope)",
                    queuedFile.UID.Value);
                QueueFilePrintJob(queuedFile.UID.Value, queuedPrinterID.Value);

                // ====================================================================
                // OPTIONAL: Also prompt user to download file after print job is queued
                // 
                // Set to true for testing/debugging, false for production.
                // This should later become a user preference/configuration setting.
                // Default production behavior: queue print only, no download prompt.
                // ====================================================================
                bool alsoDownloadAfterPrint = true; // TODO: Make this configurable per user preference

                if (alsoDownloadAfterPrint)
                {
                    PXTrace.WriteInformation(
                        "[MANUAL-PRINT] Also offering file download after print job queued");
                    var svc = new PackageCarrierLabelService(Base);
                    svc.PrintSingleFile(queuedFile);
                    // NOTE: PrintSingleFile throws PXRedirectToFileException, which stops execution
                    // This is intentional - the print job was already queued before the exception
                }

                return adapter.Get();
            }
            else if (fallbackFile != null)
            {
                // FIX #6: Fallback file download happens OUTSIDE scope
                PXTrace.WriteWarning(
                    "[MANUAL-PRINT] No printer configured, using file download fallback (OUTSIDE scope)");
                var svc = new PackageCarrierLabelService(Base);
                svc.PrintSingleFile(fallbackFile);
                return adapter.Get();
            }
            else
            {
                throw new PXException("Label file could not be queued for printing.");
            }
        }

        protected virtual void _(Events.RowSelected<SOShipment> e)
        {
            if (e.Row == null)
                return;

            PrintSelectedPackageLabel.SetVisible(true);
            PrintSelectedPackageLabel.SetEnabled(true);
        }

        /// <summary>
        /// Resolve DeviceHub printer for carrier label printing.
        /// 
        /// Uses Acumatica's native notification utility to find printer
        /// by PrintLabels report.
        /// 
        /// Returns null if no printer is configured.
        /// </summary>
        protected virtual Guid? ResolveDeviceHubPrinter()
        {
            try
            {
                var notificationUtility = new NotificationUtility(Base);
                Guid? printerID = notificationUtility.SearchPrinter(
                    SONotificationSource.Customer,
                    SOReports.PrintLabels,
                    Base.Accessinfo.BranchID);

                if (printerID.HasValue && printerID.Value != Guid.Empty)
                {
                    PXTrace.WriteInformation(
                        "[PRINT-RESOLVE] Printer resolved. PrinterID={0}",
                        printerID.Value);
                    return printerID;
                }

                PXTrace.WriteWarning(
                    "[PRINT-RESOLVE] No printer resolved for PrintLabels");
                return null;
            }
            catch (Exception ex)
            {
                PXTrace.WriteError(
                    "[PRINT-RESOLVE] Exception resolving printer: {0}",
                    ex);
                return null;
            }
        }

        /// <summary>
        /// Queue raw ZPL label file to DeviceHub printer via LongOperationManager.
        /// 
        /// Purpose:
        /// Creates async print job so file is sent to printer, not downloaded.
        /// Uses LongOperationManager.StartAsyncOperation for proper Acumatica pattern.
        /// 
        /// Parameters:
        /// - fileID: The persisted file UID
        /// - printerID: The DeviceHub printer GUID
        /// 
        /// Why LongOperationManager:
        /// - Proper async context in Acumatica
        /// - Avoids graph lifetime issues
        /// - Integrates with UI progress/feedback
        /// </summary>
        protected virtual void QueueFilePrintJob(Guid fileID, Guid printerID)
        {
            if (fileID == Guid.Empty)
                throw new PXException("File ID is required for queuing print job.");

            if (printerID == Guid.Empty)
                throw new PXException("Printer ID is required for queuing print job.");

            PXTrace.WriteInformation(
                "[MANUAL-PRINT] Queuing print job for file {0} to printer {1}",
                fileID, printerID);

            // ====================================================================
            // Queue async print job using LongOperationManager
            // Creates fresh graph inside async context
            // ====================================================================
            Base.LongOperationManager.StartAsyncOperation(ct =>
            {
                // TRACE #3: Async operation start
                PXTrace.WriteInformation(
                    "[PRINT-ASYNC] Starting async DeviceHub operation for file {0}",
                    fileID);

                // Create fresh graph instance inside async context
                SOShipmentEntry freshGraph = PXGraph.CreateInstance<SOShipmentEntry>();
                PackageCarrierLabelService svc = new PackageCarrierLabelService(freshGraph);

                // Call async method that uses fresh graph
                return svc.SendRawFileToPrinterAsync(fileID, printerID, ct);
            });

            PXTrace.WriteInformation(
                "[MANUAL-PRINT] Print job queued successfully");
        }

        public delegate void ShipPackagesDelegate(SOShipment shiporder);

        [PXOverride]
        public virtual void ShipPackages(SOShipment shiporder, ShipPackagesDelegate baseMethod)
        {
            if (shiporder == null)
            {
                baseMethod(shiporder);
                return;
            }

            PXTrace.WriteInformation(
                "[SHIP-PACKAGES-SAFE-GUARD] Entered ShipPackages for shipment {0}",
                shiporder.ShipmentNbr);

            List<SOPackageDetailEx> allPackages = PXSelect<
                SOPackageDetailEx,
                Where<SOPackageDetailEx.shipmentNbr, Equal<Required<SOPackageDetailEx.shipmentNbr>>>>
                .Select(Base, shiporder.ShipmentNbr)
                .RowCast<SOPackageDetailEx>()
                .ToList();

            int totalPackages = allPackages.Count;
            int packagesWithTracking = allPackages.Count(p => !string.IsNullOrWhiteSpace(p.TrackNumber));
            int packagesWithoutTracking = totalPackages - packagesWithTracking;

            PXTrace.WriteInformation(
                "[SHIP-PACKAGES-SAFE-GUARD] Package analysis: Total={0}, WithTracking={1}, WithoutTracking={2}",
                totalPackages,
                packagesWithTracking,
                packagesWithoutTracking);

            foreach (var pkg in allPackages)
            {
                PXTrace.WriteInformation(
                    "[SHIP-PACKAGES-SAFE-GUARD] Package LineNbr={0}, TrackNumber={1}",
                    pkg.LineNbr,
                    string.IsNullOrWhiteSpace(pkg.TrackNumber) ? "(empty)" : pkg.TrackNumber);
            }

            // CASE 1: No package rows. Let native behavior decide.
            if (totalPackages == 0)
            {
                PXTrace.WriteInformation(
                    "[SHIP-PACKAGES-SAFE-GUARD] No package rows found. Calling base ShipPackages.");
                baseMethod(shiporder);
                return;
            }

            // CASE 2: All packages already tracked.
            // Do NOT call baseMethod, because baseMethod may contact the carrier again.
            // However, we must still validate packed quantities if carrier requires it.
            if (packagesWithTracking == totalPackages)
            {
                PXTrace.WriteWarning(
                    "[SHIP-PACKAGES-SAFE-GUARD] All packages already have tracking. Skipping native carrier ShipPackages to prevent duplicate carrier shipment generation.");

                // ================================================================
                // PRESERVE NATIVE VALIDATION: Check if carrier requires packed qty validation
                // ================================================================
                Carrier carrier = Carrier.PK.Find(Base, shiporder.ShipVia);
                if (carrier != null && carrier.ValidatePackedQty == true)
                {
                    PXTrace.WriteInformation(
                        "[SHIP-PACKAGES-SAFE-GUARD] Carrier {0} requires packed quantity validation. Validating before skipping carrier generation.",
                        carrier.CarrierID);

                    // Run equivalent of Acumatica's ValidatePackagedQuantities
                    ValidatePackagedQuantities(shiporder);

                    PXTrace.WriteInformation(
                        "[SHIP-PACKAGES-SAFE-GUARD] ✅ Packed quantity validation passed.");
                }
                else if (carrier != null)
                {
                    PXTrace.WriteInformation(
                        "[SHIP-PACKAGES-SAFE-GUARD] Carrier {0} does not require packed quantity validation.",
                        carrier.CarrierID);
                }
                else
                {
                    PXTrace.WriteWarning(
                        "[SHIP-PACKAGES-SAFE-GUARD] Carrier {0} not found. Skipping validation.",
                        shiporder.ShipVia);
                }

                EnsureShippedViaCarrierWhenAllPackagesTracked(shiporder);

                PXTrace.WriteInformation(
                    "[SHIP-PACKAGES-SAFE-GUARD] Exiting ShipPackages without calling baseMethod (all packages already tracked).");
                return;
            }

            // CASE 3: No packages tracked.
            // Safe to allow native carrier generation.
            if (packagesWithTracking == 0)
            {
                PXTrace.WriteInformation(
                    "[SHIP-PACKAGES-SAFE-GUARD] No packages have tracking. Calling native ShipPackages normally.");

                baseMethod(shiporder);

                PXTrace.WriteInformation(
                    "[SHIP-PACKAGES-SAFE-GUARD] Native ShipPackages completed.");
                return;
            }

            // CASE 4: Mixed state (some packages have tracking, some do not).
            // This is INTENTIONALLY BLOCKED to prevent duplicate FedEx/UPS shipment generation.
            // We do NOT use GetPackages filtering for mixed state - instead, we throw an exception.
            // This is the safest approach: fail fast before contacting carrier.
            PXTrace.WriteError(
                "[SHIP-PACKAGES-SAFE-GUARD] ❌ MIXED STATE DETECTED - {0} packages have tracking, {1} packages need tracking. Throwing exception.",
                packagesWithTracking,
                packagesWithoutTracking);

            throw new PXException(
                "Confirm Shipment was stopped because this shipment has a mix of tracked and untracked packages. " +
                "Some packages already have carrier tracking, while others do not. " +
                "To prevent duplicate FedEx/UPS shipment generation, native carrier processing will not run in this mixed state. " +
                "Please generate labels for the remaining untracked packages individually first, then confirm the shipment again.");
        }

        /// <summary>
        /// Validate that all shipment lines/splits have matching packed quantities.
        /// This mimics Acumatica's native ValidatePackagedQuantities logic to preserve
        /// the native validation behavior while still preventing duplicate carrier generation.
        /// 
        /// Purpose:
        /// When all packages already have tracking, we skip baseMethod to avoid FedEx/UPS
        /// carrier call. However, we must still enforce packed quantity validation if the
        /// carrier requires it (carrier.ValidatePackedQty == true).
        /// 
        /// Logic:
        /// - Only validate for Issue shipments (not Returns, Transfers, etc)
        /// - For each SOShipLine where LineType is Inventory:
        ///   - Check if BaseShippedQty != BasePackedQty
        ///   - For tracked items or kit items, this is an error
        /// - For each SOShipLineSplit:
        ///   - Check if BaseQty != BasePackedQty
        /// 
        /// Throws PXException if validation fails (similar to native Acumatica).
        /// </summary>
        private void ValidatePackagedQuantities(SOShipment shipment)
        {
            if (shipment == null)
                return;

            // Reload shipment to ensure current state
            SOShipment current = Base.Document.Search<SOShipment.shipmentNbr>(shipment.ShipmentNbr);
            if (current == null)
                return;

            // Only validate Issue shipments (not Returns, Transfers, etc)
            if (current.ShipmentType != SOShipmentType.Issue)
            {
                PXTrace.WriteInformation(
                    "[VALIDATE-PKD-QTY] Skipping validation - shipment type is {0}, not Issue.",
                    current.ShipmentType);
                return;
            }

            PXTrace.WriteInformation(
                "[VALIDATE-PKD-QTY] Validating packed quantities for shipment {0}",
                current.ShipmentNbr);

            // Query all shipment lines
            var shipLines = PXSelect<
                SOShipLine,
                Where<SOShipLine.shipmentNbr, Equal<Required<SOShipLine.shipmentNbr>>>>
                .Select(Base, current.ShipmentNbr)
                .RowCast<SOShipLine>()
                .ToList();

            PXTrace.WriteInformation(
                "[VALIDATE-PKD-QTY] Found {0} shipment lines",
                shipLines.Count);

            // Check each shipment line
            foreach (var shipLine in shipLines)
            {
                // Only validate Inventory type lines
                if (shipLine.LineType != SOLineType.Inventory)
                {
                    PXTrace.WriteInformation(
                        "[VALIDATE-PKD-QTY] Skipping LineNbr={0} - LineType is {1}, not Inventory",
                        shipLine.LineNbr,
                        shipLine.LineType);
                    continue;
                }

                PXTrace.WriteInformation(
                    "[VALIDATE-PKD-QTY] Checking LineNbr={0}, BaseShippedQty={1}, BasePackedQty={2}",
                    shipLine.LineNbr,
                    shipLine.BaseShippedQty,
                    shipLine.BasePackedQty);

                // Check if shipped qty != packed qty
                if (shipLine.BaseShippedQty != shipLine.BasePackedQty)
                {
                    // Find the inventory item to get InventoryCD for error message
                    InventoryItem item = InventoryItem.PK.Find(Base, shipLine.InventoryID);
                    string itemCD = item?.InventoryCD?.Trim() ?? $"(ID: {shipLine.InventoryID})";

                    // For tracked items or non-kit items, throw error
                    if (item != null && (item.StkItem == true || item.KitItem != true))
                    {
                        PXTrace.WriteError(
                            "[VALIDATE-PKD-QTY] ❌ LineNbr={0} ({1}) - Shipped qty {2} != Packed qty {3}",
                            shipLine.LineNbr,
                            itemCD,
                            shipLine.BaseShippedQty,
                            shipLine.BasePackedQty);

                        throw new PXException(Messages.ShipmentLineQuantityNotPacked, itemCD);
                    }
                }

                // Check splits
                var splits = PXSelect<
                    SOShipLineSplit,
                    Where<SOShipLineSplit.shipmentNbr, Equal<Required<SOShipLineSplit.shipmentNbr>>,
                        And<SOShipLineSplit.lineNbr, Equal<Required<SOShipLineSplit.lineNbr>>>>>
                    .Select(Base, current.ShipmentNbr, shipLine.LineNbr)
                    .RowCast<SOShipLineSplit>()
                    .ToList();

                foreach (var split in splits)
                {
                    PXTrace.WriteInformation(
                        "[VALIDATE-PKD-QTY] Checking split LineNbr={0}, SplitLineNbr={1}, BaseQty={2}, BasePackedQty={3}",
                        split.LineNbr,
                        split.SplitLineNbr,
                        split.BaseQty,
                        split.BasePackedQty);

                    if (split.BaseQty != split.BasePackedQty)
                    {
                        InventoryItem item = InventoryItem.PK.Find(Base, shipLine.InventoryID);
                        string itemCD = item?.InventoryCD?.Trim() ?? $"(ID: {shipLine.InventoryID})";

                        PXTrace.WriteError(
                            "[VALIDATE-PKD-QTY] ❌ Split LineNbr={0}, SplitLineNbr={1} ({2}) - Qty {3} != PackedQty {4}",
                            split.LineNbr,
                            split.SplitLineNbr,
                            itemCD,
                            split.BaseQty,
                            split.BasePackedQty);

                        throw new PXException(Messages.ShipmentLineQuantityNotPacked, itemCD);
                    }
                }
            }

            PXTrace.WriteInformation(
                "[VALIDATE-PKD-QTY] ✅ All packed quantities validated successfully");
        }

        /// <summary>
        /// Ensure ShippedViaCarrier flag is set when all packages have tracking.
        /// This is critical for Correct Shipment to work properly.
        /// </summary>
        private void EnsureShippedViaCarrierWhenAllPackagesTracked(SOShipment shipment)
        {
            if (shipment == null)
                return;

            List<SOPackageDetailEx> packages = PXSelect<
                SOPackageDetailEx,
                Where<SOPackageDetailEx.shipmentNbr, Equal<Required<SOPackageDetailEx.shipmentNbr>>>>
                .Select(Base, shipment.ShipmentNbr)
                .RowCast<SOPackageDetailEx>()
                .ToList();

            int totalPackages = packages.Count;
            int packagesWithTracking = packages.Count(p => !string.IsNullOrWhiteSpace(p.TrackNumber));

            PXTrace.WriteInformation(
                "[ENSURE-SHIPPED-VIA-CARRIER] Checking ShippedViaCarrier state. Total={0}, Tracked={1}",
                totalPackages,
                packagesWithTracking);

            if (totalPackages == 0 || packagesWithTracking != totalPackages)
            {
                PXTrace.WriteInformation("[ENSURE-SHIPPED-VIA-CARRIER] Not setting ShippedViaCarrier - not all packages tracked");
                return;
            }

            // Re-query shipment to get current state
            SOShipment current = Base.Document.Search<SOShipment.shipmentNbr>(shipment.ShipmentNbr);

            if (current == null)
            {
                PXTrace.WriteWarning("[ENSURE-SHIPPED-VIA-CARRIER] Shipment not found");
                return;
            }

            if (current.ShippedViaCarrier == true)
            {
                PXTrace.WriteInformation("[ENSURE-SHIPPED-VIA-CARRIER] ShippedViaCarrier already true, no change needed");
                return;
            }

            PXTrace.WriteInformation(
                "[ENSURE-SHIPPED-VIA-CARRIER] All packages tracked. Setting ShippedViaCarrier=true");

            current.ShippedViaCarrier = true;
            Base.Document.Update(current);

            // NOTE: Do NOT call Base.Save.Press() here.
            // Let the native ConfirmShipment flow handle the transaction save.
            // This ensures we remain within the original transaction context.

            PXTrace.WriteInformation("[ENSURE-SHIPPED-VIA-CARRIER] ✅ ShippedViaCarrier set (save deferred to native flow)");
        }
    }
}