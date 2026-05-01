import { Messages as SysMessages } from "client-controls/services/messages";
import { createCollection, createSingle, PXScreen, graphInfo, PXActionState, viewInfo, handleEvent, CustomEventType, actionConfig, RowSelectedHandlerArgs, PXViewCollection, PXPageLoadBehavior, ControlParameter } from "client-controls";
import { TCHeaderTable, SOOrder } from "./views";

@graphInfo({graphType: "TCAddon.TCProcessShipmentAutomation", primaryView: "Filter", })
export class TC501000 extends PXScreen {

   	@viewInfo({containerName: "Selection"})
	Filter = createSingle(TCHeaderTable);
   	@viewInfo({containerName: "Orders"})
	Orders = createCollection(SOOrder);
}