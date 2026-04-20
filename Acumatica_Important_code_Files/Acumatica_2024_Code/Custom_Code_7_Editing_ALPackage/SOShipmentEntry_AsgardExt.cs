using System;
using System.Collections;
using PX.Data;
using PX.Data.DependencyInjection;
using PX.Objects.SO;
using AA.Objects.AL.License;

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

        [InjectDependency]
        private IALLicenseManager _licenseManager { get; set; }

        public PXAction<SOShipment> PrintAsgardPackageLabel;

        [PXButton(CommitChanges = true)]
        [PXUIField(DisplayName = "Print Asgard Label", Visible = true, Enabled = true)]
        protected virtual IEnumerable printAsgardPackageLabel(PXAdapter adapter)
        {
            SOShipment shipment = Base.Document.Current;

            if (shipment == null)
                throw new PXException("No shipment is currently selected.");

            if (Base.IsDirty)
            {
                Base.Actions.PressSave();
                shipment = Base.Document.Current;
            }

            string shipmentNbr = shipment.ShipmentNbr;

            PXLongOperation.StartOperation(Base, delegate()
            {
                SOShipmentEntry graph = PXGraph.CreateInstance<SOShipmentEntry>();

                SOShipment shipmentInLongOp = SOShipment.PK.Find(graph, shipmentNbr);
                if (shipmentInLongOp == null)
                {
                    throw new PXException(
                        $"Shipment '{shipmentNbr}' could not be reloaded inside the long operation.");
                }

                graph.Document.Current = shipmentInLongOp;

                var asgardService = new AsgardLabelService(graph, _labelGenerator, _licenseManager);

                const bool preferBoxPrintModel = true;
                const string fallbackModelName = "istar_test_label";

                Guid? modelId = asgardService.ResolveModelId(fallbackModelName, preferBoxPrintModel);

                if (modelId == null || modelId == Guid.Empty)
                {
                    throw new PXException(
                        "Could not resolve an Asgard label model for diagnostic selected-package printing. " +
                        "Please verify ALSetupSlot.BoxPrintModelID or the fallback model name.");
                }

                ALModel resolvedModel = asgardService.GetModelById(modelId);
                asgardService.TraceModelDiagnostics(resolvedModel, modelId, shipmentInLongOp);

                PackagePrintSummary summary = asgardService.TestFirstCheckedALPackagesRow(
                    shipmentInLongOp,
                    modelId);

                if (summary == null)
                    throw new PXException("Diagnostic label printing returned no summary.");

                PXTrace.WriteInformation(
                    $"Diagnostic print summary: SelectedPackageCount={summary.SelectedPackageCount}, LabelsPrinted={summary.LabelsPrinted}, Shipment={shipmentInLongOp.ShipmentNbr}");
            });

            return adapter.Get();
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