import { IN305000, INPIHeader } from "src/screens/IN/IN305000/IN305000";
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

export interface IN305000_PrinterOverride extends IN305000 {

}
export class IN305000_PrinterOverride {
   
}

export interface INPIHeader_PrinterOverride extends INPIHeader { }
export class INPIHeader_PrinterOverride {
    UsrALPrinterID: PXFieldState<PXFieldOptions.CommitChanges>;
    UsrALNbrOfCopies: PXFieldState<PXFieldOptions.CommitChanges>;
}
