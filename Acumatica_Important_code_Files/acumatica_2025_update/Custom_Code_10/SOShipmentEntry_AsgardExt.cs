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

                // ✅ Fetch the selected package row
                SOPackageDetailEx selectedPackage = PXSelect<
                    SOPackageDetailEx,
                    Where<SOPackageDetailEx.shipmentNbr, Equal<Required<SOPackageDetailEx.shipmentNbr>>,
                    And<SOPackageDetailEx.lineNbr, Equal<Required<SOPackageDetailEx.lineNbr>>>>>
                    .Select(graph, shipmentNbr, packageLineNbr).FirstOrDefault();

                if (selectedPackage == null)
                {
                    throw new PXException($"Package line {packageLineNbr} not found in shipment {shipmentNbr}");
                }

                PXTrace.WriteInformation("[LONGOP] Printing package {0} using CreateSingleRowPrintContext", packageLineNbr);

                // ✅ Use Asgard's native single-row context — matches ALBoxPrintSOShipmentEntryExt pattern exactly.
                // IsAlwaysPrint=true bypasses CheckLineDoPrint, IsSilent=true suppresses the completion popup.
                // SingleRow limits the resultset to the one selected package row.
                AcuLabelContext labelContext = AcuLabelContext.CreateSingleRowPrintContext(
                    graph.GetType(),
                    shipmentInLongOp,
                    selectedPackage,
                    modelId,
                    shipmentInLongOp.CustomerID);

                PrintResults results = _labelGenerator.PrintLabels(labelContext);

                if (results == null)
                    throw new PXException("Label printing returned no results.");

                if (results.NbLabels <= 0)
                {
                    throw new PXException(
                        "No labels were generated. Please verify the selected package is valid and the selected label model is configured correctly.");
                }

                PXTrace.WriteInformation("[LONGOP] Successfully printed {0} label(s) for package line {1}", results.NbLabels, packageLineNbr);
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