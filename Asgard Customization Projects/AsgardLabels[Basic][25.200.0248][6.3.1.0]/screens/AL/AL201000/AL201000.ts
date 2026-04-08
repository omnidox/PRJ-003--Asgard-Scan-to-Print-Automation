import { Messages as SysMessages } from "client-controls/services/messages";
import {
    createCollection,
    createSingle,
    PXScreen,
    graphInfo,
    PXActionState,
    viewInfo,
    handleEvent,
    CustomEventType,
    actionConfig,
    RowSelectedHandlerArgs,
    PXViewCollection,
    PXPageLoadBehavior,
    ControlParameter,
} from "client-controls";
import {
    ALModel,
    ALModel2,
    ALModelExpr,
    ALModelGraphic,
    ALModelPrinter,
    ALModelChild,
    ALDataElement,
    ALPrintLog,
    ALChangeIDParam,
    ALDataElementFilter,
    ALDataElement2,
} from "./views";

@graphInfo({
    graphType: "AA.Objects.Labels.ALModelMaint",
    primaryView: "Model",
})
export class AL201000 extends PXScreen {
    CurrentModel = createSingle(ALModel);
    @viewInfo({ containerName: "Asgard Labels" })
    Model = createSingle(ALModel2);
    @viewInfo({ containerName: "Expressions" })
    Expressions = createCollection(ALModelExpr);
    @viewInfo({ containerName: "Graphics" })
    Graphics = createCollection(ALModelGraphic);
    @viewInfo({ containerName: "Printers" })
    Printers = createCollection(ALModelPrinter);
    @viewInfo({ containerName: "Child Labels" })
    Children = createCollection(ALModelChild);
    @viewInfo({ containerName: "Used By Data Elements" })
    UsedByDataElements = createCollection(ALDataElement);
    @viewInfo({ containerName: "Print Log" })
    PrintLog = createCollection(ALPrintLog);
    @viewInfo({ containerName: "Specify New ID" })
    ChangeIDDialog = createSingle(ALChangeIDParam);
    @viewInfo({ containerName: "DataElementFilter" })
    DataElementFilter = createSingle(ALDataElementFilter);
    @viewInfo({ containerName: "SelectedDataElements" })
    SelectedDataElements = createCollection(ALDataElement2);
    ViewLabelChild: PXActionState;
    loadDataElements: PXActionState;
    ValidateLabelZoom: PXActionState;
    ValidateMongoDb: PXActionState;
}
