import { PXView, PXFieldState, gridConfig, treeConfig, fieldConfig, controlConfig, actionConfig, headerDescription, ICurrencyInfo, disabled, PXFieldOptions, linkCommand, columnConfig, GridColumnShowHideMode, GridColumnType, PXActionState, TextAlign, GridPreset, GridFilterBarVisibility, GridFastFilterVisibility } from "client-controls";


// Views

export class TCShipmentNos extends PXView  {

	DateFrom : PXFieldState<PXFieldOptions.CommitChanges>;
	ShipmentNbrFrom : PXFieldState<PXFieldOptions.CommitChanges>;
	DocStatus : PXFieldState<PXFieldOptions.CommitChanges>;
	PurchaseOrderNumber : PXFieldState<PXFieldOptions.CommitChanges>;
	EDIDistributionCenter : PXFieldState<PXFieldOptions.CommitChanges>;
	Customer : PXFieldState<PXFieldOptions.CommitChanges>;
	LabelPartner : PXFieldState<PXFieldOptions.CommitChanges>;
	Level : PXFieldState<PXFieldOptions.CommitChanges>;
	DateTo : PXFieldState<PXFieldOptions.CommitChanges>;
	ShipmentNbrTo : PXFieldState<PXFieldOptions.CommitChanges>;
	LabelTemplate : PXFieldState<PXFieldOptions.CommitChanges>;
	ASNType : PXFieldState<PXFieldOptions.CommitChanges>;
	ValidateRequiredField : PXFieldState<PXFieldOptions.CommitChanges>;
	PrintDestination : PXFieldState<PXFieldOptions.CommitChanges>;
}

@gridConfig({
	syncPosition: true,
	autoAdjustColumns: true,
	showFastFilter: GridFastFilterVisibility.False,
	mergeToolbarWith: "ScreenToolbar",
	preset: GridPreset.Inquiry
})
export class TCBatchPrintPacks extends PXView  {

	@columnConfig({allowSort: false, width: 60, allowFilter: false, textAlign: TextAlign.Center, type: GridColumnType.CheckBox})	Selected : PXFieldState;
	@columnConfig({width: 70})	ShipmentNbr : PXFieldState;
	@columnConfig({width: 70})	Customer : PXFieldState;
	@columnConfig({width: 70})	LabelPartner : PXFieldState;
	@columnConfig({width: 70})	UCC128 : PXFieldState;
	@columnConfig({width: 70})	Level : PXFieldState;
	@columnConfig({width: 70})	Location : PXFieldState;
	@columnConfig({width: 70})	PONbr : PXFieldState;
	@columnConfig({width: 70})	DCCode : PXFieldState;
	@columnConfig({width: 70})	StoreNumber : PXFieldState;
	@columnConfig({width: 90})	ShipDate : PXFieldState;
	@columnConfig({width: 140})	PrintStatus : PXFieldState;
	@columnConfig({width: 200, format: "MM/dd/yyyy hh:mm tt"})	PrintDate : PXFieldState;
}