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
import { AL201000 } from "./AL201000";

export class ALModelChildType {
    HideWhenInGroup: PXFieldState;
    Description: PXFieldState;
    FormatID: PXFieldState;
    ReverseFilter: PXFieldState;
    FilterRuleID: PXFieldState;
    ReversePrint: PXFieldState;
    PrintRuleID: PXFieldState;
    NbCopiesExpr: PXFieldState;
}
export class ALDataElementType {
    Name: PXFieldState;
}

// Views

export class ALModel extends PXView {
    Name: PXFieldState<PXFieldOptions.CommitChanges>;
    LabelID: PXFieldState<PXFieldOptions.CommitChanges>;
    ModelType: PXFieldState<PXFieldOptions.CommitChanges>;
    Description: PXFieldState<PXFieldOptions.CommitChanges>;
    CloudID: PXFieldState<PXFieldOptions.CommitChanges>;
    ScreenID: PXFieldState<PXFieldOptions.CommitChanges>;
    GraphType: PXFieldState<PXFieldOptions.CommitChanges>;
    BasedOnView: PXFieldState<PXFieldOptions.CommitChanges>;
    Message: PXFieldState<PXFieldOptions.CommitChanges>;
    Active: PXFieldState<PXFieldOptions.CommitChanges>;
    AllowExport: PXFieldState<PXFieldOptions.CommitChanges>;
    IsSystem: PXFieldState<PXFieldOptions.CommitChanges>;
    HideWhenInGroup: PXFieldState<PXFieldOptions.CommitChanges>;
    IgnoreRotationOnRender: PXFieldState<PXFieldOptions.CommitChanges>;
    FormatID: PXFieldState<PXFieldOptions.CommitChanges>;
    MarginID: PXFieldState<PXFieldOptions.CommitChanges>;
    ShowTemplate: PXFieldState;
    ShowExprs: PXFieldState;
    ShowSetup: PXFieldState;
    ShowRendered: PXFieldState;
    ShowPrinters: PXFieldState;
    ShowPrintLog: PXFieldState;
    ShowChildren: PXFieldState;
    ShowUsedBy: PXFieldState;
    Body: PXFieldState;
    Rendered: PXFieldState;
    @controlConfig({ allowEdit: true })
    FilterRuleID: PXFieldState<PXFieldOptions.CommitChanges>;
    ReverseFilter: PXFieldState<PXFieldOptions.CommitChanges>;
    HideInstead: PXFieldState<PXFieldOptions.CommitChanges>;
    @controlConfig({ allowEdit: true })
    PrintRuleID: PXFieldState<PXFieldOptions.CommitChanges>;
    ReversePrint: PXFieldState<PXFieldOptions.CommitChanges>;
    Language: PXFieldState<PXFieldOptions.CommitChanges>;
    PrintOnOtherDensity: PXFieldState<PXFieldOptions.CommitChanges>;
    Tooltip: PXFieldState;
    DealingMode: PXFieldState<PXFieldOptions.CommitChanges>;
    DealingCountExpr: PXFieldState;
    MergeDetails: PXFieldState<PXFieldOptions.CommitChanges>;
    PrintDetails: PXFieldState<PXFieldOptions.CommitChanges>;
    NbCopiesExpr: PXFieldState;
    SendPauseEvery: PXFieldState;
    @controlConfig({ allowEdit: true })
    NumberingID: PXFieldState<PXFieldOptions.CommitChanges>;
    DefaultSize: PXFieldState<PXFieldOptions.CommitChanges>;
    SizeUnit: PXFieldState<PXFieldOptions.CommitChanges>;
    Encoding: PXFieldState<PXFieldOptions.CommitChanges>;
    @controlConfig({ allowEdit: true })
    CategoryID: PXFieldState<PXFieldOptions.CommitChanges>;
    ImageUrl: PXFieldState<PXFieldOptions.CommitChanges>;
}

