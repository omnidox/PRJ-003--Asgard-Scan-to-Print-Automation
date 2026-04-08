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
import { AL206500 } from "./AL206500";

export class ALSubstitutionDetailType {
  Name: PXFieldState;
}
export class ALDataElementType {
  Name: PXFieldState;
}
export class ALContentElementType {
  Name: PXFieldState;
}

// Views

export class ALSubstitution extends PXView {
  Name: PXFieldState;
  Description: PXFieldState;
  CategoryID: PXFieldState;
  TypeName: PXFieldState<PXFieldOptions.Disabled>;
  FunctionName: PXFieldState<PXFieldOptions.Disabled>;
  Signature: PXFieldState<PXFieldOptions.Disabled>;
  InternalName: PXFieldState<PXFieldOptions.Disabled>;
  ReturnTypeName: PXFieldState<PXFieldOptions.Disabled>;
  Active: PXFieldState<PXFieldOptions.CommitChanges>;
  IsComposite: PXFieldState<PXFieldOptions.CommitChanges>;
  IsSystem: PXFieldState<PXFieldOptions.CommitChanges>;
  AllowExport: PXFieldState<PXFieldOptions.CommitChanges>;
}

export class ALSubstitution2 extends PXView {
  ShowChildren: PXFieldState;
}

@gridConfig({
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.Details,
})
export class ALSubstitutionDetail extends PXView {
  @columnConfig({ width: 200 })
  SubstitutionID: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ allowNull: false })
  Active: PXFieldState<PXFieldOptions.CommitChanges>;
  LineNbr: PXFieldState;
  SortOrder: PXFieldState;
  @linkCommand("ViewSubstitution")
  @columnConfig({ width: 250, editorConfig: {} })
  ChildSubstitutionID: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 100 })
  Arg1: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 300 })
  Arg2: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 300 })
  Arg3: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 250 })
  Arg4: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 200 })
  Arg5: PXFieldState<PXFieldOptions.CommitChanges>;
  @columnConfig({ width: 150 })
  Arg6: PXFieldState<PXFieldOptions.CommitChanges>;
}

@gridConfig({
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.ReadOnly,
})
export class ALSubstitutionDetail2 extends PXView {
  @linkCommand("ViewModel")
  @columnConfig({ width: 350 })
  ALSubstitution: ALSubstitutionDetailType;
  @columnConfig({ allowNull: false }) Active: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedDateTime: PXFieldState;
}

@gridConfig({
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.ReadOnly,
})
export class ALDataElement extends PXView {
  @linkCommand("ViewModel")
  @columnConfig({ width: 350 })
  ALDataSource: ALDataElementType;
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
export class ALContentElement extends PXView {
  @linkCommand("ViewModel")
  @columnConfig({ width: 350 })
  ALContent: ALContentElementType;
  @columnConfig({ allowNull: false }) Active: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedDateTime: PXFieldState;
}

export class ALChangeIDParam extends PXView {
  Name: PXFieldState;
}
