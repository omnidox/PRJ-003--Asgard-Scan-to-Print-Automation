import { PXView, PXFieldState, gridConfig, treeConfig, fieldConfig, controlConfig, actionConfig, headerDescription, ICurrencyInfo, disabled, PXFieldOptions, linkCommand, columnConfig, GridColumnShowHideMode, GridColumnType, PXActionState, TextAlign, GridPreset, GridFilterBarVisibility, GridFastFilterVisibility } from "client-controls";


// Views

export class TCTemplateHeader extends PXView  {

	TemplateID : PXFieldState<PXFieldOptions.CommitChanges>;
	TemplateDesc : PXFieldState;
}

@gridConfig({
	showFastFilter: GridFastFilterVisibility.False,
	preset: GridPreset.Details
})
export class TCTemplateDetail extends PXView  {

	@columnConfig({width: 120})	InventoryID : PXFieldState<PXFieldOptions.CommitChanges>;
	@columnConfig({width: 110, textAlign: TextAlign.Left})	PackageSize : PXFieldState;
	@columnConfig({textAlign: TextAlign.Left})	BoxNo : PXFieldState<PXFieldOptions.CommitChanges>;
	@columnConfig({width: 120})	SalesUnit : PXFieldState<PXFieldOptions.CommitChanges>;
}