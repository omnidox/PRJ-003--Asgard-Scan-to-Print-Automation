using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PX.Data;
using PX.Objects.SO;

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

                if (printContext.DetailRows == null)
                {
                    throw new PXException(
                        "CreatePrintContext did not populate DetailRows. The selected model may not be package-based or the shipment may not have package detail rows.");
                }

                int originalCount;
                IPXResultset filteredRows = FilterDetailRowsToSelectedPackages(printContext.DetailRows, out originalCount);

                if (filteredRows == null)
                    throw new PXException("Filtering the package detail rows returned null.");

                int filteredCount = CountRows(filteredRows);
                if (filteredCount <= 0)
                {
                    throw new PXException(
                        "No packages are marked for Asgard printing. Please check the Print Label box on at least one package and save the shipment before printing.");
                }

                printContext.DetailRows = filteredRows;

                PXTrace.WriteInformation(
                    $"Filtered-context print context ready: Model={printContext.Model.Name}, Printer={printContext.Printer.Name}, Shipment={shipment.ShipmentNbr}, OriginalPackageCount={originalCount}, SelectedPackageCount={filteredCount}");

                PrintResults results = _labelGenerator.PrintLabels(printContext);

                if (results == null)
                    throw new PXException("PrintLabels returned null.");

                PXTrace.WriteInformation(
                    $"Filtered-context print finished: Shipment={shipment.ShipmentNbr}, SelectedPackageCount={filteredCount}, NbLabels={results.NbLabels}");

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

        protected virtual IPXResultset FilterDetailRowsToSelectedPackages(IPXResultset detailRows, out int originalCount)
        {
            if (detailRows == null)
                throw new PXException("detailRows cannot be null.");

            IEnumerable enumerableRows = detailRows as IEnumerable;
            if (enumerableRows == null)
                throw new PXException("detailRows does not implement IEnumerable, so it cannot be filtered.");

            List<object> selectedRows = new List<object>();
            originalCount = 0;

            foreach (object row in enumerableRows)
            {
                originalCount++;

                SOPackageDetail package = PXResult.Unwrap<SOPackageDetail>(row);
                if (package == null)
                    continue;

                if (!IsPackageMarkedForPrint(package))
                    continue;

                selectedRows.Add(row);
            }

            PXTrace.WriteInformation(
                $"Filtered-context print: found {selectedRows.Count} selected package row(s) out of {originalCount} detail row(s).");

            object filteredResultsetObject = Activator.CreateInstance(detailRows.GetType());
            if (filteredResultsetObject == null)
                throw new PXException($"Could not create a filtered resultset instance of type '{detailRows.GetType().FullName}'.");

            foreach (object selectedRow in selectedRows)
            {
                AddRowToResultset(filteredResultsetObject, selectedRow);
            }

            IPXResultset filteredResultset = filteredResultsetObject as IPXResultset;
            if (filteredResultset == null)
            {
                throw new PXException(
                    $"The filtered resultset instance of type '{detailRows.GetType().FullName}' does not implement IPXResultset.");
            }

            return filteredResultset;
        }

        protected virtual void AddRowToResultset(object resultset, object row)
        {
            if (resultset == null)
                throw new ArgumentNullException(nameof(resultset));

            if (row == null)
                throw new ArgumentNullException(nameof(row));

            MethodInfo addMethod = resultset
                .GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .FirstOrDefault(m =>
                {
                    if (!string.Equals(m.Name, "Add", StringComparison.Ordinal))
                        return false;

                    ParameterInfo[] parameters = m.GetParameters();
                    return parameters.Length == 1 && parameters[0].ParameterType.IsAssignableFrom(row.GetType());
                });

            if (addMethod == null)
            {
                throw new PXException(
                    $"Could not find a compatible Add method on resultset type '{resultset.GetType().FullName}' for row type '{row.GetType().FullName}'.");
            }

            addMethod.Invoke(resultset, new[] { row });
        }

        protected virtual int CountRows(IPXResultset rows)
        {
            if (rows == null)
                return 0;

            IEnumerable enumerableRows = rows as IEnumerable;
            if (enumerableRows == null)
                return 0;

            int count = 0;
            foreach (object row in enumerableRows)
            {
                count++;
            }

            return count;
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