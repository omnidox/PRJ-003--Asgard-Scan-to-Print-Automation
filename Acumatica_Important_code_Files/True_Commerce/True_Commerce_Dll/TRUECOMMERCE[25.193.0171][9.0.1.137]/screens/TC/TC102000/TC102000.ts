import { Messages as SysMessages } from "client-controls/services/messages";
import { createCollection, createSingle, PXScreen, graphInfo, PXActionState, viewInfo, handleEvent, CustomEventType, actionConfig, RowSelectedHandlerArgs, PXViewCollection, PXPageLoadBehavior, ControlParameter } from "client-controls";
import { TCCustomerSettings } from "./views";

@graphInfo({graphType: "TCAddon.TCCustomerSettingsSetupMaint", primaryView: "TCCustomerRecord", })
export class TC102000 extends PXScreen {

	TCCustomerRecord = createSingle(TCCustomerSettings);
}