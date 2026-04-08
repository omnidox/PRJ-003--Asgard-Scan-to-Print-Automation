import { SO302000, Transactions, Packages } from "src/screens/SO/SO302000/SO302000";
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

export interface SO302000_NoOfCopies extends SO302000 {

}
export class SO302000_NoOfCopies {
    @viewInfo({ containerName: "Details" })
    ToggleSelected: PXActionState;
    @viewInfo({containerName: "Packages" })
    toggleSelectedSP: PXActionState;
}

export interface Transactions_NoOfCopies extends Transactions { }
export class Transactions_NoOfCopies {
    ToggleSelected: PXActionState;
    @placeAfterProperty("UOM") @columnConfig({ allowShowHide: GridColumnShowHideMode.Server })
    UsrALPrintLabel: PXFieldState;
    @placeAfterProperty("UsrALPrintLabel") @columnConfig({ allowShowHide: GridColumnShowHideMode.Server })
    UsrALNbrOfCopies: PXFieldState;
    @placeAfterProperty("UsrALNbrOfCopies") @columnConfig({ allowShowHide: GridColumnShowHideMode.Server })
    UsrALLabelQty: PXFieldState;
    @placeAfterProperty("UsrALLabelQty") @columnConfig({ allowShowHide: GridColumnShowHideMode.Server })
    UsrALBoxXofY: PXFieldState;
}

export interface Packages_NoOfCopies extends Packages { }
export class Packages_NoOfCopies {
    toggleSelectedSP: PXActionState;
    @placeBeforeProperty("Confirmed") @columnConfig({ allowShowHide: GridColumnShowHideMode.Server })
    UsrALPrintLabel: PXFieldState;
    @placeAfterProperty("UsrALPrintLabel") @columnConfig({ allowShowHide: GridColumnShowHideMode.Server })
    UsrALNbrOfCopies: PXFieldState;
    @placeAfterProperty("UsrALNbrOfCopies") @columnConfig({ allowShowHide: GridColumnShowHideMode.Server })
    UsrALLabelQty: PXFieldState;
    @placeAfterProperty("UsrALLabelQty") @columnConfig({ allowShowHide: GridColumnShowHideMode.Server })
    UsrALBoxXofY: PXFieldState;

}
