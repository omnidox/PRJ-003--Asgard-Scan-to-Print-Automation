import { Messages as SysMessages } from "client-controls/services/messages";
import { createCollection, createSingle, PXScreen, graphInfo, PXActionState, viewInfo, handleEvent, CustomEventType, actionConfig, RowSelectedHandlerArgs, PXViewCollection, PXPageLoadBehavior, ControlParameter } from "client-controls";
import { TCCustomerSettings, TCLabelFieldMap } from "./views";

@graphInfo({graphType: "TCAddon.TCLabelCustomerSettingsSetupMaint", primaryView: "TCCustomerRecord", })
export class TC102010 extends PXScreen {

	TCCustomerRecord = createSingle(TCCustomerSettings);
   	@viewInfo({containerName: "Label Fields Maps"})
	TCLabelFieldMapRecord = createCollection(TCLabelFieldMap);
}