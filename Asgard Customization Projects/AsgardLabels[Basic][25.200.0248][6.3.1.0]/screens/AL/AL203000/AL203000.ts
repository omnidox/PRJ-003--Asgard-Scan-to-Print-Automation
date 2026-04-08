import { Messages as SysMessages } from "client-controls/services/messages";
import { createCollection, createSingle, PXScreen, graphInfo, PXActionState, viewInfo, handleEvent, CustomEventType, actionConfig, RowSelectedHandlerArgs, PXViewCollection, PXPageLoadBehavior, ControlParameter } from "client-controls";
import { ALPrinter, CacheEntityItem, ALPrinter2, ALModelPrinter, ALPrintLog, ALPrintJob, ALPrinterFileTransfer, ALChangeIDParam, ALPrinterCommandFilter } from "./views";

@graphInfo({graphType: "AA.Objects.Labels.ALPrinterMaint", primaryView: "Printer", pageLoadBehavior: PXPageLoadBehavior.SearchSavedKeys})
export class AL203000 extends PXScreen {

   	@viewInfo({containerName: "Asgard Printers"})
	Printer = createSingle(ALPrinter);
   	@viewInfo({containerName: "Asgard Printers"})
	EntityItems = createCollection(CacheEntityItem);
   	@viewInfo({containerName: "Hidden Form needed for VisibleExp of TabItems"})
	CurrentPrinter = createSingle(ALPrinter2);
	@viewInfo({ containerName: "Used By" })
	UsedBy = createCollection(ALModelPrinter);
   	@viewInfo({containerName: "Print Logs"})
	PrintLogs = createCollection(ALPrintLog);
   	@viewInfo({containerName: "Print Jobs"})
	PrintJobs = createCollection(ALPrintJob);
   	@viewInfo({containerName: "File Transfers"})
	FileTransfers = createCollection(ALPrinterFileTransfer);
   	@viewInfo({containerName: "Specify New ID"})
	ChangeIDDialog = createSingle(ALChangeIDParam);
   	@viewInfo({containerName: "Send Command"})
	PrinterCommandFilter = createSingle(ALPrinterCommandFilter);
}