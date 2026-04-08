/*import { AM201500 } from "src/screens/AM/AM201500/AM201500";*/
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

import { AM201500 } from "src/screens/AM/AM201500/AM201500";
import { AM201500_LineSplittingTab, AM201500_LineSplittingDetails } from "src/screens/AM/AM201500/extensions/AM201500_LineSplittingTab";
import { LineSplittingTabBase } from "src/screens/IN/common/line-splitting/tab-line-splitting/tab-line-splitting";
/*import { LineSplittingTabBase } from "src/screens/IN/common/line-splitting/views";*/

export interface AM201500_NoOfCopies extends AM201500, LineSplittingTabBase, AM201500_LineSplittingTab {

}
export class AM201500_NoOfCopies {
    
}

export interface Transactions_NoOfCopies extends AM201500_LineSplittingDetails { }
export class Transactions_NoOfCopies {
    @placeAfterProperty("LotSerialNbr") @columnConfig({ allowShowHide: GridColumnShowHideMode.Server })
    UsrALPrintLabel: PXFieldState;
    @placeAfterProperty("UsrALPrintLabel") @columnConfig({ allowShowHide: GridColumnShowHideMode.Server })
    UsrALNbrOfCopies: PXFieldState;
    @placeAfterProperty("UsrALNbrOfCopies") @columnConfig({ allowShowHide: GridColumnShowHideMode.Server })
    UsrALLabelQty: PXFieldState;
    @placeAfterProperty("UsrALLabelQty") @columnConfig({ allowShowHide: GridColumnShowHideMode.Server })
    UsrALBoxXofY: PXFieldState;
}
