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
import { AL202000 } from "./AL202000";

export class ALFormatRuleType {
  Name: PXFieldState;
}

// Views

export class ALFormat extends PXView {
  Name: PXFieldState;
  Active: PXFieldState<PXFieldOptions.CommitChanges>;
  IsSystem: PXFieldState<PXFieldOptions.CommitChanges>;
  AllowExport: PXFieldState<PXFieldOptions.CommitChanges>;
  IsComposite: PXFieldState<PXFieldOptions.CommitChanges>;
  Description: PXFieldState;
  PrintDensityType: PXFieldState<PXFieldOptions.CommitChanges>;
  PrintDensity: PXFieldState<PXFieldOptions.CommitChanges>;
  Rotation: PXFieldState;
  Width: PXFieldState;
  Height: PXFieldState;
  SizeUnit: PXFieldState;
  WidthDots: PXFieldState;
  HeightDots: PXFieldState;
  @controlConfig({ allowEdit: true })
  MarginID: PXFieldState<PXFieldOptions.CommitChanges>;
  CategoryID: PXFieldState;
  UseWithPdf: PXFieldState<PXFieldOptions.CommitChanges>;
  PageSize: PXFieldState<PXFieldOptions.CommitChanges>;
  PageOrientation: PXFieldState<PXFieldOptions.CommitChanges>;
  PageHAlign: PXFieldState<PXFieldOptions.CommitChanges>;
  PageVAlign: PXFieldState<PXFieldOptions.CommitChanges>;
}

export class ALFormat2 extends PXView {
  ShowChildren: PXFieldState;
}

export class ALModel extends PXView {
  @linkCommand("ViewModel")
  @columnConfig({ width: 300, editorConfig: {} })
  Name: PXFieldState;
  @columnConfig({ width: 300 }) Description: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedDateTime: PXFieldState;
}

@gridConfig({
  initNewRow: true,
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.Details,
})
export class ALFormatRule extends PXView {
  @columnConfig({ allowNull: false })
  Active: PXFieldState<PXFieldOptions.CommitChanges>;
  LineNbr: PXFieldState;
  SortOrder: PXFieldState;
  @linkCommand("ViewBAccount")
  @columnConfig({ width: 250, editorConfig: {} })
  BAccountID: PXFieldState<PXFieldOptions.CommitChanges>;
  @linkCommand("ViewRule")
  @columnConfig({ width: 250, editorConfig: {} })
  RuleID: PXFieldState<PXFieldOptions.CommitChanges>;
  ReverseRule: PXFieldState;
  @linkCommand("ViewFormat")
  @columnConfig({ width: 250, editorConfig: {} })
  ChildFormatID: PXFieldState<PXFieldOptions.CommitChanges>;
  DoThrow: PXFieldState;
  @columnConfig({ width: 500 }) Message: PXFieldState;
}

@gridConfig({
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.ReadOnly,
})
export class ALFormatRule2 extends PXView {
  @linkCommand("ViewFormat")
  @columnConfig({ width: 300 })
  ALFormat: ALFormatRuleType;
  @linkCommand("ViewRule")
  @columnConfig({ width: 300 })
  ALRule: ALFormatRuleType;
  @columnConfig({ width: 150 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedDateTime: PXFieldState;
}

export class ALChangeIDParam extends PXView {
  Name: PXFieldState;
}
