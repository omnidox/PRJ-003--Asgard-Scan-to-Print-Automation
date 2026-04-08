import { PO302000, POReceipt } from "src/screens/PO/PO302000/PO302000";
import {
    PXFieldState,
    PXFieldOptions,
    viewInfo,
    PXActionState,
    GridColumnShowHideMode,
    columnConfig,
    featureInstalled,
    placeAfterProperty,
    PXView,
    createCollection,
    gridConfig,
    GridPreset,
    placeBeforeProperty,
} from "client-controls";

export interface PO302000_PrinterOverride extends PO302000 {

}
export class PO302000_PrinterOverride {
   
}

export interface POReceipt_PrinterOverride extends POReceipt { }
export class POReceipt_PrinterOverride {
    UsrALPrinterID: PXFieldState<PXFieldOptions.CommitChanges>;
    UsrALNbrOfCopies: PXFieldState<PXFieldOptions.CommitChanges>;
}


