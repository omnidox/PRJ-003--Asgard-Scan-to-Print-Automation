import { Messages as SysMessages } from "client-controls/services/messages";
import { createCollection, createSingle, PXScreen, graphInfo, PXActionState, viewInfo, handleEvent, CustomEventType, actionConfig, RowSelectedHandlerArgs, PXViewCollection, PXPageLoadBehavior, ControlParameter } from "client-controls";
import { TCPurchaseOrderFilter, POOrder } from "./views";

@graphInfo({graphType: "TCAddon.TCProcessPurchaseOrders", primaryView: "TransactionStatus", })
export class TC502040 extends PXScreen {

   	@viewInfo({containerName: "Process Purchase Orders"})
	TransactionStatus = createSingle(TCPurchaseOrderFilter);
	Orders = createCollection(POOrder);
}