import { PXView, PXFieldState, gridConfig, treeConfig, fieldConfig, controlConfig, actionConfig, headerDescription, ICurrencyInfo, disabled, PXFieldOptions, linkCommand, columnConfig, GridColumnShowHideMode, GridColumnType, PXActionState, TextAlign, GridPreset, GridFilterBarVisibility, GridFastFilterVisibility } from "client-controls";


// Views

export class TCLabelLogHeader extends PXView  {

	LogName: PXFieldState<PXFieldOptions.Disabled>;
	LogDate: PXFieldState<PXFieldOptions.Disabled>;
	LogPrintStatus: PXFieldState<PXFieldOptions.Disabled>;
	ErrorMessage: PXFieldState<PXFieldOptions.Disabled>;
	@controlConfig({ rows: 75 })
	LabelXml: PXFieldState<PXFieldOptions.Disabled | PXFieldOptions.Multiline>;
}