import { IN203000, InventoryItem } from "src/screens/IN/IN203000/IN203000";
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

export interface IN203000_PrinterOverride extends IN203000 {

}
export class IN203000_PrinterOverride {
   
}

export interface InventoryItem_PrinterOverride extends InventoryItem { }
export class InventoryItem_PrinterOverride {
    UsrALPrinterID: PXFieldState<PXFieldOptions.CommitChanges>;
    UsrALNbrOfCopies: PXFieldState<PXFieldOptions.CommitChanges>;
}
