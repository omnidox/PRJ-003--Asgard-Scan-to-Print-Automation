import { Messages as SysMessages } from "client-controls/services/messages";
import { createCollection, createSingle, PXScreen, graphInfo, PXActionState, viewInfo, handleEvent, CustomEventType, actionConfig, RowSelectedHandlerArgs, PXViewCollection, PXPageLoadBehavior, ControlParameter } from "client-controls";
import { TCShipFromAddress } from "./views";

@graphInfo({graphType: "TCAddon.TCShipFromAddressEntry", primaryView: "TCShipFromAddress", })
export class TC105000 extends PXScreen {

	TCShipFromAddress = createSingle(TCShipFromAddress);
}