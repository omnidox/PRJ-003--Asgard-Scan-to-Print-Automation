import { IN202500, InventoryItem } from "src/screens/IN/IN202500/IN202500";
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

export interface IN202500_PrinterOverride extends IN202500 {

}
export class IN202500_PrinterOverride {
   
}

export interface InventoryItem_PrinterOverride extends InventoryItem { }
export class InventoryItem_PrinterOverride {
    UsrALPrinterID: PXFieldState<PXFieldOptions.CommitChanges>;
    UsrALNbrOfCopies: PXFieldState<PXFieldOptions.CommitChanges>;
}
