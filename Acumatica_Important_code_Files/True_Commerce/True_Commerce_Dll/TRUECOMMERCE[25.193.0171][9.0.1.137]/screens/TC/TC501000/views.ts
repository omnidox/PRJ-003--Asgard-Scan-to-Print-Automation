import { PXView, PXFieldState, gridConfig, treeConfig, fieldConfig, controlConfig, actionConfig, headerDescription, ICurrencyInfo, disabled, PXFieldOptions, linkCommand, columnConfig, GridColumnShowHideMode, GridColumnType, PXActionState, TextAlign, GridPreset, GridFilterBarVisibility, GridFastFilterVisibility } from "client-controls";


// Views

export class TCHeaderTable extends PXView  {

	OrderNbrFrom : PXFieldState<PXFieldOptions.CommitChanges>;
	DateFrom : PXFieldState<PXFieldOptions.CommitChanges>;
	AutoPack : PXFieldState;
	OrderNbrTo : PXFieldState<PXFieldOptions.CommitChanges>;
	DateTo : PXFieldState<PXFieldOptions.CommitChanges>;
	ShipmentConfirm : PXFieldState;
}

@gridConfig({
	syncPosition: true,
	autoAdjustColumns: true,
	fastFilterByAllFields: false,
	showFastFilter: GridFastFilterVisibility.ToolBar,
	mergeToolbarWith: "ScreenToolbar",
	preset: GridPreset.Inquiry
})
export class SOOrder extends PXView  {

	@columnConfig({allowNull: false, width: 60, allowFilter: false, textAlign: TextAlign.Center, type: GridColumnType.CheckBox})	Selected : PXFieldState;
	@columnConfig({width: 70})	OrderType : PXFieldState;
	@linkCommand("ViewDocument")
	@columnConfig({allowUpdate: false, width: 100, allowFastFilter: true})	OrderNbr : PXFieldState;
	@columnConfig({allowUpdate: false, width: 200, allowFastFilter: true})	UsrTCPONumber : PXFieldState;
	@columnConfig({allowUpdate: false, hideViewLink: true, width: 120, allowFastFilter: true})	CustomerID : PXFieldState;
	@columnConfig({width: 90})	OrderDate : PXFieldState;
}