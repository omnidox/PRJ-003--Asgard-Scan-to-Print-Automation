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
import { AL303000 } from "./AL303000";

// Views

export class ALDataSource extends PXView {
  Name: PXFieldState;
  Description: PXFieldState;
  @controlConfig({ displayMode: "id" })
  ScreenID: PXFieldState<PXFieldOptions.CommitChanges>;
  GraphType: PXFieldState<PXFieldOptions.CommitChanges>;
  Active: PXFieldState<PXFieldOptions.CommitChanges>;
  IsSystem: PXFieldState<PXFieldOptions.CommitChanges>;
  AllowExport: PXFieldState<PXFieldOptions.CommitChanges>;
}

export class ALDataSource2 extends PXView {
  IsGlobal: PXFieldState;
  IsScreenBased: PXFieldState;
  ShowImages: PXFieldState;
  ShowFixed: PXFieldState;
  ShowFunctions: PXFieldState;
  ShowScreens: PXFieldState;
  ShowIterators: PXFieldState;
  ShowScripts: PXFieldState;
}

@gridConfig({
  initNewRow: true,
  syncPosition: true,
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.Details,
  topBarItems: {
    DuplicateRowContent: {
      index: 0,
      config: { commandName: "DuplicateRowContent", text: "Duplicate" },
    },
  },
})
export class ALDataElement extends PXView {
  DuplicateRowContent: PXActionState;
  SourceID: PXFieldState;
  @columnConfig({ width: 60 })
  Active: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 70 })
  IsSystem: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 60 })
  GenName: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ textAlign: TextAlign.Right }) LineNbr: PXFieldState;
  @columnConfig({ width: 150, editorConfig: { allowEdit: true } })
  CategoryID: PXFieldState<PXFieldOptions.CommitChanges>;
  @linkCommand("ViewDataElement")
  @columnConfig({ width: 300 })
  Name: PXFieldState;
  @columnConfig({ width: 300, editorConfig: { allowEdit: true } })
  ContentID: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 250, editorConfig: { allowEdit: true } })
  BarcodeID: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 150 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedDateTime: PXFieldState;
}

@gridConfig({
  initNewRow: true,
  syncPosition: true,
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.Details,
  topBarItems: {
    DuplicateRowFunction: {
      index: 0,
      config: { commandName: "DuplicateRowFunction", text: "Duplicate" },
    },
  },
})
export class ALDataElement2 extends PXView {
  DuplicateRowFunction: PXActionState;
  SourceID: PXFieldState;
  @columnConfig({ width: 60 })
  Active: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 70 })
  IsSystem: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 60 })
  GenName: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ textAlign: TextAlign.Right }) LineNbr: PXFieldState;
  @columnConfig({ width: 150, editorConfig: { allowEdit: true } })
  CategoryID: PXFieldState<PXFieldOptions.CommitChanges>;
  @linkCommand("ViewDataElement")
  @columnConfig({ width: 350 })
  Name: PXFieldState;
  @columnConfig({ width: 90 })
  BasedOn: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 120 })
  ExprValue: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 170 })
  Arg1: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 170 })
  Arg2: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 170 })
  Arg3: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 100 })
  Arg4: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 100 })
  Arg5: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 100 })
  Arg6: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 90 })
  SampleType: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 100 })
  SampleBasedOn: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 100 })
  SampleValue: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ allowNull: false, width: 120 })
  DoSubstitute: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 150, editorConfig: { allowEdit: true } })
  SubstitutionID: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 150, editorConfig: { allowEdit: true } })
  BarcodeID: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 150 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedDateTime: PXFieldState;
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

