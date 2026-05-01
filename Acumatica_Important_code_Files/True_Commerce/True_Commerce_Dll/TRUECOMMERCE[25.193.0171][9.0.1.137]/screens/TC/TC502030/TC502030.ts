import { Messages as SysMessages } from "client-controls/services/messages";
import { createCollection, createSingle, PXScreen, graphInfo, PXActionState, viewInfo, handleEvent, CustomEventType, actionConfig, RowSelectedHandlerArgs, PXViewCollection, PXPageLoadBehavior, ControlParameter } from "client-controls";
import { TCInvoiceFilter, ARRegister } from "./views";

@graphInfo({graphType: "TCAddon.TCProcessInvoice", primaryView: "TransactionStatus", })
export class TC502030 extends PXScreen {

   	@viewInfo({containerName: "Process Invoice AR"})
	TransactionStatus = createSingle(TCInvoiceFilter);
	Orders = createCollection(ARRegister);
}