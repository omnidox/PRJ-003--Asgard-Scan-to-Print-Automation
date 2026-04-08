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
import { AL503500 } from "./AL503500";

// Views

export class ALPrintJobFilter extends PXView {
  Action: PXFieldState<PXFieldOptions.CommitChanges>;
  StartDate: PXFieldState<PXFieldOptions.CommitChanges>;
  EndDate: PXFieldState<PXFieldOptions.CommitChanges>;
  ContentType: PXFieldState<PXFieldOptions.CommitChanges>;
  UserID: PXFieldState<PXFieldOptions.CommitChanges>;
  PrintNodeComputerID: PXFieldState<PXFieldOptions.CommitChanges>;
  PrintNodePrinterID: PXFieldState<PXFieldOptions.CommitChanges>;
}

@gridConfig({
  syncPosition: true,
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.ReadOnly,
})
export class ALPrintJob extends PXView {
  @columnConfig({ width: 60 }) Selected: PXFieldState;
  @linkCommand("ViewJob")
  @columnConfig({ width: 90, textAlign: TextAlign.Right, editorConfig: {} })
  RecordID: PXFieldState;
  @columnConfig({ width: 100, textAlign: TextAlign.Right })
  PrintJobID: PXFieldState;
  @columnConfig({ width: 100 }) State: PXFieldState;
  @columnConfig({ width: 400 }) Title: PXFieldState;
  @columnConfig({ width: 250 }) Source: PXFieldState;
  @columnConfig({ width: 160, format: "g" }) StateDate: PXFieldState;
  @columnConfig({ width: 160, format: "g" }) ReceivedAt: PXFieldState;
  @columnConfig({ width: 160, format: "g" }) ExpiresAt: PXFieldState;
  @columnConfig({ width: 160, format: "g" }) SentToClientAt: PXFieldState;
  @columnConfig({ width: 160, format: "g" }) InProgressAt: PXFieldState;
  @columnConfig({ width: 160, format: "g" }) DoneAt: PXFieldState;
  @columnConfig({ width: 160, format: "g" }) ExpiredAt: PXFieldState;
  @linkCommand("ViewLog")
  @columnConfig({ textAlign: TextAlign.Right })
  PrintLogID: PXFieldState;
}
