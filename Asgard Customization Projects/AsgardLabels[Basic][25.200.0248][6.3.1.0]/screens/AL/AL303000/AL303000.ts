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
  ALDataSource,
  ALDataSource2,
  ALDataElement,
  ALDataElement2,
  CacheEntityItem,
  CacheEntityItem2,
  ALDataElement3,
  CacheEntityItem3,
  CacheEntityItem4,
  ALDataElement4,
  ALDataElement5,
  ALDataElement6,
  ALDataElement7,
  ALChangeIDParam,
} from "./views";

@graphInfo({
  graphType: "AA.Objects.Labels.ALDataSourceMaint",
  primaryView: "Document",
})
export class AL303000 extends PXScreen {
  Document = createSingle(ALDataSource);
  @viewInfo({ containerName: "Hidden Form needed for VisibleExp of TabItems" })
  CurrentDocument = createSingle(ALDataSource2);
  @viewInfo({ containerName: "Contents" })
  ContentElements = createCollection(ALDataElement);
  @viewInfo({ containerName: "Functions" })
  FunctionElements = createCollection(ALDataElement2);
  @viewInfo({ containerName: "Functions" })
  EntityItemsFunctionBasedOn = createCollection(CacheEntityItem);
  @viewInfo({ containerName: "Functions" })
  EntityItemsFunctionChildren = createCollection(CacheEntityItem2);
  @viewInfo({ containerName: "Screens" })
  ScreenElements = createCollection(ALDataElement3);
  @viewInfo({ containerName: "Screens" })
  EntityItemsScreenBasedOn = createCollection(CacheEntityItem3);
  @viewInfo({ containerName: "Screens" })
  EntityItemsScreenChildren = createCollection(CacheEntityItem4);
  @viewInfo({ containerName: "Hardcoded" })
  FixedElements = createCollection(ALDataElement4);
  @viewInfo({ containerName: "Images" })
  ImageElements = createCollection(ALDataElement5);
  @viewInfo({ containerName: "Iterators" })
  IteratorElements = createCollection(ALDataElement6);
  @viewInfo({ containerName: "Scripts" })
  ScriptElements = createCollection(ALDataElement7);
  @viewInfo({ containerName: "Specify New ID" })
  ChangeIDDialog = createSingle(ALChangeIDParam);
}
