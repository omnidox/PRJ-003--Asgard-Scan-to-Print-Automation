using System;
using PX.Data;
using PX.Objects.SO;
using AA.Objects.AL;
using AA.Objects.AL.Integration;

namespace AA.Objects.AL.Integration.PerPackage
{
    public class AsgardLabelService
    {
        private const string PackagePrintFlagField = "UsrALPrintLabel";

        private readonly SOShipmentEntry _graph;
        private readonly ILabelGenerator _labelGenerator;

        public AsgardLabelService(
            SOShipmentEntry graph,
            ILabelGenerator labelGenerator)
        {
            _graph = graph ?? throw new ArgumentNullException(nameof(graph));
            _labelGenerator = labelGenerator ?? throw new ArgumentNullException(nameof(labelGenerator));
        }

        public virtual void ValidateShipmentForAsgardPrint(SOShipment shipment)
        {
            if (shipment == null)
                throw new PXException("No shipment is currently selected.");

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
                        $"Filtered-context print: using ALSetupSlot.BoxPrintModelID = {boxPrintModelId}");

                    return boxPrintModelId;
                }

                PXTrace.WriteInformation(
                    "Filtered-context print: ALSetupSlot.BoxPrintModelID is empty, falling back to model name lookup.");
            }

            Guid? modelIdByName = GetModelIdByName(modelName);

            PXTrace.WriteInformation(
                $"Filtered-context print: resolved model '{modelName}' to ModelID = {modelIdByName}");

            return modelIdByName;
        }

        public virtual void ValidateModelForPackageFilteredPrinting(ALModel model, Guid? modelId)
        {
            if (modelId == null || modelId == Guid.Empty)
                throw new PXException("Please choose a valid Asgard label model.");

            if (model == null)
            {
                throw new PXException(
                    $"The selected label model (ID: {modelId}) could not be found.");
            }

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                throw new PXException(
                    $"The selected label model (ID: {modelId}) does not have a valid name.");
            }

            if (string.IsNullOrWhiteSpace(model.ScreenID))
                throw new PXException("The selected label model is not tied to a screen.");

            if (!string.Equals(model.ScreenID, "SO302000", StringComparison.OrdinalIgnoreCase))
            {
                throw new PXException(
                    $"The selected label model must belong to the Shipments screen (SO302000). Current ScreenID: '{model.ScreenID}'.");
            }

            if (!string.Equals(model.ModelType, "S", StringComparison.OrdinalIgnoreCase))
            {
                throw new PXException(
                    $"The selected label model '{model.Name}' is not a single label model. Current ModelType: '{model.ModelType}'.");
            }

            if (!string.Equals(model.BasedOnView, "ALPackages", StringComparison.OrdinalIgnoreCase))
            {
                throw new PXException(
                    $"The selected label model '{model.Name}' is not package-based. Expected BasedOnView = 'ALPackages', but found '{model.BasedOnView}'.");
            }

            Models.Model slotModel;
            Models.TryGetModelByID(modelId, out slotModel);

            if (slotModel == null)
            {
                throw new PXException(
                    $"The selected label model '{model.Name}' was not found in the Asgard model slot cache.");
            }
        }

        public virtual void TraceModelDiagnostics(ALModel model, Guid? modelId, SOShipment shipment)
        {
            string modelName = model != null ? model.Name : "<null>";
            string screenId = model != null ? model.ScreenID : "<null>";
            string basedOnView = model != null ? model.BasedOnView : "<null>";
            string shipmentNbr = shipment != null ? shipment.ShipmentNbr : "<null>";

            PXTrace.WriteInformation(
                $"Filtered-context print diagnostics: Shipment={shipmentNbr}, ModelID={modelId}, ModelName={modelName}, ScreenID={screenId}, BasedOnView={basedOnView}, GraphType={_graph.GetType().FullName}");
        }

        public virtual PrintResults PrintSelectedPackageLabelsMenuStyle(
            SOShipment shipment,
            Guid? modelId,
            PXAdapter adapter)
        {
            ValidateShipmentForAsgardPrint(shipment);

            _graph.Document.Current = shipment;

            ALModel model = GetModelById(modelId);
            ValidateModelForPackageFilteredPrinting(model, modelId);
            TraceModelDiagnostics(model, modelId, shipment);

            try
            {
                LabelContext printContext = LabelContext.CreatePrintContext(
                    _graph.GetType(),
                    shipment,
                    modelId,
                    false,
                    adapter);

                if (printContext == null)
                    throw new PXException("CreatePrintContext returned null.");

                printContext.IsSilent = true;

                if (printContext.Model == null)
                    throw new PXException("printContext.Model is null.");

                if (printContext.Row == null)
                    throw new PXException("printContext.Row is null.");

                if (printContext.Printer == null)
                {
                    throw new PXException(
                        "No printer is configured for this model. Please configure a printer for the model or printer override as needed.");
                }

                PXResultset<SOPackageDetail> selectedPackageRows = GetSelectedPackageRows(shipment);

                if (selectedPackageRows == null || selectedPackageRows.Count == 0)
                {
                    throw new PXException(
                        "No packages are marked for Asgard printing. Please check the Print Label box on at least one package and save the shipment before printing.");
                }

                printContext.DetailRows = selectedPackageRows;

                PXTrace.WriteInformation(
                    $"Filtered-context print context ready: Model={printContext.Model.Name}, Printer={printContext.Printer.Name}, Shipment={shipment.ShipmentNbr}, SelectedPackageCount={selectedPackageRows.Count}");

                PrintResults results = _labelGenerator.PrintLabels(printContext);

                if (results == null)
                    throw new PXException("PrintLabels returned null.");

                PXTrace.WriteInformation(
                    $"Filtered-context print finished: Shipment={shipment.ShipmentNbr}, SelectedPackageCount={selectedPackageRows.Count}, NbLabels={results.NbLabels}");

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
                    $"An error occurred while generating filtered package labels for shipment {shipment.ShipmentNbr}: {ex.Message}",
                    ex);
            }
        }

        protected virtual PXResultset<SOPackageDetail> GetSelectedPackageRows(SOShipment shipment)
        {
            if (shipment == null)
                throw new PXException("Shipment cannot be null while gathering selected packages.");

            _graph.Document.Current = shipment;

            ALSOShipmentEntryExt asgardShipmentExt = _graph.GetExtension<ALSOShipmentEntryExt>();
            if (asgardShipmentExt?.ALPackages == null)
            {
                throw new PXException(
                    "Could not access Asgard's ALPackages view on SOShipmentEntry.");
            }

            PXResultset<SOPackageDetail> selectedRows = new PXResultset<SOPackageDetail>();

            foreach (object result in asgardShipmentExt.ALPackages.Select())
            {
                SOPackageDetail package = PXResult.Unwrap<SOPackageDetail>(result);
                if (package == null)
                    continue;

                bool isMarkedForPrint = IsPackageMarkedForPrint(package);
                if (!isMarkedForPrint)
                    continue;

                selectedRows.Add(result);
            }

            PXTrace.WriteInformation(
                $"Filtered-context print: found {selectedRows.Count} selected package row(s) in ALPackages for shipment {shipment.ShipmentNbr}.");

            return selectedRows;
        }

        protected virtual bool IsPackageMarkedForPrint(SOPackageDetail package)
        {
            if (package == null)
                return false;

            object value = _graph.Packages.Cache.GetValue(package, PackagePrintFlagField);
            if (value == null)
                return false;

            return value is bool boolValue && boolValue;
        }
    }
}