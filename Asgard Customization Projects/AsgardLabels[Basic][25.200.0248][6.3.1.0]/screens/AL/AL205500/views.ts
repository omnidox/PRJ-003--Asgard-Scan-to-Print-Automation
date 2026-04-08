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
import { AL205500 } from "./AL205500";

// Views

export class ALStandard extends PXView {
  Name: PXFieldState;
  Description: PXFieldState;
  Active: PXFieldState<PXFieldOptions.CommitChanges>;
  IsSystem: PXFieldState<PXFieldOptions.CommitChanges>;
  AllowExport: PXFieldState<PXFieldOptions.CommitChanges>;
}

export class ALStandard2 extends PXView {}

@gridConfig({
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.Details,
})
export class ALStandardIdentifier extends PXView {
  @columnConfig({ width: 100 }) Identifier: PXFieldState;
  @columnConfig({ width: 400 }) Description: PXFieldState;
  @columnConfig({ width: 200 }) ShortName: PXFieldState;
  @columnConfig({ width: 200 }) Regex: PXFieldState;
  @columnConfig({ width: 80 }) FixedLength: PXFieldState;
  @columnConfig({ width: 200, editorConfig: {} }) CategoryCode: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedDateTime: PXFieldState;
}

@gridConfig({
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.ReadOnly,
})
export class ALContent extends PXView {
  @columnConfig({ allowNull: false }) Active: PXFieldState;
  @linkCommand("ViewModel")
  @columnConfig({ width: 350 })
  Name: PXFieldState;
  @columnConfig({ width: 300 }) Description: PXFieldState;
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

@gridConfig({
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.Details,
})
export class ALStandardCategory extends PXView {
  @columnConfig({ width: 100 }) CategoryCode: PXFieldState;
  @columnConfig({ width: 400 }) Description: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedDateTime: PXFieldState;
}

export class ALChangeIDParam extends PXView {
  Name: PXFieldState;
}
