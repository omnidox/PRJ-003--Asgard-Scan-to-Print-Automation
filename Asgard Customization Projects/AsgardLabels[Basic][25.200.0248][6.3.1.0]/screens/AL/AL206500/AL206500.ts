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
  ALSubstitution,
  ALSubstitution2,
  ALSubstitutionDetail,
  ALSubstitutionDetail2,
  ALDataElement,
  ALContentElement,
  ALChangeIDParam,
} from "./views";

@graphInfo({
  graphType: "AA.Objects.Labels.ALSubstitutionMaint",
  primaryView: "Document",
  pageLoadBehavior: PXPageLoadBehavior.SearchSavedKeys,
})
export class AL206500 extends PXScreen {
  @viewInfo({ containerName: "Substitution" })
  Document = createSingle(ALSubstitution);
  @viewInfo({ containerName: "Hidden Form needed for VisibleExp of TabItems" })
  CurrentDocument = createSingle(ALSubstitution2);
  @viewInfo({ containerName: "Substitution Details" })
  SubstitutionDetails = createCollection(ALSubstitutionDetail);
  @viewInfo({ containerName: "Used By Composites" })
  UsedByComposites = createCollection(ALSubstitutionDetail2);
  @viewInfo({ containerName: "Used By Data Elements" })
  UsedByDataElements = createCollection(ALDataElement);
  @viewInfo({ containerName: "Used By Content Elements" })
  UsedByContentElements = createCollection(ALContentElement);
  @viewInfo({ containerName: "Specify New ID" })
  ChangeIDDialog = createSingle(ALChangeIDParam);
}
