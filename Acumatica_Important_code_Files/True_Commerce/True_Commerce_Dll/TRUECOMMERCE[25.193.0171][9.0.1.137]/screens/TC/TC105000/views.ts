import { PXView, PXFieldState, gridConfig, treeConfig, fieldConfig, controlConfig, actionConfig, headerDescription, ICurrencyInfo, disabled, PXFieldOptions, linkCommand, columnConfig, GridColumnShowHideMode, GridColumnType, PXActionState, TextAlign, GridPreset, GridFilterBarVisibility, GridFastFilterVisibility } from "client-controls";


// Views

export class TCShipFromAddress extends PXView  {

	TCSFAddressID : PXFieldState;
	TCShipFromName : PXFieldState;
	TCShipFromAddress1 : PXFieldState;
	TCShipFromAddress2 : PXFieldState;
	TCShipFromCity : PXFieldState;
	TCShipFromState : PXFieldState<PXFieldOptions.CommitChanges>;
	TCShipFromCountry : PXFieldState<PXFieldOptions.CommitChanges>;
	TCShipFromPostCode : PXFieldState;
	TCShipFromPhoneNo : PXFieldState;
	TCDefaultAddress : PXFieldState;
}