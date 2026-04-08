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
import { AL207000 } from "./AL207000";

export class ALModelExprType {
  Name: PXFieldState;
  Description: PXFieldState;
}

// Views

export class ALFont extends PXView {
  Name: PXFieldState;
  Description: PXFieldState;
  Language: PXFieldState<PXFieldOptions.CommitChanges>;
  @controlConfig({ allowEdit: true })
  FontFileID: PXFieldState<PXFieldOptions.CommitChanges>;
  FontType: PXFieldState<PXFieldOptions.CommitChanges>;
  Family: PXFieldState<PXFieldOptions.CommitChanges>;
  Style: PXFieldState<PXFieldOptions.CommitChanges>;
  Height: PXFieldState<PXFieldOptions.CommitChanges>;
  Width: PXFieldState<PXFieldOptions.CommitChanges>;
  SizeUnit: PXFieldState<PXFieldOptions.CommitChanges>;
  Active: PXFieldState<PXFieldOptions.CommitChanges>;
  IsSystem: PXFieldState<PXFieldOptions.CommitChanges>;
  AllowExport: PXFieldState<PXFieldOptions.CommitChanges>;
  @controlConfig({ allowEdit: true })
  FormatID: PXFieldState<PXFieldOptions.CommitChanges>;
  CategoryID: PXFieldState;
  SampleValue: PXFieldState;
  Message: PXFieldState;
}

export class ALFont2 extends PXView {
  ImageUrl: PXFieldState;
}

export class ALModelExpr extends PXView {
  @linkCommand("ViewModel")
  @columnConfig({ width: 300, editorConfig: {} })
  ALModel: ALModelExprType;
  @columnConfig({ width: 100 }) LineNbr: PXFieldState;
  @linkCommand("ViewDataElement")
  @columnConfig({ width: 300 })
  ALDataElement: ALModelExprType;
  @columnConfig({ width: 300 }) ExprValue: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedDateTime: PXFieldState;
}

export class ALChangeIDParam extends PXView {
  Name: PXFieldState;
}
