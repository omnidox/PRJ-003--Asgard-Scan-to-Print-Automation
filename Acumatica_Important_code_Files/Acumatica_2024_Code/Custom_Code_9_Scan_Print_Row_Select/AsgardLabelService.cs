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

            SOPackageDetailEx packageToVerify = PXSelect<
                SOPackageDetailEx,
                Where<
                    SOPackageDetailEx.shipmentNbr, Equal<Required<SOPackageDetailEx.shipmentNbr>>,
                    And<SOPackageDetailEx.lineNbr, Equal<Required<SOPackageDetailEx.lineNbr>>>>>
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

            // ✅ PATH C: Set SingleRow to bypass view resolution
            // BasicLabelGenerator.PrintLabelInternal() checks SingleRow first. If populated, it uses that directly.
            // Otherwise it falls back to ViewUtils.GetViewDefinition(graph, model.BasedOnView)
            //
            // CRITICAL: SingleRow must match the row type of BasedOnView (ALPackages)
            // The model's BasedOnView determines the expected row shape. If BasedOnView=ALPackages,
            // SingleRow must be SOPackageDetailEx (matching what ALPackages yields), not SOPackageDetail.
            // Mismatched row shapes cause AsgardUtils.GetAsResultset() to wrap incorrectly,
            // leading to NullReferenceException in ParseAndPrintMultiple -> PXResult.UnwrapMain(obj)
            PXTrace.WriteInformation("[SERVICE] === PATH C: SingleRow Population ===");
            
            try
            {
                // Query the selected package as SOPackageDetailEx to match ALPackages row type
                PXResultset<SOPackageDetailEx> packageResult = PXSelect<SOPackageDetailEx,
                    Where<SOPackageDetailEx.shipmentNbr, Equal<Required<SOPackageDetailEx.shipmentNbr>>,
                    And<SOPackageDetailEx.lineNbr, Equal<Required<SOPackageDetailEx.lineNbr>>>>>
                    .Select(_graph, shipment.ShipmentNbr, selectedPackageLineNbr);

                if (packageResult != null && packageResult.Count > 0)
                {
                    // ✅ Extract the plain DAC from the PXResult wrapper
                    // packageResult[0] is a PXResult<SOPackageDetailEx>, we need the plain SOPackageDetailEx
                    PXResult<SOPackageDetailEx> wrappedRow = packageResult[0];
                    SOPackageDetailEx selectedPackageRow = (SOPackageDetailEx)wrappedRow;
                    
                    printContext.SingleRow = selectedPackageRow;
                    PXTrace.WriteInformation("[SERVICE] ✅ Set printContext.SingleRow to selected package row (type: {0})", 
                        selectedPackageRow.GetType().Name);
                    PXTrace.WriteInformation("[SERVICE] Row type matches BasedOnView (ALPackages) - GetAsResultset will wrap correctly");
                }
                else
                {
                    PXTrace.WriteInformation("[SERVICE] ⚠️ Could not populate SingleRow - package query returned empty");
                }
            }
            catch (Exception ex)
            {
                PXTrace.WriteInformation("[SERVICE] ⚠️ Error populating SingleRow: {0}", ex.Message);
            }

            // ✅ DIAGNOSTIC: Trace context state before PrintLabels
            PXTrace.WriteInformation("[SERVICE] === CONTEXT STATE (BEFORE PRINTLABELS) ===");
            PXTrace.WriteInformation("[SERVICE] printContext.SingleRow: {0}", 
                printContext.SingleRow != null ? printContext.SingleRow.GetType().Name : "null");
            PXTrace.WriteInformation("[SERVICE] printContext.Row (shipment): {0}", 
                printContext.Row != null ? printContext.Row.GetType().Name : "null");
            PXTrace.WriteInformation("[SERVICE] printContext.Model.BasedOnView: {0}", printContext.Model.BasedOnView);
            PXTrace.WriteInformation("[SERVICE] printContext.IsDesignMode: {0}", printContext.IsDesignMode);
            PXTrace.WriteInformation("[SERVICE] === END CONTEXT STATE ===");

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