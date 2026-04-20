using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PX.Data;
using PX.Objects.SO;
using AA.Objects.AL.Integration;
using AA.Objects.AL.License;

namespace AA.Objects.AL.Integration.PerPackage
{
    public sealed class PackagePrintSummary
    {
        public int SelectedPackageCount { get; set; }
        public int LabelsPrinted { get; set; }
    }

    public class AsgardLabelService
    {
        private const string PackagePrintFlagField = "UsrALPrintLabel";

        private readonly SOShipmentEntry _graph;
        private readonly ILabelGenerator _labelGenerator;
        private readonly IALLicenseManager _licenseManager;

        public AsgardLabelService(
            SOShipmentEntry graph,
            ILabelGenerator labelGenerator,
            IALLicenseManager licenseManager = null)
        {
            _graph = graph ?? throw new ArgumentNullException(nameof(graph));
            _labelGenerator = labelGenerator ?? throw new ArgumentNullException(nameof(labelGenerator));
            _licenseManager = licenseManager;
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
                        $"Diagnostic print: using ALSetupSlot.BoxPrintModelID = {boxPrintModelId}");

                    return boxPrintModelId;
                }

                PXTrace.WriteInformation(
                    "Diagnostic print: ALSetupSlot.BoxPrintModelID is empty, falling back to model name lookup.");
            }

            Guid? modelIdByName = GetModelIdByName(modelName);

            PXTrace.WriteInformation(
                $"Diagnostic print: resolved model '{modelName}' to ModelID = {modelIdByName}");

            return modelIdByName;
        }

        public virtual void ValidateModelForPackagePrinting(ALModel model, Guid? modelId)
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
                $"Diagnostic model info: Shipment={shipmentNbr}, ModelID={modelId}, ModelName={modelName}, ScreenID={screenId}, BasedOnView={basedOnView}, GraphType={_graph.GetType().FullName}");
        }

        public virtual void EnsureShipmentCurrent(SOShipment shipment)
        {
            _graph.Document.Current = shipment;

            try
            {
                var currentDocumentCache = _graph.Caches[typeof(SOShipment)];
                if (currentDocumentCache != null)
                {
                    currentDocumentCache.Current = shipment;
                }
            }
            catch
            {
                // Diagnostic-only best effort. Safe to ignore.
            }
        }

        public virtual void TracePackagesRowTypes(SOShipment shipment)
        {
            ValidateShipmentForAsgardPrint(shipment);
            EnsureShipmentCurrent(shipment);

            int i = 0;

            foreach (SOPackageDetail pkg in _graph.Packages.Select())
            {
                i++;

                string pkgType = pkg?.GetType().FullName ?? "<null>";
                string lineNbr = pkg?.LineNbr?.ToString() ?? "<null>";
                bool isChecked = IsPackageMarkedForPrint(pkg);

                PXTrace.WriteInformation(
                    $"Packages row #{i}: packageType={pkgType}, lineNbr={lineNbr}, checked={isChecked}");
            }

            if (i == 0)
            {
                PXTrace.WriteInformation("Packages.Select() returned 0 rows.");
            }
        }

        public virtual void TraceALPackagesRowTypes(SOShipment shipment)
        {
            ValidateShipmentForAsgardPrint(shipment);
            EnsureShipmentCurrent(shipment);

            ALSOShipmentEntryExt asgardExt = _graph.GetExtension<ALSOShipmentEntryExt>();
            if (asgardExt?.ALPackages == null)
                throw new PXException("ALPackages view is not available.");

            int i = 0;

            foreach (object result in asgardExt.ALPackages.Select())
            {
                i++;

                string resultType = result?.GetType().FullName ?? "<null>";
                SOPackageDetail pkg = PXResult.Unwrap<SOPackageDetail>(result);
                string pkgType = pkg?.GetType().FullName ?? "<null>";
                string lineNbr = pkg?.LineNbr?.ToString() ?? "<null>";
                bool isChecked = IsPackageMarkedForPrint(pkg);

                PXTrace.WriteInformation(
                    $"ALPackages row #{i}: resultType={resultType}, unwrappedPackageType={pkgType}, lineNbr={lineNbr}, checked={isChecked}");

                PXResult pxResult = result as PXResult;
                if (pxResult != null)
                {
                    IBqlTable[] rows = pxResult.GetResults();
                    string innerTypes = string.Join(", ", rows.Select(r => r?.GetType().FullName ?? "<null>"));

                    PXTrace.WriteInformation(
                        $"ALPackages row #{i}: PXResult inner row types = [{innerTypes}]");
                }
                else
                {
                    PXTrace.WriteInformation(
                        $"ALPackages row #{i}: result is NOT a PXResult.");
                }
            }

            if (i == 0)
            {
                PXTrace.WriteInformation("ALPackages.Select() returned 0 rows.");
            }
        }

        public virtual PrintResults PrintSingleALPackagesResult(
            SOShipment shipment,
            object alPackagesResult,
            Guid? modelId)
        {
            ValidateShipmentForAsgardPrint(shipment);
            EnsureShipmentCurrent(shipment);

            if (alPackagesResult == null)
                throw new PXException("ALPackages result row is null.");

            SOPackageDetail package = PXResult.Unwrap<SOPackageDetail>(alPackagesResult);

            PXTrace.WriteInformation(
                $"Diagnostic single-row print: raw ALPackages result type = {alPackagesResult.GetType().FullName}");

            PXResult rawPxResult = alPackagesResult as PXResult;
            if (rawPxResult != null)
            {
                IBqlTable[] rawRows = rawPxResult.GetResults();
                string rawInnerTypes = string.Join(", ", rawRows.Select(r => r?.GetType().FullName ?? "<null>"));

                PXTrace.WriteInformation(
                    $"Diagnostic single-row print: raw ALPackages PXResult inner row types = [{rawInnerTypes}]");
            }
            else
            {
                PXTrace.WriteInformation(
                    "Diagnostic single-row print: raw ALPackages result is NOT a PXResult.");
            }

            PXTrace.WriteInformation(
                $"Diagnostic single-row print: package line = {package?.LineNbr?.ToString() ?? "<null>"}");

            LabelContext printContext = LabelContext.CreateSingleRowPrintContext(
                _graph.GetType(),
                shipment,
                alPackagesResult,
                modelId,
                shipment.CustomerID);

            if (printContext == null)
                throw new PXException("CreateSingleRowPrintContext returned null.");

            printContext.IsSilent = true;

            PXTrace.WriteInformation(
                $"Diagnostic single-row print: printContext.Row type = {printContext.Row?.GetType().FullName ?? "<null>"}");

            PXTrace.WriteInformation(
                $"Diagnostic single-row print: printContext.SingleRow type = {printContext.SingleRow?.GetType().FullName ?? "<null>"}");

            PXResult singleRowPxResult = printContext.SingleRow as PXResult;
            if (singleRowPxResult != null)
            {
                IBqlTable[] singleRows = singleRowPxResult.GetResults();
                string singleInnerTypes = string.Join(", ", singleRows.Select(r => r?.GetType().FullName ?? "<null>"));

                PXTrace.WriteInformation(
                    $"Diagnostic single-row print: printContext.SingleRow PXResult inner row types = [{singleInnerTypes}]");
            }
            else
            {
                PXTrace.WriteInformation(
                    "Diagnostic single-row print: printContext.SingleRow is NOT a PXResult.");
            }

            if (printContext.Model == null)
                throw new PXException("printContext.Model is null.");

            if (printContext.Printer == null)
                throw new PXException("printContext.Printer is null.");

            PXTrace.WriteInformation(
                $"Diagnostic single-row print context ready: Model={printContext.Model.Name}, Printer={printContext.Printer.Name}, Shipment={shipment.ShipmentNbr}");

            PrintResults results = _labelGenerator.PrintLabels(printContext);

            if (results == null)
                throw new PXException("PrintLabels returned null.");

            if (_licenseManager != null)
            {
                _licenseManager.UpdateFeatureConsumption(_graph.GetType(), results.NbLabels);
            }

            PXTrace.WriteInformation(
                $"Diagnostic single-row print finished: NbLabels={results.NbLabels}");

            return results;
        }

        public virtual PackagePrintSummary TestFirstCheckedALPackagesRow(
            SOShipment shipment,
            Guid? modelId)
        {
            ValidateShipmentForAsgardPrint(shipment);
            EnsureShipmentCurrent(shipment);

            ALModel model = GetModelById(modelId);
            ValidateModelForPackagePrinting(model, modelId);
            TraceModelDiagnostics(model, modelId, shipment);

            TracePackagesRowTypes(shipment);
            TraceALPackagesRowTypes(shipment);

            ALSOShipmentEntryExt asgardExt = _graph.GetExtension<ALSOShipmentEntryExt>();
            if (asgardExt?.ALPackages == null)
                throw new PXException("ALPackages view is not available.");

            foreach (object result in asgardExt.ALPackages.Select())
            {
                SOPackageDetail pkg = PXResult.Unwrap<SOPackageDetail>(result);
                if (pkg == null)
                    continue;

                bool isChecked = IsPackageMarkedForPrint(pkg);

                PXTrace.WriteInformation(
                    $"Diagnostic candidate ALPackages row: lineNbr={pkg.LineNbr}, checked={isChecked}, resultType={result.GetType().FullName}");

                if (!isChecked)
                    continue;

                PrintResults printResult = PrintSingleALPackagesResult(shipment, result, modelId);

                return new PackagePrintSummary
                {
                    SelectedPackageCount = 1,
                    LabelsPrinted = printResult?.NbLabels ?? 0
                };
            }

            throw new PXException(
                "No checked ALPackages rows were found. Please check the Print Label box on at least one package and save the shipment.");
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