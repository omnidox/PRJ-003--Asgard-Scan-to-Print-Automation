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
import { AL503510 } from "./AL503510";

// Views

export class ALPrintJob extends PXView {
  RecordID: PXFieldState;
  PrintJobID: PXFieldState;
  PrintNodeComputerID: PXFieldState;
  PrintNodePrinterID: PXFieldState;
  State: PXFieldState;
  Title: PXFieldState;
  Source: PXFieldState;
  CreatedDateTime: PXFieldState;
  ReceivedAt: PXFieldState;
  SentToClientAt: PXFieldState;
  QueuedAt: PXFieldState;
  InProgressAt: PXFieldState;
  DoneAt: PXFieldState;
  ExpiredAt: PXFieldState;
  StateDate: PXFieldState;
  LastModifiedDateTime: PXFieldState;
  PrintLogID: PXFieldState;
}
