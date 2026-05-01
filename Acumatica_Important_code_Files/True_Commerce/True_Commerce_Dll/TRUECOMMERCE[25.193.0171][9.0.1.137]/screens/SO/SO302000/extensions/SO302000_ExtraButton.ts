import {
	PXView,
	PXActionState,
	PXFieldState,
	PXFieldOptions,

	createSingle,
	createCollection,

	viewInfo,
	gridConfig,
	columnConfig,
	GridPreset,
} from "client-controls";
import { Packages, SO302000 } from "../../../../../../screens/SO/SO302000/SO302000";
export interface SO302000_ExtraButton extends Packages { }
export class SO302000_ExtraButton {
	TCOpenAutoPackConfirm: PXActionState;
	TCOpenAssginPallet: PXActionState;
	TCOpenDeletePallet: PXActionState;
	TCGenerateUCC128: PXActionState;
	TCClearUCC128: PXActionState;
	TCPrintSelectedLabels: PXActionState;
	TCPrintAllLabels: PXActionState;
	TCAutoPack: PXActionState;

	UsrTCSelected: PXFieldState<PXFieldOptions.CommitChanges>;
	UsrTCPalletID: PXFieldState<PXFieldOptions.CommitChanges>;
	UsrTCUCC128: PXFieldState;
	UsrTCUCC128P: PXFieldState;
	UsrTCLabelPrintStatus: PXFieldState;
	UsrTCLabelPrintDate: PXFieldState;
	TCAssignPallet:PXActionState;
	TCDeletePallet:PXActionState;
}

export interface SO302000_CustomView extends SO302000 { }
export class SO302000_CustomView {
	TCAssignpalletfilterView = createSingle(TCAssignPalletFilter);
	TCDeletepalletfilterView = createSingle(TCDeletePalletFilter);
}
export class TCAutoPackFilter extends PXView {

}

export class TCAssignPalletFilter extends PXView {

	palletID: PXFieldState;
	PackageStart: PXFieldState;
	PackageEnd: PXFieldState;
}

export class TCDeletePalletFilter extends PXView {

	palletID: PXFieldState;
}