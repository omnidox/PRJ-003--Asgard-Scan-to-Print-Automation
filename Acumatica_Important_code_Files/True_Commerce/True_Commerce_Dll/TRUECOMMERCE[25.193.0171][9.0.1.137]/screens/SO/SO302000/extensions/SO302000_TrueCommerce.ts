import {
    PXView,
    PXActionState,
    PXFieldState,
    PXFieldOptions,
    fieldConfig,

    createCollection,
    createSingle,

    viewInfo,
    gridConfig,
    columnConfig,
    GridPreset,
    GridAutoGrowMode,
    autoRefresh,
} from "client-controls";
import { SO302000, SOShipment, Transactions } from "../../../../../../screens/SO/SO302000/SO302000";
//import { SO302000, SOShipment, Transactions } from "../SO302000";
export interface SO302000_TrueCommerce extends SOShipment { }
export class SO302000_TrueCommerce {
    UsrTCPONumber: PXFieldState;
    UsrTCDCCode: PXFieldState;
    UsrTCStoreNumber: PXFieldState;
    UsrTCDepartmentNumber: PXFieldState;
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
    UsrTCLabelTradingPartner: PXFieldState<PXFieldOptions.CommitChanges>;
    UsrTCLabelTemplate: PXFieldState<PXFieldOptions.CommitChanges>;
    UsrTCPrintDestination: PXFieldState;

    UsrTCShipFromName: PXFieldState<PXFieldOptions.Readonly>;

    UsrTCShipFromAddress1: PXFieldState<PXFieldOptions.Readonly>;
    UsrTCShipFromAddress2: PXFieldState<PXFieldOptions.Readonly>;
    UsrTCShipFromCity: PXFieldState<PXFieldOptions.Readonly>;

    UsrTCShipFromCountry: PXFieldState<PXFieldOptions.Readonly>;

    UsrTCShipFromState: PXFieldState<PXFieldOptions.Readonly>;
    UsrTCShipFromPostCode: PXFieldState<PXFieldOptions.Readonly>;
    UsrTCShipFromPhoneNo: PXFieldState<PXFieldOptions.Readonly>;
    UsrTCASNSentTime: PXFieldState<PXFieldOptions.Readonly>;
    TCOpenAutoPackConfirm: PXActionState;
}
export interface SO302000_ShipLineExtension extends Transactions { }
export class SO302000_ShipLineExtension extends Transactions {
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