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
import { ALPrintJobFilter, ALPrintJob } from "./views";

@graphInfo({
  graphType: "AA.Objects.Labels.ALPrintJobProcess",
  primaryView: "Filter",
})
export class AL503500 extends PXScreen {
  Filter = createSingle(ALPrintJobFilter);
  @viewInfo({ containerName: "Jobs" })
  Records = createCollection(ALPrintJob);
}
