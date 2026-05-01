import { Messages as SysMessages } from "client-controls/services/messages";
import { createCollection, createSingle, PXScreen, graphInfo, PXActionState, viewInfo, handleEvent, CustomEventType, actionConfig, RowSelectedHandlerArgs, PXViewCollection, PXPageLoadBehavior, ControlParameter } from "client-controls";
import { TCLabelSettings } from "./views";

@graphInfo({graphType: "TCAddon.TCLabelSettingsSetupMaint", primaryView: "TCLabelSettingsRecord", })
export class TC103040 extends PXScreen {

	TCLabelSettingsRecord = createSingle(TCLabelSettings);
}