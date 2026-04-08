import {
    PXView,
    PXFieldState,
    gridConfig,
    treeConfig,
    fieldConfig,
    controlConfig,
    actionConfig,
    headerDescription,
    ICurrencyInfo,
    disabled,
    PXFieldOptions,
    linkCommand,
    columnConfig,
    GridColumnShowHideMode,
    GridColumnType,
    PXActionState,
    TextAlign,
    GridPreset,
    GridFilterBarVisibility,
    GridFastFilterVisibility,
    ISelectorControlConfig,
    ControlParameter,
} from "client-controls";
import { AL204000 } from "./AL204000";

// Views

export class ALBarcode extends PXView {
    Name: PXFieldState;
    Description: PXFieldState;
    BarcodeType: PXFieldState<PXFieldOptions.CommitChanges>;
    SampleValue: PXFieldState;
    CategoryID: PXFieldState;
    Message: PXFieldState;
    Active: PXFieldState<PXFieldOptions.CommitChanges>;
    IsSystem: PXFieldState<PXFieldOptions.CommitChanges>;
    AllowExport: PXFieldState<PXFieldOptions.CommitChanges>;
    Language: PXFieldState<PXFieldOptions.Disabled>;
    Dimension: PXFieldState<PXFieldOptions.CommitChanges>;
    @controlConfig({ allowEdit: true })
    FormatID: PXFieldState<PXFieldOptions.CommitChanges>;
}

export class ALBarcode2 extends PXView {
    ImageUrl: PXFieldState;
}

@gridConfig({
    allowDelete: false,
    allowInsert: false,
    showFastFilter: GridFastFilterVisibility.False,
    preset: GridPreset.Details,
})
export class ALBarcodeOption extends PXView {
    @columnConfig({ width: 200 }) BarcodeID: PXFieldState;
    LineNbr: PXFieldState;
    SortOrder: PXFieldState;
    @columnConfig({ width: 100 }) Option: PXFieldState;
    @columnConfig({ width: 300 }) Description: PXFieldState;
    @columnConfig({ width: 200 }) Constraint: PXFieldState;
    Value: PXFieldState;
}

@gridConfig({
    showFastFilter: GridFastFilterVisibility.False,
    preset: GridPreset.ReadOnly,
})
export class ALDataElement extends PXView {
    @linkCommand("ViewModel")
    @columnConfig({ width: 350 })
    Name: PXFieldState;
    @columnConfig({ allowNull: false }) Active: PXFieldState;
    @columnConfig({ width: 150 }) LastModifiedByID: PXFieldState;
    @columnConfig({ width: 150 }) LastModifiedDateTime: PXFieldState;
}

export class ALChangeIDParam extends PXView {
    Name: PXFieldState;
}
