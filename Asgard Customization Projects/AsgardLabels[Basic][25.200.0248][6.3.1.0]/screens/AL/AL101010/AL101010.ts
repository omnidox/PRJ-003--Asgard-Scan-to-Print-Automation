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
  ALSetup,
  ALPrinterFile,
  ALPrinterFile2,
  ALPrinterFile3,
} from "./views";

@graphInfo({
  graphType: "AA.Objects.Labels.ALPrinterFileProcess",
  primaryView: "Setup",
})
export class AL101010 extends PXScreen {
  Setup = createSingle(ALSetup);
  @viewInfo({ containerName: "Images" })
  ImageFiles = createCollection(ALPrinterFile);
  @viewInfo({ containerName: "Fonts" })
  FontFiles = createCollection(ALPrinterFile2);
  @viewInfo({ containerName: "Others" })
  OtherFiles = createCollection(ALPrinterFile3);
}
