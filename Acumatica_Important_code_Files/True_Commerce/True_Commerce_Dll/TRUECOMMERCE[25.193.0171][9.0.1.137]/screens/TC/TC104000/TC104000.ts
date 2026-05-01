import { Messages as SysMessages } from "client-controls/services/messages";
import { createCollection, createSingle, PXScreen, graphInfo, PXActionState, viewInfo, handleEvent, CustomEventType, actionConfig, RowSelectedHandlerArgs, PXViewCollection, PXPageLoadBehavior, ControlParameter } from "client-controls";
import { TCTransactionStatus } from "./views";

@graphInfo({graphType: "TCAddon.TCTransactionStatusEntry", primaryView: "TransactionStatus", })
export class TC104000 extends PXScreen {

   	@viewInfo({containerName: "Transaction Service Configuration"})
	TransactionStatus = createSingle(TCTransactionStatus);
}