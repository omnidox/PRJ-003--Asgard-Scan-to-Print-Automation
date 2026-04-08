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
  ALCategory,
  ALCategory2,
  ALModel,
  ALDataElement,
  ALContent,
  ALSubstitution,
  ALColor,
  ALRule,
  ALFormat,
  ALMargin,
  ALJustification,
  ALBarcode,
  ALFont,
  ALChangeIDParam,
} from "./views";

@graphInfo({
  graphType: "AA.Objects.Labels.ALCategoryMaint",
  primaryView: "Document",
  pageLoadBehavior: PXPageLoadBehavior.SearchSavedKeys,
})
export class AL207500 extends PXScreen {
  @viewInfo({ containerName: "Standard" })
  Document = createSingle(ALCategory);
  CurrentDocument = createSingle(ALCategory2);
  @viewInfo({ containerName: "Models" })
  UsedByModels = createCollection(ALModel);
  @viewInfo({ containerName: "Data Elements" })
  UsedByDataElements = createCollection(ALDataElement);
  @viewInfo({ containerName: "Contents" })
  UsedByContents = createCollection(ALContent);
  @viewInfo({ containerName: "Substitutions" })
  UsedBySubstitutions = createCollection(ALSubstitution);
  @viewInfo({ containerName: "Colors" })
  UsedByColors = createCollection(ALColor);
  @viewInfo({ containerName: "Rules" })
  UsedByRules = createCollection(ALRule);
  @viewInfo({ containerName: "Formats" })
  UsedByFormats = createCollection(ALFormat);
  @viewInfo({ containerName: "Margins" })
  UsedByMargins = createCollection(ALMargin);
  @viewInfo({ containerName: "Justifications" })
  UsedByJustifications = createCollection(ALJustification);
  @viewInfo({ containerName: "Barcodes" })
  UsedByBarcodes = createCollection(ALBarcode);
  @viewInfo({ containerName: "Fonts" })
  UsedByFonts = createCollection(ALFont);
  @viewInfo({ containerName: "Specify New ID" })
  ChangeIDDialog = createSingle(ALChangeIDParam);
}
