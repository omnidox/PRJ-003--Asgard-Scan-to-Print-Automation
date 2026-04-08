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
import { AL303500 } from "./AL303500";

export class ALModelExprType {
  Name: PXFieldState;
  Description: PXFieldState;
}
export class ALContentElementType {
  Name: PXFieldState;
  Description: PXFieldState;
}

// Views

export class ALDataElement extends PXView {
  Name: PXFieldState<PXFieldOptions.CommitChanges>;
  GenName: PXFieldState<PXFieldOptions.CommitChanges>;
  Active: PXFieldState<PXFieldOptions.CommitChanges>;
  IsSystem: PXFieldState<PXFieldOptions.CommitChanges>;
  ExprType: PXFieldState<PXFieldOptions.CommitChanges>;
  @controlConfig({ allowEdit: true })
  SourceID: PXFieldState<PXFieldOptions.CommitChanges>;
  @fieldConfig({
    controlType: "qp-tree-selector",
    controlConfig: {
      treeConfig: {
        idField: ["Key"],
        textField: "Name",
        toolTipField: "Path",
        iconField: "Icon",
        dynamic: true,
        hideRootNode: true,
        openedLayers: 0,
        syncPosition: true,
        modifiable: false,
        mode: "single",
        topBarItems: {},
      },
      allowEditValue: true,
    },
  })
  BasedOn: PXFieldState<PXFieldOptions.CommitChanges>;
  @fieldConfig({
    controlType: "qp-tree-selector",
    controlConfig: {
      treeConfig: {
        idField: ["Key"],
        textField: "Name",
        toolTipField: "Path",
        iconField: "Icon",
        dynamic: true,
        hideRootNode: true,
        openedLayers: 0,
        syncPosition: true,
        modifiable: false,
        mode: "single",
        topBarItems: {},
      },
      allowEditValue: true,
    },
  })
  ExprValue: PXFieldState<PXFieldOptions.CommitChanges>;
  @controlConfig({ allowEdit: true })
  SnippetID: PXFieldState<PXFieldOptions.CommitChanges>;
  @controlConfig({ allowEdit: true })
  ContentID: PXFieldState<PXFieldOptions.CommitChanges>;
  @controlConfig({ allowEdit: true })
  PrinterFileGUID: PXFieldState<PXFieldOptions.CommitChanges>;
  ArgName1: PXFieldState<
    PXFieldOptions.CommitChanges | PXFieldOptions.Disabled
  >;
  Arg1: PXFieldState<PXFieldOptions.CommitChanges>;
  ArgName2: PXFieldState<
    PXFieldOptions.CommitChanges | PXFieldOptions.Disabled
  >;
  Arg2: PXFieldState<PXFieldOptions.CommitChanges>;
  ArgName3: PXFieldState<
    PXFieldOptions.CommitChanges | PXFieldOptions.Disabled
  >;
  Arg3: PXFieldState<PXFieldOptions.CommitChanges>;
  ArgName4: PXFieldState<
    PXFieldOptions.CommitChanges | PXFieldOptions.Disabled
  >;
  Arg4: PXFieldState<PXFieldOptions.CommitChanges>;
  ArgName5: PXFieldState<
    PXFieldOptions.CommitChanges | PXFieldOptions.Disabled
  >;
  Arg5: PXFieldState<PXFieldOptions.CommitChanges>;
  ArgName6: PXFieldState<
    PXFieldOptions.CommitChanges | PXFieldOptions.Disabled
  >;
  Arg6: PXFieldState<PXFieldOptions.CommitChanges>;
  DoSubstitute: PXFieldState<PXFieldOptions.CommitChanges>;
  @controlConfig({ allowEdit: true })
  SubstitutionID: PXFieldState<PXFieldOptions.CommitChanges>;
  @controlConfig({ allowEdit: true })
  BarcodeID: PXFieldState<PXFieldOptions.CommitChanges>;
  @controlConfig({ allowEdit: true })
  CategoryID: PXFieldState<PXFieldOptions.CommitChanges>;
  SampleType: PXFieldState<PXFieldOptions.CommitChanges>;
  @fieldConfig({
    controlType: "qp-tree-selector",
    controlConfig: {
      treeConfig: {
        idField: ["Key"],
        textField: "Name",
        toolTipField: "Path",
        iconField: "Icon",
        dynamic: true,
        hideRootNode: true,
        openedLayers: 0,
        syncPosition: true,
        modifiable: false,
        mode: "single",
        topBarItems: {},
      },
      allowEditValue: true,
    },
  })
  SampleBasedOn: PXFieldState<PXFieldOptions.CommitChanges>;
  @fieldConfig({
    controlType: "qp-tree-selector",
    controlConfig: {
      treeConfig: {
        idField: ["Key"],
        textField: "Name",
        toolTipField: "Path",
        iconField: "Icon",
        dynamic: true,
        hideRootNode: true,
        openedLayers: 0,
        syncPosition: true,
        modifiable: false,
        mode: "single",
        topBarItems: {},
      },
      allowEditValue: true,
    },
  })
  SampleValue: PXFieldState<PXFieldOptions.CommitChanges>;
}

