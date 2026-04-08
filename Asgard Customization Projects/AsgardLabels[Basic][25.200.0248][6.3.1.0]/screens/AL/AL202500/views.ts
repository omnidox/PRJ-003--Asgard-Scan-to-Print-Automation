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
import { AL202500 } from "./AL202500";

export class ALModelExprType {
  Name: PXFieldState;
  Description: PXFieldState;
}
export class ALColorRuleType {
  Name: PXFieldState;
}

// Views

export class ALColor extends PXView {
  Name: PXFieldState;
  Description: PXFieldState;
  PrimaryColor: PXFieldState<PXFieldOptions.CommitChanges>;
  CategoryID: PXFieldState;
  Active: PXFieldState<PXFieldOptions.CommitChanges>;
  IsSystem: PXFieldState<PXFieldOptions.CommitChanges>;
  IsComposite: PXFieldState<PXFieldOptions.CommitChanges>;
  AllowExport: PXFieldState<PXFieldOptions.CommitChanges>;
  Alpha: PXFieldState<PXFieldOptions.Disabled>;
  Red: PXFieldState<PXFieldOptions.Disabled>;
  Green: PXFieldState<PXFieldOptions.Disabled>;
  Blue: PXFieldState<PXFieldOptions.Disabled>;
}

export class ALColor2 extends PXView {
  ShowChildren: PXFieldState;
}

export class ALColorRule extends PXView {
  @linkCommand("ViewBAccount")
  @columnConfig({ width: 250, editorConfig: {} })
  BAccountID: PXFieldState<PXFieldOptions.CommitChanges>;
  @linkCommand("ViewColor")
  @columnConfig({ width: 250, editorConfig: {} })
  ChildColorID: PXFieldState<PXFieldOptions.CommitChanges>;
  @linkCommand("ViewRule")
  @columnConfig({ width: 250, editorConfig: {} })
  RuleID: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ allowNull: false })
  Active: PXFieldState<PXFieldOptions.CommitChanges>;
  LineNbr: PXFieldState;
  SortOrder: PXFieldState;
  ReverseRule: PXFieldState;
  DoThrow: PXFieldState;
  @columnConfig({ width: 500 }) Message: PXFieldState;
}

@gridConfig({
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.ReadOnly,
})
export class ALModelExpr extends PXView {
  @linkCommand("ViewModel")
  @columnConfig({ width: 300 })
  ALModel: ALModelExprType;
  @linkCommand("ViewModelExpr")
  @columnConfig({ width: 300 })
  ExprCode: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedDateTime: PXFieldState;
}

@gridConfig({
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.ReadOnly,
})
export class ALColorRule2 extends PXView {
  @linkCommand("ViewColor")
  @columnConfig({ width: 300 })
  ALColor: ALColorRuleType;
  @linkCommand("ViewRule")
  @columnConfig({ width: 300 })
  ALRule: ALColorRuleType;
  @columnConfig({ width: 150 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedDateTime: PXFieldState;
}

export class ALChangeIDParam extends PXView {
  Name: PXFieldState;
}
