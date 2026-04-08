import { IN304000, INTran } from "src/screens/IN/IN304000/IN304000";
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

export interface IN304000_NoOfCopies extends IN304000 {

}
export class IN304000_NoOfCopies {
    @viewInfo({ containerName: "Details" })
    ToggleSelected: PXActionState;
}

export interface INTran_NoOfCopies extends INTran { }
export class INTran_NoOfCopies {
    ToggleSelected: PXActionState;
    @placeAfterProperty("LineNbr") @columnConfig({ allowShowHide: GridColumnShowHideMode.Server })
    UsrALPrintLabel: PXFieldState;
    @placeAfterProperty("UsrALPrintLabel") @columnConfig({ allowShowHide: GridColumnShowHideMode.Server })
    UsrALNbrOfCopies: PXFieldState;
    @placeAfterProperty("UsrALNbrOfCopies") @columnConfig({ allowShowHide: GridColumnShowHideMode.Server })
    UsrALLabelQty: PXFieldState;
    @placeAfterProperty("UsrALLabelQty") @columnConfig({ allowShowHide: GridColumnShowHideMode.Server })
    UsrALBoxXofY: PXFieldState;
}

