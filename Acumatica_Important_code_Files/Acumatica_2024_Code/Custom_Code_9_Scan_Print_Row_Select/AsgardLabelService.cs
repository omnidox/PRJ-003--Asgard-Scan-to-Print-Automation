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

            // ✅ BEST PRACTICE TEST: Use the actual row from the filtered ALPackages view
            // ALPackages is a PXSelectJoin with joins to SOShipment, CSBox, InventoryItem, ALTemplateItem
            // So a real row from ALPackages is a joined PXResult, not a simple SOPackageDetailEx
            // This test compares: synthetic wrapping (broken) vs real view row (expected to work)
            PXTrace.WriteInformation("[SERVICE] === PATH C: SingleRow Population (ALPackages Real Row) ===");
            
            try
            {
                // ✅ DIAGNOSTIC: First, try to get the row from the actual filtered ALPackages view
                // This row should already be in the correct joined shape that Asgard expects
                PXView alPackagesView = _graph.Views["ALPackages"];
                if (alPackagesView != null)
                {
                    PXTrace.WriteInformation("[SERVICE] ALPackages view found, attempting to select first row");
                    
                    // SelectMultiBound will use the current filter scope to return only the filtered row
                    // CRITICAL: Do NOT force into PXResultset<SOPackageDetail> — preserve native row shape
                    // ALPackages is a PXSelectJoin, so the real row is a joined PXResult with multiple tables
                    var alPackagesRows = alPackagesView.SelectMultiBound(new object[] { _graph.Document.Current });
                    object alPackagesFirstRow = alPackagesRows?.Cast<object>().FirstOrDefault();
                    
                    if (alPackagesFirstRow != null)
                    {
                        PXTrace.WriteInformation("[DIAG-ALPACKAGES] ALPackages first row obtained");
                        PXTrace.WriteInformation("[DIAG-ALPACKAGES] Real row type: {0}", 
                            alPackagesFirstRow.GetType().FullName);
                        PXTrace.WriteInformation("[DIAG-ALPACKAGES] Real row is PXResult: {0}", alPackagesFirstRow is PXResult);
                        PXTrace.WriteInformation("[DIAG-ALPACKAGES] Real row is IBqlTable: {0}", alPackagesFirstRow is IBqlTable);
                        PXTrace.WriteInformation("[DIAG-ALPACKAGES] Real row is IPXResultset: {0}", alPackagesFirstRow is IPXResultset);
                        
                        // Log if it's a PXResult and what it wraps
                        if (alPackagesFirstRow is PXResult realRowResult)
                        {
                            PXTrace.WriteInformation("[DIAG-ALPACKAGES] Real row IS PXResult with type arguments: {0}", 
                                string.Join(",", realRowResult.GetType().GenericTypeArguments.Select(t => t.Name)));
                        }
                        
                        // ✅ Assign the REAL row from ALPackages to SingleRow
                        // This is the shape Asgard expects for BasedOnView=ALPackages
                        printContext.SingleRow = alPackagesFirstRow;
                        PXTrace.WriteInformation("[SERVICE] ✅ Set printContext.SingleRow to REAL ALPackages row (type: {0})", 
                            alPackagesFirstRow.GetType().Name);
                        PXTrace.WriteInformation("[SERVICE] This is the actual joined row shape that ALPackages yields (NOT forced into typed resultset)");
                    }
                    else
                    {
                        PXTrace.WriteInformation("[SERVICE] ⚠️ Could not get row from ALPackages view - result was null or empty");
                    }
                }
                else
                {
                    PXTrace.WriteInformation("[SERVICE] ⚠️ ALPackages view not found in graph");
                }
            }
            catch (Exception ex)
            {
                PXTrace.WriteInformation("[SERVICE] ⚠️ Error getting row from ALPackages view: {0}: {1}", 
                    ex.GetType().FullName, ex.Message);
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

            // ✅ DIAGNOSTIC: Deep inspection of SingleRow before GetAsResultset
            PXTrace.WriteInformation("[SERVICE] === DIAGNOSTIC: SingleRow Analysis (INPUT TO GETASRESULTSET) ===");
            try
            {
                if (printContext.SingleRow != null)
                {
                    PXTrace.WriteInformation("[DIAG] SingleRow.GetType(): {0}", printContext.SingleRow.GetType().FullName);
                    PXTrace.WriteInformation("[DIAG] SingleRow is IBqlTable: {0}", printContext.SingleRow is IBqlTable);
                    PXTrace.WriteInformation("[DIAG] SingleRow is PXResult: {0}", printContext.SingleRow is PXResult);
                    PXTrace.WriteInformation("[DIAG] SingleRow is IPXResultset: {0}", printContext.SingleRow is IPXResultset);
                    PXTrace.WriteInformation("[DIAG] SingleRow is IList: {0}", printContext.SingleRow is System.Collections.IList);
                    
                    // Check if it's a PXResult and what it wraps
                    if (printContext.SingleRow is PXResult pr)
                    {
                        PXTrace.WriteInformation("[DIAG] SingleRow IS PXResult - checking wrapped types");
                        PXTrace.WriteInformation("[DIAG] PXResult.GetType().GenericTypeArguments: {0}", 
                            string.Join(",", pr.GetType().GenericTypeArguments.Select(t => t.Name)));
                    }
                }
                else
                {
                    PXTrace.WriteInformation("[DIAG] SingleRow is NULL");
                }
            }
            catch (Exception diagEx)
            {
                PXTrace.WriteInformation("[DIAG] Error during SingleRow analysis: {0}", diagEx.Message);
            }
            PXTrace.WriteInformation("[SERVICE] === END SingleRow Analysis ===");

            // ✅ DIAGNOSTIC: Reproduce the exact GetAsResultset pipeline that BasicLabelGenerator uses
            // This simulates what happens inside BasicLabelGenerator.PrintLabelInternal() when SingleRow is a plain DAC
            PXTrace.WriteInformation("[SERVICE] === DIAGNOSTIC: GetAsResultset Pipeline Simulation ===");
            try
            {
                // This is what happens in BasicLabelGenerator when SingleRow is NOT a PXResult/IPXResultset:
                // else { type = singleRow.GetType(); basedOnResult2 = new List<object> { singleRow }; }
                object singleRow = printContext.SingleRow;
                object basedOnResult2 = new List<object> { singleRow };

                PXTrace.WriteInformation("[DIAG-PIPELINE] basedOnResult2.GetType(): {0}", 
                    basedOnResult2?.GetType().FullName ?? "null");

                // Call the same Asgard utility that BasicLabelGenerator calls
                IPXResultset asResultset = AsgardUtils.GetAsResultset(basedOnResult2);

                PXTrace.WriteInformation("[DIAG-PIPELINE] asResultset.GetType(): {0}", 
                    asResultset?.GetType().FullName ?? "null");
                PXTrace.WriteInformation("[DIAG-PIPELINE] asResultset.GetRowCount(): {0}", 
                    asResultset?.GetRowCount() ?? -1);
                PXTrace.WriteInformation("[DIAG-PIPELINE] asResultset.GetTableCount(): {0}", 
                    asResultset?.GetTableCount() ?? -1);

                // Get the collection that ParseAndPrintMultiple will iterate
                object collectionObj = asResultset?.GetCollection();
                System.Collections.IList list = collectionObj as System.Collections.IList;

                PXTrace.WriteInformation("[DIAG-PIPELINE] GetCollection() returned null: {0}", list == null);
                PXTrace.WriteInformation("[DIAG-PIPELINE] list type: {0}", 
                    list?.GetType().FullName ?? "null");
                PXTrace.WriteInformation("[DIAG-PIPELINE] list.Count: {0}", list != null ? list.Count : -1);

                if (list != null && list.Count > 0)
                {
                    object listItem0 = list[0];

                    PXTrace.WriteInformation("[DIAG-PIPELINE] list[0] is null: {0}", listItem0 == null);
                    PXTrace.WriteInformation("[DIAG-PIPELINE] list[0].GetType(): {0}", 
                        listItem0?.GetType().FullName ?? "null");
                    PXTrace.WriteInformation("[DIAG-PIPELINE] list[0] is PXResult: {0}", listItem0 is PXResult);
                    PXTrace.WriteInformation("[DIAG-PIPELINE] list[0] is IBqlTable: {0}", listItem0 is IBqlTable);

                    // This is where ParseAndPrintMultiple calls UnwrapMain - test it here
                    try
                    {
                        object unwrappedMain = PXResult.UnwrapMain(listItem0);
                        PXTrace.WriteInformation("[DIAG-PIPELINE] PXResult.UnwrapMain(list[0]) succeeded: {0}", 
                            unwrappedMain?.GetType().FullName ?? "null");
                    }
                    catch (Exception unwrapEx)
                    {
                        PXTrace.WriteInformation("[DIAG-PIPELINE] ⚠️ PXResult.UnwrapMain(list[0]) FAILED: {0}: {1}", 
                            unwrapEx.GetType().FullName, unwrapEx.Message);
                    }

                    // Also try UnwrapFirst to see if that breaks too
                    try
                    {
                        object unwrappedFirst = PXResult.UnwrapFirst(listItem0);
                        PXTrace.WriteInformation("[DIAG-PIPELINE] PXResult.UnwrapFirst(list[0]) succeeded: {0}", 
                            unwrappedFirst?.GetType().FullName ?? "null");
                    }
                    catch (Exception unwrapFirstEx)
                    {
                        PXTrace.WriteInformation("[DIAG-PIPELINE] ⚠️ PXResult.UnwrapFirst(list[0]) FAILED: {0}: {1}", 
                            unwrapFirstEx.GetType().FullName, unwrapFirstEx.Message);
                    }
                }
                else
                {
                    PXTrace.WriteInformation("[DIAG-PIPELINE] ⚠️ GetCollection() returned empty or null list");
                }

                PXTrace.WriteInformation("[SERVICE] === END GetAsResultset Pipeline Simulation ===");
            }
            catch (Exception pipelineEx)
            {
                PXTrace.WriteInformation("[DIAG-PIPELINE] ⚠️ GetAsResultset pipeline simulation FAILED: {0}: {1}", 
                    pipelineEx.GetType().FullName, pipelineEx.Message);
                PXTrace.WriteInformation("[SERVICE] === END GetAsResultset Pipeline Simulation (with error) ===");
            }

            PXTrace.WriteInformation("[SERVICE] Calling _labelGenerator.PrintLabels()");

            // ✅ DIAGNOSTIC: Wrap PrintLabels call to capture what happens
            try
            {
                PrintResults results = _labelGenerator.PrintLabels(printContext);
                
                if (results == null)
                    throw new PXException("PrintLabels returned null.");

                PXTrace.WriteInformation(
                    $"[SERVICE] Row-selection native print finished: Shipment={shipment.ShipmentNbr}, Package={selectedPackageLineNbr}, NbLabels={results.NbLabels}");

                return results;
            }
            catch (Exception printEx)
            {
                // ✅ DIAGNOSTIC: Detailed error analysis
                PXTrace.WriteInformation("[SERVICE] === DIAGNOSTIC: PrintLabels Exception Analysis ===");
                PXTrace.WriteInformation("[DIAG] Exception Type: {0}", printEx.GetType().FullName);
                PXTrace.WriteInformation("[DIAG] Exception Message: {0}", printEx.Message);
                PXTrace.WriteInformation("[DIAG] Exception StackTrace (first 500 chars): {0}", 
                    printEx.StackTrace?.Substring(0, Math.Min(500, printEx.StackTrace.Length)) ?? "NO STACKTRACE");
                
                // Check InnerException
                if (printEx.InnerException != null)
                {
                    PXTrace.WriteInformation("[DIAG] InnerException Type: {0}", printEx.InnerException.GetType().FullName);
                    PXTrace.WriteInformation("[DIAG] InnerException Message: {0}", printEx.InnerException.Message);
                }

                PXTrace.WriteInformation("[SERVICE] === END Exception Analysis ===");
                
                // Re-throw the original exception
                throw;
            }
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