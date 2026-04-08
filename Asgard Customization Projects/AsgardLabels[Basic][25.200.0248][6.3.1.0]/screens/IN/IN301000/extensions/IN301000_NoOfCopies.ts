import { IN301000, INTran } from "src/screens/IN/IN301000/IN301000";
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

export interface IN301000_NoOfCopies extends IN301000 {

}
export class IN301000_NoOfCopies {
    @viewInfo({ containerName: "Details" })
    ToggleSelected: PXActionState;
}

export interface INTran_NoOfCopies extends INTran { }
export class INTran_NoOfCopies {
    ToggleSelected: PXActionState;
    @placeAfterProperty("TranCost") @columnConfig({ allowShowHide: GridColumnShowHideMode.Server })
    UsrALPrintLabel: PXFieldState;
    @placeAfterProperty("UsrALPrintLabel") @columnConfig({ allowShowHide: GridColumnShowHideMode.Server })
    UsrALNbrOfCopies: PXFieldState;
    @placeAfterProperty("UsrALNbrOfCopies") @columnConfig({ allowShowHide: GridColumnShowHideMode.Server })
    UsrALLabelQty: PXFieldState;
    @placeAfterProperty("UsrALLabelQty") @columnConfig({ allowShowHide: GridColumnShowHideMode.Server })
    UsrALBoxXofY: PXFieldState;
}

