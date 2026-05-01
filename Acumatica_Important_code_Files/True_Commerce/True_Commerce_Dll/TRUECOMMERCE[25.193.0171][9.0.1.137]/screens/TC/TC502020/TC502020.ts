import { Messages as SysMessages } from "client-controls/services/messages";
import { createCollection, createSingle, PXScreen, graphInfo, PXActionState, viewInfo, handleEvent, CustomEventType, actionConfig, RowSelectedHandlerArgs, PXViewCollection, PXPageLoadBehavior, ControlParameter } from "client-controls";
import { TCShipmentFilter, SOShipment } from "./views";

@graphInfo({graphType: "TCAddon.TCProcessShipments", primaryView: "TransactionStatus", })
export class TC502020 extends PXScreen {

   	@viewInfo({containerName: "Process Shipment"})
	TransactionStatus = createSingle(TCShipmentFilter);
   	@viewInfo({containerName: "Orders"})
	Orders = createCollection(SOShipment);
}