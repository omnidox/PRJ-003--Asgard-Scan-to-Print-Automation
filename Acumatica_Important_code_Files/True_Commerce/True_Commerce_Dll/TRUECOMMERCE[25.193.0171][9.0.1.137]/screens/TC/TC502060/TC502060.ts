import { Messages as SysMessages } from "client-controls/services/messages";
import { createCollection, createSingle, PXScreen, graphInfo, PXActionState, viewInfo, handleEvent, CustomEventType, actionConfig, RowSelectedHandlerArgs, PXViewCollection, PXPageLoadBehavior, ControlParameter } from "client-controls";
import { TCSalesInvFilter, SOOrder } from "./views";

@graphInfo({graphType: "TCAddon.TCProcessSalesInvoice", primaryView: "TransactionStatus", })
export class TC502060 extends PXScreen {

   	@viewInfo({containerName: "Process SalesOrder Invoice"})
	TransactionStatus = createSingle(TCSalesInvFilter);
   	@viewInfo({containerName: "Orders"})
	Orders = createCollection(SOOrder);
}