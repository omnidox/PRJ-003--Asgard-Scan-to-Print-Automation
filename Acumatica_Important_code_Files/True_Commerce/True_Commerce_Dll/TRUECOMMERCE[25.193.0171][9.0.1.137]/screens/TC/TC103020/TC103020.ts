import { Messages as SysMessages } from "client-controls/services/messages";
import { createCollection, createSingle, PXScreen, graphInfo, PXActionState, viewInfo, handleEvent, CustomEventType, actionConfig, RowSelectedHandlerArgs, PXViewCollection, PXPageLoadBehavior, ControlParameter } from "client-controls";
import { TCReportSetting, TCReportDetail } from "./views";

@graphInfo({graphType: "TCAddon.TCReportSettingEntry", primaryView: "ReportSetting", })
export class TC103020 extends PXScreen {

	ReportSetting = createSingle(TCReportSetting);
	ReportDetails = createCollection(TCReportDetail);
}