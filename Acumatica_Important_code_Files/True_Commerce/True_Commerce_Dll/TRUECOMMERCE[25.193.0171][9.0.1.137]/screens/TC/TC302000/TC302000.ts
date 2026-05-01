import { Messages as SysMessages } from "client-controls/services/messages";
import { createCollection, createSingle, PXScreen, graphInfo, PXActionState, viewInfo,controlConfig,PXFieldState, handleEvent,PXFieldOptions, CustomEventType, actionConfig, RowSelectedHandlerArgs, PXViewCollection, PXPageLoadBehavior, ControlParameter } from "client-controls";
import { TCLabelLogHeader } from "./views";

@graphInfo({graphType: "TCAddon.TCLabelLogEntry", primaryView: "TCLabelLogHeaderRecord", })
export class TC302000 extends PXScreen {	
	TCLabelLogHeaderRecord = createSingle(TCLabelLogHeader);
}