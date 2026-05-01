import { Messages as SysMessages } from "client-controls/services/messages";
import { createCollection, createSingle, PXScreen, graphInfo, PXActionState, viewInfo, handleEvent, CustomEventType, actionConfig, RowSelectedHandlerArgs, PXViewCollection, PXPageLoadBehavior, ControlParameter } from "client-controls";
import { TCTemplateHeader, TCTemplateDetail } from "./views";

@graphInfo({graphType: "TCAddon.TCAutoPackTemplateEntry", primaryView: "TemplateHeader", })
export class TC301000 extends PXScreen {

	TemplateHeader = createSingle(TCTemplateHeader);
	TemplateDetails = createCollection(TCTemplateDetail);
}