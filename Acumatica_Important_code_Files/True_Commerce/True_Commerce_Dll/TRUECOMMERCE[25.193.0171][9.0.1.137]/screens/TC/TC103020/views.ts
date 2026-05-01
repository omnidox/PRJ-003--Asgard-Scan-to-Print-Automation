import { PXView, PXFieldState, gridConfig, treeConfig, fieldConfig, controlConfig, actionConfig, headerDescription, ICurrencyInfo, disabled, PXFieldOptions, linkCommand, columnConfig, GridColumnShowHideMode, GridColumnType, PXActionState, TextAlign, GridPreset, GridFilterBarVisibility, GridFastFilterVisibility } from "client-controls";


// Views

export class TCReportSetting extends PXView  {

	ReportID : PXFieldState;
	DashBoardMenuName : PXFieldState<PXFieldOptions.Disabled>;
	CompanyName : PXFieldState;
	ProductName : PXFieldState;
	KPIViewName : PXFieldState;
}

@gridConfig({
	showFastFilter: GridFastFilterVisibility.False,
	preset: GridPreset.Details
})
export class TCReportDetail extends PXView  {

	@columnConfig({width: 220})	KPI : PXFieldState<PXFieldOptions.CommitChanges>;
	@columnConfig({width: 220})	PanelTitle : PXFieldState;
	@columnConfig({width: 280})	TxnType : PXFieldState;
	@columnConfig({width: 280})	TradingPartner : PXFieldState;
	@columnConfig({width: 90})	DateFrom : PXFieldState;
	@columnConfig({width: 90})	DateTo : PXFieldState;
}