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
import { AL203020 } from "./AL203020";

// Views

export class ALPrinterFile extends PXView {
  Name: PXFieldState;
  Active: PXFieldState<PXFieldOptions.CommitChanges>;
  AllowExport: PXFieldState;
  IsComposite: PXFieldState<PXFieldOptions.CommitChanges>;
  Description: PXFieldState;
  FontStyle: PXFieldState<PXFieldOptions.CommitChanges>;
  FileName: PXFieldState<PXFieldOptions.CommitChanges>;
  ShortFileName: PXFieldState<PXFieldOptions.CommitChanges>;
  Status: PXFieldState<PXFieldOptions.CommitChanges>;
  Extension: PXFieldState;
  Size: PXFieldState;
  Width: PXFieldState;
  Height: PXFieldState;
  MaxWidth: PXFieldState;
  MaxHeight: PXFieldState;
  PixelFormat: PXFieldState;
  Ascent: PXFieldState;
  Descent: PXFieldState;
  LineSpacing: PXFieldState;
}

export class ALPrinterFile2 extends PXView {
  ShowChildren: PXFieldState;
  ImageUrl: PXFieldState;
}

@gridConfig({
  syncPosition: true,
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.ReadOnly,
})
export class ALDataElement extends PXView {
  @linkCommand("ViewDataElement")
  @columnConfig({ width: 180, editorConfig: {} })
  Name: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedDateTime: PXFieldState;
}

@gridConfig({
  syncPosition: true,
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.ReadOnly,
})
export class ALFont extends PXView {
  @linkCommand("ViewFont")
  @columnConfig({ width: 180, editorConfig: {} })
  Name: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedDateTime: PXFieldState;
}

@gridConfig({
  initNewRow: true,
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.Details,
})
export class ALPrinterFileRule extends PXView {
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
  @linkCommand("ViewrinterFile")
  @columnConfig({ width: 250, editorConfig: {} })
  ChildPrinterFileID: PXFieldState<PXFieldOptions.CommitChanges>;
  Height: PXFieldState;
  Width: PXFieldState;
  DoThrow: PXFieldState;
  @columnConfig({ width: 500 }) Message: PXFieldState;
}
