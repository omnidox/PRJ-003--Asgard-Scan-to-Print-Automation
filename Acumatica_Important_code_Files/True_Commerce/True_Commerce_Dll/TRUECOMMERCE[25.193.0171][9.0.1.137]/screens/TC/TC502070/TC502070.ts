import { Messages as SysMessages } from "client-controls/services/messages";
import { createCollection, createSingle, PXScreen, graphInfo, PXActionState, viewInfo, handleEvent, CustomEventType, actionConfig, RowSelectedHandlerArgs, PXViewCollection, PXPageLoadBehavior, ControlParameter } from "client-controls";
import { TCInvoiceFilterSO, ARRegister } from "./views";

@graphInfo({graphType: "TCAddon.TCProcessInvoiceSO", primaryView: "TransactionStatus", })
export class TC502070 extends PXScreen {

   	@viewInfo({containerName: "Process Invoice SO"})
	TransactionStatus = createSingle(TCInvoiceFilterSO);
	Orders = createCollection(ARRegister);
}