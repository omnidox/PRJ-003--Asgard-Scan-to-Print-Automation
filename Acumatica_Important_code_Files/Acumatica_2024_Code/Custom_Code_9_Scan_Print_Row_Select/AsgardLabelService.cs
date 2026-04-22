using System;
using System.Collections.Generic;
using System.Linq;
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
                        $"Selected-package native print: using ALSetupSlot.BoxPrintModelID = {boxPrintModelId}");

                    return boxPrintModelId;
                }

                PXTrace.WriteInformation(
                    "Selected-package native print: ALSetupSlot.BoxPrintModelID is empty, falling back to model name lookup.");
            }

            Guid? modelIdByName = GetModelIdByName(modelName);

            PXTrace.WriteInformation(
                $"Selected-package native print: resolved model '{modelName}' to ModelID = {modelIdByName}");

            return modelIdByName;
        }

        public virtual void ValidateModelForNativeContextPrinting(ALModel model, Guid? modelId)
        {
            if (modelId == null || modelId == Guid.Empty)
                throw new PXException("Please choose a valid Asgard label model.");

            if (model == null)
                throw new PXException($"The selected label model (ID: {modelId}) could not be found.");

            if (string.IsNullOrWhiteSpace(model.Name))
                throw new PXException($"The selected label model (ID: {modelId}) does not have a valid name.");

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
                $"Selected-package native print diagnostics: Shipment={shipmentNbr}, ModelID={modelId}, ModelName={modelName}, ScreenID={screenId}, BasedOnView={basedOnView}, GraphType={_graph.GetType().FullName}");
        }

        public virtual PrintResults PrintSelectedPackageUsingNativeContext(
            SOShipment shipment,
            Guid? modelId,
            int? selectedPackageLineNbr,
            PXAdapter adapter)
        {
            PXTrace.WriteInformation("[SERVICE] PrintSelectedPackageUsingNativeContext called - Shipment={0}, Package={1}, ModelID={2}", 
                shipment?.ShipmentNbr, selectedPackageLineNbr, modelId);

            ValidateShipmentForAsgardPrint(shipment);

            if (selectedPackageLineNbr == null)
                throw new PXException("No package line number was specified for printing.");

            ALModel model = GetModelById(modelId);
            ValidateModelForNativeContextPrinting(model, modelId);
            TraceModelDiagnostics(model, modelId, shipment);

            PXTrace.WriteInformation("[SERVICE] Verifying package {0} exists in shipment {1}", selectedPackageLineNbr, shipment.ShipmentNbr);

            SOPackageDetail packageToVerify = PXSelect<
                SOPackageDetail,
                Where<
                    SOPackageDetail.shipmentNbr, Equal<Required<SOPackageDetail.shipmentNbr>>,
                    And<SOPackageDetail.lineNbr, Equal<Required<SOPackageDetail.lineNbr>>>>>
                .Select(_graph, shipment.ShipmentNbr, selectedPackageLineNbr);

            if (packageToVerify == null)
            {
                throw new PXException(
                    $"Package line {selectedPackageLineNbr} not found in shipment {shipment.ShipmentNbr}.");
            }

            PXTrace.WriteInformation(
                $"[SERVICE] Package {selectedPackageLineNbr} verified. Graph type: {_graph.GetType().FullName}");

            PXTrace.WriteInformation(
                $"[SERVICE] Row-selection native print: shipment {shipment.ShipmentNbr} will print package line {selectedPackageLineNbr}");

            // ✅ CRITICAL: Assume filter scope is already activated by the caller (SOShipmentEntry_AsgardExt)
            // This allows the scope to remain active across the fresh graph context
            PXTrace.WriteInformation("[SERVICE] Calling CreatePrintContext with Graph={0}, ShipmentNbr={1}, ModelID={2}", 
                _graph.GetType().Name, shipment.ShipmentNbr, modelId);

            LabelContext printContext = LabelContext.CreatePrintContext(
                _graph.GetType(),
                shipment,
                modelId,
                false,
                adapter);

            if (printContext == null)
                throw new PXException("CreatePrintContext returned null.");

            PXTrace.WriteInformation("[SERVICE] CreatePrintContext succeeded. Context Model={0}, Printer={1}", 
                printContext.Model?.Name ?? "<null>", printContext.Printer?.Name ?? "<null>");

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

            PXTrace.WriteInformation(
                $"[SERVICE] Row-selection native print context ready: Model={printContext.Model.Name}, Printer={printContext.Printer.Name}, Shipment={shipment.ShipmentNbr}, Package={selectedPackageLineNbr}");

            // ✅ DIAGNOSTIC: Inspect context data structures BEFORE manual injection
            PXTrace.WriteInformation("[SERVICE] === CONTEXT DIAGNOSTICS (BEFORE INJECTION) ===");
            PXTrace.WriteInformation("[SERVICE] printContext.SingleRow: {0}", 
                printContext.SingleRow != null ? printContext.SingleRow.GetType().Name : "null");
            PXTrace.WriteInformation("[SERVICE] printContext.Row: {0}", 
                printContext.Row != null ? printContext.Row.GetType().Name : "null");
            PXTrace.WriteInformation("[SERVICE] printContext.DetailRows BEFORE injection: {0}", 
                printContext.DetailRows != null ? printContext.DetailRows.GetType().Name : "null");

            // ✅ MANUAL INJECTION: Load filtered packages and inject into context
            PXTrace.WriteInformation("[SERVICE] === MANUAL INJECTION PHASE ===");
            try
            {
                // Query the filtered ALPackages view directly from the graph
                // The scope is still active, so the filter will apply
                var alPackagesView = _graph.Views["ALPackages"];
                if (alPackagesView == null)
                {
                    PXTrace.WriteInformation("[SERVICE] WARNING: ALPackages view not found on graph");
                }
                else
                {
                    object[] currents = new object[] { shipment };
                    IEnumerable filteredPackages = alPackagesView.SelectMultiBound(currents);
                    
                    PXTrace.WriteInformation("[SERVICE] Queried ALPackages view with active filter scope");
                    
                    // Convert to IPXResultset for injection
                    IPXResultset packageResultset = AsgardUtils.GetAsResultset(filteredPackages);
                    
                    if (packageResultset != null)
                    {
                        int packageCount = packageResultset.GetRowCount();
                        PXTrace.WriteInformation("[SERVICE] Filtered package result set contains {0} packages", packageCount);
                        
                        // Inject into context DetailRows
                        printContext.DetailRows = packageResultset;
                        PXTrace.WriteInformation("[SERVICE] ✅ Injected filtered packages into printContext.DetailRows");
                    }
                    else
                    {
                        PXTrace.WriteInformation("[SERVICE] ⚠️ packageResultset is null - could not inject");
                    }
                }
            }
            catch (Exception injectionEx)
            {
                PXTrace.WriteInformation("[SERVICE] ⚠️ Error during manual injection: {0}", injectionEx.Message);
            }

            // ✅ DIAGNOSTIC: Inspect context data structures AFTER manual injection
            PXTrace.WriteInformation("[SERVICE] === CONTEXT DIAGNOSTICS (AFTER INJECTION) ===");
            PXTrace.WriteInformation("[SERVICE] printContext.DetailRows AFTER injection: {0}", 
                printContext.DetailRows != null ? printContext.DetailRows.GetType().Name : "null");
            
            if (printContext.DetailRows != null)
            {
                try
                {
                    IPXResultset detailRowsSet = printContext.DetailRows as IPXResultset;
                    int detailRowCount = detailRowsSet?.GetRowCount() ?? 0;
                    PXTrace.WriteInformation("[SERVICE] printContext.DetailRows row count AFTER injection: {0}", detailRowCount);
                    
                    // Try to read first row if available
                    if (detailRowCount > 0)
                    {
                        object firstRow = detailRowsSet?.GetItem(0, 0);
                        PXTrace.WriteInformation("[SERVICE] First DetailRow type: {0}", 
                            firstRow != null ? firstRow.GetType().Name : "null");
                    }
                }
                catch (Exception ex)
                {
                    PXTrace.WriteInformation("[SERVICE] Error inspecting DetailRows: {0}", ex.Message);
                }
            }

            PXTrace.WriteInformation("[SERVICE] printContext.Model.ModelType: {0}", printContext.Model.ModelType);
            PXTrace.WriteInformation("[SERVICE] printContext.Model.BasedOnView: {0}", printContext.Model.BasedOnView);
            PXTrace.WriteInformation("[SERVICE] printContext.IsDesignMode: {0}", printContext.IsDesignMode);
            PXTrace.WriteInformation("[SERVICE] printContext.IsRender: {0}", printContext.IsRender);
            PXTrace.WriteInformation("[SERVICE] === END CONTEXT DIAGNOSTICS ===");

            // ✅ CRITICAL: Trace immediately before PrintLabels to confirm injection persists
            PXTrace.WriteInformation("[SERVICE] ⚠️ FINAL CHECK before PrintLabels: DetailRows={0}, RowCount={1}", 
                printContext.DetailRows != null ? "POPULATED" : "NULL",
                printContext.DetailRows != null ? (printContext.DetailRows as IPXResultset)?.GetRowCount().ToString() : "N/A");

            PXTrace.WriteInformation("[SERVICE] Calling _labelGenerator.PrintLabels()");

            PrintResults results = _labelGenerator.PrintLabels(printContext);

            if (results == null)
                throw new PXException("PrintLabels returned null.");

            PXTrace.WriteInformation(
                $"[SERVICE] Row-selection native print finished: Shipment={shipment.ShipmentNbr}, Package={selectedPackageLineNbr}, NbLabels={results.NbLabels}");

            return results;
        }

        [Obsolete("Use PrintSelectedPackageUsingNativeContext instead")]
        public virtual PrintResults PrintCheckedPackagesUsingNativeContext(
            SOShipment shipment,
            Guid? modelId,
            PXAdapter adapter)
        {
            throw new PXException(
                "PrintCheckedPackagesUsingNativeContext is deprecated. Use PrintSelectedPackageUsingNativeContext with a specific package line number instead.");
        }
    }
}