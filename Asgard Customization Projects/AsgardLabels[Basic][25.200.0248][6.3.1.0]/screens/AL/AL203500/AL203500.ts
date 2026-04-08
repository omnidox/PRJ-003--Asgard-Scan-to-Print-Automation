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
  ALRule,
  CacheEntityItem,
  ALRule2,
  ALRuleDetail,
  ALModel,
  ALModelExpr,
  ALRuleDetail2,
  ALColorRule,
  ALFormatRule,
  ALContentElement,
  ALAutoPrint,
  ALChangeIDParam,
} from "./views";

@graphInfo({
  graphType: "AA.Objects.Labels.ALRuleMaint",
  primaryView: "Document",
  pageLoadBehavior: PXPageLoadBehavior.SearchSavedKeys,
})
export class AL203500 extends PXScreen {
  @viewInfo({ containerName: "Rule" })
  Document = createSingle(ALRule);
  @viewInfo({ containerName: "Rule" })
  EntityItems = createCollection(CacheEntityItem);
  @viewInfo({ containerName: "Hidden Form needed for VisibleExp of TabItems" })
  CurrentDocument = createSingle(ALRule2);
  @viewInfo({ containerName: "Rule Details" })
  Details = createCollection(ALRuleDetail);
  @viewInfo({ containerName: "Used by Models" })
  UsedByModels = createCollection(ALModel);
  @viewInfo({ containerName: "Used by Expressions" })
  UsedByExprs = createCollection(ALModelExpr);
  @viewInfo({ containerName: "Used by Composites" })
  UsedByComposites = createCollection(ALRuleDetail2);
  @viewInfo({ containerName: "Used by Colors" })
  UsedByColors = createCollection(ALColorRule);
  @viewInfo({ containerName: "Used by Formats" })
  UsedByFormats = createCollection(ALFormatRule);
  @viewInfo({ containerName: "Used by Content Elements" })
  UsedByContentElements = createCollection(ALContentElement);
  @viewInfo({ containerName: "Used by Auto Prints" })
  UsedByAutoPrints = createCollection(ALAutoPrint);
  @viewInfo({ containerName: "Specify New ID" })
  ChangeIDDialog = createSingle(ALChangeIDParam);
}
