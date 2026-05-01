import { PXView, PXFieldState, gridConfig, treeConfig, fieldConfig, controlConfig, actionConfig, headerDescription, ICurrencyInfo, disabled, PXFieldOptions, linkCommand, columnConfig, GridColumnShowHideMode, GridColumnType, PXActionState, TextAlign, GridPreset, GridFilterBarVisibility, GridFastFilterVisibility } from "client-controls";


// Views

export class TCLabelSettings extends PXView  {

	LabelPrintPageSize : PXFieldState;
	EnableLogLabelXml : PXFieldState;
	KeepLogInDays : PXFieldState;
	UCCNumber : PXFieldState;
	UCCExtension : PXFieldState<PXFieldOptions.CommitChanges>;
	UCCCompany : PXFieldState<PXFieldOptions.CommitChanges>;
	UCCNextSerialNo : PXFieldState<PXFieldOptions.CommitChanges>;
	UCCAutoCreate : PXFieldState;
}