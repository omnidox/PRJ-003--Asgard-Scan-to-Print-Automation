using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AA.Objects.Labels;
using AA.Objects.Core;
using Asgard.Labels.Abstractions.Interface;
using Asgard.Labels.Impl.Context;
using Asgard.Labels.Impl.Poco;
using PX.Data;
using PX.Objects.SO;

namespace AA.Objects.AL.Integration.PerPackage
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

            // ✅ [PKG PRINT] Diagnostics: Log selected package state BEFORE CreatePrintContext
            PXTrace.WriteInformation("[PKG PRINT] Shipment={0}", shipment.ShipmentNbr);
            PXTrace.WriteInformation("[PKG PRINT] Selected LineNbr={0}", selectedPackageLineNbr);
            
            // Get UCC128 via reflection
            object selectedUcc128 = null;
            try
            {
                selectedUcc128 = packageToVerify.GetType().GetProperty("UsrTCUCC128")?.GetValue(packageToVerify);
            }
            catch { }
            PXTrace.WriteInformation("[PKG PRINT] Selected UsrTCUCC128={0}", selectedUcc128 ?? "null");

            PXTrace.WriteInformation(
                $"[SERVICE] Row-selection native print: shipment {shipment.ShipmentNbr} will print package line {selectedPackageLineNbr}");

            // ✅ CRITICAL: Ensure the selected package has UsrALPrintLabel = true in THIS graph
            // Asgard's CheckLineDoPrint() checks the extension flag on the row being printed
            // So we must ensure the correct row in the correct graph has the flag set
            PXTrace.WriteInformation("[SERVICE] === Setting UsrALPrintLabel via cache extension ===");
            try
            {
                foreach (SOPackageDetailEx pkg in PXSelect<
                    SOPackageDetailEx,
                    Where<SOPackageDetailEx.shipmentNbr, Equal<Required<SOPackageDetailEx.shipmentNbr>>>>
                    .Select(_graph, shipment.ShipmentNbr))
                {
                    if (pkg != null)
                    {
                        bool shouldPrint = pkg.LineNbr == selectedPackageLineNbr;
                        
                        // Try to use cache extension directly if available
                        try
                        {
                            object ext = TryFindExtension(pkg, Type.GetType("AA.Objects.AL.ILabelOption, AA.Objects.AL.Basic"));
                            if (ext != null)
                            {
                                // Set via extension property
                                ext.GetType().GetProperty("UsrALPrintLabel")?.SetValue(ext, shouldPrint);
                                _graph.Packages.Cache.Update(pkg);
                                PXTrace.WriteInformation("[SERVICE] Set UsrALPrintLabel={0} on package line {1}", shouldPrint, pkg.LineNbr);
                            }
                        }
                        catch (Exception setEx)
                        {
                            PXTrace.WriteInformation("[SERVICE] ⚠️ Error setting UsrALPrintLabel on line {0}: {1}", pkg.LineNbr, setEx.Message);
                        }
                    }
                }
                
                _graph.Actions.PressSave();
                PXTrace.WriteInformation("[SERVICE] UsrALPrintLabel state saved for all packages");
            }
            catch (Exception extSetEx)
            {
                PXTrace.WriteInformation("[SERVICE] ⚠️ Error setting extension flags: {0}", extSetEx.Message);
            }
            PXTrace.WriteInformation("[SERVICE] === END Setting UsrALPrintLabel ===");

            // ✅ Reload the selected package after setting flags
            packageToVerify = PXSelect<
                SOPackageDetailEx,
                Where<
                    SOPackageDetailEx.shipmentNbr, Equal<Required<SOPackageDetailEx.shipmentNbr>>,
                    And<SOPackageDetailEx.lineNbr, Equal<Required<SOPackageDetailEx.lineNbr>>>>>
                .Select(_graph, shipment.ShipmentNbr, selectedPackageLineNbr);

            if (packageToVerify == null)
            {
                throw new PXException($"Package line {selectedPackageLineNbr} not found after flag update.");
            }

            PXTrace.WriteInformation("[SERVICE] Selected package reloaded after flag update");

            // ✅ CRITICAL: Assume filter scope is already activated by the caller (SOShipmentEntry_AsgardExt)
            // This allows the scope to remain active across the fresh graph context
            PXTrace.WriteInformation("[SERVICE] Calling CreatePrintContext with Graph={0}, ShipmentNbr={1}, ModelID={2}", 
                _graph.GetType().Name, shipment.ShipmentNbr, modelId);

            AcuLabelContext printContext = AcuLabelContext.CreatePrintContext(
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

            // ✅ CRITICAL FIX: Get the correct PXResult row from Asgard's ALPackages view
            // This is the architectural solution: use ViewUtils to get the row in the correct shape
            PXTrace.WriteInformation("[SERVICE] === CRITICAL FIX: Fetching Asgard row from ALPackages ===");
            
            object selectedAsgardRow = null;
            string basedOnViewName = printContext.Model?.BasedOnView ?? "ALPackages";
            
            try
            {
                // Fetch all rows from Asgard's ALPackages view
                object viewSelectResult = ViewUtils.ViewSelect(_graph, basedOnViewName);
                
                foreach (object row in (System.Collections.IEnumerable)viewSelectResult)
                {
                    SOPackageDetail pkg = PXResult.Unwrap<SOPackageDetail>(row);
                    
                    if (pkg != null && pkg.LineNbr == selectedPackageLineNbr)
                    {
                        selectedAsgardRow = row;
                        
                        // ✅ [ROW-MATCH] Diagnostics: Log immediately after match while row is in scope
                        var matchedPkg = PXResult.Unwrap<SOPackageDetail>(selectedAsgardRow);
                        PXTrace.WriteInformation("[ROW-MATCH] === Verifying Asgard Row Match ===");
                        PXTrace.WriteInformation("[ROW-MATCH] Selected LineNbr={0}", selectedPackageLineNbr);
                        PXTrace.WriteInformation("[ROW-MATCH] Asgard row type: {0}", selectedAsgardRow.GetType().FullName);
                        PXTrace.WriteInformation("[ROW-MATCH] Is PXResult: {0}", selectedAsgardRow is PXResult);
                        PXTrace.WriteInformation("[ROW-MATCH] Bound LineNbr: {0}", matchedPkg?.LineNbr);
                        PXTrace.WriteInformation("[ROW-MATCH] ✅ MATCH: Selected PXResult row found");
                        PXTrace.WriteInformation("[ROW-MATCH] === End Verification ===");
                        
                        break;
                    }
                }
                
                if (selectedAsgardRow == null)
                {
                    throw new PXException(
                        $"Could not find package line {selectedPackageLineNbr} in {basedOnViewName}.");
                }
                
                // ✅ Create a filtered resultset containing only the selected row
                // Use Activator.CreateInstance to dynamically create the correct PXResultset<SOPackageDetail> type
                // Use IList interface to add the row (avoids compile-time generic type constraint)
                object filteredResultSet = Activator.CreateInstance(viewSelectResult.GetType());
                ((IList)filteredResultSet).Add(selectedAsgardRow);
                
                // ✅ Assign the PXResultset to SingleRow
                // This matches Asgard's expected type: IPXResultset with item type SOPackageDetail
                printContext.SingleRow = filteredResultSet;
                printContext.IsSilent = true;
                
                PXTrace.WriteInformation("[SERVICE] ✓ Filtered PXResultset created and assigned to printContext.SingleRow");
                PXTrace.WriteInformation("[SERVICE] SingleRow type: {0}", filteredResultSet.GetType().FullName);
            }
            catch (Exception asgardRowEx)
            {
                PXTrace.WriteInformation("[SERVICE] ⚠️ Error fetching Asgard row: {0}", asgardRowEx.Message);
                throw;
            }
            
            PXTrace.WriteInformation("[SERVICE] === END CRITICAL FIX ===");

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

            // ✅ [SECONDARY/VERBOSE] DIAGNOSTIC: Instrument the native ViewDef → ViewResult → ViewSelect path
            // NOTE: Trace already confirmed that ViewSelect still returns the full Packages collection
            // regardless of PXView replacement. This block is kept for reference but is no longer the
            // primary diagnostic focus. The real fix is the Asgard template (ALDetailRows.Row vs Packages).
            PXTrace.WriteInformation("[SERVICE] === [SECONDARY] Native ALPackages Path Resolution ===");
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
                
                // ✅ [VIEW-REPLACE] CRITICAL DIAGNOSTIC: Verify PXView replacement affects ViewSelect
                PXTrace.WriteInformation("[VIEW-REPLACE] === Checking whether PXView replacement affected native path ===");
                PXTrace.WriteInformation("[VIEW-REPLACE] _graph.Views[ALPackages] type: {0}",
                    _graph.Views["ALPackages"]?.GetType().FullName ?? "null");
                PXTrace.WriteInformation("[VIEW-REPLACE] _graph.Views[ALPackages] is PXView: {0}",
                    _graph.Views["ALPackages"] is PXView);
                
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
                        
                        // ✅ [VIEW-REPLACE] DIAGNOSTIC: Log whether filtered view actually affected the count
                        if (viewSelectRowCount == 1)
                        {
                            PXTrace.WriteInformation("[VIEW-REPLACE] ✅ SUCCESS: ViewSelect now returns 1 row (was previously 2)");
                        }
                        else if (viewSelectRowCount == 2)
                        {
                            PXTrace.WriteInformation("[VIEW-REPLACE] ⚠️ UNCHANGED: ViewSelect still returns {0} rows (filter may not be applied)", viewSelectRowCount);
                        }
                        else
                        {
                            PXTrace.WriteInformation("[VIEW-REPLACE] ⚠️ UNEXPECTED: ViewSelect returns {0} rows", viewSelectRowCount);
                        }
                        
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
                                    
                                    // ✅ [VIEW-REPLACE] Also log UCC128 value if available
                                    object ucc128Value = null;
                                    try
                                    {
                                        ucc128Value = pkgDetail.GetType().GetProperty("UsrTCUCC128")?.GetValue(pkgDetail);
                                    }
                                    catch { }
                                    
                                    PXTrace.WriteInformation("[DIAG-NATIVE] ViewSelect row {0}: SOPackageDetail.LineNbr = {1}, UsrTCUCC128 = {2}", 
                                        i, displayValue, ucc128Value ?? "null");
                                    
                                    // ✅ [VIEW-REPLACE] DIAGNOSTIC: Confirm selected row matches expected LineNbr
                                    if (displayValue == selectedPackageLineNbr)
                                    {
                                        PXTrace.WriteInformation("[VIEW-REPLACE] ✅ ROW {0} MATCHES selected LineNbr {1}", i, selectedPackageLineNbr);
                                    }
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
                                        
                                        // ✅ [VIEW-REPLACE] Also log UCC128 value if available
                                        object ucc128Value = null;
                                        try
                                        {
                                            ucc128Value = pkgDetail.GetType().GetProperty("UsrTCUCC128")?.GetValue(pkgDetail);
                                        }
                                        catch { }
                                        
                                        PXTrace.WriteInformation("[DIAG-NATIVE] Manual row {0}: LineNbr = {1}, UsrTCUCC128 = {2}", 
                                            i, displayValue, ucc128Value ?? "null");
                                    }
                                }
                                catch { }
                            }
                        }
                        
                        // ✅ [VIEW-REPLACE] DIAGNOSTIC: Compare whether filtering is working
                        if (manualRowCount == 1 && viewSelectRowCount == 1)
                        {
                            PXTrace.WriteInformation("[VIEW-REPLACE] ✅ MATCH: Both SelectMultiBound and ViewSelect return 1 row (filter IS working)");
                        }
                        else if (manualRowCount == 1 && viewSelectRowCount != 1)
                        {
                            PXTrace.WriteInformation("[VIEW-REPLACE] ⚠️ MISMATCH: SelectMultiBound returns 1 row, but ViewSelect returns {0} rows (filter NOT applied to native path)", viewSelectRowCount);
                        }
                        else
                        {
                            PXTrace.WriteInformation("[VIEW-REPLACE] ⚠️ UNEXPECTED: SelectMultiBound={0}, ViewSelect={1}", manualRowCount, viewSelectRowCount);
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

                PXTrace.WriteInformation("[SERVICE] === [SECONDARY] END Native ALPackages Path Resolution ===");
            }
            catch (Exception nativePathEx)
            {
                PXTrace.WriteInformation("[DIAG-NATIVE] ⚠️ Error during native path diagnostics: {0}: {1}", 
                    nativePathEx.GetType().FullName, nativePathEx.Message);
                PXTrace.WriteInformation("[SERVICE] === [SECONDARY] END Native ALPackages Path Resolution (with error) ===");
            }

            // ✅ DIAGNOSTIC: Inspect print eligibility before calling PrintLabels
            // Focus on: Factual state only, no speculation
            PXTrace.WriteInformation("[SERVICE] === DIAGNOSTIC: Print Eligibility Pre-Inspection ===");
            try
            {
                // Log factual state before PrintLabels
                PXTrace.WriteInformation("[DIAG-ELIGIBILITY] printContext.SingleRow is set to: {0}", 
                    printContext.SingleRow?.GetType().FullName ?? "null");
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
                    
                    // Get the unwrapped package detail from the Asgard PXResult row
                    // This is what Asgard will use for template evaluation
                    SOPackageDetail unwrappedRow = PXResult.Unwrap<SOPackageDetail>(selectedAsgardRow);
                    if (unwrappedRow != null)
                    {
                        PXTrace.WriteInformation("[DIAG-GATE] Unwrapped row type: {0}", unwrappedRow.GetType().FullName);
                        PXTrace.WriteInformation("[DIAG-GATE] Unwrapped row LineNbr: {0}", unwrappedRow.LineNbr);
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

            // ✅ DIAGNOSTIC: PRINTER ASSIGNMENT & CHECKLINEDOBPRINT FOCUS
            // From attached analysis: NbLabels=0 is caused by printer resolution FAIL and/or CheckLineDoPrint=False
            // Copy-count is proven WORKING (FinalCopies=1 per trace)
            PXTrace.WriteInformation("[DIAG-PRINTER] === BEGIN PRINTER ASSIGNMENT DIAGNOSTICS ===");
            try
            {
                PXTrace.WriteInformation("[DIAG-PRINTER] Model Name: {0}", printContext.Model?.Name ?? "null");
                PXTrace.WriteInformation("[DIAG-PRINTER] Printer Name: {0}", printContext.Printer?.Name ?? "null");
                PXTrace.WriteInformation("[DIAG-PRINTER] Printer Description: {0}", printContext.Printer?.Description ?? "null");
                
                if (printContext.Printer == null)
                {
                    PXTrace.WriteInformation("[DIAG-PRINTER] ⚠️ CRITICAL BLOCKER: Printer is NULL");
                    PXTrace.WriteInformation("[DIAG-PRINTER] This matches trace message: 'Model {0} has no printer for you'", 
                        printContext.Model?.Name ?? "unknown");
                }
                else
                {
                    PXTrace.WriteInformation("[DIAG-PRINTER] ✓ Printer resolved: {0}", printContext.Printer.Name);
                }

                PXTrace.WriteInformation("[DIAG-PRINTER] === END PRINTER ASSIGNMENT DIAGNOSTICS ===");
            }
            catch (Exception printerEx)
            {
                PXTrace.WriteInformation("[DIAG-PRINTER] ⚠️ Error during printer diagnostics: {0}: {1}", 
                    printerEx.GetType().FullName, printerEx.Message);
                PXTrace.WriteInformation("[DIAG-PRINTER] === END PRINTER ASSIGNMENT DIAGNOSTICS (with error) ===");
            }

            // ✅ [UCC-BINDING] PRE-PRINT DIAGNOSTIC: Template binding warning
            // Root cause confirmed: Packages.UsrTCUCC128 reads from the full Packages collection
            // and defaults to the FIRST row regardless of which package is selected.
            // The correct template expression is: ALDetailRows.Row.UsrTCUCC128
            PXTrace.WriteInformation("[UCC-BINDING] === PRE-PRINT TEMPLATE BINDING DIAGNOSTICS ===");
            PXTrace.WriteInformation("[UCC-BINDING] Selected LineNbr to print: {0}", selectedPackageLineNbr);
            PXTrace.WriteInformation("[UCC-BINDING] Selected package UsrTCUCC128: {0}", selectedUcc128 ?? "null");
            PXTrace.WriteInformation("[UCC-BINDING] printContext.SingleRow type: {0}",
                printContext.SingleRow?.GetType().FullName ?? "null");
            PXTrace.WriteInformation("[UCC-BINDING] ALDetailRows expected to be active during PrintLabels: TRUE");
            PXTrace.WriteInformation("[UCC-BINDING] ALDetailRows.Row is expected to resolve to the current detail row during Asgard iteration. Verify via template test expressions below.");
            PXTrace.WriteInformation("[UCC-BINDING] ⚠️ WARNING: If template uses {{Packages.UsrTCUCC128}}, it will ALWAYS read row 1 of the full Packages collection.");
            PXTrace.WriteInformation("[UCC-BINDING] ✅ REQUIRED template fix: Replace {{Packages.UsrTCUCC128}} with {{ALDetailRows.Row.UsrTCUCC128}}");
            PXTrace.WriteInformation("[UCC-BINDING] ✅ REQUIRED template fix: Replace {{(Packages.UsrTCUCC128)|zpl.ToBarcode 'GS1-Code128-175-NoHRI'}} with {{(ALDetailRows.Row.UsrTCUCC128)|zpl.ToBarcode 'GS1-Code128-175-NoHRI'}}");
            PXTrace.WriteInformation("[UCC-BINDING] === END PRE-PRINT TEMPLATE BINDING DIAGNOSTICS ===");

            // ✅ DIAGNOSTIC: Wrap PrintLabels call to capture what happens
            try
            {
                    PrintResults results = _labelGenerator.PrintLabels(printContext);
                
                if (results == null)
                    throw new PXException("PrintLabels returned null.");

                // ✅ [RESULT] Diagnostics: Log results immediately after PrintLabels
                PXTrace.WriteInformation("[RESULT] NbLabels={0}", results.NbLabels);
                PXTrace.WriteInformation("[RESULT] PrintResults type: {0}", results.GetType().FullName);
                PXTrace.WriteInformation("[RESULT] === SUCCESS SIGNATURE TEST ===");
                PXTrace.WriteInformation("[RESULT] Selected LineNbr: {0}, NbLabels: {1}", selectedPackageLineNbr, results.NbLabels);
                if (results.NbLabels == 1)
                {
                    PXTrace.WriteInformation("[RESULT] ✅ PASS: Single label printed as expected");
                }
                else if (results.NbLabels == 0)
                {
                    PXTrace.WriteInformation("[RESULT] ❌ FAIL: No labels generated (expected 1)");
                }
                else
                {
                    PXTrace.WriteInformation("[RESULT] ⚠️ UNEXPECTED: Multiple labels printed (expected 1, got {0})", results.NbLabels);
                }

                // ✅ [UCC-BINDING] POST-PRINT DIAGNOSTIC: UCC value verification reminder
                PXTrace.WriteInformation("[UCC-BINDING] === POST-PRINT UCC VERIFICATION ===");
                PXTrace.WriteInformation("[UCC-BINDING] Selected LineNbr: {0}", selectedPackageLineNbr);
                PXTrace.WriteInformation("[UCC-BINDING] Expected UsrTCUCC128 on printed label: {0}", selectedUcc128 ?? "null");
                PXTrace.WriteInformation("[UCC-BINDING] ⚠️ ACTION REQUIRED: Inspect the ZPL/label output and verify the barcode value matches the expected UCC above.");
                PXTrace.WriteInformation("[UCC-BINDING] If the printed UCC does NOT match, the template is still using {{Packages.UsrTCUCC128}} (full collection, defaults to row 1).");
                PXTrace.WriteInformation("[UCC-BINDING] Fix: Update the Asgard label template to use {{ALDetailRows.Row.UsrTCUCC128}} instead.");
                PXTrace.WriteInformation("[UCC-BINDING] Diagnostic template expressions to test in order:");
                PXTrace.WriteInformation("[UCC-BINDING]   1. {{ALDetailRows}}                                    (should render as non-empty object)");
                PXTrace.WriteInformation("[UCC-BINDING]   2. {{ALDetailRows.Row}}                                (should render as a DAC row object)");
                PXTrace.WriteInformation("[UCC-BINDING]   3. {{ALDetailRows.Row.UsrTCUCC128}}                   (should render the expected UCC value above)");
                PXTrace.WriteInformation("[UCC-BINDING] === END POST-PRINT UCC VERIFICATION ===");

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

        /// <summary>
        /// Helper method to invoke AsgardUtils.FindExtension<T> via reflection for any type.
        /// This avoids compile-time type resolution issues.
        /// </summary>
        private object TryFindExtension(object row, Type extensionType)
        {
            if (row == null || extensionType == null)
                return null;

            try
            {
                // Get all public static methods from AsgardUtils
                var methods = typeof(AsgardUtils)
                    .GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

                // Find the generic FindExtension<T> method that takes one parameter
                var targetMethod = methods.FirstOrDefault(m =>
                    m.Name == "FindExtension" &&
                    m.IsGenericMethodDefinition &&
                    m.GetParameters().Length == 1);

                if (targetMethod == null)
                {
                    PXTrace.WriteInformation("[PROOF] Could not find generic AsgardUtils.FindExtension<T>(row)");
                    return null;
                }

                // Create the closed generic method: FindExtension<extensionType>
                var closedMethod = targetMethod.MakeGenericMethod(extensionType);
                
                // Invoke the method with the row as argument
                return closedMethod.Invoke(null, new[] { row });
            }
            catch (Exception ex)
            {
                PXTrace.WriteInformation("[PROOF] TryFindExtension error: {0}", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Helper method to safely access field values on Asgard extension objects using reflection.
        /// </summary>
        private string GetFieldValueAsString(object obj, string fieldName)
        {
            if (obj == null)
                return "null";

            try
            {
                var property = obj.GetType().GetProperty(fieldName);
                if (property == null)
                    return "field_not_found";

                var value = property.GetValue(obj);
                return value?.ToString() ?? "null";
            }
            catch (Exception ex)
            {
                return "error_" + ex.GetType().Name;
            }
        }

        /// <summary>
        /// Helper method to invoke NbCopiesHelper.CheckLineDoPrint via reflection.
        /// Returns TRUE on failure (allow printing) - don't block on reflection errors.
        /// </summary>
        private bool InvokeCheckLineDoPrint(object labelContext)
        {
            try
            {
                // Get NbCopiesHelper type from Asgard assembly
                Type nbCopiesHelperType = Type.GetType("AA.Objects.AL.NbCopiesHelper, AA.Objects.AL.Basic");
                if (nbCopiesHelperType == null)
                {
                    PXTrace.WriteInformation("[DIAG-GATE-PRINT] ⚠️ NbCopiesHelper type not found via reflection");
                    PXTrace.WriteInformation("[DIAG-GATE-PRINT] Defaulting to TRUE (allow printing) - native path will decide");
                    return true;  // ← ALLOW PRINTING ON REFLECTION FAILURE
                }

                // Get CheckLineDoPrint static method
                var method = nbCopiesHelperType.GetMethod("CheckLineDoPrint", 
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                
                if (method == null)
                {
                    PXTrace.WriteInformation("[DIAG-GATE-PRINT] ⚠️ CheckLineDoPrint method not found on NbCopiesHelper");
                    PXTrace.WriteInformation("[DIAG-GATE-PRINT] Defaulting to TRUE (allow printing) - native path will decide");
                    return true;  // ← ALLOW PRINTING ON REFLECTION FAILURE
                }

                // Invoke the method
                object result = method.Invoke(null, new[] { labelContext });
                bool methodResult = (bool)(result ?? false);
                
                PXTrace.WriteInformation("[DIAG-GATE-PRINT] ✓ Native CheckLineDoPrint invoked successfully: {0}", methodResult);
                return methodResult;
            }
            catch (TargetInvocationException tiEx)
            {
                // Unwrap the inner exception from reflection invocation
                PXTrace.WriteInformation("[DIAG-GATE-PRINT] ⚠️ CheckLineDoPrint threw exception during invocation");
                PXTrace.WriteInformation("[DIAG-GATE-PRINT] Inner exception type: {0}", 
                    tiEx.InnerException?.GetType().FullName ?? "null");
                PXTrace.WriteInformation("[DIAG-GATE-PRINT] Inner exception message: {0}", 
                    tiEx.InnerException?.Message ?? "null");
                PXTrace.WriteInformation("[DIAG-GATE-PRINT] Defaulting to TRUE (allow printing)");
                return true;  // ← ALLOW PRINTING ON INVOCATION ERROR
            }
            catch (Exception ex)
            {
                PXTrace.WriteInformation("[DIAG-GATE-PRINT] ⚠️ Error invoking CheckLineDoPrint: {0}", ex.GetType().FullName);
                PXTrace.WriteInformation("[DIAG-GATE-PRINT] Exception message: {0}", ex.Message);
                PXTrace.WriteInformation("[DIAG-GATE-PRINT] Defaulting to TRUE (allow printing) - native path will decide");
                return true;  // ← ALLOW PRINTING ON REFLECTION ERROR
            }
        }
    }
}