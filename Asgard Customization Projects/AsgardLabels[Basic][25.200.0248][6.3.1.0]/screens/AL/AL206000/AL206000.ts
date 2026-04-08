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
import {
  ALContent,
  ALContent2,
  ALContentElement,
  ALDataElement,
  ALChangeIDParam,
} from "./views";

@graphInfo({
  graphType: "AA.Objects.Labels.ALContentMaint",
  primaryView: "Document",
})
export class AL206000 extends PXScreen {
  @viewInfo({ containerName: "Content" })
  Document = createSingle(ALContent);
  @viewInfo({ containerName: "Content" })
  CurrentDocument = createSingle(ALContent2);
  @viewInfo({ containerName: "Elements" })
  Elements = createCollection(ALContentElement);
  @viewInfo({ containerName: "Used By Data Elements" })
  UsedByDataElements = createCollection(ALDataElement);
  @viewInfo({ containerName: "Specify New ID" })
  ChangeIDDialog = createSingle(ALChangeIDParam);
}
