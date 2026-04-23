using System;
using System.Collections;
using System.Linq;
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
        private ILabelGenerator _labelGenerator { get; set; }

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

                // ✅ PROOF TEST: Set UsrALPrintLabel on the selected package before printing
                // This tests the hypothesis that row-selection fails because it doesn't establish
                // the same native print-flag state that the checkbox version relies on.
                // If this fixes NbLabels=0, the hypothesis is strongly supported.
                PXTrace.WriteInformation("[PROOF-TEST] === BEGIN USRALPRINTLABEL PROOF TEST ===");
                try
                {
                    PXTrace.WriteInformation("[PROOF-TEST] Setting UsrALPrintLabel = true on package line {0}", packageLineNbr);
                    
                    // Set the print flag to true in cache
                    graph.Packages.Cache.SetValue(selectedPackage, "UsrALPrintLabel", true);
                    graph.Packages.Cache.Update(selectedPackage);
                    
                    // Save the change to establish the native state
                    graph.Actions.PressSave();
                    
                    PXTrace.WriteInformation("[PROOF-TEST] UsrALPrintLabel set and saved on package line {0}", packageLineNbr);
                    PXTrace.WriteInformation("[PROOF-TEST] Package now has the same print-eligible state the checkbox version establishes");
                }
                catch (Exception proofEx)
                {
                    PXTrace.WriteInformation("[PROOF-TEST] ⚠️ Error setting UsrALPrintLabel: {0}", proofEx.Message);
                    PXTrace.WriteInformation("[PROOF-TEST] Proceeding anyway - will show if this was the blocker");
                }
                PXTrace.WriteInformation("[PROOF-TEST] === END USRALPRINTLABEL PROOF TEST ===");

                // ✅ CRITICAL: Activate scope FIRST, THEN load the view so filter sees active scope
                using (ALPackagesFilterScope.Activate(shipmentNbr, new int?[] { packageLineNbr }))
                {
                    PXTrace.WriteInformation("[LONGOP] Filter scope activated for shipment {0}, package {1}", shipmentNbr, packageLineNbr);

                    // ✅ NOW load the ALPackages view with scope active - filter will see it!
                    var alPackagesData = graph.Views["ALPackages"].SelectMultiBound(new object[] { shipmentInLongOp });
                    PXTrace.WriteInformation("[LONGOP] ALPackages view loaded with {0} packages", alPackagesData.Count());

                    // Call service with scope active AND Packages.Current set AND UsrALPrintLabel = true
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
                        $"Successfully printed {results.NbLabels} label(s) using row-selection print for shipment {shipmentInLongOp.ShipmentNbr}, package line {packageLineNbr}.");
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