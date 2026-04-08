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
    ALFormat,
    ALFormat2,
    ALModel,
    ALFormatRule,
    ALFormatRule2,
    ALChangeIDParam,
} from "./views";

@graphInfo({
    graphType: "AA.Objects.Labels.ALFormatMaint",
    primaryView: "Document",
    pageLoadBehavior: PXPageLoadBehavior.SearchSavedKeys,
})
export class AL202000 extends PXScreen {
    Document = createSingle(ALFormat);
    @viewInfo({ containerName: "Hidden Form needed for VisibleExp of TabItems" })
    CurrentDocument = createSingle(ALFormat2);
    @viewInfo({ containerName: "Used by Models" })
    UsedByModels = createCollection(ALModel);
    @viewInfo({ containerName: "Rule Details" })
    Rules = createCollection(ALFormatRule);
    @viewInfo({ containerName: "Used by Rules" })
    UsedByRules = createCollection(ALFormatRule2);
    @viewInfo({ containerName: "Specify New ID" })
    ChangeIDDialog = createSingle(ALChangeIDParam);
}
