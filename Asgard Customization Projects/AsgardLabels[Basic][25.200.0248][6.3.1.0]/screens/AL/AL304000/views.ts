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
import { AL304000 } from "./AL304000";

export class ALModelExprType {
  Name: PXFieldState;
  Description: PXFieldState;
}

// Views

export class ALJustification extends PXView {
  Name: PXFieldState;
  Description: PXFieldState;
  Justification: PXFieldState<PXFieldOptions.CommitChanges>;
  CategoryID: PXFieldState;
  FromX: PXFieldState<PXFieldOptions.CommitChanges>;
  ToX: PXFieldState<PXFieldOptions.CommitChanges>;
  MaxLines: PXFieldState<PXFieldOptions.CommitChanges>;
  SizeUnit: PXFieldState<PXFieldOptions.CommitChanges>;
  SpaceBetweenLines: PXFieldState<PXFieldOptions.CommitChanges>;
  HangingIndent: PXFieldState<PXFieldOptions.CommitChanges>;
  Active: PXFieldState<PXFieldOptions.CommitChanges>;
  IsSystem: PXFieldState<PXFieldOptions.CommitChanges>;
  AllowExport: PXFieldState<PXFieldOptions.CommitChanges>;
}

@gridConfig({
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.ReadOnly,
})
export class ALModelExpr extends PXView {
  @columnConfig({ allowNull: false }) Active: PXFieldState;
  @linkCommand("ViewModel")
  @columnConfig({ width: 200 })
  ALModel: ALModelExprType;
  @columnConfig({ width: 200 }) ExprCode: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedDateTime: PXFieldState;
}

export class ALChangeIDParam extends PXView {
  Name: PXFieldState;
}
