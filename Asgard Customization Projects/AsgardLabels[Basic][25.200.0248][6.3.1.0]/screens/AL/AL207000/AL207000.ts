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
import { ALFont, ALFont2, ALModelExpr, ALChangeIDParam } from "./views";

@graphInfo({
  graphType: "AA.Objects.Labels.ALFontMaint",
  primaryView: "Document",
  pageLoadBehavior: PXPageLoadBehavior.SearchSavedKeys,
})
export class AL207000 extends PXScreen {
  @viewInfo({ containerName: "Rule" })
  Document = createSingle(ALFont);
  @viewInfo({ containerName: "Rule" })
  CurrentDocument = createSingle(ALFont2);
  @viewInfo({ containerName: "Used By Models" })
  UsedByModels = createCollection(ALModelExpr);
  @viewInfo({ containerName: "Specify New ID" })
  ChangeIDDialog = createSingle(ALChangeIDParam);
}
