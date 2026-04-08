import {
	GridFilterBarVisibility,
    GridColumnShowHideMode,
    GridPreset,
    PXFieldOptions,
    PXFieldState,
    PXScreen,
    PXView,
    columnConfig,
    createCollection,
    createSingle,
    graphInfo,
    gridConfig,
    viewInfo
} from "client-controls";

@graphInfo({
	graphType: "AA.Objects.Labels.ALPrintStationMaint",
	primaryView: "Document"
})

export class AL204500 extends PXScreen {
	@viewInfo({ containerName: "Print Station Summary" })
	Document = createSingle(ALPrintStation);

	@viewInfo({ containerName: "Print Station Summary" })
	CurrentDocument = createSingle(ALPrintStationCurrent);

	@viewInfo({ containerName: "UsedByPrinters_grid" })
	UsedByPrinters = createCollection(ALPrinter);

	@viewInfo({ containerName: "UsedByModelPrinters" })
	UsedByModelPrinters = createCollection(ALModelPrinter);

	@viewInfo({ containerName: "UsedByUsers" })
	UsedByUsers = createCollection(UserPreferences);

	@viewInfo({ containerName: "Specify New ID" })
	ChangeIDDialog = createSingle(ALChangeIDParam);
}

export class ALPrintStation extends PXView {
	@columnConfig({ hideViewLink: true })
	Name: PXFieldState;
	Description: PXFieldState;
	Active: PXFieldState<PXFieldOptions.CommitChanges>;
	AllowExport: PXFieldState<PXFieldOptions.CommitChanges>;
}

export class ALPrintStationCurrent extends PXView {
	Name: PXFieldState;
	Description: PXFieldState;
	Active: PXFieldState<PXFieldOptions.CommitChanges>;
	AllowExport: PXFieldState<PXFieldOptions.CommitChanges>;
}


@gridConfig({
	preset: GridPreset.ReadOnly,
	adjustPageSize: true,
	syncPosition: true,
	allowUpdate: false,
	showFilterBar: GridFilterBarVisibility.OnDemand,
})
export class ALPrinter extends PXView {
	@columnConfig({ allowShowHide: GridColumnShowHideMode.Server })
	@columnConfig({ hideViewLink: true })
	Name: PXFieldState<PXFieldOptions.CommitChanges>;
	Description: PXFieldState<PXFieldOptions.CommitChanges>;
	Active: PXFieldState;
	LastModifiedByID: PXFieldState<PXFieldOptions.CommitChanges>;
	LastModifiedDateTime: PXFieldState<PXFieldOptions.CommitChanges>;
}

@gridConfig({
	preset: GridPreset.ReadOnly,
	adjustPageSize: true,
	syncPosition: true,
	allowUpdate: false,
	showFilterBar: GridFilterBarVisibility.OnDemand,
})
export class ALModelPrinter extends PXView {
	@columnConfig({ allowShowHide: GridColumnShowHideMode.Server })
	@columnConfig({ hideViewLink: true })
	ALModel__Name: PXFieldState<PXFieldOptions.CommitChanges>;
	ALModel__Description: PXFieldState<PXFieldOptions.CommitChanges>;
	Active: PXFieldState;
	LastModifiedByID: PXFieldState<PXFieldOptions.CommitChanges>;
	LastModifiedDateTime: PXFieldState<PXFieldOptions.CommitChanges>;
}

@gridConfig({
	preset: GridPreset.ReadOnly,
	adjustPageSize: true,
	syncPosition: true,
	allowUpdate: false,
	showFilterBar: GridFilterBarVisibility.OnDemand,
})
export class UserPreferences extends PXView {
	@columnConfig({ allowShowHide: GridColumnShowHideMode.Server })
	@columnConfig({ hideViewLink: true })
	Users__Username: PXFieldState<PXFieldOptions.CommitChanges>;
	@columnConfig({ hideViewLink: true })
	Users__DisplayName: PXFieldState<PXFieldOptions.CommitChanges>;
	Users__State: PXFieldState;
	LastModifiedByID: PXFieldState<PXFieldOptions.CommitChanges>;
	LastModifiedDateTime: PXFieldState<PXFieldOptions.CommitChanges>;
}
export class ALChangeIDParam extends PXView {
	Name: PXFieldState;
}
