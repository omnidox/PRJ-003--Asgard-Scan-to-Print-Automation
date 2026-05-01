import { Messages as SysMessages } from "client-controls/services/messages";
import { createCollection, createSingle, PXScreen, graphInfo, PXActionState, viewInfo, handleEvent, CustomEventType, actionConfig, RowSelectedHandlerArgs, PXViewCollection, PXPageLoadBehavior, ControlParameter } from "client-controls";
import { TCUserCredentialTable,TCLabelConnectFilter,TCReportConnectFilter,TCTransactionConnectFilter } from "./views";

@graphInfo({graphType: "TCAddon.TCReportLogon", primaryView: "ReportLink", })
export class TC103000 extends PXScreen {	
	ReportLink = createSingle(TCUserCredentialTable);
	TCLabelConnectFilterView=createSingle(TCLabelConnectFilter);
	TCLabelDisconnectConfirm:PXActionState;
	TCTransactionConnectFilterView=createSingle(TCTransactionConnectFilter);
	TCReportDisconnectConfirm:PXActionState;
	TCReportConnectFilterView=createSingle(TCReportConnectFilter);
	TCTransactionDisconnectConfirm:PXActionState;
	
}