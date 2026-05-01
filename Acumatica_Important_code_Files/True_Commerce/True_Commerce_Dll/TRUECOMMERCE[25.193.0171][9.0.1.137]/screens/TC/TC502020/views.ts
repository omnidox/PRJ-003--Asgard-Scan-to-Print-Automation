import { PXView, PXFieldState, gridConfig, treeConfig, fieldConfig, controlConfig, actionConfig, headerDescription, ICurrencyInfo, disabled, PXFieldOptions, linkCommand, columnConfig, GridColumnShowHideMode, GridColumnType, PXActionState, TextAlign, GridPreset, GridFilterBarVisibility, GridFastFilterVisibility } from "client-controls";


// Views

export class TCShipmentFilter extends PXView  {

	OrderNbrFrom : PXFieldState<PXFieldOptions.CommitChanges>;
	DateFrom : PXFieldState<PXFieldOptions.CommitChanges>;
	OrderNbrTo : PXFieldState<PXFieldOptions.CommitChanges>;
	DateTo : PXFieldState<PXFieldOptions.CommitChanges>;
}

@gridConfig({
	syncPosition: true,
	autoAdjustColumns: true,
	showFastFilter: GridFastFilterVisibility.False,
	mergeToolbarWith: "ScreenToolbar",
	preset: GridPreset.Inquiry
})
export class SOShipment extends PXView  {

	@columnConfig({width: 60, type: GridColumnType.CheckBox})	Selected : PXFieldState;
	@columnConfig({width: 140})	ShipmentNbr : PXFieldState<PXFieldOptions.CommitChanges>;
	@columnConfig({width: 70})	ShipmentType : PXFieldState;
	@columnConfig({width: 90})	ShipDate : PXFieldState;
}