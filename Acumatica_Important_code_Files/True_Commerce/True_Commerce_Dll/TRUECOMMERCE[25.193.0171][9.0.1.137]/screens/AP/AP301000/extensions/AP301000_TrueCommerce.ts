import {
    PXView,
    PXFieldState,
    PXFieldOptions,
    createCollection,

    viewInfo,
    gridConfig,
    GridPreset,
} from "client-controls";
import { APInvoice, APTran } from "../../../../../../screens/AP/AP301000/AP301000";
//import { APInvoice, APTran } from "../AP301000";
export interface AP301000_TrueCommerce extends APInvoice { }
export class AP301000_TrueCommerce extends APInvoice {
   
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
}

export interface AP301000_APLineExtension extends APTran { }
export class AP301000_APLineExtension extends APTran {
    UsrTCPOLineNumber: PXFieldState;
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
}