@gridConfig({
  initNewRow: true,
  syncPosition: true,
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.Details,
  topBarItems: {
    DuplicateRowScreen: {
      index: 0,
      config: { commandName: "DuplicateRowScreen", text: "Duplicate" },
    },
  },
})
export class ALDataElement3 extends PXView {
  DuplicateRowScreen: PXActionState;
  SourceID: PXFieldState;
  @columnConfig({ width: 60 })
  Active: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 70 })
  IsSystem: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 60 })
  GenName: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ textAlign: TextAlign.Right }) LineNbr: PXFieldState;
  @columnConfig({ width: 150, editorConfig: { allowEdit: true } })
  CategoryID: PXFieldState<PXFieldOptions.CommitChanges>;
  @linkCommand("ViewDataElement")
  @columnConfig({ width: 300 })
  Name: PXFieldState;
  @columnConfig({ width: 150 })
  BasedOn: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 300 })
  ExprValue: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 90 })
  SampleType: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 150 })
  SampleBasedOn: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 200 })
  SampleValue: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 150, editorConfig: { allowEdit: true } })
  BarcodeID: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ allowNull: false, width: 120 })
  DoSubstitute: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 150, editorConfig: { allowEdit: true } })
  SubstitutionID: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 150 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedDateTime: PXFieldState;
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
  initNewRow: true,
  syncPosition: true,
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.Details,
  topBarItems: {
    DuplicateRowFixed: {
      index: 0,
      config: { commandName: "DuplicateRowFixed", text: "Duplicate" },
    },
  },
})
export class ALDataElement4 extends PXView {
  DuplicateRowFixed: PXActionState;
  SourceID: PXFieldState;
  @columnConfig({ width: 60 })
  Active: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 70 })
  IsSystem: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 60 })
  GenName: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ textAlign: TextAlign.Right }) LineNbr: PXFieldState;
  @columnConfig({ width: 150, editorConfig: { allowEdit: true } })
  CategoryID: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 300 })
  ExprValue: PXFieldState<PXFieldOptions.CommitChanges>;
  @linkCommand("ViewDataElement")
  @columnConfig({ width: 140 })
  Name: PXFieldState;
  @columnConfig({ allowNull: false, width: 120 })
  DoSubstitute: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 150, editorConfig: { allowEdit: true } })
  SubstitutionID: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 150, editorConfig: { allowEdit: true } })
  BarcodeID: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 150 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedDateTime: PXFieldState;
}

@gridConfig({
  initNewRow: true,
  syncPosition: true,
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.Details,
  topBarItems: {
    DuplicateRowImage: {
      index: 0,
      config: { commandName: "DuplicateRowImage", text: "Duplicate" },
    },
  },
})
export class ALDataElement5 extends PXView {
  DuplicateRowImage: PXActionState;
  SourceID: PXFieldState;
  @columnConfig({ width: 60 })
  Active: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 70 })
  IsSystem: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 60 })
  GenName: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ textAlign: TextAlign.Right }) LineNbr: PXFieldState;
  @columnConfig({ width: 150, editorConfig: { allowEdit: true } })
  CategoryID: PXFieldState<PXFieldOptions.CommitChanges>;
  @linkCommand("ViewPrinterFile")
  @columnConfig({ width: 250, editorConfig: { allowEdit: true } })
  PrinterFileGUID: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 200 })
  Arg1: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 200 })
  Arg2: PXFieldState<PXFieldOptions.CommitChanges>;
  @linkCommand("ViewDataElement")
  @columnConfig({ width: 250 })
  Name: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedDateTime: PXFieldState;
}

@gridConfig({
  initNewRow: true,
  syncPosition: true,
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.Details,
  topBarItems: {
    DuplicateRowIterator: {
      index: 0,
      config: { commandName: "DuplicateRowIterator", text: "Duplicate" },
    },
  },
})
export class ALDataElement6 extends PXView {
  DuplicateRowIterator: PXActionState;
  SourceID: PXFieldState;
  @columnConfig({ width: 60 })
  Active: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 70 })
  IsSystem: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 60 })
  GenName: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ textAlign: TextAlign.Right }) LineNbr: PXFieldState;
  @columnConfig({ width: 150, editorConfig: { allowEdit: true } })
  CategoryID: PXFieldState<PXFieldOptions.CommitChanges>;
  @linkCommand("ViewDataElement")
  @columnConfig({ width: 300 })
  Name: PXFieldState;
  @columnConfig({ width: 150, editorConfig: { allowEdit: true } })
  SnippetID: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 130, textAlign: TextAlign.Right })
  Arg1: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 170, textAlign: TextAlign.Right })
  Arg2: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 130, textAlign: TextAlign.Right })
  Arg3: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 200, textAlign: TextAlign.Right })
  Arg4: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 170, textAlign: TextAlign.Right })
  Arg5: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 150 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedDateTime: PXFieldState;
}

@gridConfig({
  initNewRow: true,
  syncPosition: true,
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.Details,
  topBarItems: {
    DuplicateRowScript: {
      index: 0,
      config: { commandName: "DuplicateRowScript", text: "Duplicate" },
    },
  },
})
export class ALDataElement7 extends PXView {
  DuplicateRowScript: PXActionState;
  SourceID: PXFieldState;
  @columnConfig({ width: 60 })
  Active: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 70 })
  IsSystem: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 60 })
  GenName: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ textAlign: TextAlign.Right }) LineNbr: PXFieldState;
  @columnConfig({ width: 150, editorConfig: { allowEdit: true } })
  CategoryID: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 800 })
  ExprValue: PXFieldState<PXFieldOptions.CommitChanges>;
  @linkCommand("ViewDataElement")
  @columnConfig({ width: 140 })
  Name: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedDateTime: PXFieldState;
}

export class ALChangeIDParam extends PXView {
  Name: PXFieldState;
}
