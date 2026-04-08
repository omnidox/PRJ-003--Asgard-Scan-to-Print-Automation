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
    ALColor,
    ALColor2,
    ALColorRule,
    ALModelExpr,
    ALColorRule2,
    ALChangeIDParam,
} from "./views";

@graphInfo({
    graphType: "AA.Objects.Labels.ALColorMaint",
    primaryView: "Document",
    pageLoadBehavior: PXPageLoadBehavior.SearchSavedKeys,
})
export class AL202500 extends PXScreen {

    Document = createSingle(ALColor);
    @viewInfo({ containerName: "Hidden Form needed for VisibleExp of TabItems" })
    CurrentDocument = createSingle(ALColor2);
    @viewInfo({ containerName: "Rule Details" })
    Rules = createCollection(ALColorRule);
    @viewInfo({ containerName: "Used by Expressions" })
    UsedByExprs = createCollection(ALModelExpr);
    @viewInfo({ containerName: "Used by Rules" })
    UsedByRules = createCollection(ALColorRule2);
    @viewInfo({ containerName: "Specify New ID" })
    ChangeIDDialog = createSingle(ALChangeIDParam);
}
