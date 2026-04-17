using System;
using PX.Data;
using PX.Objects.SO;

namespace AA.Objects.AL.Integration.PerPackage
{
    public class AsgardLabelService
    {
        private readonly SOShipmentEntry _graph;
        private readonly ILabelGenerator _labelGenerator;

        public AsgardLabelService(
            SOShipmentEntry graph,
            ILabelGenerator labelGenerator)
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

        public virtual ALModel GetModelById(Guid? modelId)
        {
            if (modelId == null || modelId == Guid.Empty)
                return null;

            return PXSelect<
                ALModel,
                Where<ALModel.labelID, Equal<Required<ALModel.labelID>>>>
                .Select(_graph, modelId);
        }

        public virtual Guid? ResolveModelId(string modelName, bool preferBoxPrintModel)
        {
            if (preferBoxPrintModel)
            {
                Guid? boxPrintModelId = ALSetupSlot.BoxPrintModelID;

                if (boxPrintModelId != null && boxPrintModelId != Guid.Empty)
                {
                    PXTrace.WriteInformation(
                        $"Per-package print: using ALSetupSlot.BoxPrintModelID = {boxPrintModelId}");

                    return boxPrintModelId;
                }

                PXTrace.WriteInformation(
                    "Per-package print: ALSetupSlot.BoxPrintModelID is empty, falling back to model name lookup.");
            }

            Guid? modelIdByName = GetModelIdByName(modelName);

            PXTrace.WriteInformation(
                $"Per-package print: resolved model '{modelName}' to ModelID = {modelIdByName}");

            return modelIdByName;
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
            {
                throw new PXException(
                    $"The selected label model must belong to the Shipments screen (SO302000). Current ScreenID: '{model.ScreenID}'.");
            }

            if (!string.Equals(model.BasedOnView, "ALPackages", StringComparison.OrdinalIgnoreCase))
            {
                throw new PXException(
                    $"The selected label model must be based on the ALPackages view for 2024 package printing. Current BasedOnView: '{model.BasedOnView}'.");
            }

            Models.Model slotModel;
            Models.TryGetModelByID(modelId, out slotModel);

            if (slotModel == null)
            {
                throw new PXException(
                    $"The selected label model '{model.Name}' was not found in the Asgard model slot cache.");
            }
        }

        public virtual void TraceModelDiagnostics(ALModel model, Guid? modelId, SOShipment shipment, SOPackageDetail package)
        {
            string modelName = model != null ? model.Name : "<null>";
            string screenId = model != null ? model.ScreenID : "<null>";
            string basedOnView = model != null ? model.BasedOnView : "<null>";
            string shipmentNbr = shipment != null ? shipment.ShipmentNbr : "<null>";
            string packageLineNbr = package?.LineNbr?.ToString() ?? "<null>";

            PXTrace.WriteInformation(
                $"Per-package print diagnostics: Shipment={shipmentNbr}, PackageLine={packageLineNbr}, ModelID={modelId}, ModelName={modelName}, ScreenID={screenId}, BasedOnView={basedOnView}, GraphType={_graph.GetType().FullName}");
        }

        public virtual PrintResults PrintAsgardLabelForPackage(
            SOShipment shipment,
            SOPackageDetail package,
            Guid? modelId)
        {
            ValidatePackageForAsgardPrint(shipment, package);

            ALModel model = GetModelById(modelId);
            ValidateModelForPackagePrinting(model, modelId);
            TraceModelDiagnostics(model, modelId, shipment, package);

            try
            {
                LabelContext printContext = LabelContext.CreateSingleRowPrintContext(
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
                    $"Per-package print context ready: Model={printContext.Model.Name}, Printer={printContext.Printer.Name}, Shipment={shipment.ShipmentNbr}, PackageLine={package.LineNbr}");

                PrintResults results = _labelGenerator.PrintLabels(printContext);

                if (results == null)
                    throw new PXException("PrintLabels returned null.");

                PXTrace.WriteInformation(
                    $"Per-package print finished: Shipment={shipment.ShipmentNbr}, PackageLine={package.LineNbr}, NbLabels={results.NbLabels}");

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