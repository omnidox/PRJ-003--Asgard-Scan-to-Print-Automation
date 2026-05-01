import {
    PXView,
    PXFieldState,
} from "client-controls";
import { POReceipt, POReceiptLine } from "../../../../../../screens/PO/PO302000/PO302000";

export interface PO302000_TrueCommerce extends POReceipt { }
export class PO302000_TrueCommerce extends POReceipt {

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

export interface PO302000_POLineExtension extends POReceiptLine { }
export class PO302000_POLineExtension extends POReceiptLine {
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

