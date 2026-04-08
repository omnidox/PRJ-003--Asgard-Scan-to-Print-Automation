import { IN305010, INPIHeader } from "src/screens/IN/IN305010/IN305010";
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

import { IN305010_AddLineByBarcode } from "src/screens/IN/IN305010/extensions/IN305010_AddLineByBarcode";
import { LineSplittingTabBase } from "src/screens/IN/common/line-splitting/tab-line-splitting/tab-line-splitting";
export interface IN305010_PrinterOverride extends IN305010, IN305010_AddLineByBarcode {

}
export class IN305010_PrinterOverride {
   
}

export interface INPIHeader_PrinterOverride extends INPIHeader { }
export class INPIHeader_PrinterOverride {
    UsrALPrinterID: PXFieldState<PXFieldOptions.CommitChanges>;
    UsrALNbrOfCopies: PXFieldState<PXFieldOptions.CommitChanges>;
}
