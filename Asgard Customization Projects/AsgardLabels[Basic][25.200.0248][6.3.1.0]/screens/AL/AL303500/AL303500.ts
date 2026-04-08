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
  ALDataElement,
  CacheEntityItem,
  CacheEntityItem2,
  CacheEntityItem3,
  CacheEntityItem4,
  ALModelExpr,
  ALContentElement,
} from "./views";

@graphInfo({
  graphType: "AA.Objects.Labels.ALDataElementMaint",
  primaryView: "Document",
})
export class AL303500 extends PXScreen {
  Document = createSingle(ALDataElement);
  EntityItemsBasedOn = createCollection(CacheEntityItem);
  EntityItemsExprValue = createCollection(CacheEntityItem2);
  EntityItemsSampleBasedOn = createCollection(CacheEntityItem3);
  EntityItemsSampleValue = createCollection(CacheEntityItem4);
  @viewInfo({ containerName: "Used By Models" })
  UsedByModels = createCollection(ALModelExpr);
  @viewInfo({ containerName: "Used By Contents" })
  UsedByContents = createCollection(ALContentElement);
}
