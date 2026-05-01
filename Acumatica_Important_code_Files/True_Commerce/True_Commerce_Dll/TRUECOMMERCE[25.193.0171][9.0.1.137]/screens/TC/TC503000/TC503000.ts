import { Messages as SysMessages } from "client-controls/services/messages";
import { createCollection, createSingle, PXScreen, graphInfo, PXActionState, viewInfo, handleEvent, CustomEventType, actionConfig, RowSelectedHandlerArgs, PXViewCollection, PXPageLoadBehavior, ControlParameter } from "client-controls";
import { TCShipmentNos, TCBatchPrintPacks } from "./views";

@graphInfo({graphType: "TCAddon.TCProcessBatchLabelPrinting", primaryView: "Filter", })
export class TC503000 extends PXScreen {

	Filter = createSingle(TCShipmentNos);
   	@viewInfo({containerName: "Shipment Packages"})
	Packs = createCollection(TCBatchPrintPacks);
}