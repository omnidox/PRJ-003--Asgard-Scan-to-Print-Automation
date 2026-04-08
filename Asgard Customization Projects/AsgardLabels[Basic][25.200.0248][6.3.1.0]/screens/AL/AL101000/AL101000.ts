import {
	PXScreen, createSingle, graphInfo, PXView, PXFieldState, PXFieldOptions, createCollection, columnConfig, linkCommand, PXActionState, gridConfig, GridPreset
} from "client-controls";

@graphInfo({ graphType: "AA.Objects.Labels.ALSetupMaint", primaryView: "Setup" })
export class AL101000 extends PXScreen {

	Setup = createSingle(Setup);

	Renderers = createCollection(Renderers);

	ValidateLabelary: PXActionState;
	ValidatePrintNode: PXActionState;
	ValidateLabelZoom: PXActionState;
	ValidateMongoDb: PXActionState;
}

export class Setup extends PXView {

	@linkCommand("ValidateLabelary")
	LabelaryAPI: PXFieldState<PXFieldOptions.CommitChanges>;
	LabelaryAPIKey: PXFieldState<PXFieldOptions.CommitChanges>;
	
	@linkCommand("ValidatePrintNode")
	PrintNodeAPI: PXFieldState<PXFieldOptions.CommitChanges>;
	PrintNodeAPIKey: PXFieldState<PXFieldOptions.CommitChanges>;

	@linkCommand("ValidateLabelZoom")
	LabelZoomAPI: PXFieldState<PXFieldOptions.CommitChanges>;
	LabelZoomAPIKey: PXFieldState<PXFieldOptions.CommitChanges>;
	LabelZoomCategoryID: PXFieldState<PXFieldOptions.CommitChanges>;
	LabelZoomImageSubstitutionID: PXFieldState<PXFieldOptions.CommitChanges>;

	@linkCommand("ValidateMongoDb")
	MongoURL: PXFieldState<PXFieldOptions.CommitChanges>;
	MongoOptions: PXFieldState<PXFieldOptions.CommitChanges>;

	ZplGraphicCreator: PXFieldState<PXFieldOptions.CommitChanges>;
	DefaultLanguage: PXFieldState<PXFieldOptions.CommitChanges>;
	DefaultFormatID: PXFieldState<PXFieldOptions.CommitChanges>;
	DefaultMarginID: PXFieldState<PXFieldOptions.CommitChanges>;
	DefaultCategoryID: PXFieldState<PXFieldOptions.CommitChanges>;

	EnableIntegration: PXFieldState<PXFieldOptions.CommitChanges>;

	EnablePrinterOverride: PXFieldState<PXFieldOptions.CommitChanges>;

	EnableCopiesOverride: PXFieldState<PXFieldOptions.CommitChanges>;

	NbDaysToKeep: PXFieldState<PXFieldOptions.CommitChanges>;
	DevMode: PXFieldState<PXFieldOptions.CommitChanges>;
	RecordImportMode: PXFieldState<PXFieldOptions.CommitChanges>;
}

@gridConfig({
	preset: GridPreset.Details
})
export class Renderers extends PXView {

	Active: PXFieldState;
	FromContent: PXFieldState<PXFieldOptions.CommitChanges>;

	//@linkCommand("ViewAssignmentMap")
	ToContent: PXFieldState<PXFieldOptions.CommitChanges>;

	RenderingPrinterID: PXFieldState;

}
