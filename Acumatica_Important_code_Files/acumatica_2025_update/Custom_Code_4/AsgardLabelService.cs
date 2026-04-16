using System;
using PX.Data;
using PX.Objects.SO;
using Asgard.Labels.Abstractions.Interface;
using Asgard.Labels.Abstractions.Service;
using Asgard.Labels.Impl;
using Asgard.Labels.Impl.Context;
using Asgard.Labels.Impl.Poco;

namespace AA.Objects.Labels.Integration.PerPackage
{
    public class AsgardLabelService
    {
        private readonly SOShipmentEntry _graph;
        private readonly ILabelGenerator<IAcuLabelContext> _labelGenerator;

        public AsgardLabelService(
            SOShipmentEntry graph,
            ILabelGenerator<IAcuLabelContext> labelGenerator)
        {
            _graph = graph ?? throw new ArgumentNullException(nameof(graph));
            _labelGenerator = labelGenerator ?? throw new ArgumentNullException(nameof(labelGenerator));
        }

        public virtual void ValidatePackageForAsgardPrint(SOShipment shipment, SOPackageDetail package)
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

        public virtual Guid? GetModelIdByName(string modelName)
        {
            if (string.IsNullOrWhiteSpace(modelName))
                throw new PXException("Model name cannot be empty.");

            ALModel model = PXSelect<
                ALModel,
                Where<ALModel.name, Equal<Required<ALModel.name>>>>
                .Select(_graph, modelName);

            return model?.LabelID;
        }

        public virtual void ValidateModelForPackagePrinting(ALModel model, Guid? modelId)
        {
            if (modelId == null || modelId == Guid.Empty)
                throw new PXException("Please choose a valid Asgard label model.");

            if (model == null)
                throw new PXException(
                    $"The selected label model (ID: {modelId}) could not be found.");

            if (string.IsNullOrWhiteSpace(model.Name))
                throw new PXException(
                    $"The selected label model (ID: {modelId}) does not have a valid name.");

            if (string.IsNullOrWhiteSpace(model.ScreenID))
                throw new PXException("The selected label model is not tied to a screen.");

            if (!string.Equals(model.ScreenID, "SO302000", StringComparison.OrdinalIgnoreCase))
                throw new PXException("The selected label model must belong to the Shipments screen.");

            if (string.IsNullOrWhiteSpace(model.BasedOnView) ||
                model.BasedOnView.IndexOf("Packages", StringComparison.OrdinalIgnoreCase) < 0)
            {
                throw new PXException("The selected label model must be based on the Packages view.");
            }
        }

        public virtual PrintResults PrintAsgardLabelForPackage(
            SOShipment shipment,
            SOPackageDetail package,
            Guid? modelId)
        {
            ValidatePackageForAsgardPrint(shipment, package);

            ALModel model = ALModel.PK.Find(_graph, modelId);
            ValidateModelForPackagePrinting(model, modelId);

            try
            {
                PXTrace.WriteInformation(
                    $"Asgard per-package print starting. Shipment={shipment.ShipmentNbr}, PackageLine={package.LineNbr}, ModelID={modelId}");

                AcuLabelContext printContext = AcuLabelContext.CreateSingleRowPrintContext(
                    _graph.GetType(),
                    shipment,
                    package,
                    modelId,
                    shipment.CustomerID);

                if (printContext == null)
                    throw new PXException("CreateSingleRowPrintContext returned null.");

                printContext.IsSilent = true;

                if (printContext.Model == null)
                    throw new PXException("printContext.Model is null.");

                if (printContext.Row == null)
                    throw new PXException("printContext.Row is null.");

                if (printContext.SingleRow == null)
                    throw new PXException("printContext.SingleRow is null.");

                if (printContext.Printer == null)
                {
                    throw new PXException(
                        "No printer is configured for this model. Please configure a printer for the model or printer override as needed.");
                }

                PXTrace.WriteInformation(
                    $"Asgard print context ready. Model={printContext.Model?.Name}, Printer={printContext.Printer?.Name}, GraphType={_graph.GetType().FullName}");

                PrintResults results = _labelGenerator.PrintLabels(printContext);

                if (results == null)
                    throw new PXException("PrintLabels returned null.");

                PXTrace.WriteInformation(
                    $"Asgard per-package print finished. Shipment={shipment.ShipmentNbr}, PackageLine={package.LineNbr}, NbLabels={results.NbLabels}");

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
                    $"An error occurred while generating the Asgard label for package line {package.LineNbr}: {ex}",
                    ex);
            }
        }
    }
}