export class ALModel2 extends PXView {
    Name: PXFieldState;
    LabelID: PXFieldState;
    ImageUrl: PXFieldState;
    Description: PXFieldState;
    ModelType: PXFieldState<PXFieldOptions.CommitChanges>;
    CloudID: PXFieldState<PXFieldOptions.CommitChanges | PXFieldOptions.Disabled>;
    @controlConfig({ displayMode: "id" })
    ScreenID: PXFieldState<PXFieldOptions.CommitChanges>;
    GraphType: PXFieldState<PXFieldOptions.CommitChanges>;
    BasedOnView: PXFieldState;
    Message: PXFieldState;
    Active: PXFieldState<PXFieldOptions.CommitChanges>;
    AllowExport: PXFieldState<PXFieldOptions.CommitChanges>;
    IsSystem: PXFieldState<PXFieldOptions.CommitChanges>;
    HideWhenInGroup: PXFieldState<PXFieldOptions.CommitChanges>;
    IgnoreRotationOnRender: PXFieldState<PXFieldOptions.CommitChanges>;
    @controlConfig({ allowEdit: true })
    FormatID: PXFieldState<PXFieldOptions.CommitChanges>;
    @controlConfig({ allowEdit: true })
    MarginID: PXFieldState<PXFieldOptions.CommitChanges>;
}

@gridConfig({
    initNewRow: true,
    syncPosition: true,
    showFastFilter: GridFastFilterVisibility.False,
    preset: GridPreset.Details,
    topBarItems: {
        LoadLabelZoomDetails: {
            index: 0,
            config: {
                commandName: "LoadLabelZoomDetails",
                toolTip:
                    "Load Label elements associated with the CloudID from the LabelZoom site",
                text: "Pos",
            },
        },
        ClearLabelZoomDetails: {
            index: 1,
            config: {
                commandName: "ClearLabelZoomDetails",
                toolTip: "Delete Details",
                text: "Pos",
            },
        },
    },
})
export class ALModelExpr extends PXView {
    LoadLabelZoomDetails: PXActionState;
    ClearLabelZoomDetails: PXActionState;
    LabelID: PXFieldState;
    @columnConfig({ width: 50, textAlign: TextAlign.Right })
    LineNbr: PXFieldState;
    @columnConfig({ width: 60 }) Active: PXFieldState;
    @columnConfig({ width: 110 }) ExprCode: PXFieldState;
    @linkCommand("ViewDataElement")
    @columnConfig({ width: 220, editorConfig: { allowEdit: true } })
    DataElementID: PXFieldState<PXFieldOptions.CommitChanges>;
    @columnConfig({ width: 90 })
    ExprType: PXFieldState<PXFieldOptions.CommitChanges>;
    @columnConfig({ width: 150 })
    ExprValue: PXFieldState<PXFieldOptions.CommitChanges>;
    @columnConfig({ width: 120 }) LabelZoomType: PXFieldState;
    @columnConfig({ width: 80 }) PosX: PXFieldState<PXFieldOptions.CommitChanges>;
    @columnConfig({ width: 80 }) PosY: PXFieldState<PXFieldOptions.CommitChanges>;
    @columnConfig({ width: 80 }) LabelZoomID: PXFieldState;
    @linkCommand("ViewFont")
    @columnConfig({ width: 80, editorConfig: { allowEdit: true } })
    FontID: PXFieldState<PXFieldOptions.CommitChanges>;
    ValueRequired: PXFieldState;
    Description: PXFieldState;
    @linkCommand("ViewJustification")
    @columnConfig({ width: 120, editorConfig: { allowEdit: true } })
    JustificationID: PXFieldState<PXFieldOptions.CommitChanges>;
    @columnConfig({ width: 90 })
    Orientation: PXFieldState<PXFieldOptions.CommitChanges>;
    ReverseDots: PXFieldState;
    @columnConfig({ width: 150, editorConfig: { allowEdit: true } })
    RuleID: PXFieldState<PXFieldOptions.CommitChanges>;
    @columnConfig({ width: 90 })
    ReverseRule: PXFieldState<PXFieldOptions.CommitChanges>;
    @columnConfig({ editorConfig: { allowEdit: true } })
    ForeColorID: PXFieldState<PXFieldOptions.CommitChanges>;
    @columnConfig({ editorConfig: { allowEdit: true } })
    BackColorID: PXFieldState<PXFieldOptions.CommitChanges>;
    @columnConfig({ width: 90 })
    HexEncoding: PXFieldState<PXFieldOptions.CommitChanges>;
    LastModifiedByID: PXFieldState;
    LastModifiedDateTime: PXFieldState;
}

