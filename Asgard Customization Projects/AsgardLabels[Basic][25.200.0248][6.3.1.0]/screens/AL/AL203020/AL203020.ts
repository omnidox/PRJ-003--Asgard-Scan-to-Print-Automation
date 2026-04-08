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
  ALPrinterFile,
  ALPrinterFile2,
  ALDataElement,
  ALFont,
  ALPrinterFileRule,
} from "./views";

@graphInfo({
  graphType: "AA.Objects.Labels.ALPrinterFileMaint",
  primaryView: "Document",
  pageLoadBehavior: PXPageLoadBehavior.SearchSavedKeys,
})
export class AL203020 extends PXScreen {
  Document = createSingle(ALPrinterFile);
  @viewInfo({ containerName: "Hidden Form needed for VisibleExp of TabItems" })
  CurrentDocument = createSingle(ALPrinterFile2);
  @viewInfo({ containerName: "Used By Data Elements" })
  UsedByDataElements = createCollection(ALDataElement);
  @viewInfo({ containerName: "Used By Fonts" })
  UsedByFonts = createCollection(ALFont);
  @viewInfo({ containerName: "Rule Details" })
  Rules = createCollection(ALPrinterFileRule);
}
