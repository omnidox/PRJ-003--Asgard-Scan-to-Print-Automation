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
import { AL203500 } from "./AL203500";

export class ALModelExprType {
  Name: PXFieldState;
  Description: PXFieldState;
}
export class ALRuleDetailType {
  Name: PXFieldState;
  LastModifiedByID: PXFieldState;
  LastModifiedDateTime: PXFieldState;
}
export class ALColorRuleType {
  Name: PXFieldState;
  Description: PXFieldState;
}
export class ALFormatRuleType {
  Name: PXFieldState;
  Description: PXFieldState;
}
export class ALContentElementType {
  Name: PXFieldState;
  Description: PXFieldState;
}

// Views

export class ALRule extends PXView {
  Name: PXFieldState;
  Description: PXFieldState;
  @controlConfig({ displayMode: "id" })
  ScreenID: PXFieldState<PXFieldOptions.CommitChanges>;
  GraphType: PXFieldState<PXFieldOptions.CommitChanges>;
  @controlConfig({ allowEdit: true })
  CategoryID: PXFieldState;
  @fieldConfig({
      controlType: "qp-tree-selector",
      controlConfig: {
          treeConfig: {
              idField: "Key",
              valueField: "Path",
              dataMember: "EntityItems",
              textField: "Name",
              iconField: "Icon",
              mode: 'single',
              openedLayers: 0,
              modifiable: false,
              hideRootNode: true,
          },
          allowEditValue: true,
          appendSelectedValue: true,
      }
  })
  Expression: PXFieldState<PXFieldOptions.CommitChanges>;
  Active: PXFieldState<PXFieldOptions.CommitChanges>;
  IsSystem: PXFieldState<PXFieldOptions.CommitChanges>;
  AllowExport: PXFieldState<PXFieldOptions.CommitChanges>;
  IsComposite: PXFieldState<PXFieldOptions.CommitChanges>;
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

export class ALRule2 extends PXView {
  ShowExpr: PXFieldState;
  ShowUsedBy: PXFieldState;
  ShowChildren: PXFieldState;
}

@gridConfig({
  initNewRow: true,
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.Details,
})
export class ALRuleDetail extends PXView {
  @columnConfig({ allowNull: false })
  Active: PXFieldState<PXFieldOptions.CommitChanges>;
  LineNbr: PXFieldState;
  SortOrder: PXFieldState;
  OpenBracket: PXFieldState;
  Reverse: PXFieldState;
  @linkCommand("ViewRule")
  @columnConfig({ width: 250, editorConfig: {} })
  ChildRuleID: PXFieldState<PXFieldOptions.CommitChanges>;
  CloseBracket: PXFieldState;
  Operation: PXFieldState;
}

@gridConfig({
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.ReadOnly,
})
export class ALModel extends PXView {
  @linkCommand("ViewModel")
  @columnConfig({ width: 300, editorConfig: {} })
  Name: PXFieldState;
  @columnConfig({ width: 300 }) Description: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedDateTime: PXFieldState;
}

@gridConfig({
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.ReadOnly,
})
export class ALModelExpr extends PXView {
  @linkCommand("ViewModel")
  @columnConfig({ width: 300 })
  ALModel: ALModelExprType;
  @linkCommand("ViewModelExpr")
  @columnConfig({ width: 300 })
  ExprCode: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedDateTime: PXFieldState;
}

@gridConfig({
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.ReadOnly,
})
export class ALRuleDetail2 extends PXView {
  @linkCommand("ViewModel")
  @columnConfig({ width: 300 })
  ALRule: ALRuleDetailType;
}

@gridConfig({
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.ReadOnly,
})
export class ALColorRule extends PXView {
  @linkCommand("ViewModel")
  @columnConfig({ width: 300 })
  ALColor: ALColorRuleType;
  @columnConfig({ width: 150 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedDateTime: PXFieldState;
}

@gridConfig({
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.ReadOnly,
})
export class ALFormatRule extends PXView {
  @linkCommand("ViewModel")
  @columnConfig({ width: 300 })
  ALFormat: ALFormatRuleType;
  @columnConfig({ width: 150 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedDateTime: PXFieldState;
}

@gridConfig({
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.ReadOnly,
})
export class ALContentElement extends PXView {
  @linkCommand("ViewModel")
  @columnConfig({ width: 300 })
  ALContent: ALContentElementType;
  @columnConfig({ width: 150 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedDateTime: PXFieldState;
}

@gridConfig({
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.ReadOnly,
})
export class ALAutoPrint extends PXView {
  @linkCommand("ViewAutoPrint")
  @columnConfig({ width: 300 })
  Name: PXFieldState;
  @columnConfig({ width: 300 }) Description: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedDateTime: PXFieldState;
}

export class ALChangeIDParam extends PXView {
  Name: PXFieldState;
}
