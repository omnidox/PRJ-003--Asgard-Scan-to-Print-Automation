using System;
using System.Collections;
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
            SOShipment shipment = Base.Document.Current;
            SOPackageDetail package = Base.Packages.Current;

            if (shipment == null)
                throw new PXException("No shipment is currently selected.");

            if (package == null)
            {
                PXTrace.WriteInformation(
                    "Menu-style test: no package selected. Continuing anyway because CreatePrintContext uses the shipment row, not the selected package row.");
            }

            try
            {
                PXCache shipmentCache = Base.Document.Cache;
                if (shipmentCache != null &&
                    shipmentCache.IsDirty &&
                    shipmentCache.AllowUpdate &&
                    !adapter.ExternalCall)
                {
                    Base.Actions.PressSave();
                    shipment = Base.Document.Current;
                    package = Base.Packages.Current;
                }

                var asgardService = new AsgardLabelService(Base, _labelGenerator);

                // Toggle this to true when you want to test using the built-in 2024 box print model.
                const bool preferBoxPrintModel = true;

                // Used only when preferBoxPrintModel = false,
                // or when BoxPrintModelID is empty and fallback is needed.
                const string fallbackModelName = "istar_test_label";

                Guid? modelId = asgardService.ResolveModelId(fallbackModelName, preferBoxPrintModel);

                if (modelId == null || modelId == Guid.Empty)
                {
                    throw new PXException(
                        "Could not resolve an Asgard label model for menu-style printing. " +
                        "Please verify ALSetupSlot.BoxPrintModelID or the fallback model name.");
                }

                ALModel resolvedModel = asgardService.GetModelById(modelId);
                asgardService.TraceModelDiagnostics(resolvedModel, modelId, shipment, package);

                PrintResults results = asgardService.PrintAsgardLabelForShipmentMenuStyle(
                    shipment,
                    package,
                    modelId,
                    adapter);

                if (results == null)
                    throw new PXException("Label printing returned no results.");

                if (results.NbLabels <= 0)
                {
                    throw new PXException(
                        "No labels were generated. Please verify the selected label model is configured correctly.");
                }

                PXTrace.WriteInformation(
                    $"Successfully printed {results.NbLabels} label(s) using menu-style context for shipment {shipment.ShipmentNbr}. Selected package line was {(package != null ? package.LineNbr.ToString() : "<none>")}.");
            }
            catch (PXException)
            {
                throw;
            }
            catch (Exception ex)
            {
                PXTrace.WriteError(ex);
                throw new PXException(
                    $"An error occurred while printing the Asgard label using menu-style context: {ex}",
                    ex);
            }

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
                // Keep enabled because this experiment is shipment/menu-context based.
                PrintAsgardPackageLabel.SetEnabled(Base.Document.Current != null &&
                                                  !string.IsNullOrWhiteSpace(Base.Document.Current.ShipmentNbr));
                return;
            }

            PrintAsgardPackageLabel.SetEnabled(true);
        }
    }
}