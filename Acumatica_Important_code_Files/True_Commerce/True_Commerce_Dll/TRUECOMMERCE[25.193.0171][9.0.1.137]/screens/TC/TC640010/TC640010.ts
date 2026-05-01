import { Messages as SysMessages } from "client-controls/services/messages";
import { createCollection, createSingle, PXScreen, graphInfo, PXActionState, viewInfo, handleEvent, CustomEventType, actionConfig, RowSelectedHandlerArgs, PXViewCollection, PXPageLoadBehavior, ControlParameter } from "client-controls";
import { TCReportFilter } from "./views";

@graphInfo({graphType: "TCAddon.TCReportDisplayEntry1", primaryView: "ReportCredential", })
export class TC640010 extends PXScreen {

	ReportCredential = createSingle(TCReportFilter);
}