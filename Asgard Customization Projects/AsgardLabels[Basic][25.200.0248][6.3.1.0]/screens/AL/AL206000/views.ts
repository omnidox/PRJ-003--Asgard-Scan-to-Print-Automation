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
import { AL206000 } from "./AL206000";

export class ALContentElementType {
  ShortName: PXFieldState;
}

// Views

export class ALContent extends PXView {
  Name: PXFieldState;
  Description: PXFieldState;
  @controlConfig({ displayMode: "id" })
  ScreenID: PXFieldState<PXFieldOptions.CommitChanges>;
  GraphType: PXFieldState<PXFieldOptions.CommitChanges>;
  @controlConfig({ allowEdit: true })
  StandardID: PXFieldState<PXFieldOptions.CommitChanges>;
  @controlConfig({ allowEdit: true })
  FormatID: PXFieldState<PXFieldOptions.CommitChanges>;
  @controlConfig({ allowEdit: true })
  BarcodeID: PXFieldState<PXFieldOptions.CommitChanges>;
  @controlConfig({ allowEdit: true })
  CategoryID: PXFieldState<PXFieldOptions.CommitChanges>;
  Message: PXFieldState;
  Active: PXFieldState<PXFieldOptions.CommitChanges>;
  IsSystem: PXFieldState<PXFieldOptions.CommitChanges>;
  AllowExport: PXFieldState<PXFieldOptions.CommitChanges>;
}

export class ALContent2 extends PXView {
  ImageUrl: PXFieldState;
}

@gridConfig({
  initNewRow: true,
  syncPosition: true,
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.Details,
})
export class ALContentElement extends PXView {
  @columnConfig({ width: 200 }) ContentID: PXFieldState;
  LineNbr: PXFieldState;
  SortOrder: PXFieldState;
  @columnConfig({ width: 60 }) Active: PXFieldState;
  @columnConfig({ width: 100 })
  HriUsage: PXFieldState<PXFieldOptions.CommitChanges>;
  @linkCommand("ViewPreHumanSequence")
  @columnConfig({ width: 80, editorConfig: {} })
  PreHumanSequenceID: PXFieldState<PXFieldOptions.CommitChanges>;
  @linkCommand("ViewIdentifier")
  @columnConfig({ width: 80, editorConfig: {} })
  Identifier: PXFieldState<PXFieldOptions.CommitChanges>;
  @linkCommand("ViewPostHumanSequence")
  @columnConfig({ width: 80, editorConfig: {} })
  PostHumanSequenceID: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 100 }) ALStandardIdentifier: ALContentElementType;
  @linkCommand("ViewRule")
  @columnConfig({ width: 150, editorConfig: {} })
  RuleID: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 70 })
  ReverseRule: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 120 })
  PrePostUsage: PXFieldState<PXFieldOptions.CommitChanges>;
  @linkCommand("ViewPreExprSequence")
  @columnConfig({ width: 150, editorConfig: {} })
  PreExprSequenceID: PXFieldState<PXFieldOptions.CommitChanges>;
  @linkCommand("ViewDataElement")
  @columnConfig({ width: 200, editorConfig: { allowEdit: true } })
  DataElementID: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 90 })
  ExprType: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 200 })
  ExprValue: PXFieldState<PXFieldOptions.CommitChanges>;
  @linkCommand("ViewPostExprSequence")
  @columnConfig({ width: 150, editorConfig: {} })
  PostExprSequenceID: PXFieldState<PXFieldOptions.CommitChanges>;
  @linkCommand("ViewBarcodeSequence")
  @columnConfig({ width: 150, editorConfig: {} })
  BarcodeSequenceID: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 150 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedDateTime: PXFieldState;
}

@gridConfig({
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.ReadOnly,
})
export class ALDataElement extends PXView {
  @linkCommand("ViewModel")
  @columnConfig({ width: 350 })
  Name: PXFieldState;
  @columnConfig({ allowNull: false }) Active: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedDateTime: PXFieldState;
}

export class ALChangeIDParam extends PXView {
  Name: PXFieldState;
}
