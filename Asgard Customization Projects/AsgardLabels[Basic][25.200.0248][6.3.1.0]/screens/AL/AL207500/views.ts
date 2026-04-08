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
import { AL207500 } from "./AL207500";

// Views

export class ALCategory extends PXView {
  Name: PXFieldState;
  Description: PXFieldState;
  Active: PXFieldState<PXFieldOptions.CommitChanges>;
  IsSystem: PXFieldState<PXFieldOptions.CommitChanges>;
  AllowExport: PXFieldState<PXFieldOptions.CommitChanges>;
}

export class ALCategory2 extends PXView {}

@gridConfig({
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.ReadOnly,
})
export class ALModel extends PXView {
  @linkCommand("ViewModel")
  @columnConfig({ width: 300 })
  Name: PXFieldState;
  @columnConfig({ width: 300 }) Description: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedDateTime: PXFieldState;
}

@gridConfig({
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.ReadOnly,
})
export class ALDataElement extends PXView {
  @linkCommand("ViewModel")
  @columnConfig({ width: 300 })
  Name: PXFieldState;
  @columnConfig({ width: 300 }) Description: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedDateTime: PXFieldState;
}

@gridConfig({
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.ReadOnly,
})
export class ALContent extends PXView {
  @linkCommand("ViewContent")
  @columnConfig({ width: 300 })
  Name: PXFieldState;
  @columnConfig({ width: 300 }) Description: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedDateTime: PXFieldState;
}

@gridConfig({
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.ReadOnly,
})
export class ALSubstitution extends PXView {
  @linkCommand("ViewSubstitution")
  @columnConfig({ width: 300 })
  Name: PXFieldState;
  @columnConfig({ width: 300 }) Description: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedDateTime: PXFieldState;
}

@gridConfig({
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.ReadOnly,
})
export class ALColor extends PXView {
  @linkCommand("ViewColor")
  @columnConfig({ width: 300 })
  Name: PXFieldState;
  @columnConfig({ width: 300 }) Description: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedDateTime: PXFieldState;
}

@gridConfig({
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.ReadOnly,
})
export class ALRule extends PXView {
  @linkCommand("ViewRule")
  @columnConfig({ width: 300 })
  Name: PXFieldState;
  @columnConfig({ width: 300 }) Description: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedDateTime: PXFieldState;
}

@gridConfig({
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.ReadOnly,
})
export class ALFormat extends PXView {
  @linkCommand("ViewFormat")
  @columnConfig({ width: 300 })
  Name: PXFieldState;
  @columnConfig({ width: 300 }) Description: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedDateTime: PXFieldState;
}

@gridConfig({
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.ReadOnly,
})
export class ALMargin extends PXView {
  @linkCommand("ViewMargin")
  @columnConfig({ width: 300 })
  Name: PXFieldState;
  @columnConfig({ width: 300 }) Description: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedDateTime: PXFieldState;
}

@gridConfig({
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.ReadOnly,
})
export class ALJustification extends PXView {
  @linkCommand("ViewJustification")
  @columnConfig({ width: 300 })
  Name: PXFieldState;
  @columnConfig({ width: 300 }) Description: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedDateTime: PXFieldState;
}

@gridConfig({
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.ReadOnly,
})
export class ALBarcode extends PXView {
  @linkCommand("ViewBarcode")
  @columnConfig({ width: 300 })
  Name: PXFieldState;
  @columnConfig({ width: 300 }) Description: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedDateTime: PXFieldState;
}

@gridConfig({
  showFastFilter: GridFastFilterVisibility.False,
  preset: GridPreset.ReadOnly,
})
export class ALFont extends PXView {
  @linkCommand("ViewFont")
  @columnConfig({ width: 300 })
  Name: PXFieldState;
  @columnConfig({ width: 300 }) Description: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedByID: PXFieldState;
  @columnConfig({ width: 150 }) LastModifiedDateTime: PXFieldState;
}

export class ALChangeIDParam extends PXView {
  Name: PXFieldState;
}
