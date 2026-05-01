import {
    PXView,
    PXFieldState,
    PXFieldOptions,
    createCollection,

    viewInfo,
    gridConfig,
    GridPreset,
} from "client-controls";
import { POOrder, POLine } from "../../../../../../screens/PO/PO301000/PO301000";

export interface PO301000_TrueCommerce extends POOrder { }
export class PO301000_TrueCommerce extends POOrder {

    UsrTCCustomField1: PXFieldState;
    UsrTCCustomField2: PXFieldState;
    UsrTCCustomField3: PXFieldState;
    UsrTCCustomField4: PXFieldState;
    UsrTCCustomField5: PXFieldState;
    UsrTCCustomField6: PXFieldState;
    UsrTCCustomField7: PXFieldState;
    UsrTCCustomField8: PXFieldState;
    UsrTCCustomField9: PXFieldState;
    UsrTCCustomField10: PXFieldState;
    UsrTCStatus:PXFieldState;
   
    UsrTCPOSentTime: PXFieldState<PXFieldOptions.Readonly>;

}

export interface PO301000_POLineExtension extends POLine { }
export class PO301000_POLineExtension extends POLine {
    UsrTCCustomField1: PXFieldState;
    UsrTCCustomField2: PXFieldState;
    UsrTCCustomField3: PXFieldState;
    UsrTCCustomField4: PXFieldState;
    UsrTCCustomField5: PXFieldState;
    UsrTCCustomField6: PXFieldState;
    UsrTCCustomField7: PXFieldState;
    UsrTCCustomField8: PXFieldState;
    UsrTCCustomField9: PXFieldState;
    UsrTCCustomField10: PXFieldState;
    UsrTCStatus:PXFieldState;
}

