import { Messages as SysMessages } from "client-controls/services/messages";
import { createCollection, createSingle, PXScreen, graphInfo, PXActionState, viewInfo, handleEvent, CustomEventType, actionConfig, RowSelectedHandlerArgs, PXViewCollection, PXPageLoadBehavior, ControlParameter } from "client-controls";
import { TCInventoryFilter, InventoryItem } from "./views";

@graphInfo({graphType: "TCAddon.TCItemPackageSettingsSetupMaint", primaryView: "TCItemPackageRecord", })
export class TC101000 extends PXScreen {

	TCItemPackageRecord = createSingle(TCInventoryFilter);
	TCItemList = createCollection(InventoryItem);
}