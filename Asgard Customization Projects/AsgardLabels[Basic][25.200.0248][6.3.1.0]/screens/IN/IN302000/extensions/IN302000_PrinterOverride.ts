import { IN302000, INRegister2 } from "src/screens/IN/IN302000/IN302000";
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

export interface IN302000_PrinterOverride extends IN302000 {

}
export class IN302000_PrinterOverride {
   
}

export interface INRegister2_PrinterOverride extends INRegister2 { }
export class INRegister2_PrinterOverride {
    UsrALPrinterID: PXFieldState<PXFieldOptions.CommitChanges>;
    UsrALNbrOfCopies: PXFieldState<PXFieldOptions.CommitChanges>;
}
