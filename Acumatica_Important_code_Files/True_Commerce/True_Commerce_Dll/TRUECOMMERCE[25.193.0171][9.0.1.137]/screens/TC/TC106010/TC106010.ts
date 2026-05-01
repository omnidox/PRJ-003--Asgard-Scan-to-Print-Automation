import { Messages as SysMessages } from "client-controls/services/messages";
import { createCollection, createSingle, PXScreen, graphInfo, PXActionState, viewInfo, handleEvent, CustomEventType, actionConfig, RowSelectedHandlerArgs, PXViewCollection, PXPageLoadBehavior, ControlParameter } from "client-controls";
import { TCVersionNumber } from "./views";

@graphInfo({graphType: "TCAddon.TCVersionEntry", primaryView: "TCVersionNumber", })
export class TC106010 extends PXScreen {

	TCVersionNumber = createSingle(TCVersionNumber);
}