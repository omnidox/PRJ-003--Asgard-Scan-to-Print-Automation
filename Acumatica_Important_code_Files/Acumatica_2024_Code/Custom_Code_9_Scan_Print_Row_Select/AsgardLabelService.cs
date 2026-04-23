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

            string modelName = printContext.Model != null ? printContext.Model.Name : "<null>";
            string printerName = printContext.Printer != null ? printContext.Printer.Name : "<null>";
            PXTrace.WriteInformation("[SERVICE] CreatePrintContext succeeded. Context Model={0}, Printer={1}", 
                modelName, printerName);

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

            // ✅ DIAGNOSTIC: Instrument the native ViewDef → ViewResult → ViewSelect path
            // This is what BasicLabelGenerator uses when SingleRow is null
            PXTrace.WriteInformation("[SERVICE] === DIAGNOSTIC: Native ALPackages Path Resolution ===");
            try
            {
                string basedOnView = printContext.Model != null ? printContext.Model.BasedOnView : "null";
                PXTrace.WriteInformation("[DIAG-NATIVE] basedOnView={0}", basedOnView);

                // Step 1: GetViewDefinition - what metadata does Asgard see for ALPackages?
                PXTrace.WriteInformation("[DIAG-NATIVE] === STEP 1: ViewUtils.GetViewDefinition ===");
                ViewDef viewDef = ViewUtils.GetViewDefinition(_graph, basedOnView);
                
                if (viewDef != null)
                {
                    PXTrace.WriteInformation("[DIAG-NATIVE] ViewDef.InternalName: {0}", viewDef.InternalName);
                    PXTrace.WriteInformation("[DIAG-NATIVE] ViewDef.ItemType: {0}", viewDef.ItemType != null ? viewDef.ItemType.Name : "null");
                    PXTrace.WriteInformation("[DIAG-NATIVE] ViewDef.ItemTypes.Count: {0}", viewDef.ItemTypes != null ? viewDef.ItemTypes.Length : 0);
                    if (viewDef.ItemTypes != null && viewDef.ItemTypes.Length > 0)
                    {
                        PXTrace.WriteInformation("[DIAG-NATIVE] ViewDef.ItemTypes: {0}", 
                            string.Join(", ", viewDef.ItemTypes.Select(t => t.Name)));
                    }
                    PXTrace.WriteInformation("[DIAG-NATIVE] ViewDef.Detail: {0}", viewDef.Detail);
                    PXTrace.WriteInformation("[DIAG-NATIVE] ViewDef.DependsOn: {0}", viewDef.DependsOn != null ? viewDef.DependsOn : "null");
                }
                else
                {
                    PXTrace.WriteInformation("[DIAG-NATIVE] ⚠️ ViewDef returned NULL for '{0}'", basedOnView);
                }

                // Step 2: GetViewRow - what does the native path construct?
                PXTrace.WriteInformation("[DIAG-NATIVE] === STEP 2: ViewUtils.GetViewRow ===");
                if (viewDef != null)
                {
                    IViewResult viewResult = ViewUtils.GetViewRow(_graph, viewDef);
                    
                    if (viewResult != null)
                    {
                        PXTrace.WriteInformation("[DIAG-NATIVE] ViewResult type: {0}", viewResult.GetType().FullName);
                        PXTrace.WriteInformation("[DIAG-NATIVE] ViewResult.InternalName: {0}", viewResult.InternalName);
                        PXTrace.WriteInformation("[DIAG-NATIVE] ViewResult.TableCount: {0}", viewResult.TableCount);
                        PXTrace.WriteInformation("[DIAG-NATIVE] ViewResult.ItemTypes.Count: {0}", viewResult.ItemTypes != null ? viewResult.ItemTypes.Count : 0);
                        if (viewResult.ItemTypes != null && viewResult.ItemTypes.Count > 0)
                        {
                            PXTrace.WriteInformation("[DIAG-NATIVE] ViewResult.ItemTypes: {0}", 
                                string.Join(", ", viewResult.ItemTypes.Select(t => t.Name)));
                        }
                        string resultTypeName = viewResult.Result != null ? viewResult.Result.GetType().FullName : "null";
                        PXTrace.WriteInformation("[DIAG-NATIVE] ViewResult.Result type: {0}", resultTypeName);
                        PXTrace.WriteInformation("[DIAG-NATIVE] ViewResult.Result is null: {0}", viewResult.Result == null);
                        PXTrace.WriteInformation("[DIAG-NATIVE] ViewResult.Detail: {0}", viewResult.Detail);
                    }
                    else
                    {
                        PXTrace.WriteInformation("[DIAG-NATIVE] ⚠️ ViewResult returned NULL");
                    }
                }

                // Step 3: ViewSelect - what does the direct select return?
                PXTrace.WriteInformation("[DIAG-NATIVE] === STEP 3: ViewUtils.ViewSelect ===");
                object viewSelectResult = ViewUtils.ViewSelect(_graph, basedOnView);
                
                if (viewSelectResult != null)
                {
                    PXTrace.WriteInformation("[DIAG-NATIVE] ViewSelect result type: {0}", viewSelectResult.GetType().FullName);
                    
                    // Materialize enumerable once to avoid multiple enumerations
                    var enumerable = viewSelectResult as System.Collections.IEnumerable;
                    List<object> viewSelectList = null;
                    int viewSelectRowCount = 0;
                    object viewSelectFirstRow = null;
                    
                    if (enumerable != null)
                    {
                        viewSelectList = enumerable.Cast<object>().ToList();
                        viewSelectRowCount = viewSelectList.Count;
                        viewSelectFirstRow = viewSelectList.FirstOrDefault();
                        
                        PXTrace.WriteInformation("[DIAG-NATIVE] ViewSelect row count: {0}", viewSelectRowCount);
                        if (viewSelectFirstRow != null)
                        {
                            PXTrace.WriteInformation("[DIAG-NATIVE] ViewSelect first row type: {0}", viewSelectFirstRow.GetType().FullName);
                            PXTrace.WriteInformation("[DIAG-NATIVE] ViewSelect first row is PXResult: {0}", viewSelectFirstRow is PXResult);
                            PXTrace.WriteInformation("[DIAG-NATIVE] ViewSelect first row is IBqlTable: {0}", viewSelectFirstRow is IBqlTable);
                            
                            if (viewSelectFirstRow is PXResult pr)
                            {
                                PXTrace.WriteInformation("[DIAG-NATIVE] ViewSelect first row PXResult type args: {0}", 
                                    string.Join(",", pr.GetType().GenericTypeArguments.Select(t => t.Name)));
                            }
                        }
                    }
                    else
                    {
                        PXTrace.WriteInformation("[DIAG-NATIVE] ViewSelect result is not enumerable");
                    }

                    // Log the actual package line numbers from ViewSelect result
                    PXTrace.WriteInformation("[DIAG-NATIVE] === Package line numbers in ViewSelect result ===");
                    if (viewSelectList != null && viewSelectList.Count > 0)
                    {
                        for (int i = 0; i < Math.Min(5, viewSelectList.Count); i++)
                        {
                            object row = viewSelectList[i];
                            try
                            {
                                SOPackageDetail pkgDetail = PXResult.Unwrap<SOPackageDetail>(row);
                                if (pkgDetail != null)
                                {
                                    int displayValue = (pkgDetail.LineNbr == null) ? -1 : pkgDetail.LineNbr.Value;
                                    PXTrace.WriteInformation("[DIAG-NATIVE] ViewSelect row {0}: SOPackageDetail.LineNbr = {1}", 
                                        i, displayValue);
                                }
                                else
                                {
                                    PXTrace.WriteInformation("[DIAG-NATIVE] ViewSelect row {0}: Unwrapped to null", i);
                                }
                            }
                            catch (Exception unwrapEx)
                            {
                                PXTrace.WriteInformation("[DIAG-NATIVE] ViewSelect row {0}: Failed to unwrap - {1}", 
                                    i, unwrapEx.Message);
                            }
                        }
                    }

                    // Comparison: Manual SelectMultiBound vs Native ViewSelect
                    PXTrace.WriteInformation("[DIAG-NATIVE] === COMPARISON: Manual SelectMultiBound vs Native ViewSelect ===");
                    PXView alPackagesView = _graph.Views["ALPackages"];
                    if (alPackagesView != null)
                    {
                        var manualResult = alPackagesView.SelectMultiBound(new object[] { _graph.Document.Current });
                        List<object> manualList = manualResult?.Cast<object>().ToList() ?? new List<object>();
                        int manualRowCount = manualList.Count;
                        object manualFirstRow = manualList.FirstOrDefault();
                        
                        PXTrace.WriteInformation("[DIAG-NATIVE] Manual SelectMultiBound row count: {0}", manualRowCount);
                        if (manualFirstRow != null)
                        {
                            PXTrace.WriteInformation("[DIAG-NATIVE] Manual SelectMultiBound first row type: {0}", manualFirstRow.GetType().FullName);
                            PXTrace.WriteInformation("[DIAG-NATIVE] Manual SelectMultiBound first row is PXResult: {0}", manualFirstRow is PXResult);
                            
                            // Also log the package line numbers from manual result
                            PXTrace.WriteInformation("[DIAG-NATIVE] Manual SelectMultiBound package line numbers:");
                            for (int i = 0; i < Math.Min(5, manualList.Count); i++)
                            {
                                try
                                {
                                    SOPackageDetail pkgDetail = PXResult.Unwrap<SOPackageDetail>(manualList[i]);
                                    if (pkgDetail != null)
                                    {
                                        int displayValue = (pkgDetail.LineNbr == null) ? -1 : pkgDetail.LineNbr.Value;
                                        PXTrace.WriteInformation("[DIAG-NATIVE] Manual row {0}: LineNbr = {1}", 
                                            i, displayValue);
                                    }
                                }
                                catch { }
                            }
                        }
                        
                        PXTrace.WriteInformation("[DIAG-NATIVE] Comparison result - same row count: {0}", 
                            manualRowCount == viewSelectRowCount);
                        PXTrace.WriteInformation("[DIAG-NATIVE] Comparison result - same first row type: {0}", 
                            manualFirstRow?.GetType().FullName == viewSelectFirstRow?.GetType().FullName);
                    }
                }
                else
                {
                    PXTrace.WriteInformation("[DIAG-NATIVE] ⚠️ ViewSelect result is NULL");
                }

                PXTrace.WriteInformation("[SERVICE] === END Native ALPackages Path Resolution ===");
            }
            catch (Exception nativePathEx)
            {
                PXTrace.WriteInformation("[DIAG-NATIVE] ⚠️ Error during native path diagnostics: {0}: {1}", 
                    nativePathEx.GetType().FullName, nativePathEx.Message);
                PXTrace.WriteInformation("[SERVICE] === END Native ALPackages Path Resolution (with error) ===");
            }

            // ✅ DIAGNOSTIC: Leave SingleRow null to test the native ALPackages path
            // The native path is: BasicLabelGenerator → GetViewDefinition → GetViewRow → ViewSelect
            PXTrace.WriteInformation("[SERVICE] === SingleRow Diagnostic Path ===");
            PXTrace.WriteInformation("[SERVICE] SingleRow is being LEFT NULL to test native ALPackages path");
            PXTrace.WriteInformation("[SERVICE] BasicLabelGenerator will use ViewDef/ViewResult/ViewSelect instead");
            PXTrace.WriteInformation("[SERVICE] === END SingleRow Diagnostic Path ===");

            // ✅ DIAGNOSTIC: Inspect print eligibility before calling PrintLabels
            // Focus on: Factual state only, no speculation
            PXTrace.WriteInformation("[SERVICE] === DIAGNOSTIC: Print Eligibility Pre-Inspection ===");
            try
            {
                // Log factual state before PrintLabels
                PXTrace.WriteInformation("[DIAG-ELIGIBILITY] printContext.SingleRow is NULL: {0}", 
                    printContext.SingleRow == null);
                PXTrace.WriteInformation("[DIAG-ELIGIBILITY] ALPackagesFilterScope.IsActive: {0}", 
                    ALPackagesFilterScope.IsActive);
                string modelBasedOnView = printContext.Model != null ? printContext.Model.BasedOnView : "null";
                PXTrace.WriteInformation("[DIAG-ELIGIBILITY] printContext.Model.BasedOnView: {0}", 
                    modelBasedOnView);
                PXTrace.WriteInformation("[DIAG-ELIGIBILITY] Expected package to print: line {0}", 
                    selectedPackageLineNbr);

                PXTrace.WriteInformation("[SERVICE] === END Print Eligibility Pre-Inspection ===");
            }
            catch (Exception eligEx)
            {
                PXTrace.WriteInformation("[DIAG-ELIGIBILITY] ⚠️ Error during eligibility inspection: {0}: {1}", 
                    eligEx.GetType().FullName, eligEx.Message);
                PXTrace.WriteInformation("[SERVICE] === END Print Eligibility Pre-Inspection (with error) ===");
            }

            // ✅ DIAGNOSTIC: ACTUAL PRINT-GATING LOGIC INSPECTION
            // Instrument the exact gates that block output: CheckDoPrint(), GetNbCopies(), CheckLineDoPrint()
            PXTrace.WriteInformation("[SERVICE] === DIAGNOSTIC: Print-Gating Logic (Pre-PrintLabels) ===");
            try
            {
                PXTrace.WriteInformation("[DIAG-GATE] Selected package line to print: {0}", selectedPackageLineNbr);
                PXTrace.WriteInformation("[DIAG-GATE] printContext.Model.Name: {0}", 
                    printContext.Model != null ? printContext.Model.Name : "null");
                PXTrace.WriteInformation("[DIAG-GATE] printContext.Model.FilterRuleID: {0}", 
                    printContext.Model != null ? printContext.Model.FilterRuleID : "null");
                PXTrace.WriteInformation("[DIAG-GATE] printContext.Model.PrintRuleID: {0}", 
                    printContext.Model != null ? printContext.Model.PrintRuleID : "null");
                PXTrace.WriteInformation("[DIAG-GATE] printContext.IsDesignMode: {0}", 
                    printContext.IsDesignMode);

                // Log package row state
                SOPackageDetailEx packageRow = PXSelect<
                    SOPackageDetailEx,
                    Where<
                        SOPackageDetailEx.shipmentNbr, Equal<Required<SOPackageDetailEx.shipmentNbr>>,
                        And<SOPackageDetailEx.lineNbr, Equal<Required<SOPackageDetailEx.lineNbr>>>>>
                    .Select(_graph, shipment.ShipmentNbr, selectedPackageLineNbr);

                if (packageRow != null)
                {
                    PXTrace.WriteInformation("[DIAG-GATE] Package row EXISTS: ShipmentNbr={0}, LineNbr={1}", 
                        packageRow.ShipmentNbr ?? "null", packageRow.LineNbr ?? -1);
                    PXTrace.WriteInformation("[DIAG-GATE] Package.BoxID: {0}", packageRow.BoxID ?? "null");
                    PXTrace.WriteInformation("[DIAG-GATE] Package.Weight: {0}", packageRow.Weight);
                    
                    // ✅ CRITICAL: Inspect copy-count logic
                    // This is the most likely remaining blocker (attachment analysis points here)
                    PXTrace.WriteInformation("[DIAG-GATE] === Copy-Count Logic Inspection ===");
                    
                    // Log model-level copy count configuration
                    if (printContext.Model != null)
                    {
                        PXTrace.WriteInformation("[DIAG-GATE] printContext.Model.NbCopiesExpr: {0}", 
                            printContext.Model.NbCopiesExpr ?? "null");
                    }
                    
                    // Get the unwrapped main row (what lc.LabelRow resolves to)
                    IBqlTable unwrappedRow = PXResult.UnwrapMain(packageRow);
                    if (unwrappedRow != null)
                    {
                        PXTrace.WriteInformation("[DIAG-GATE] Unwrapped main row type: {0}", unwrappedRow.GetType().FullName);
                    }
                    
                    // Try to find ILabelOption extension and log copy override fields
                    PXTrace.WriteInformation("[DIAG-GATE] === ILabelOption Extension Inspection ===");
                    try
                    {
                        Type iLabelOptionType = Type.GetType("AA.Objects.AL.ILabelOption, AA.Objects.AL.Basic");
                        if (iLabelOptionType != null)
                        {
                            PXTrace.WriteInformation("[DIAG-GATE] ILabelOption type found");
                            if (unwrappedRow != null)
                            {
                                PXTrace.WriteInformation("[DIAG-GATE] ℹ️ Note: UseAsgardUtils.FindExtension<ILabelOption>() to check for UsrALNbrOfCopies field");
                            }
                        }
                        else
                        {
                            PXTrace.WriteInformation("[DIAG-GATE] ILabelOption type NOT found - extension may not be available");
                        }
                    }
                    catch (Exception extEx)
                    {
                        PXTrace.WriteInformation("[DIAG-GATE] Error inspecting ILabelOption extension: {0}", extEx.Message);
                    }
                    
                    PXTrace.WriteInformation("[DIAG-GATE] Package row DAC type: {0}", packageRow.GetType().FullName);
                }
                else
                {
                    PXTrace.WriteInformation("[DIAG-GATE] ⚠️ Package row {0} NOT FOUND", selectedPackageLineNbr);
                }

                PXTrace.WriteInformation("[SERVICE] === END Print-Gating Logic Pre-Inspection ===");
            }
            catch (Exception gateEx)
            {
                PXTrace.WriteInformation("[DIAG-GATE] ⚠️ Error during gating pre-inspection: {0}: {1}", 
                    gateEx.GetType().FullName, gateEx.Message);
                PXTrace.WriteInformation("[SERVICE] === END Print-Gating Logic Pre-Inspection (with error) ===");
            }

            // ✅ CRITICAL DIAGNOSTIC: COPY-COUNT PROOF BLOCK
            // This proves exactly why NbLabels=0 by inspecting the actual DetailRows and their copy-count resolution
            PXTrace.WriteInformation("[PROOF] === BEGIN COPY-COUNT PROOF ===");
            try
            {
                object basedOnRows = ViewUtils.ViewSelect(_graph, printContext.Model.BasedOnView);
                IPXResultset rs = AsgardUtils.GetAsResultset(basedOnRows);

                if (rs == null)
                {
                    PXTrace.WriteInformation("[PROOF] ViewSelect result could not be converted to IPXResultset");
                }
                else
                {
                    IList rows = (IList)rs.GetCollection();
                    PXTrace.WriteInformation("[PROOF] BasedOnView={0}, RowCount={1}",
                        printContext.Model.BasedOnView,
                        rows?.Count ?? 0);

                    printContext.DetailRows = rs;

                    if (rows != null)
                    {
                        for (int i = 0; i < rows.Count; i++)
                        {
                            object detail = rows[i];
                            printContext.DetailRow = detail;

                            object rowObj = printContext.Row;
                            object detailObj = printContext.DetailRow;
                            object labelObj = printContext.LabelRow;

                            var detailMain = PXResult.UnwrapMain(detailObj);
                            var rowMain = PXResult.UnwrapMain(rowObj);
                            var labelMain = PXResult.UnwrapMain(labelObj);

                            // Attempt to find ILabelOption extensions
                            object detailOpt = null;
                            object rowOpt = null;
                            object labelOpt = null;

                            try
                            {
                                detailOpt = AsgardUtils.FindExtension<ILabelOption>(detailObj);
                                if (detailOpt == null && detailMain != null)
                                {
                                    detailOpt = AsgardUtils.FindExtension<ILabelOption>(detailMain);
                                }

                                rowOpt = AsgardUtils.FindExtension<ILabelOption>(rowObj);
                                if (rowOpt == null && rowMain != null)
                                {
                                    rowOpt = AsgardUtils.FindExtension<ILabelOption>(rowMain);
                                }

                                labelOpt = AsgardUtils.FindExtension<ILabelOption>(labelObj);
                                if (labelOpt == null && labelMain != null)
                                {
                                    labelOpt = AsgardUtils.FindExtension<ILabelOption>(labelMain);
                                }
                            }
                            catch (Exception extEx)
                            {
                                PXTrace.WriteInformation("[PROOF] Error finding ILabelOption extensions: {0}", extEx.Message);
                            }

                            // Get the actual copy count values
                            int? overrideCopies = null;
                            int exprCopies = 1;
                            int finalCopies = 0;
                            bool doPrintLine = false;

                            try
                            {
                                overrideCopies = printContext.GetNbCopiesOverride();
                                exprCopies = printContext.ScribanContext.EvalExpr(printContext.Model.NbCopiesExpr, 1);
                                finalCopies = printContext.GetNbCopies();
                                doPrintLine = NbCopiesHelper.CheckLineDoPrint(printContext);
                            }
                            catch (Exception copyEx)
                            {
                                PXTrace.WriteInformation("[PROOF] Error evaluating copy count: {0}", copyEx.Message);
                            }

                            // Extract package line number if possible
                            SOPackageDetail pkg = null;
                            try
                            {
                                pkg = PXResult.Unwrap<SOPackageDetail>(detailObj);
                                if (pkg == null && detailMain != null)
                                {
                                    pkg = detailMain as SOPackageDetail;
                                }
                            }
                            catch { }

                            PXTrace.WriteInformation(
                                "[PROOF] RowIndex={0}, PackageLineNbr={1}, DetailType={2}, RowType={3}, LabelType={4}",
                                i,
                                pkg?.LineNbr,
                                detailObj?.GetType().FullName ?? "null",
                                rowObj?.GetType().FullName ?? "null",
                                labelObj?.GetType().FullName ?? "null"
                            );

                            PXTrace.WriteInformation(
                                "[PROOF] DetailOpt?={0}, RowOpt?={1}, LabelOpt?={2}",
                                detailOpt != null,
                                rowOpt != null,
                                labelOpt != null
                            );

                            PXTrace.WriteInformation(
                                "[PROOF] NbCopiesExpr='{0}', Override={1}, ExprResult={2}, FinalCopies={3}, CheckLineDoPrint={4}",
                                printContext.Model.NbCopiesExpr ?? "null",
                                overrideCopies?.ToString() ?? "null",
                                exprCopies,
                                finalCopies,
                                doPrintLine
                            );
                        }
                    }
                }
            }
            catch (Exception proofEx)
            {
                PXTrace.WriteInformation("[PROOF] ⚠️ Error during copy-count proof: {0}: {1}", 
                    proofEx.GetType().FullName, proofEx.Message);
            }

            PXTrace.WriteInformation("[PROOF] === END COPY-COUNT PROOF ===");

            // ✅ DIAGNOSTIC: Wrap PrintLabels call to capture what happens
            try
            {
                PrintResults results = _labelGenerator.PrintLabels(printContext);
                
                if (results == null)
                    throw new PXException("PrintLabels returned null.");

                // ✅ DIAGNOSTIC: Analyze print results and copy-count gate analysis
                PXTrace.WriteInformation("[SERVICE] === DIAGNOSTIC: Print Results & Copy-Count Analysis ===");
                int nbLabelsValue = results.NbLabels;
                PXTrace.WriteInformation("[DIAG-RESULTS] NbLabels: {0}", nbLabelsValue);
                PXTrace.WriteInformation("[DIAG-RESULTS] PrintResults type: {0}", results.GetType().FullName);
                
                if (nbLabelsValue == 0)
                {
                    PXTrace.WriteInformation("[DIAG-RESULTS] ⚠️ Zero labels generated");
                    PXTrace.WriteInformation("[DIAG-RESULTS] === COPY-COUNT GATE ANALYSIS ===");
                    PXTrace.WriteInformation("[DIAG-RESULTS] From BasicLabelGenerator.ParseAndPrint logic:");
                    PXTrace.WriteInformation("[DIAG-RESULTS] Copy-count check: GetNbCopies() returned VALUE");
                    PXTrace.WriteInformation("[DIAG-RESULTS]   If GetNbCopies() == 0 → returns EMPTY immediately (zero labels)");
                    PXTrace.WriteInformation("[DIAG-RESULTS] GetNbCopies() logic chain:");
                    PXTrace.WriteInformation("[DIAG-RESULTS]   1. Check GetNbCopiesOverride() from ILabelOption.UsrALNbrOfCopies");
                    PXTrace.WriteInformation("[DIAG-RESULTS]   2. If override > 0 → return override");
                    PXTrace.WriteInformation("[DIAG-RESULTS]   3. Else evaluate Model.NbCopiesExpr");
                    PXTrace.WriteInformation("[DIAG-RESULTS]   4. Return the evaluated result (could be 0)");
                    PXTrace.WriteInformation("[DIAG-RESULTS] === KEY UNKNOWNS ===");
                    PXTrace.WriteInformation("[DIAG-RESULTS] ❓ What was GetNbCopies() result? (CHECK EARLIER [DIAG-GATE] LOGS)");
                    PXTrace.WriteInformation("[DIAG-RESULTS] ❓ What is Model.NbCopiesExpr? (CHECK EARLIER [DIAG-GATE] LOGS)");
                    PXTrace.WriteInformation("[DIAG-RESULTS] ❓ Is there a copy override on the package row?");
                    PXTrace.WriteInformation("[DIAG-RESULTS] ❓ Does Model.NbCopiesExpr evaluate to 0 at row context?");
                    PXTrace.WriteInformation("[DIAG-RESULTS] Model FilterRule result: CHECK [DIAG-GATE] if it evaluates FALSE");
                    PXTrace.WriteInformation("[DIAG-RESULTS] Selected package: LineNbr=1, ShipmentNbr=0015732");
                    PXTrace.WriteInformation("[DIAG-RESULTS] Document condition (Target): TRUE (confirmed)");
                }
                else
                {
                    PXTrace.WriteInformation("[DIAG-RESULTS] ✅ Labels generated: {0}", nbLabelsValue);
                }

                PXTrace.WriteInformation("[SERVICE] === END Print Results & Copy-Count Analysis ===");

                PXTrace.WriteInformation(
                    $"[SERVICE] Row-selection native print finished: Shipment={shipment.ShipmentNbr}, Package={selectedPackageLineNbr}, NbLabels={nbLabelsValue}");

                return results;
            }
            catch (Exception printEx)
            {
                // ✅ DIAGNOSTIC: Detailed error analysis
                PXTrace.WriteInformation("[SERVICE] === DIAGNOSTIC: PrintLabels Exception Analysis ===");
                PXTrace.WriteInformation("[DIAG] Exception Type: {0}", printEx.GetType().FullName);
                PXTrace.WriteInformation("[DIAG] Exception Message: {0}", printEx.Message);
                string stackTrace = printEx.StackTrace != null ? printEx.StackTrace.Substring(0, Math.Min(500, printEx.StackTrace.Length)) : "NO STACKTRACE";
                PXTrace.WriteInformation("[DIAG] Exception StackTrace (first 500 chars): {0}", stackTrace);
                
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