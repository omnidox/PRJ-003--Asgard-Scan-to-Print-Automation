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
import { ALMargin, ALMargin2, ALModel, ALChangeIDParam } from "./views";

@graphInfo({
  graphType: "AA.Objects.Labels.ALMarginMaint",
  primaryView: "Document",
  pageLoadBehavior: PXPageLoadBehavior.SearchSavedKeys,
})
export class AL205000 extends PXScreen {
  @viewInfo({ containerName: "Rule" })
  Document = createSingle(ALMargin);
  CurrentDocument = createSingle(ALMargin2);
  @viewInfo({ containerName: "Used by Models" })
  UsedByModels = createCollection(ALModel);
  @viewInfo({ containerName: "Specify New ID" })
  ChangeIDDialog = createSingle(ALChangeIDParam);
}
