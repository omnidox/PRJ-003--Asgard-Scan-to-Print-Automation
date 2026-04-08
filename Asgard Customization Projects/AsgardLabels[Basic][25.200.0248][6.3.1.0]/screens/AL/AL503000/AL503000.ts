import { Messages as SysMessages } from "client-controls/services/messages";
import {
  createCollection,
  createSingle,
  PXScreen,
  graphInfo,
  PXActionState,
  viewInfo,
  handleEvent,
  CustomEventType,
  actionConfig,
  RowSelectedHandlerArgs,
  PXViewCollection,
  PXPageLoadBehavior,
  ControlParameter,
} from "client-controls";
import { ALPrintLogFilter, ALPrintLog, ALPrintLog2 } from "./views";

@graphInfo({
  graphType: "AA.Objects.Labels.ALPrintLogProcess",
  primaryView: "Filter",
})
export class AL503000 extends PXScreen {
  Filter = createSingle(ALPrintLogFilter);
  @viewInfo({ containerName: "Labels" })
  Records = createCollection(ALPrintLog);
  ImageViewer = createSingle(ALPrintLog2);
}
