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
import { ALSequence, ALSequence2, ALContent, ALChangeIDParam } from "./views";

@graphInfo({
  graphType: "AA.Objects.Labels.ALSequenceMaint",
  primaryView: "Document",
  pageLoadBehavior: PXPageLoadBehavior.SearchSavedKeys,
})
export class AL208000 extends PXScreen {
  @viewInfo({ containerName: "Sequence" })
  Document = createSingle(ALSequence);
  CurrentDocument = createSingle(ALSequence2);
  @viewInfo({ containerName: "Used By Contents" })
  UsedByContents = createCollection(ALContent);
  @viewInfo({ containerName: "Specify New ID" })
  ChangeIDDialog = createSingle(ALChangeIDParam);
}
