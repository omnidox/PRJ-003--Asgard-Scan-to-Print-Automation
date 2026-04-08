import {
  PXView,
  PXFieldState,
  gridConfig,
  treeConfig,
  fieldConfig,
  controlConfig,
  actionConfig,
  headerDescription,
  ICurrencyInfo,
  disabled,
  PXFieldOptions,
  linkCommand,
  columnConfig,
  GridColumnShowHideMode,
  GridColumnType,
  PXActionState,
  TextAlign,
  GridPreset,
  GridFilterBarVisibility,
  GridFastFilterVisibility,
  ISelectorControlConfig,
  ControlParameter,
} from "client-controls";
import { AL209500 } from "./AL209500";

// Views

export class ALAutoPrint extends PXView {
  Name: PXFieldState;
  Description: PXFieldState;
  @controlConfig({ displayMode: "id" })
  ScreenID: PXFieldState<PXFieldOptions.CommitChanges>;
  GraphType: PXFieldState<PXFieldOptions.CommitChanges>;
  @controlConfig({ allowEdit: true })
  BAccountID: PXFieldState<PXFieldOptions.CommitChanges>;
  ReverseRule: PXFieldState<PXFieldOptions.CommitChanges>;
  @controlConfig({ allowEdit: true })
  RuleID: PXFieldState<PXFieldOptions.CommitChanges>;
  @controlConfig({ allowEdit: true })
  ModelID: PXFieldState<PXFieldOptions.CommitChanges>;
  Active: PXFieldState<PXFieldOptions.CommitChanges>;
  IsSystem: PXFieldState<PXFieldOptions.CommitChanges>;
  AllowExport: PXFieldState<PXFieldOptions.CommitChanges>;
}

export class ALChangeIDParam extends PXView {
  Name: PXFieldState;
}
