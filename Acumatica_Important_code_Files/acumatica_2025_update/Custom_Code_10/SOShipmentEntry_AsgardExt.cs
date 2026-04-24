using System;
using System.Collections;
using System.Linq;
using AA.Objects.Labels;
using Asgard.Labels.Abstractions.Interface;
using Asgard.Labels.Impl.Context;
using Asgard.Labels.Impl.Poco;
using PX.Data;
using PX.Data.DependencyInjection;
using PX.Objects.SO;

namespace AA.Objects.AL.Integration.PerPackage
{
    public class SOShipmentEntry_AsgardExt : PXGraphExtension<SOShipmentEntry>
    {
        public static bool IsActive()
        {
            return true;
        }

        [InjectDependency]
        private ILabelGenerator<IAcuLabelContext> _labelGenerator { get; set; }

        public PXAction<SOShipment> PrintAsgardPackageLabel;

        [PXButton(CommitChanges = true)]
        [PXUIField(DisplayName = "Print Asgard Label", Visible = true, Enabled = true)]
        protected virtual IEnumerable printAsgardPackageLabel(PXAdapter adapter)
        {
            // Delegate to the action method
            PrintForPackage(adapter);
            return adapter.Get();
        }

        /// <summary>
        /// Core action method - called by BOTH button AND scan trigger
        /// This is the single source of truth for print logic
        ///
        /// NEW: Accepts optional selectedPackageLineNbr parameter for row-selection printing
        /// If null, prints the currently selected package in the grid
        /// </summary>
        public virtual void PrintForPackage(PXAdapter adapter, int? selectedPackageLineNbr = null)
        {
            SOShipment shipment = Base.Document.Current;

            if (shipment == null)
                throw new PXException("No shipment is currently selected.");

            if (Base.IsDirty)
            {
                Base.Actions.PressSave();
                shipment = Base.Document.Current;
            }

            // ✅ NEW: If no package specified, get the currently selected row
            if (selectedPackageLineNbr == null)
            {
                SOPackageDetail currentSelected = Base.Packages.Current;
                if (currentSelected?.LineNbr == null)
                {
                    throw new PXException(
                        "No package is selected. Please select a package row and try again.");
                }
                selectedPackageLineNbr = currentSelected.LineNbr;
                PXTrace.WriteInformation("[PRINT] Using currently selected package: {0}", selectedPackageLineNbr);
            }
            else
            {
                PXTrace.WriteInformation("[PRINT] Using explicitly passed package: {0}", selectedPackageLineNbr);
            }

            string shipmentNbr = shipment.ShipmentNbr;
            int packageLineNbr = (int)selectedPackageLineNbr;

            PXLongOperation.StartOperation(Base, delegate()
            {
                PXTrace.WriteInformation("[LONGOP] Creating fresh graph and reloading shipment {0}", shipmentNbr);

                SOShipmentEntry graph = PXGraph.CreateInstance<SOShipmentEntry>();

                SOShipment shipmentInLongOp = SOShipment.PK.Find(graph, shipmentNbr);
                if (shipmentInLongOp == null)
                {
                    throw new PXException(
                        $"Shipment '{shipmentNbr}' could not be reloaded inside the long operation.");
                }

                graph.Document.Current = shipmentInLongOp;

                var asgardService = new AsgardLabelService(graph, _labelGenerator);

                const bool preferBoxPrintModel = false;
                const string fallbackModelName = "RS-8A-Packing for Target";

                Guid? modelId = asgardService.ResolveModelId(fallbackModelName, preferBoxPrintModel);

                if (modelId == null || modelId == Guid.Empty)
                {
                    throw new PXException(
                        "Could not resolve an Asgard label model for selected-package native printing. " +
                        "Please verify ALSetupSlot.BoxPrintModelID or the fallback model name.");
                }

                ALModel resolvedModel = asgardService.GetModelById(modelId);
                asgardService.TraceModelDiagnostics(resolvedModel, modelId, shipmentInLongOp);

                // ✅ CRITICAL FIX: Use the filter scope AND manually set the Packages view current
                // to ensure Asgard only sees the selected package
                PXTrace.WriteInformation("[LONGOP] Setting selected package to {0} in Packages view", packageLineNbr);

                SOPackageDetailEx selectedPackage = PXSelect<
                    SOPackageDetailEx,
                    Where<SOPackageDetailEx.shipmentNbr, Equal<Required<SOPackageDetailEx.shipmentNbr>>,
                    And<SOPackageDetailEx.lineNbr, Equal<Required<SOPackageDetailEx.lineNbr>>>>>
                    .Select(graph, shipmentNbr, packageLineNbr).FirstOrDefault();

                if (selectedPackage == null)
                {
                    throw new PXException($"Package line {packageLineNbr} not found in shipment {shipmentNbr}");
                }

                // Set the current package in the view
                graph.Packages.Current = selectedPackage;
                PXTrace.WriteInformation("[LONGOP] Packages.Current set to line {0}", packageLineNbr);

                // ✅ STATELESS ROW-ONLY BEHAVIOR: Clear all → Set selected → Print → Clear selected
                // Uses a flag-tracking pattern with try/finally to ensure cleanup runs whether print succeeds or fails
                bool selectedFlagWasSet = false;

                try
                {
                    // ✅ STEP 1-2: Clear all flags, then set ONLY selected flag, save once
                    // Previously flagged rows persist in native state and remain print-eligible,
                    // so we must explicitly clear all flags first.
                    PXTrace.WriteInformation("[ROW-ONLY] STEP 1: Clearing UsrALPrintLabel on all packages for shipment {0}", shipmentNbr);
                    
                    foreach (SOPackageDetailEx pkg in PXSelect<
                        SOPackageDetailEx,
                        Where<SOPackageDetailEx.shipmentNbr, Equal<Required<SOPackageDetailEx.shipmentNbr>>>>
                        .Select(graph, shipmentNbr))
                    {
                        if (pkg != null)
                        {
                            graph.Packages.Cache.SetValue(pkg, "UsrALPrintLabel", false);
                            graph.Packages.Cache.Update(pkg);
                        }
                    }
                    
                    PXTrace.WriteInformation("[ROW-ONLY] STEP 2: Setting UsrALPrintLabel = true only on selected package line {0}", packageLineNbr);
                    
                    graph.Packages.Cache.SetValue(selectedPackage, "UsrALPrintLabel", true);
                    graph.Packages.Cache.Update(selectedPackage);
                    
                    // Single save: transition from "all false" to "only selected true"
                    graph.Actions.PressSave();
                    selectedFlagWasSet = true;
                    
                    PXTrace.WriteInformation("[ROW-ONLY] All flags updated: only package {0} marked for printing", packageLineNbr);
                    
                    // ✅ Re-query selected package after save to avoid stale cache
                    selectedPackage = PXSelect<
                        SOPackageDetailEx,
                        Where<SOPackageDetailEx.shipmentNbr, Equal<Required<SOPackageDetailEx.shipmentNbr>>,
                        And<SOPackageDetailEx.lineNbr, Equal<Required<SOPackageDetailEx.lineNbr>>>>>
                        .Select(graph, shipmentNbr, packageLineNbr).FirstOrDefault();
                    
                    if (selectedPackage != null)
                    {
                        graph.Packages.Current = selectedPackage;
                        PXTrace.WriteInformation("[ROW-ONLY] Selected package reloaded after save");
                    }

                    // ✅ Activate scope to bias native Asgard processing toward only the selected package
                    // This combines the filter scope with the temporary print flag for safer multi-layered control.
                    using (ALPackagesFilterScope.Activate(shipmentNbr, new int?[] { packageLineNbr }))
                    {
                        PXTrace.WriteInformation("[LONGOP] Filter scope activated for shipment {0}, package {1}", shipmentNbr, packageLineNbr);

                        // ✅ Load the ALPackages view with scope active to ensure filter state is visible
                        var alPackagesData = graph.Views["ALPackages"].SelectMultiBound(new object[] { shipmentInLongOp });
                        PXTrace.WriteInformation("[LONGOP] ALPackages view loaded with {0} packages", alPackagesData.Count());

                        // ✅ STEP 3: Call native print service
                        // This runs while selectedFlagWasSet=true, scope is active, and only selected package has flag=true
                        PrintResults results = asgardService.PrintSelectedPackageUsingNativeContext(
                            shipmentInLongOp,
                            modelId,
                            packageLineNbr,
                            null);

                        if (results == null)
                            throw new PXException("Label printing returned no results.");

                        if (results.NbLabels <= 0)
                        {
                            throw new PXException(
                                "No labels were generated. Please verify the selected package is valid and the selected label model is configured correctly.");
                        }

                        PXTrace.WriteInformation(
                            $"[ROW-ONLY] STEP 3 COMPLETE: Successfully printed {results.NbLabels} label(s) for package line {packageLineNbr}");
                    }
                }
                finally
                {
                    // ✅ STEP 4: Always cleanup the selected package flag (runs whether print succeeds or fails)
                    // This ensures the checkbox is truly a temporary control, not persistent state.
                    // The finally block guarantees cleanup even if PrintLabels throws an exception.
                    if (selectedFlagWasSet)
                    {
                        PXTrace.WriteInformation("[ROW-ONLY] STEP 4: Clearing UsrALPrintLabel on selected package (finally block)");
                        
                        try
                        {
                            SOPackageDetailEx selectedAfterPrint = PXSelect<
                                SOPackageDetailEx,
                                Where<SOPackageDetailEx.shipmentNbr, Equal<Required<SOPackageDetailEx.shipmentNbr>>,
                                And<SOPackageDetailEx.lineNbr, Equal<Required<SOPackageDetailEx.lineNbr>>>>>
                                .Select(graph, shipmentNbr, packageLineNbr).FirstOrDefault();

                            if (selectedAfterPrint != null)
                            {
                                graph.Packages.Cache.SetValue(selectedAfterPrint, "UsrALPrintLabel", false);
                                graph.Packages.Cache.Update(selectedAfterPrint);
                                graph.Actions.PressSave();
                                
                                PXTrace.WriteInformation("[ROW-ONLY] Cleanup complete: package {0} flag cleared from database", packageLineNbr);
                                PXTrace.WriteInformation("[ROW-ONLY] === STATELESS ROW-ONLY BEHAVIOR COMPLETE ===");
                            }
                        }
                        catch (Exception cleanupEx)
                        {
                            PXTrace.WriteInformation("[ROW-ONLY] ⚠️ CRITICAL: Cleanup failed: {0}", cleanupEx.Message);
                            PXTrace.WriteInformation("[ROW-ONLY] ⚠️ Package {0} flag may remain checked in database", packageLineNbr);
                            // Don't re-throw in finally to preserve original exception context if printing failed
                        }
                    }
                }
            });
        }

        protected virtual void _(Events.RowSelected<SOShipment> e)
        {
            if (PrintAsgardPackageLabel == null)
                return;

            if (e.Row == null)
            {
                PrintAsgardPackageLabel.SetVisible(false);
                PrintAsgardPackageLabel.SetEnabled(false);
                return;
            }

            PrintAsgardPackageLabel.SetVisible(true);
            PrintAsgardPackageLabel.SetEnabled(!string.IsNullOrWhiteSpace(e.Row.ShipmentNbr));
        }

        protected virtual void _(Events.RowSelected<SOPackageDetail> e)
        {
            if (PrintAsgardPackageLabel == null)
                return;

            if (e.Row == null)
            {
                PrintAsgardPackageLabel.SetEnabled(Base.Document.Current != null &&
                                                  !string.IsNullOrWhiteSpace(Base.Document.Current.ShipmentNbr));
                return;
            }

            PrintAsgardPackageLabel.SetEnabled(true);
        }
    }
}