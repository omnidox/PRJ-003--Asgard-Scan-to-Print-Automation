import {
    PXView,
    PXFieldState,
    PXFieldOptions,
    createCollection,

    viewInfo,
    gridConfig,
    GridPreset,
} from "client-controls";
import { SOOrder, SOLine } from "../../../../../../screens/SO/SO301000/SO301000";

export interface SO301000_TrueCommerce extends SOOrder { }
export class SO301000_TrueCommerce extends SOOrder {
    UsrTCPONumber: PXFieldState;
    UsrTCStoreNumber: PXFieldState;
    UsrTCDCCode: PXFieldState;
    UsrTCCustomField1: PXFieldState;
    UsrTCCustomField2: PXFieldState;
    UsrTCCustomField3: PXFieldState;
    UsrTCCustomField4: PXFieldState;
    UsrTCCustomField5: PXFieldState;
    UsrTCCustomField6: PXFieldState;
    UsrTCCustomField7: PXFieldState;
    UsrTCCustomField8: PXFieldState;
    UsrTCCustomField9: PXFieldState;
    UsrTCCustomField10: PXFieldState;
    UsrTCASNType: PXFieldState;
    UsrTCRevision: PXFieldState;
    UsrTCLabelTradingPartner: PXFieldState<PXFieldOptions.CommitChanges>;
    UsrTCLabelTemplate: PXFieldState<PXFieldOptions.CommitChanges>;
    UsrTCPrintDestination: PXFieldState<PXFieldOptions.CommitChanges>;
    UsrTCEDIAcknowledgmentReady: PXFieldState;

    UsrTCShipFromName: PXFieldState;
    UsrTCShipFromAddress1: PXFieldState;
    UsrTCShipFromAddress2: PXFieldState;
    UsrTCShipFromCity: PXFieldState;
    UsrTCShipFromCountry: PXFieldState<PXFieldOptions.CommitChanges>;
    UsrTCShipFromState: PXFieldState<PXFieldOptions.CommitChanges>;
    UsrTCShipFromPostCode: PXFieldState;
    UsrTCShipFromPhoneNo:PXFieldState;

    UsrTCPOACKSentTime: PXFieldState<PXFieldOptions.Readonly>;
    UsrTCPOACKRevisionNumber: PXFieldState<PXFieldOptions.Readonly>;
    UsrTCSOInvSentTime: PXFieldState<PXFieldOptions.Readonly>;
    UsrTCWSOSentTime: PXFieldState<PXFieldOptions.Readonly>;
    UsrTCWSORevisionNumber: PXFieldState<PXFieldOptions.Readonly>;

}

export interface SO301000_SOLineExtension extends SOLine { }
export class SO301000_SOLineExtension extends SOLine {
    UsrTCPOLineNumber: PXFieldState;
    UsrTCCustomField1: PXFieldState;
    UsrTCCustomField2: PXFieldState;
    UsrTCCustomField3: PXFieldState;
    UsrTCCustomField4: PXFieldState;
    UsrTCCustomField5: PXFieldState;
    UsrTCCustomField6: PXFieldState;
    UsrTCCustomField7: PXFieldState;
    UsrTCCustomField8: PXFieldState;
    UsrTCCustomField9: PXFieldState;
    UsrTCCustomField10: PXFieldState;
}

