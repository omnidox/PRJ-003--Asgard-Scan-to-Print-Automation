import { IN202000, InventoryItem } from "src/screens/IN/IN202000/IN202000";
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

export interface IN202000_PrinterOverride extends IN202000 {

}
export class IN202000_PrinterOverride {
   
}

export interface InventoryItem_PrinterOverride extends InventoryItem { }
export class InventoryItem_PrinterOverride {
    UsrALPrinterID: PXFieldState<PXFieldOptions.CommitChanges>;
    UsrALNbrOfCopies: PXFieldState<PXFieldOptions.CommitChanges>;
}
