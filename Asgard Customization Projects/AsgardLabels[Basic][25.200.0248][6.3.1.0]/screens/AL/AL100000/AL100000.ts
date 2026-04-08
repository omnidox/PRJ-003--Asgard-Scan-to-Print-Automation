import {
	PXScreen, createSingle, graphInfo, PXView, PXFieldState, PXFieldOptions, createCollection, columnConfig, linkCommand, PXActionState, gridConfig, GridPreset
} from "client-controls";

@graphInfo({ graphType: "AA.Objects.Labels.ALAboutMaint", primaryView: "ALAbout" })
export class AL100000 extends PXScreen {

	ALAbout = createSingle(ALAbout);
}

export class ALAbout extends PXView {

	AcumaticaVersion: PXFieldState;
	BasicVersion: PXFieldState;
	WikiVersion: PXFieldState;
	Integrations: PXFieldState;
	SupportEmail: PXFieldState<PXFieldOptions.CommitChanges>;
	SupportLink: PXFieldState<PXFieldOptions.CommitChanges>;
	SupportNum: PXFieldState;
	TermsLink: PXFieldState<PXFieldOptions.CommitChanges>;
	NbPrinters: PXFieldState;
	NbPrintStations: PXFieldState;
	NbModels: PXFieldState;
}