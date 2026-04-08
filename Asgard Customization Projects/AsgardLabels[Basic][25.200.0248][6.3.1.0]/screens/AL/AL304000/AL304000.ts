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
import { ALJustification, ALModelExpr, ALChangeIDParam } from "./views";

@graphInfo({
  graphType: "AA.Objects.Labels.ALJustificationMaint",
  primaryView: "Document",
})
export class AL304000 extends PXScreen {
  Document = createSingle(ALJustification);
  @viewInfo({ containerName: "Used By Models" })
  UsedByModels = createCollection(ALModelExpr);
  @viewInfo({ containerName: "Specify New ID" })
  ChangeIDDialog = createSingle(ALChangeIDParam);
}
