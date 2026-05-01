import { PXView, PXFieldState, gridConfig, treeConfig, fieldConfig, controlConfig, actionConfig, headerDescription, ICurrencyInfo, disabled, PXFieldOptions, linkCommand, columnConfig, GridColumnShowHideMode, GridColumnType, PXActionState, TextAlign, GridPreset, GridFilterBarVisibility, GridFastFilterVisibility } from "client-controls";


// Views

export class TCCustomerSettings extends PXView  {

	CustomerID : PXFieldState<PXFieldOptions.CommitChanges>;
	TCDefaultShipFromAddress : PXFieldState<PXFieldOptions.CommitChanges>;
	DefaultLabelPartner : PXFieldState<PXFieldOptions.CommitChanges>;
	DefaultLabelTemplate : PXFieldState<PXFieldOptions.CommitChanges>;
	ProcessingUnmatchingLine : PXFieldState<PXFieldOptions.CommitChanges>;
	ValidateUCC128 : PXFieldState<PXFieldOptions.CommitChanges>;
	ComplianceSequence : PXFieldState<PXFieldOptions.CommitChanges>;
	ValidateRequiredFields : PXFieldState<PXFieldOptions.CommitChanges>;
	PullAddFromShipment : PXFieldState<PXFieldOptions.CommitChanges>;
	UCCNumber : PXFieldState;
	UCCExtension : PXFieldState<PXFieldOptions.CommitChanges>;
	UCCCompany : PXFieldState<PXFieldOptions.CommitChanges>;
	UCCNextSerialNo : PXFieldState<PXFieldOptions.CommitChanges>;
	UCCAutoCreate : PXFieldState;
	PrintDestination : PXFieldState;
	TMTransaction : PXFieldState<PXFieldOptions.CommitChanges>;
}

@gridConfig({
	syncPosition: true,
	showFastFilter: GridFastFilterVisibility.False,
	preset: GridPreset.Empty,
	topBarItems: {
	TCLoadFields: {index: 0, config: {commandName: "TCLoadFields", text: "Load Label Fields"}},
	TCRefreshFieldLookups: {index: 1, config: {commandName: "TCRefreshFieldLookups", text: "Load Acumatica Fields"}},
}
})
export class TCLabelFieldMap extends PXView  {

	TCLoadFields : PXActionState;
	TCRefreshFieldLookups : PXActionState;
	LabelField : PXFieldState<PXFieldOptions.CommitChanges>;
	@columnConfig({allowUpdate: false, width: 140, allowFocus: false, textAlign: TextAlign.Left})	LabelFieldLength : PXFieldState;
	@columnConfig({allowUpdate: false, width: 100, allowFocus: false})	Used : PXFieldState;
	@columnConfig({allowUpdate: false, width: 100, allowFocus: false})	Required : PXFieldState;
	@columnConfig({width: 200})	LabelFieldLevel : PXFieldState<PXFieldOptions.CommitChanges>;
	AcumaticaTable : PXFieldState<PXFieldOptions.CommitChanges>;
	AcumaticaField : PXFieldState<PXFieldOptions.CommitChanges>;
}