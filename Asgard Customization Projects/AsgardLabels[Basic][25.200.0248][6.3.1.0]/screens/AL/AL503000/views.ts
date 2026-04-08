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
import { AL503000 } from "./AL503000";

// Views

export class ALPrintLogFilter extends PXView {
  Action: PXFieldState<PXFieldOptions.CommitChanges>;
  StartDate: PXFieldState<PXFieldOptions.CommitChanges>;
  EndDate: PXFieldState<PXFieldOptions.CommitChanges>;
  ContentType: PXFieldState<PXFieldOptions.CommitChanges>;
  UserID: PXFieldState<PXFieldOptions.CommitChanges>;
  OwnerID: PXFieldState<PXFieldOptions.CommitChanges>;
  BAccountID: PXFieldState<PXFieldOptions.CommitChanges>;
  ModelID: PXFieldState<PXFieldOptions.CommitChanges>;
  ScreenID: PXFieldState<PXFieldOptions.CommitChanges>;
  InventoryID: PXFieldState<PXFieldOptions.CommitChanges>;
  LotSerialNbr: PXFieldState<PXFieldOptions.CommitChanges>;
  LabelKey: PXFieldState<PXFieldOptions.CommitChanges>;
  PrinterID: PXFieldState<PXFieldOptions.CommitChanges>;
  NewPrinterID: PXFieldState<PXFieldOptions.CommitChanges>;
  PrintStationID: PXFieldState<PXFieldOptions.CommitChanges>;
  FormatID: PXFieldState<PXFieldOptions.CommitChanges>;
  LabelFilename: PXFieldState<PXFieldOptions.CommitChanges>;
}

@gridConfig({
  syncPosition: true,
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.ReadOnly,
  topBarItems: {
    DoubleClick: {
      index: 0,
      config: { commandName: "DoubleClick", text: "Double Click" },
    },
  },
})
export class ALPrintLog extends PXView {
  DoubleClick: PXActionState;
  @columnConfig({ width: 60 }) Selected: PXFieldState;
  @linkCommand("ViewLog")
  @columnConfig({ textAlign: TextAlign.Right, editorConfig: {} })
  RecordID: PXFieldState;
  @columnConfig({ width: 140, format: "g" }) CreatedDateTime: PXFieldState;
  @linkCommand("ViewBAccount")
  @columnConfig({ width: 150, editorConfig: {} })
  BAccountID: PXFieldState;
  @linkCommand("ViewInventoryItem")
  @columnConfig({ width: 150 })
  InventoryID: PXFieldState;
  @columnConfig({ width: 150 }) LotSerialNbr: PXFieldState;
  @columnConfig({ width: 120 }) LabelKey: PXFieldState;
  @linkCommand("ViewModel")
  @columnConfig({ width: 120 })
  ModelID: PXFieldState;
  @columnConfig({ width: 150 }) LabelFilename: PXFieldState;
  @linkCommand("ViewScreen")
  @columnConfig({ width: 96, editorConfig: { displayMode: "id" } })
  ScreenID: PXFieldState;
  @linkCommand("ViewPrintStation")
  @columnConfig({ width: 150 })
  PrintStationID: PXFieldState;
  @linkCommand("ViewPrinter")
  @columnConfig({ width: 180 })
  PrinterID: PXFieldState;
  @linkCommand("ViewModelFormat")
  @columnConfig({ width: 150 })
  ModelFormatID: PXFieldState;
  @linkCommand("ViewPrinterFormat")
  @columnConfig({ width: 150 })
  PrinterFormatID: PXFieldState;
  @columnConfig({ width: 100 }) UserID: PXFieldState;
  @columnConfig({ width: 100 }) OwnerID: PXFieldState;
  ContentType: PXFieldState;
  @columnConfig({ textAlign: TextAlign.Right }) NbCopies: PXFieldState;
  @columnConfig({ width: 120 })
  ImageUrl: PXFieldState<PXFieldOptions.CommitChanges>;
}

export class ALPrintLog2 extends PXView {
  ImageUrl: PXFieldState;
}