@treeConfig({
  idField: ["Key"],
  textField: "Name",
  toolTipField: "Path",
  iconField: "Icon",
  dynamic: true,
  hideRootNode: true,
  openedLayers: 0,
  syncPosition: true,
  modifiable: false,
  mode: "single",
  topBarItems: {},
})
export class CacheEntityItem extends PXView {
  Key: PXFieldState;
  Icon: PXFieldState;
  Name: PXFieldState;
  Path: PXFieldState;
}

@treeConfig({
  idField: ["Key"],
  textField: "Name",
  toolTipField: "Path",
  iconField: "Icon",
  dynamic: true,
  hideRootNode: true,
  openedLayers: 0,
  syncPosition: true,
  modifiable: false,
  mode: "single",
  topBarItems: {},
})
export class CacheEntityItem2 extends PXView {
  Key: PXFieldState;
  Icon: PXFieldState;
  Name: PXFieldState;
  Path: PXFieldState;
}

@treeConfig({
  idField: ["Key"],
  textField: "Name",
  toolTipField: "Path",
  iconField: "Icon",
  dynamic: true,
  hideRootNode: true,
  openedLayers: 0,
  syncPosition: true,
  modifiable: false,
  mode: "single",
  topBarItems: {},
})
export class CacheEntityItem3 extends PXView {
  Key: PXFieldState;
  Icon: PXFieldState;
  Name: PXFieldState;
  Path: PXFieldState;
}

@treeConfig({
  idField: ["Key"],
  textField: "Name",
  toolTipField: "Path",
  iconField: "Icon",
  dynamic: true,
  hideRootNode: true,
  openedLayers: 0,
  syncPosition: true,
  modifiable: false,
  mode: "single",
  topBarItems: {},
})
export class CacheEntityItem4 extends PXView {
  Key: PXFieldState;
  Icon: PXFieldState;
  Name: PXFieldState;
  Path: PXFieldState;
}

@gridConfig({
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.ReadOnly,
})
export class ALModelExpr extends PXView {
  @columnConfig({ allowNull: false }) Active: PXFieldState;
  @linkCommand("ViewModel")
  @columnConfig({ width: 350 })
  ALModel: ALModelExprType;
  @columnConfig({ width: 150 }) LineNbr: PXFieldState;
  @linkCommand("ViewDataElement")
  @columnConfig({ width: 300 })
  ALDataElement: ALModelExprType;
  @columnConfig({ width: 300 }) ExprValue: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedDateTime: PXFieldState;
}

@gridConfig({
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.ReadOnly,
})
export class ALContentElement extends PXView {
  @linkCommand("ViewModel")
  @columnConfig({ width: 350 })
  ALContent: ALContentElementType;
  @columnConfig({ allowNull: false }) Active: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedDateTime: PXFieldState;
}