@gridConfig({
    initNewRow: true,
    syncPosition: true,
    showFastFilter: GridFastFilterVisibility.False,
    preset: GridPreset.Details,
})
export class ALModelGraphic extends PXView {
    ModelID: PXFieldState;
    @columnConfig({ textAlign: TextAlign.Right }) LineNbr: PXFieldState;
    @columnConfig({ width: 60 }) Active: PXFieldState;
    @columnConfig({ width: 100 })
    GraphicType: PXFieldState<PXFieldOptions.CommitChanges>;
    LabelZoomID: PXFieldState;
    @columnConfig({ width: 75 })
    FromX: PXFieldState<PXFieldOptions.CommitChanges>;
    @columnConfig({ width: 75 })
    FromY: PXFieldState<PXFieldOptions.CommitChanges>;
    @columnConfig({ width: 65 }) ToX: PXFieldState<PXFieldOptions.CommitChanges>;
    @columnConfig({ width: 65 }) ToY: PXFieldState<PXFieldOptions.CommitChanges>;
    @columnConfig({ width: 90 })
    Thickness: PXFieldState<PXFieldOptions.CommitChanges>;
    @columnConfig({ width: 90 })
    SizeUnit: PXFieldState<PXFieldOptions.CommitChanges>;
    @columnConfig({ width: 100 })
    Rounding: PXFieldState<PXFieldOptions.CommitChanges>;
    @columnConfig({
        width: 130,
        textAlign: TextAlign.Right,
        editorConfig: { allowEdit: true },
    })
    ForeColorID: PXFieldState<PXFieldOptions.CommitChanges>;
    @columnConfig({
        width: 130,
        textAlign: TextAlign.Right,
        editorConfig: { allowEdit: true },
    })
    BackColorID: PXFieldState<PXFieldOptions.CommitChanges>;
    LastModifiedByID: PXFieldState;
    LastModifiedDateTime: PXFieldState;
}

@gridConfig({
    initNewRow: true,
    showFastFilter: GridFastFilterVisibility.False,
    preset: GridPreset.Details,
})
export class ALModelPrinter extends PXView {
    LabelID: PXFieldState;
    @columnConfig({ textAlign: TextAlign.Right }) LineNbr: PXFieldState;
    Active: PXFieldState;
    @linkCommand("ViewUser")
    @columnConfig({ width: 100, editorConfig: { allowEdit: true } })
    UserID: PXFieldState<PXFieldOptions.CommitChanges>;
    @columnConfig({
        width: 150,
        editorConfig: { allowEdit: true, displayMode: "id" },
    })
    WorkGroupID: PXFieldState<PXFieldOptions.CommitChanges>;
    @columnConfig({
        width: 150,
        editorConfig: { allowEdit: true, displayMode: "text" },
    })
    OwnerID: PXFieldState<PXFieldOptions.CommitChanges>;
    @linkCommand("ViewPrintStation")
    @columnConfig({ width: 150, editorConfig: { allowEdit: true } })
    PrintStationID: PXFieldState<PXFieldOptions.CommitChanges>;
    @linkCommand("ViewPrinter")
    @columnConfig({ width: 150, editorConfig: { allowEdit: true } })
    PrinterID: PXFieldState<PXFieldOptions.CommitChanges>;
    @columnConfig({ width: 100 }) CreatedByID: PXFieldState;
    @columnConfig({ width: 100 }) CreatedDateTime: PXFieldState;
    @columnConfig({ width: 100 }) LastModifiedByID: PXFieldState;
    @columnConfig({ width: 100 }) LastModifiedDateTime: PXFieldState;
}

