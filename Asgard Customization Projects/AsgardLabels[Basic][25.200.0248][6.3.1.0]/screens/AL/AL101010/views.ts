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
import { AL101010 } from "./AL101010";

// Views

export class ALSetup extends PXView {}

@gridConfig({
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.Details,
})
export class ALPrinterFile extends PXView {
  @columnConfig({ width: 60 }) Active: PXFieldState;
  Status: PXFieldState;
  @linkCommand("ViewPrinterFile")
  @columnConfig({ width: 270 })
  Name: PXFieldState;
  @columnConfig({ width: 170 }) FileName: PXFieldState;
  @columnConfig({ width: 128 }) ShortFileName: PXFieldState;
  @columnConfig({ width: 90 }) Extension: PXFieldState;
  @columnConfig({ width: 280 }) Description: PXFieldState;
  Size: PXFieldState;
  Width: PXFieldState;
  Height: PXFieldState;
  @columnConfig({ width: 120 }) PixelFormat: PXFieldState;
  @columnConfig({ width: 100 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 100 }) LastModifiedDateTime: PXFieldState;
  ID: PXFieldState;
}

@gridConfig({
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.Details,
})
export class ALPrinterFile2 extends PXView {
  @columnConfig({ width: 60 }) Active: PXFieldState;
  Status: PXFieldState;
  @linkCommand("ViewPrinterFile")
  @columnConfig({ width: 180 })
  Name: PXFieldState;
  @columnConfig({ width: 160 }) FileName: PXFieldState;
  @columnConfig({ width: 120 }) ShortFileName: PXFieldState;
  @columnConfig({ width: 90 }) Extension: PXFieldState;
  @columnConfig({ width: 280 }) Description: PXFieldState;
  FontStyle: PXFieldState;
  Size: PXFieldState;
  Width: PXFieldState;
  Height: PXFieldState;
  Ascent: PXFieldState;
  Descent: PXFieldState;
  LineSpacing: PXFieldState;
  @columnConfig({ width: 100 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 100 }) LastModifiedDateTime: PXFieldState;
  ID: PXFieldState;
}

@gridConfig({
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.Details,
})
export class ALPrinterFile3 extends PXView {
  @columnConfig({ width: 60 }) Active: PXFieldState;
  Status: PXFieldState;
  @linkCommand("ViewPrinterFile")
  @columnConfig({ width: 180 })
  Name: PXFieldState;
  @columnConfig({ width: 170 }) FileName: PXFieldState;
  @columnConfig({ width: 120 }) ShortFileName: PXFieldState;
  @columnConfig({ width: 90 }) Extension: PXFieldState;
  @columnConfig({ width: 600 }) Description: PXFieldState;
  Size: PXFieldState;
  @columnConfig({ width: 100 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 100 }) LastModifiedDateTime: PXFieldState;
  ID: PXFieldState;
}
