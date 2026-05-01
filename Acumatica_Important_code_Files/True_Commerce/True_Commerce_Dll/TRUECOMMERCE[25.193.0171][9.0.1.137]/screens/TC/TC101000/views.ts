import { PXView, PXFieldState, gridConfig, treeConfig, fieldConfig, controlConfig, actionConfig, headerDescription, ICurrencyInfo, disabled, PXFieldOptions, linkCommand, columnConfig, GridColumnShowHideMode, GridColumnType, PXActionState, TextAlign, GridPreset, GridFilterBarVisibility, GridFastFilterVisibility } from "client-controls";


// Views

export class TCInventoryFilter extends PXView  {

	InventoryID : PXFieldState<PXFieldOptions.CommitChanges>;
}

@gridConfig({
	allowDelete: false,
	allowInsert: false,
	showFastFilter: GridFastFilterVisibility.False,
	preset: GridPreset.ReadOnly
})
export class InventoryItem extends PXView  {

	@columnConfig({hideViewLink: true, width: 100})	InventoryCD : PXFieldState;
	@columnConfig({width: 200})	Descr : PXFieldState;
	@columnConfig({width: 150, textAlign: TextAlign.Right})	UsrTCPackageSize : PXFieldState;
	@columnConfig({width: 120})	SalesUnit : PXFieldState;
	@columnConfig({width: 100, textAlign: TextAlign.Right})	BasePrice : PXFieldState;
	@columnConfig({width: 100, textAlign: TextAlign.Right})	BaseItemWeight : PXFieldState;
}