import {
    PXView,
    PXFieldState,
    PXFieldOptions,
    createCollection,

    viewInfo,
    gridConfig,
    GridPreset,
} from "client-controls";
import { ARInvoice, ARTran } from "../../../../../../screens/AR/AR301000/AR301000";
//import { ARInvoice,ARTran } from "../AR301000";

export interface AR301000_TrueCommerce extends ARInvoice { }
export class AR301000_TrueCommerce extends ARInvoice {
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

    UsrTCINVSentTime: PXFieldState<PXFieldOptions.Readonly>;

}

export interface AR301000_ARLineExtension extends ARTran { }
export class AR301000_ARLineExtension extends ARTran {
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

