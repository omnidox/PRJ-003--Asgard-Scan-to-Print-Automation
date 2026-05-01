import { PXView, PXFieldState, gridConfig, treeConfig, fieldConfig, controlConfig, actionConfig, headerDescription, ICurrencyInfo, disabled, PXFieldOptions, linkCommand, columnConfig, GridColumnShowHideMode, GridColumnType, PXActionState, TextAlign, GridPreset, GridFilterBarVisibility, GridFastFilterVisibility } from "client-controls";


// Views

export class TCUserCredentialTable extends PXView  {

	ServerName : PXFieldState;
	UserName : PXFieldState;
	Password : PXFieldState;
	Tenant : PXFieldState;
	ReportAuth : PXFieldState;
	LabelAuth : PXFieldState;
	IndicatorAuth : PXFieldState;
	ReportHistory : PXFieldState<PXFieldOptions.Disabled>;
	LabelHistory : PXFieldState<PXFieldOptions.Disabled>;
	IndicatorHistory : PXFieldState<PXFieldOptions.Disabled>;
	TCTransactionDisconnect: PXActionState;
	TCLabelDisconnect: PXActionState;
	TCReportDisconnect:PXActionState;
	
}

export class TCLabelConnectFilter extends PXView{

}
export class TCTransactionConnectFilter extends PXView{

}
export class TCReportConnectFilter extends PXView{

}