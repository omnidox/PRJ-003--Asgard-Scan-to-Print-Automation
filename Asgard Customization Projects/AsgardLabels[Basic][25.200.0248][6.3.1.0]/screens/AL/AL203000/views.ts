import { PXView, PXFieldState, gridConfig, treeConfig, fieldConfig, controlConfig, actionConfig, headerDescription, ICurrencyInfo, disabled, PXFieldOptions, linkCommand, columnConfig, GridColumnShowHideMode, GridColumnType, PXActionState, TextAlign, GridPreset, GridFilterBarVisibility, GridFastFilterVisibility, ISelectorControlConfig, ControlParameter } from "client-controls";
import { AL203000 } from "./AL203000";

export class ALModelPrinterType {
	PrintDensity : PXFieldState;	
	SizeUnit : PXFieldState;	
	Description : PXFieldState;	
	Name : PXFieldState;	
	Width : PXFieldState;	
	Height : PXFieldState;	
}
export class ALPrinterFileTransferType {
	Size : PXFieldState;	
	Description : PXFieldState;	
}

// Views

export class ALPrinter extends PXView  {

	Name : PXFieldState;
	Active : PXFieldState<PXFieldOptions.CommitChanges>;
	IsSystem : PXFieldState<PXFieldOptions.CommitChanges>;
	AllowExport : PXFieldState<PXFieldOptions.CommitChanges>;
	IsRendering : PXFieldState;
	AllowOtherSize : PXFieldState<PXFieldOptions.CommitChanges>;
	SupportsLongFiles : PXFieldState<PXFieldOptions.CommitChanges>;
	PushFonts : PXFieldState<PXFieldOptions.CommitChanges>;
	Description : PXFieldState;
	PrinterType : PXFieldState<PXFieldOptions.CommitChanges>;
	@controlConfig({displayMode:"text"})
	DeviceHubID : PXFieldState<PXFieldOptions.CommitChanges>;
	@controlConfig({allowEdit:true,displayMode:"text"})
	AcuPrinterID : PXFieldState<PXFieldOptions.CommitChanges>;
	@controlConfig({allowEdit:true})
	FormatID : PXFieldState<PXFieldOptions.CommitChanges>;
	@controlConfig({allowEdit:true})
	MarginID : PXFieldState<PXFieldOptions.CommitChanges>;
	PrintNodeAPIKey : PXFieldState<PXFieldOptions.CommitChanges>;
	PrintNodeComputerID : PXFieldState<PXFieldOptions.CommitChanges>;
	ComputerState : PXFieldState;
	ComputerStateIcon : PXFieldState<PXFieldOptions.Disabled>;
	PrintNodeComputerLink : PXFieldState<PXFieldOptions.Disabled>;
	PrintNodePrinterID : PXFieldState<PXFieldOptions.CommitChanges>;
	PrinterState : PXFieldState;
	PrinterStateIcon : PXFieldState<PXFieldOptions.Disabled>;
	PrintNodePrinterLink : PXFieldState<PXFieldOptions.Disabled>;
	Encoding : PXFieldState<PXFieldOptions.CommitChanges>;
	ContentType : PXFieldState<PXFieldOptions.CommitChanges>;
	@controlConfig({allowEdit:true})
	PrintStationID : PXFieldState<PXFieldOptions.CommitChanges>;
	Drive : PXFieldState<PXFieldOptions.CommitChanges>;
	IsEpson : PXFieldState<PXFieldOptions.CommitChanges>;
	MediaShape : PXFieldState<PXFieldOptions.CommitChanges>;
	MediaSource : PXFieldState<PXFieldOptions.CommitChanges>;
	MediaForm : PXFieldState<PXFieldOptions.CommitChanges>;
	MediaType : PXFieldState<PXFieldOptions.CommitChanges>;
	EdgeDetection : PXFieldState<PXFieldOptions.CommitChanges>;
	PrintMode : PXFieldState<PXFieldOptions.CommitChanges>;
	@controlConfig({displayMode:"id"})
	ScreenID : PXFieldState<PXFieldOptions.CommitChanges>;
	GraphType : PXFieldState<PXFieldOptions.CommitChanges>;
	@fieldConfig({
  controlType: "qp-tree-selector",
  controlConfig: {
    treeConfig: {
      idField: [
        "Key"
      ],
      textField: "Name",
      toolTipField: "Path",
      iconField: "Icon",
      dynamic: true,
      hideRootNode: true,
      openedLayers: 0,
      syncPosition: true,
      modifiable: false,
      mode: "single",
      topBarItems: {}
    },
    allowEditValue: true
  }
})
	FieldName : PXFieldState;
}

@treeConfig({
  idField: [
    "Key"
  ],
  textField: "Name",
  toolTipField: "Path",
  iconField: "Icon",
  dynamic: true,
  hideRootNode: true,
  openedLayers: 0,
  syncPosition: true,
  modifiable: false,
  mode: "single",
  topBarItems: {}
})
export class CacheEntityItem extends PXView  {

	Key : PXFieldState;
	Icon : PXFieldState;
	Name : PXFieldState;
	Path : PXFieldState;
}

export class ALPrinter2 extends PXView  {

	ShowFileTransfers : PXFieldState;
	ShowChildren : PXFieldState;
	ShowPrintJobs : PXFieldState;
	ShowCapabilities : PXFieldState;
	Capabilities : PXFieldState;
}

@gridConfig({
	showFastFilter: GridFastFilterVisibility.False,
	preset: GridPreset.ReadOnly
})
export class ALModelPrinter extends PXView  {

