import { Messages as SysMessages } from "client-controls/services/messages";
import { createCollection, createSingle, PXScreen, graphInfo, PXActionState, viewInfo, handleEvent, CustomEventType, actionConfig, RowSelectedHandlerArgs, PXViewCollection, PXPageLoadBehavior, ControlParameter } from "client-controls";
import { TCSalesOrderFilter, SOOrder } from "./views";

@graphInfo({graphType: "TCAddon.TCProcessSalesOrders", primaryView: "TransactionStatus", })
export class TC502010 extends PXScreen {

   	@viewInfo({containerName: "Process Purchase Order Acknowledgement"})
	TransactionStatus = createSingle(TCSalesOrderFilter);
   	@viewInfo({containerName: "Orders"})
	Orders = createCollection(SOOrder);
}