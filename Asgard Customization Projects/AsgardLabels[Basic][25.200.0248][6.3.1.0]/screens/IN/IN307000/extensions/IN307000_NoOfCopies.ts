/*import { IN307000 } from "src/screens/AM/IN307000/IN307000";*/
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

import { IN307000 } from "src/screens/IN/IN307000/IN307000";
import { IN307000_LineSplittingTab, IN307000_LineSplittingDetails } from "src/screens/IN/IN307000/extensions/IN307000_LineSplittingTab";
import { LineSplittingTabBase } from "src/screens/IN/common/line-splitting/tab-line-splitting/tab-line-splitting";
/*import { LineSplittingTabBase } from "src/screens/IN/common/line-splitting/views";*/

export interface IN307000_NoOfCopies extends IN307000, LineSplittingTabBase, IN307000_LineSplittingTab {

}
export class IN307000_NoOfCopies {
    
}

export interface LineSplittingDetails_NoOfCopies extends IN307000_LineSplittingDetails { }
export class LineSplittingDetails_NoOfCopies {
    @placeAfterProperty("UOM") @columnConfig({ allowShowHide: GridColumnShowHideMode.Server })
    UsrALPrintLabel: PXFieldState;
    @placeAfterProperty("UsrALPrintLabel") @columnConfig({ allowShowHide: GridColumnShowHideMode.Server })
    UsrALNbrOfCopies: PXFieldState;
    @placeAfterProperty("UsrALNbrOfCopies") @columnConfig({ allowShowHide: GridColumnShowHideMode.Server })
    UsrALLabelQty: PXFieldState;
    @placeAfterProperty("UsrALLabelQty") @columnConfig({ allowShowHide: GridColumnShowHideMode.Server })
    UsrALBoxXofY: PXFieldState;
}
