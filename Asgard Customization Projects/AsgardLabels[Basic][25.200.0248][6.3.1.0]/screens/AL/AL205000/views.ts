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
import { AL205000 } from "./AL205000";

// Views

export class ALMargin extends PXView {
  Name: PXFieldState;
  Description: PXFieldState;
  CategoryID: PXFieldState;
  SizeUnit: PXFieldState<PXFieldOptions.CommitChanges>;
  Left: PXFieldState<PXFieldOptions.CommitChanges>;
  Right: PXFieldState<PXFieldOptions.CommitChanges>;
  Top: PXFieldState<PXFieldOptions.CommitChanges>;
  Bottom: PXFieldState;
  Active: PXFieldState<PXFieldOptions.CommitChanges>;
  IsSystem: PXFieldState<PXFieldOptions.CommitChanges>;
  AllowExport: PXFieldState<PXFieldOptions.CommitChanges>;
}

export class ALMargin2 extends PXView {}

@gridConfig({
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.ReadOnly,
})
export class ALModel extends PXView {
  @linkCommand("ViewModel")
  @columnConfig({ width: 300, editorConfig: {} })
  Name: PXFieldState;
  @columnConfig({ width: 300 }) Description: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedDateTime: PXFieldState;
}

export class ALChangeIDParam extends PXView {
  Name: PXFieldState;
}
