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
  ALBarcode,
  ALBarcode2,
  ALBarcodeOption,
  ALDataElement,
  ALChangeIDParam,
} from "./views";

@graphInfo({
  graphType: "AA.Objects.Labels.ALBarcodeMaint",
  primaryView: "Document",
  pageLoadBehavior: PXPageLoadBehavior.SearchSavedKeys,
})
export class AL204000 extends PXScreen {
  @viewInfo({ containerName: "Rule" })
  Document = createSingle(ALBarcode);
  @viewInfo({ containerName: "Rule" })
  CurrentDocument = createSingle(ALBarcode2);
  @viewInfo({ containerName: "Options" })
  Options = createCollection(ALBarcodeOption);
  @viewInfo({ containerName: "Used By Data Elements" })
  UsedByDataElements = createCollection(ALDataElement);
  @viewInfo({ containerName: "Specify New ID" })
  ChangeIDDialog = createSingle(ALChangeIDParam);
}
