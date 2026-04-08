import { IN303000, INRegister2 } from "src/screens/IN/IN303000/IN303000";
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

export interface IN303000_PrinterOverride extends IN303000 {

}
export class IN303000_PrinterOverride {
   
}

export interface INRegister2_PrinterOverride extends INRegister2 { }
export class INRegister2_PrinterOverride {
    UsrALPrinterID: PXFieldState<PXFieldOptions.CommitChanges>;
    UsrALNbrOfCopies: PXFieldState<PXFieldOptions.CommitChanges>;
}
