import { Messages as SysMessages } from "client-controls/services/messages";
import { createCollection, createSingle, PXScreen, graphInfo, PXActionState, viewInfo, handleEvent, CustomEventType, actionConfig, RowSelectedHandlerArgs, PXViewCollection, PXPageLoadBehavior, ControlParameter } from "client-controls";
import { TCWSOFilter, SOOrder } from "./views";

@graphInfo({graphType: "TCAddon.TCProcessWarehouseShippingOrder", primaryView: "TransactionStatus", })
export class TC502050 extends PXScreen {

   	@viewInfo({containerName: "Process Warehouse Shipping Order"})
	TransactionStatus = createSingle(TCWSOFilter);
   	@viewInfo({containerName: "Orders"})
	Orders = createCollection(SOOrder);
}