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
  ALStandard,
  ALStandard2,
  ALStandardIdentifier,
  ALContent,
  ALDataElement,
  ALStandardCategory,
  ALChangeIDParam,
} from "./views";

@graphInfo({
  graphType: "AA.Objects.Labels.ALStandardMaint",
  primaryView: "Document",
  pageLoadBehavior: PXPageLoadBehavior.SearchSavedKeys,
})
export class AL205500 extends PXScreen {
  @viewInfo({ containerName: "Standard" })
  Document = createSingle(ALStandard);
  CurrentDocument = createSingle(ALStandard2);
  @viewInfo({ containerName: "Identifiers" })
  Details = createCollection(ALStandardIdentifier);
  @viewInfo({ containerName: "Used By Contents" })
  UsedByContents = createCollection(ALContent);
  @viewInfo({ containerName: "Used By Data Elements" })
  UsedByDataElements = createCollection(ALDataElement);
  @viewInfo({ containerName: "Categories" })
  Categories = createCollection(ALStandardCategory);
  @viewInfo({ containerName: "Specify New ID" })
  ChangeIDDialog = createSingle(ALChangeIDParam);
}
