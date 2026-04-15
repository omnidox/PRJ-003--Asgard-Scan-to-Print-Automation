using System;
using AA.Objects.License;
using Asgard.Labels.Abstractions.Interface;
using Asgard.Labels.Abstractions.Service;
using Asgard.Labels.Impl.Context;
using Asgard.Labels.Impl.Poco;
using PX.Data;
using PX.Objects.SO;

namespace AA.Objects.Labels.Integration.PerPackage
{
    /// <summary>
    /// Service for generating and printing Asgard labels on a per-package basis.
    /// </summary>
    public class AsgardLabelService
    {
        private readonly SOShipmentEntry _graph;
        private readonly IALLicenseManager _licenseManager;
        private readonly ILabelGenerator<IAcuLabelContext> _labelGenerator;
        private readonly IModelProvider _modelProvider;

        public AsgardLabelService(
            SOShipmentEntry graph,
            IALLicenseManager licenseManager,
            ILabelGenerator<IAcuLabelContext> labelGenerator,
            IModelProvider modelProvider)
        {
            _graph = graph ?? throw new ArgumentNullException(nameof(graph));
            _licenseManager = licenseManager ?? throw new ArgumentNullException(nameof(licenseManager));
            _labelGenerator = labelGenerator ?? throw new ArgumentNullException(nameof(labelGenerator));
            _modelProvider = modelProvider ?? throw new ArgumentNullException(nameof(modelProvider));
        }

        /// <summary>
        /// Validates that the shipment and package are ready for printing.
        /// </summary>
        public virtual void ValidatePackageForAsgardPrint(SOShipment shipment, SOPackageDetailEx package)
        {
            if (shipment == null)
                throw new PXException("No shipment is currently selected.");

            if (package == null)
                throw new PXException("No package is currently selected on the Packages tab.");

            if (package.LineNbr == null)
                throw new PXException("The selected package does not have a valid line number.");

            if (string.IsNullOrWhiteSpace(shipment.ShipmentNbr))
                throw new PXException("Shipment does not have a valid shipment number.");

            if (shipment.CustomerID == null)
                throw new PXException("Shipment does not have a valid customer ID.");
        }

        /// <summary>
        /// Validates that the chosen model exists and looks usable.
        /// </summary>
        public virtual void ValidateModelForPackagePrinting(IModel model, Guid? modelId)
        {
            if (modelId == null || modelId == Guid.Empty)
                throw new PXException("Please choose an Asgard label model.");

            if (model == null)
                throw new PXException(
                    $"The selected label model (ID: {modelId}) could not be found.");

            if (string.IsNullOrWhiteSpace(model.Name))
                throw new PXException(
                    $"The selected label model (ID: {modelId}) does not have a valid name.");

            // Lightweight safeguards.
            // You can tighten these later if needed.
            if (string.IsNullOrWhiteSpace(model.ScreenID))
                throw new PXException("The selected label model is not tied to a screen.");

            if (!string.Equals(model.ScreenID, ACConstants.ScreenIDs.Shipments, StringComparison.OrdinalIgnoreCase))
                throw new PXException("The selected label model must belong to the Shipments screen.");

            if (string.IsNullOrWhiteSpace(model.BasedOnView) ||
                model.BasedOnView.IndexOf(ALConstants.ViewNames.Packages, StringComparison.OrdinalIgnoreCase) < 0)
            {
                throw new PXException("The selected label model must be based on the Packages view.");
            }
        }

        /// <summary>
        /// Prints the selected Asgard model for the selected package only.
        /// </summary>
        public virtual PrintResults PrintAsgardLabelForPackage(
            SOShipment shipment,
            SOPackageDetailEx package,
            Guid? modelId)
        {
            ValidatePackageForAsgardPrint(shipment, package);

            IModel model = _modelProvider.GetModel(modelId);
            ValidateModelForPackagePrinting(model, modelId);

            try
            {
                AcuLabelContext printContext = AcuLabelContext.CreateSingleRowPrintContext(
                    typeof(SOShipmentEntry),
                    shipment,
                    package,
                    modelId,
                    shipment.CustomerID);

                printContext.IsSilent = true;

                PrintResults results = _labelGenerator.PrintLabels(printContext);

                if (results == null)
                    throw new PXException("Label generator returned null results.");

                if (results.NbLabels > 0)
                {
                    _licenseManager.UpdateFeatureConsumption(typeof(SOShipmentEntry), results.NbLabels);
                }

                return results;
            }
            catch (PXException)
            {
                throw;
            }
            catch (Exception ex)
            {
                PXTrace.WriteError(ex);
                throw new PXException(
                    $"An error occurred while generating the Asgard label for package line {package.LineNbr}: {ex.Message}",
                    ex);
            }
        }

        /// <summary>
        /// Keeps button visibility simple.
        /// We only require that Asgard integration for Shipment Entry is active.
        /// We do NOT require BoxPrintModelID, because the user will choose the model at runtime.
        /// </summary>
        public virtual bool IsAsgardPerPackagePrintingEnabled()
        {
            return ALSetupSlot.IsActive(typeof(SOShipmentEntry));
        }
    }
}