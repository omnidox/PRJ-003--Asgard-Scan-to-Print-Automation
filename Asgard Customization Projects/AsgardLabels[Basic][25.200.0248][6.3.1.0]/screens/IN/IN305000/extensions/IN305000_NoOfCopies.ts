import { IN305000, INPIDetail } from "src/screens/IN/IN305000/IN305000";
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

export interface IN305000_NoOfCopies extends IN305000 {

}
export class IN305000_NoOfCopies {
    @viewInfo({ containerName: "PIDetail" })
    ToggleSelected: PXActionState;
}

export interface INPIDetail_NoOfCopies extends INPIDetail { }
export class INPIDetail_NoOfCopies {
    @placeAfterProperty("ExpireDate") @columnConfig({ allowShowHide: GridColumnShowHideMode.Server })
    UsrALPrintLabel: PXFieldState;
    @placeAfterProperty("UsrALPrintLabel") @columnConfig({ allowShowHide: GridColumnShowHideMode.Server })
    UsrALNbrOfCopies: PXFieldState;
    @placeAfterProperty("UsrALNbrOfCopies") @columnConfig({ allowShowHide: GridColumnShowHideMode.Server })
    UsrALLabelQty: PXFieldState;
    @placeAfterProperty("UsrALLabelQty") @columnConfig({ allowShowHide: GridColumnShowHideMode.Server })
    UsrALBoxXofY: PXFieldState;
}