	@columnConfig({width: 60})	Active : PXFieldState;
	@linkCommand("ViewUser")
	@columnConfig({width: 100, editorConfig : {}})	UserID : PXFieldState;
	@linkCommand("ViewPrintStation")
	@columnConfig({width: 200, editorConfig : {}})	PrintStationID : PXFieldState;
	@linkCommand("ViewOwner")
	@columnConfig({width: 150, editorConfig : {displayMode:"text"}})	OwnerID : PXFieldState;
	@linkCommand("ViewWorkgroup")
	@columnConfig({width: 100, editorConfig : {displayMode:"id"}})	WorkgroupID : PXFieldState;
	@linkCommand("ViewLabel")
	@columnConfig({width: 200, editorConfig : {}})	LabelID : PXFieldState;
	@columnConfig({width: 120})	ALFormat : ALModelPrinterType;
	@columnConfig({width: 200})	ALModel : ALModelPrinterType;
	@columnConfig({width: 100})	CreatedByID : PXFieldState;
	@columnConfig({width: 100})	CreatedDateTime : PXFieldState;
	@columnConfig({width: 120})	LastModifiedByID : PXFieldState;
	@columnConfig({width: 140})	LastModifiedDateTime : PXFieldState;
}

@gridConfig({
	showFastFilter: GridFastFilterVisibility.False,
	preset: GridPreset.ReadOnly
})
export class ALPrintLog extends PXView  {

	@linkCommand("ViewLog")
	@columnConfig({textAlign: TextAlign.Right})	RecordID : PXFieldState;
	@columnConfig({width: 140, format: "g"})	CreatedDateTime : PXFieldState;
	@linkCommand("ViewBAccount")
	@columnConfig({width: 150})	BAccountID : PXFieldState;
	@linkCommand("ViewModel")
	@columnConfig({width: 120})	ModelID : PXFieldState;
	@columnConfig({width: 150})	LabelFilename : PXFieldState;
	@columnConfig({width: 120})	LabelKey : PXFieldState;
	@columnConfig({width: 96})	ScreenID : PXFieldState;
	@linkCommand("ViewModelFormat")
	@columnConfig({width: 150})	ModelFormatID : PXFieldState;
	@columnConfig({width: 100})	UserID : PXFieldState;
	@columnConfig({width: 100})	OwnerID : PXFieldState;
	ContentType : PXFieldState;
	@columnConfig({textAlign: TextAlign.Right})	NbCopies : PXFieldState;
}

@gridConfig({
	showFastFilter: GridFastFilterVisibility.False,
	preset: GridPreset.ReadOnly
})
export class ALPrintJob extends PXView  {

	@linkCommand("ViewJob")
	@columnConfig({textAlign: TextAlign.Right})	RecordID : PXFieldState;
	@columnConfig({width: 100, textAlign: TextAlign.Right})	PrintJobID : PXFieldState;
	@columnConfig({width: 100})	State : PXFieldState;
	@columnConfig({width: 140, format: "g"})	StateDate : PXFieldState;
	Title : PXFieldState;
	Source : PXFieldState;
	@columnConfig({width: 140, format: "g"})	ReceivedAt : PXFieldState;
	@columnConfig({width: 140, format: "g"})	ExpiresAt : PXFieldState;
	@columnConfig({width: 140, format: "g"})	SentToClientAt : PXFieldState;
	@columnConfig({width: 140, format: "g"})	InProgressAt : PXFieldState;
	@columnConfig({width: 140, format: "g"})	DoneAt : PXFieldState;
	@columnConfig({width: 140, format: "g"})	ExpiredAt : PXFieldState;
	@linkCommand("ViewLog")
	@columnConfig({textAlign: TextAlign.Right})	PrintLogID : PXFieldState;
}

@gridConfig({
	syncPosition: true,
	allowInsert: false,
	showFastFilter: GridFastFilterVisibility.False,
	preset: GridPreset.Details,
	topBarItems: {
	LoadFiles: {index: 0, config: {commandName: "LoadFiles", text: "Load Files"}},
	SendToPrinter: {index: 1, config: {commandName: "SendToPrinter", text: "Send To Printer"}},
	DeleteFromPrinter: {index: 2, config: {commandName: "DeleteFromPrinter", text: "Delete From Printer"}},
	AssignLetterToFont: {index: 3, config: {commandName: "AssignLetterToFont", text: "Assign Letter To Font"}},
	PrintDirectoryForDrive: {index: 4, config: {commandName: "PrintDirectoryForDrive", text: "Print Drive Dir."}},
	PrintDirectoryForExtension: {index: 5, config: {commandName: "PrintDirectoryForExtension", text: "Print Ext. Dir."}},
}
})
export class ALPrinterFileTransfer extends PXView  {

	LoadFiles : PXActionState;
	SendToPrinter : PXActionState;
	DeleteFromPrinter : PXActionState;
	AssignLetterToFont : PXActionState;
	PrintDirectoryForDrive : PXActionState;
	PrintDirectoryForExtension : PXActionState;
	@linkCommand("ViewPrinterFile")
	@columnConfig({width: 180})	PrinterFileID : PXFieldState;
	@columnConfig({width: 80})	ALPrinterFile : ALPrinterFileTransferType;
	@columnConfig({width: 100})	FontCode : PXFieldState;
	@columnConfig({width: 180})	ObjectName : PXFieldState;
	@columnConfig({width: 180})	SentOn : PXFieldState;
	@columnConfig({width: 180})	SentAs : PXFieldState;
	@columnConfig({width: 150})	LastModifiedByID : PXFieldState;
	@columnConfig({width: 150})	LastModifiedDateTime : PXFieldState;
}

export class ALChangeIDParam extends PXView  {

	Name : PXFieldState;
}

export class ALPrinterCommandFilter extends PXView  {

	Command : PXFieldState<PXFieldOptions.CommitChanges>;
	Content : PXFieldState<PXFieldOptions.CommitChanges>;
}