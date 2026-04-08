import { SM203010 } from "src/screens/SM/SM203010/SM203010";
import { UserPreferences } from "src/screens/SM/SM203010/views";
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

export interface SM203010_PrintStation extends SM203010 {

}
export class SM203010_PrintStation {
   
}

export interface UserPreferences_PrintStation extends UserPreferences { }
export class UserPreferences_PrintStation {
    UsrALPrintStationID: PXFieldState<PXFieldOptions.CommitChanges>;
}


