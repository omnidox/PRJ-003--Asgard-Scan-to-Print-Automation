import { AM201500, AMProdItem } from "src/screens/AM/AM201500/AM201500";
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

export interface AM201500_PrinterOverride extends AM201500 {

}
export class AM201500_PrinterOverride {
   
}

export interface AMProdItem_PrinterOverride extends AMProdItem { }
export class AMProdItem_PrinterOverride {
    UsrALPrinterID: PXFieldState<PXFieldOptions.CommitChanges>;
    UsrALNbrOfCopies: PXFieldState<PXFieldOptions.CommitChanges>;
}
