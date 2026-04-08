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
import { ALPrintJob } from "./views";

@graphInfo({
  graphType: "AA.Objects.Labels.ALPrintJobMaint",
  primaryView: "Document",
  pageLoadBehavior: PXPageLoadBehavior.SearchSavedKeys,
})
export class AL503510 extends PXScreen {
  Document = createSingle(ALPrintJob);
}
