using System;
using System.Collections;
using AA.Objects.License;
using Asgard.Labels.Abstractions.Interface;
using Asgard.Labels.Abstractions.Service;
using Asgard.Labels.Impl.Poco;
using PX.Data;
using PX.Objects.SO;

namespace AA.Objects.Labels.Integration.PerPackage
{
    /// <summary>
    /// Shipment screen extension that lets the user choose an Asgard model at runtime
    /// and print it for the selected package only.
    /// </summary>
    public class SOShipmentEntry_AsgardExt : PXGraphExtension<SOShipmentEntry>
    {
        /// <summary>
        /// Keep activation simple.
        /// Do not require BoxPrintModelID because the model is user-selected in a popup.
        /// </summary>
        public static bool IsActive()
        {
            return ALSetupSlot.IsActive(typeof(SOShipmentEntry));
        }

        [InjectDependency]
        private IALLicenseManager _licenseManager { get; set; }

        [InjectDependency]
        private ILabelGenerator<IAcuLabelContext> _labelGenerator { get; set; }

        [InjectDependency]
        private IModelProvider _modelProvider { get; set; }

        /// <summary>
        /// Popup filter used to choose the model at runtime.
        /// </summary>
        public PXFilter<AsgardPackagePrintFilter> PackagePrintFilter;

        public PXAction<SOShipment> PrintAsgardPackageLabel;

        [PXButton(CommitChanges = true)]
        [PXUIField(DisplayName = "Print Asgard Label", Visible = true, Enabled = true)]
        protected virtual IEnumerable printAsgardPackageLabel(PXAdapter adapter)
        {
            SOShipment shipment = Base.Document.Current;
            SOPackageDetailEx package = Base.Packages.Current;

            if (shipment == null)
                throw new PXException("No shipment is currently selected.");

            if (package == null)
                throw new PXException("Please select a package from the Packages tab before printing.");

            // Reset popup state each time so the user explicitly chooses a model.
            PackagePrintFilter.Cache.Clear();
            PackagePrintFilter.Cache.ClearQueryCache();
            AsgardPackagePrintFilter filter = PackagePrintFilter.Insert(new AsgardPackagePrintFilter());

            WebDialogResult dialogResult = PackagePrintFilter.AskExt(
                (graph, viewName) => { },
                "Choose Asgard Label Model");

            if (dialogResult != WebDialogResult.OK)
                return adapter.Get();

            filter = PackagePrintFilter.Current;
            if (filter?.SelectedModelID == null || filter.SelectedModelID == Guid.Empty)
                throw new PXException("Please choose an Asgard label model.");

            try
            {
                var asgardService = new AsgardLabelService(
                    Base,
                    _licenseManager,
                    _labelGenerator,
                    _modelProvider);

                asgardService.ValidatePackageForAsgardPrint(shipment, package);

                PrintResults results = asgardService.PrintAsgardLabelForPackage(
                    shipment,
                    package,
                    filter.SelectedModelID);

                if (results == null)
                    throw new PXException("Label printing returned no results.");

                if (results.NbLabels <= 0)
                {
                    throw new PXException(
                        "No labels were generated. Please verify the selected label model is configured correctly.");
                }
            }
            catch (PXException)
            {
                throw;
            }
            catch (Exception ex)
            {
                PXTrace.WriteError(ex);
                throw new PXException(
                    $"An error occurred while printing the Asgard label: {ex.Message}",
                    ex);
            }

            return adapter.Get();
        }

        protected virtual void _(Events.RowSelected<SOShipment> e)
        {
            if (e.Row == null)
            {
                PrintAsgardPackageLabel.SetVisible(false);
                PrintAsgardPackageLabel.SetEnabled(false);
                return;
            }

            PrintAsgardPackageLabel.SetVisible(true);
            PrintAsgardPackageLabel.SetEnabled(e.Row.ShipmentNbr != null);
        }

        protected virtual void _(Events.RowSelected<SOPackageDetailEx> e)
        {
            if (e.Row == null)
            {
                PrintAsgardPackageLabel.SetEnabled(false);
                return;
            }

            PrintAsgardPackageLabel.SetEnabled(e.Row.LineNbr != null);
        }
    }
}