@gridConfig({
    initNewRow: true,
    syncPosition: true,
    showFastFilter: GridFastFilterVisibility.False,
    preset: GridPreset.Details,
    topBarItems: {
        LoadChildren: {
            index: 0,
            config: { commandName: "LoadChildren", text: "Load Children" },
        },
    },
})
export class ALModelChild extends PXView {
    LoadChildren: PXActionState;
    LabelID: PXFieldState;
    @columnConfig({ textAlign: TextAlign.Right }) LineNbr: PXFieldState;
    Active: PXFieldState;
    @linkCommand("ViewLabelChild")
    @columnConfig({ width: 150, editorConfig: { allowEdit: true } })
    LabelChildID: PXFieldState<PXFieldOptions.CommitChanges>;
    @columnConfig({ width: 100 }) ALModel: ALModelChildType;
    @columnConfig({ width: 100 }) LastModifiedByID: PXFieldState;
    @columnConfig({ width: 100 }) LastModifiedDateTime: PXFieldState;
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

@gridConfig({
    showFastFilter: GridFastFilterVisibility.False,
    preset: GridPreset.ReadOnly,
})
export class ALPrintLog extends PXView {
    @linkCommand("ViewPrintLog")
    @columnConfig({ textAlign: TextAlign.Right })
    RecordID: PXFieldState;
    @columnConfig({ width: 150 }) LabelKey: PXFieldState;
    @columnConfig({ width: 350 }) LabelFilename: PXFieldState;
    @columnConfig({ width: 100 }) UserID: PXFieldState;
    @linkCommand("ViewPrinter")
    @columnConfig({ width: 180 })
    PrinterID: PXFieldState;
    @linkCommand("ViewFormat")
    @columnConfig({ width: 120 })
    PrinterFormatID: PXFieldState;
    @linkCommand("ViewPrintStation")
    @columnConfig({ width: 120 })
    PrintStationID: PXFieldState;
    @linkCommand("ViewBAccount")
    @columnConfig({ width: 140 })
    BAccountID: PXFieldState;
    @linkCommand("ViewInventory")
    @columnConfig({ width: 140 })
    InventoryID: PXFieldState;
    @columnConfig({ width: 140 }) LotSerialNbr: PXFieldState;
    @columnConfig({ width: 140 }) PrintJobID: PXFieldState;
    @columnConfig({ width: 90 }) CreatedDateTime: PXFieldState;
}

export class ALChangeIDParam extends PXView {
    Name: PXFieldState;
}

export class ALDataElementFilter extends PXView {
    ExprType: PXFieldState<PXFieldOptions.CommitChanges>;
    BasedOn: PXFieldState<PXFieldOptions.CommitChanges>;
    ExprValue: PXFieldState<PXFieldOptions.CommitChanges>;
    WithBarcode: PXFieldState<PXFieldOptions.CommitChanges>;
    CategoryID: PXFieldState<PXFieldOptions.CommitChanges>;
    ContentID: PXFieldState<PXFieldOptions.CommitChanges>;
    SubstitutionID: PXFieldState<PXFieldOptions.CommitChanges>;
}

@gridConfig({
    allowDelete: false,
    allowInsert: false,
    autoAdjustColumns: true,
    showFastFilter: GridFastFilterVisibility.False,
    preset: GridPreset.ReadOnly,
})
export class ALDataElement2 extends PXView {
    @columnConfig({ allowNull: false, width: 80 }) Selected: PXFieldState;
    @columnConfig({ width: 220 }) Name: PXFieldState;
    @columnConfig({ width: 280 }) Description: PXFieldState;
    @columnConfig({ width: 180 }) ALDataSource: ALDataElementType;
    @columnConfig({ width: 180 }) CategoryID: PXFieldState;
    @columnConfig({ width: 180 }) BarcodeID: PXFieldState;
    @columnConfig({ width: 180 }) SubstitutionID: PXFieldState;
    @columnConfig({ width: 180 }) ContentID: PXFieldState;
    @columnConfig({ width: 180 }) PrinterFileGUID: PXFieldState;
}
