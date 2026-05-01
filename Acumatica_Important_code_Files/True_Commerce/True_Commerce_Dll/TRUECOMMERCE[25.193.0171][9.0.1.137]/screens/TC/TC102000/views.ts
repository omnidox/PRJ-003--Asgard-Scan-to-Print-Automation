import { PXView, PXFieldState, gridConfig, treeConfig, fieldConfig, controlConfig, actionConfig, headerDescription, ICurrencyInfo, disabled, PXFieldOptions, linkCommand, columnConfig, GridColumnShowHideMode, GridColumnType, PXActionState, TextAlign, GridPreset, GridFilterBarVisibility, GridFastFilterVisibility } from "client-controls";


// Views

export class TCCustomerSettings extends PXView  {

	CustomerID : PXFieldState<PXFieldOptions.CommitChanges>;
	AsnType : PXFieldState;
	AutoPackType : PXFieldState<PXFieldOptions.CommitChanges>;
	TemplateID : PXFieldState<PXFieldOptions.CommitChanges>;
}