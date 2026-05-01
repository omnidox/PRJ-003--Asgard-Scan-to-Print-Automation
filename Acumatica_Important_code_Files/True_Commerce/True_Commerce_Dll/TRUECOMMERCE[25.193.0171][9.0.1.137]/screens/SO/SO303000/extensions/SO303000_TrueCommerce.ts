import {
    PXView,
    PXFieldState,
    PXFieldOptions,
    createCollection,

    viewInfo,
    gridConfig,
    GridPreset,
} from "client-controls";
import { ARInvoiceCurrent, ARTran } from "../../../../../../screens/SO/SO303000/SO303000";
//import { ARInvoiceCurrent, ARTran } from "../SO303000";
export interface SO303000_TrueCommerce extends ARInvoiceCurrent { }
export class SO303000_TrueCommerce extends ARInvoiceCurrent {
    UsrTCPONumber: PXFieldState;
    UsrTCStoreNumber: PXFieldState;
    UsrTCDCCode: PXFieldState;
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
    UsrTCTrackingNumber:PXFieldState;
   
    UsrTCINVSOSentTime: PXFieldState<PXFieldOptions.Readonly>;

}

export interface SO303000_InvoiceLineExtension extends ARTran { }
export class SO303000_InvoiceLineExtension extends ARTran {
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

