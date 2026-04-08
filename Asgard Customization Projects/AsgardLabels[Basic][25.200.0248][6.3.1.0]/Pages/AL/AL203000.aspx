<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true"
    ValidateRequest="false" CodeFile="AL203000.aspx.cs" Inherits="Pages_AL203000" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>
<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" Width="100%" runat="server" Visible="True" PrimaryView="Printer" TypeName="AA.Objects.Labels.ALPrinterMaint" PageLoadBehavior="SearchSavedKeys">
        <CallbackCommands>
            <px:PXDSCallbackCommand Name="Insert" PostData="Self" />
            <px:PXDSCallbackCommand CommitChanges="True" Name="Save" />
            <px:PXDSCallbackCommand Name="First" PostData="Self" StartNewGroup="true" />
            <px:PXDSCallbackCommand Name="Last" PostData="Self" />
            <px:PXDSCallbackCommand Name="RefreshState" Visible="True" />
        </CallbackCommands>
        <DataTrees>
            <px:PXTreeDataMember TreeView="EntityItems" TreeKeys="Key" />
        </DataTrees>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" Style="z-index: 100" Width="100%"
        Caption="Asgard Printers" DataMember="Printer">
        <Template>
            <px:PXLayoutRule runat="server" LabelsWidth="SM" ControlSize="L" StartColumn="True" />
            <px:PXSelector ID="edName" runat="server" DataField="Name" DataSourceID="ds" />
            <px:PXLayoutRule runat="server" Merge="true" />
            <px:PXCheckBox ID="chkActive" runat="server" DataField="Active" CommitChanges="true" />
            <px:PXCheckBox ID="chkIsSystem" runat="server" DataField="IsSystem" CommitChanges="true" />
            <px:PXCheckBox ID="chkAllowExport" runat="server" Checked="True" DataField="AllowExport" CommitChanges="True" />
            <px:PXCheckBox ID="chkIsRendering" runat="server" Checked="True" DataField="IsRendering" />
            <px:PXLayoutRule runat="server" Merge="true" />
            <px:PXCheckBox ID="chkAllowOtherSize" runat="server" Checked="True" DataField="AllowOtherSize" CommitChanges="True" />
            <px:PXCheckBox ID="chkSupportsLongFiles" runat="server" Checked="True" DataField="SupportsLongFiles" CommitChanges="True" />
            <px:PXCheckBox ID="chkPushFonts" runat="server" Checked="True" DataField="PushFonts" CommitChanges="True" />
            <px:PXLayoutRule runat="server" />
            <px:PXTextEdit ID="edDescription" runat="server" DataField="Description" />
            <px:PXDropDown ID="edPrinterType" runat="server" DataField="PrinterType" CommitChanges="true" />
            <px:PXSelector ID="edDeviceHubID" runat="server" DataField="DeviceHubID" DisplayMode="Text" FilterByAllFields="true" CommitChanges="True" />
            <px:PXSelector ID="edAcuPrinterID" runat="server" DataField="AcuPrinterID" DisplayMode="Text" FilterByAllFields="true" CommitChanges="True" AllowEdit="true" />
            <px:PXSelector ID="edFormatID" runat="server" DataField="FormatID" CommitChanges="True" AllowEdit="true" />
            <px:PXSelector ID="edMarginID" runat="server" DataField="MarginID" CommitChanges="True" AllowEdit="true" />
            <px:PXLayoutRule runat="server" LabelsWidth="SM" ControlSize="L" StartColumn="True" />
            <px:PXTextEdit runat="server" ID="edPrintNodeAPIKey" DataField="PrintNodeAPIKey" CommitChanges="true" />
            <px:PXLayoutRule runat="server" Merge="true" />
            <px:PXSelector ID="edPrintNodeComputerID" runat="server" DataField="PrintNodeComputerID" DisplayMode="Text" FilterByAllFields="true" CommitChanges="True" Width="200px" />
            <px:PXTextEdit runat="server" ID="edComputerState" DataField="ComputerState" SuppressLabel="True" Width="90px" TextAlign="Left" />
            <px:PXImageView runat="server" ID="PXImageView1" DataField="ComputerStateIcon" Enabled="false" AlreadyLocalized="False" CallbackUpdatable="True" />
            <px:PXLayoutRule runat="server" />
            <px:PXLinkEdit runat="server" ID="edPrintNodeComputerLink" DataField="PrintNodeComputerLink" Enabled="false" AlreadyLocalized="True" CallbackUpdatable="True" SuppressLabel="True" Width="380px"/>
            <px:PXLayoutRule runat="server" Merge="true" />
            <px:PXSelector ID="edPrintNodePrinterID" runat="server" DataField="PrintNodePrinterID" DisplayMode="Text" FilterByAllFields="true" CommitChanges="True" Width="200px" />
            <px:PXTextEdit runat="server" ID="edPrinterState" DataField="PrinterState" SuppressLabel="True" Width="90px" TextAlign="Left" />
            <px:PXImageView runat="server" ID="edPrinterStateIcon" DataField="PrinterStateIcon" Enabled="false" />
            <px:PXLayoutRule runat="server" />
            <px:PXLinkEdit runat="server" ID="edPrintNodePrinterLink" DataField="PrintNodePrinterLink" Enabled="false" AlreadyLocalized="True" CallbackUpdatable="True" SuppressLabel="True" Width="380px"/>
            <px:PXLayoutRule runat="server" />
            <px:PXDropDown ID="edEncoding" runat="server" DataField="Encoding" IsClientControl="True" CommitChanges="True" />
            <px:PXDropDown ID="edContentType" runat="server" DataField="ContentType" CommitChanges="true" />
            <px:PXSelector ID="edPrintStationdID" runat="server" DataField="PrintStationID" CommitChanges="True" AllowEdit="true" />
            <px:PXLayoutRule runat="server" Merge="true" />
            <px:PXDropDown ID="edDrive" runat="server" DataField="Drive" CommitChanges="true" Width="200px" />
            <px:PXCheckBox runat="server" ID="chkIsEpson" DataField="IsEpson" CommitChanges="true" />
            <px:PXLayoutRule runat="server" LabelsWidth="SM" ControlSize="M" StartColumn="True" />
            <px:PXDropDown ID="edMediaShape" runat="server" DataField="MediaShape" CommitChanges="true" />
            <px:PXDropDown ID="edMediaSource" runat="server" DataField="MediaSource" CommitChanges="true" />
            <px:PXDropDown ID="edMediaForm" runat="server" DataField="MediaForm" CommitChanges="true" />
            <px:PXDropDown ID="edMediaType" runat="server" DataField="MediaType" CommitChanges="true" />
            <px:PXDropDown ID="edEdgeDetection" runat="server" DataField="EdgeDetection" CommitChanges="true" />
            <px:PXDropDown ID="edPrintMode" runat="server" DataField="PrintMode" CommitChanges="true" />
            <px:PXSelector runat="server" ID="edScreenID" DataField="ScreenID" FilterByAllFields="true" DisplayMode="Text" CommitChanges="True" />
            <px:PXTextEdit runat="server" ID="edGraphType" CommitChanges="true" DataField="GraphType" />
            <px:PXTreeSelector runat="server" ID="edFieldName" TreeDataSourceID="ds" TreeDataMember="EntityItems" AutoRefresh="true" InitialExpandLevel="0" MinDropWidth="468" MaxDropWidth="600" PopulateOnDemand="True" ShowRootNode="False" AllowEditValue="true" AppendSelectedValue="False" DataField="FieldName">
                <DataBindings>
                    <px:PXTreeItemBinding DataMember="EntityItems" TextField="Name" ValueField="Path" ToolTipField="Path" ImageUrlField="Icon" />
                </DataBindings>
            </px:PXTreeSelector>
            <px:PXUploadDialog ID="uploadFileDialog" runat="server" AutoSaveFile="true" Caption="Upload XML Zip" Key="Printer"
                Height="110px" SessionKey="XmlUploadAllEntities" AllowedFileTypes=".zip;" />
            <px:PXFormView ID="VisibilityForm" runat="server" DataMember="CurrentPrinter" DataSourceID="ds" Caption="Hidden Form needed for VisibleExp of TabItems"
                Visible="False" TabIndex="300">
                <Template>
                    <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="M" ControlSize="XM" />
                    <px:PXCheckBox ID="chkShowFileTransfers" runat="server" DataField="ShowFileTransfers" AlreadyLocalized="False" IsClientControl="True" />
                    <px:PXCheckBox ID="chkShowChildren" runat="server" DataField="ShowChildren" AlreadyLocalized="False" IsClientControl="True" />
                    <px:PXCheckBox ID="chkShowPrintJobs" runat="server" DataField="ShowPrintJobs" AlreadyLocalized="False" IsClientControl="True" />
                    <px:PXCheckBox ID="chkShowCapabilities" runat="server" DataField="ShowCapabilities" AlreadyLocalized="False" IsClientControl="True" />
                </Template>
            </px:PXFormView>
        </Template>
    </px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXTab ID="tab" runat="server" Width="100%" Height="365px" DataMember="UsedBy" DataSourceID="ds">
        <Activity HighlightColor="" SelectedColor="" Width="" Height=""></Activity>
        <Items>
            <px:PXTabItem Text="Used By">
                <Template>
                    <px:PXGrid ID="labelPrinterGrid" runat="server" DataSourceID="ds" Height="150px" Style="border: 0px;"
                        Width="100%" ActionsPosition="Top" SkinID="Details" MatrixMode="True" SyncPosition="True" KeepPosition="True" TabIndex="500">
                        <EmptyMsg AnonFilteredAddMessage="No records found.
Try to change filter to see records here."
                            AnonFilteredMessage="No records found.
Try to change filter to see records here."
                            ComboAddMessage="No records found.
Try to change filter or modify parameters above to see records here."
                            FilteredAddMessage="No records found.
Try to change filter to see records here."
                            FilteredMessage="No records found.
Try to change filter to see records here."
                            NamedComboAddMessage="No records found as '{0}'.
Try to change filter or modify parameters above to see records here."
                            NamedComboMessage="No records found as '{0}'.
Try to change filter or modify parameters above to see records here."
                            NamedFilteredAddMessage="No records found as '{0}'.
Try to change filter to see records here."
                            NamedFilteredMessage="No records found as '{0}'.
Try to change filter to see records here." />
                        <Levels>
                            <px:PXGridLevel DataMember="UsedBy" DataKeyNames="LabelID,LineNbr">
                                <RowTemplate>
                                    <px:PXCheckBox DataField="Active" ID="chkActive2" runat="server" AlreadyLocalized="False" IsClientControl="True" />
                                    <px:PXSelector DataField="UserID" ID="edUserID" runat="server" MatrixMode="True" />
                                    <px:PXSelector DataField="PrintStationID" ID="edPrintStationID" runat="server" />
                                    <px:PXSelector DataField="OwnerID" ID="edOwnerID" runat="server" />
                                    <px:PXSelector DataField="WorkgroupID" ID="edWorkgroupID" runat="server" />
                                    <px:PXSelector DataField="LabelID" ID="edLabelID" runat="server" />
                                    <px:PXDropDown DataField="ALFormat__PrintDensity" ID="edPrintDensity" runat="server" MatrixMode="True"/>
                                    <px:PXDropDown DataField="ALFormat__SizeUnit" ID="edSizeUnit" runat="server" MatrixMode="True"/>
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="Active" TextAlign="Center" Type="CheckBox" Width="60px" />
                                    <px:PXGridColumn DataField="LabelID" Width="200px" LinkCommand="ViewLabel" MatrixMode="True" />
                                    <px:PXGridColumn DataField="ALModel__Description" Width="200px" />
                                    <px:PXGridColumn DataField="UserID" Width="100px" LinkCommand="ViewUser" MatrixMode="True" />
                                    <px:PXGridColumn DataField="OwnerID" Width="150px" LinkCommand="ViewOwner" MatrixMode="True" />
                                    <px:PXGridColumn DataField="WorkgroupID" Width="100px" LinkCommand="ViewWorkgroup" MatrixMode="True" />
                                    <px:PXGridColumn DataField="PrintStationID" Width="200px" LinkCommand="ViewPrintStation" MatrixMode="True" />
                                    <px:PXGridColumn DataField="ALFormat__Name" Width="120px" LinkCommand="ViewFormat" />
                                    <px:PXGridColumn DataField="ALFormat__Description" Width="120px" />
                                    <px:PXGridColumn DataField="ALFormat__PrintDensity" Width="120px" Type="DropDownList" MatrixMode="True"/>
                                    <px:PXGridColumn DataField="ALFormat__Width" Width="90px" />
                                    <px:PXGridColumn DataField="ALFormat__Height" Width="90px" />
                                    <px:PXGridColumn DataField="ALFormat__SizeUnit" Width="90px" Type="DropDownList" MatrixMode="True"/>
                                    <px:PXGridColumn DataField="CreatedByID" Width="100px" />
                                    <px:PXGridColumn DataField="CreatedDateTime" Width="100px" />
                                    <px:PXGridColumn DataField="LastModifiedByID" Width="120px" />
                                    <px:PXGridColumn DataField="LastModifiedDateTime" Width="140px" />
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <AutoSize Enabled="True" MinHeight="150" />
                        <Mode AllowAddNew="False" AllowUpdate="False" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="Print Logs">
                <Template>
                    <px:PXGrid ID="printLogsGrid" runat="server" DataSourceID="ds" SkinID="Inquire" Width="100%">
                        <EmptyMsg AnonFilteredAddMessage="No records found.
Try to change filter to see records here."
                            AnonFilteredMessage="No records found.
Try to change filter to see records here."
                            ComboAddMessage="No records found.
Try to change filter or modify parameters above to see records here."
                            FilteredAddMessage="No records found.
Try to change filter to see records here."
                            FilteredMessage="No records found.
Try to change filter to see records here."
                            NamedComboAddMessage="No records found as '{0}'.
Try to change filter or modify parameters above to see records here."
                            NamedComboMessage="No records found as '{0}'.
Try to change filter or modify parameters above to see records here."
                            NamedFilteredAddMessage="No records found as '{0}'.
Try to change filter to see records here."
                            NamedFilteredMessage="No records found as '{0}'.
Try to change filter to see records here." />
                        <Levels>
                            <px:PXGridLevel DataKeyNames="RecordID" DataMember="PrintLogs">
                                <RowTemplate>
                                    <px:PXNumberEdit ID="edRecordID" runat="server" AlreadyLocalized="False" DataField="RecordID" IsClientControl="True" />
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="RecordID" TextAlign="Right" LinkCommand="ViewLog" />
                                    <px:PXGridColumn DataField="CreatedDateTime" Width="140px" DisplayFormat="g" />
                                    <px:PXGridColumn DataField="BAccountID" Width="150px" LinkCommand="ViewBAccount" />
                                    <px:PXGridColumn DataField="ModelID" Width="120px" LinkCommand="ViewModel" />
                                    <px:PXGridColumn DataField="LabelFilename" Width="150px" />
                                    <px:PXGridColumn DataField="LabelKey" Width="120px" />
                                    <px:PXGridColumn DataField="ScreenID" Width="96px" />
                                    <px:PXGridColumn DataField="ModelFormatID" Width="150px" LinkCommand="ViewModelFormat" />
                                    <px:PXGridColumn DataField="UserID" Width="100px" />
                                    <px:PXGridColumn DataField="OwnerID" Width="100px" />
                                    <px:PXGridColumn DataField="ContentType" Type="DropDownList" />
                                    <px:PXGridColumn DataField="NbCopies" TextAlign="Right" />
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <AutoSize Enabled="True" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="Print Jobs" BindingContext="VisibilityForm" VisibleExp="DataControls[&quot;chkShowPrintJobs&quot;].Value == true">
                <Template>
                    <px:PXGrid ID="printJobsGrid" runat="server" DataSourceID="ds" SkinID="Inquire" Width="100%">
                        <EmptyMsg AnonFilteredAddMessage="No records found.
Try to change filter to see records here."
                            AnonFilteredMessage="No records found.
Try to change filter to see records here."
                            ComboAddMessage="No records found.
Try to change filter or modify parameters above to see records here."
                            FilteredAddMessage="No records found.
Try to change filter to see records here."
                            FilteredMessage="No records found.
Try to change filter to see records here."
                            NamedComboAddMessage="No records found as '{0}'.
Try to change filter or modify parameters above to see records here."
                            NamedComboMessage="No records found as '{0}'.
Try to change filter or modify parameters above to see records here."
                            NamedFilteredAddMessage="No records found as '{0}'.
Try to change filter to see records here."
                            NamedFilteredMessage="No records found as '{0}'.
Try to change filter to see records here." />
                        <Levels>
                            <px:PXGridLevel DataKeyNames="RecordID" DataMember="PrintJobs">
                                <RowTemplate>
                                    <px:PXNumberEdit ID="edRecordID2" runat="server" AlreadyLocalized="False" DataField="RecordID" IsClientControl="True" />
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="RecordID" TextAlign="Right" LinkCommand="ViewJob" />
                                    <px:PXGridColumn DataField="PrintJobID" TextAlign="Right" Width="100px" />
                                    <px:PXGridColumn DataField="State" Width="100px" />
                                    <px:PXGridColumn DataField="StateDate" Width="140px" DisplayFormat="g" />
                                    <px:PXGridColumn DataField="Title" />
                                    <px:PXGridColumn DataField="Source" />
                                    <px:PXGridColumn DataField="ReceivedAt" Width="140px" DisplayFormat="g" />
                                    <px:PXGridColumn DataField="ExpiresAt" Width="140px" DisplayFormat="g" />
                                    <px:PXGridColumn DataField="SentToClientAt" Width="140px" DisplayFormat="g" />
                                    <px:PXGridColumn DataField="InProgressAt" Width="140px" DisplayFormat="g" />
                                    <px:PXGridColumn DataField="DoneAt" Width="140px" DisplayFormat="g" />
                                    <px:PXGridColumn DataField="ExpiredAt" Width="140px" DisplayFormat="g" />
                                    <px:PXGridColumn DataField="PrintLogID" TextAlign="Right" LinkCommand="ViewLog" />
<%--                                    <px:PXGridColumn DataField="UserID" Width="100px" DisplayMode="Text" />
                                    <px:PXGridColumn DataField="PrintStationID" Width="150px" LinkCommand="ViewPrintStation" DisplayMode="Text" />
                                    <px:PXGridColumn DataField="ContentType" Type="DropDownList" />--%>
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <AutoSize Enabled="True" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="File Transfers" BindingContext="VisibilityForm" VisibleExp="DataControls[&quot;chkShowFileTransfers&quot;].Value == true">
                <Template>
                    <px:PXGrid ID="printerFileGrid" runat="server" DataSourceID="ds" SkinID="Details" Width="100%" TabIndex="2700" Height="450px"
                        FilesIndicator="false" KeepPosition="true" SyncPosition="true" MatrixMode="true">
                        <EmptyMsg AnonFilteredAddMessage="No records found.
Try to change filter to see records here."
                            AnonFilteredMessage="No records found.
Try to change filter to see records here."
                            ComboAddMessage="No records found.
Try to change filter or modify parameters above to see records here."
                            FilteredAddMessage="No records found.
Try to change filter to see records here."
                            FilteredMessage="No records found.
Try to change filter to see records here."
                            NamedComboAddMessage="No records found as '{0}'.
Try to change filter or modify parameters above to see records here."
                            NamedComboMessage="No records found as '{0}'.
Try to change filter or modify parameters above to see records here."
                            NamedFilteredAddMessage="No records found as '{0}'.
Try to change filter to see records here."
                            NamedFilteredMessage="No records found as '{0}'.
Try to change filter to see records here." />
                        <Levels>
                            <px:PXGridLevel DataMember="FileTransfers">
                                <RowTemplate>
                                    <px:PXNumberEdit ID="edPrinterFileID" runat="server" AlreadyLocalized="False" DataField="PrinterFileID" IsClientControl="True" />
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="PrinterFileID" Width="180px" LinkCommand="ViewPrinterFile" />
                                    <px:PXGridColumn DataField="ALPrinterFile__Size" Width="80px" />
                                    <px:PXGridColumn DataField="FontCode" Width="100px" />
                                    <px:PXGridColumn DataField="ObjectName" Width="180px" />
                                    <px:PXGridColumn DataField="SentOn" Width="180px" />
                                    <px:PXGridColumn DataField="SentAs" Width="180px" />
                                    <px:PXGridColumn DataField="ALPrinterFile__Description" Width="300px" />
                                    <px:PXGridColumn DataField="LastModifiedByID" Width="150px" />
                                    <px:PXGridColumn DataField="LastModifiedDateTime" Width="150px" />
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <ActionBar>
                            <CustomItems>
                                <px:PXToolBarButton Text="Load Files">
                                    <AutoCallBack Command="LoadFiles" Target="ds"></AutoCallBack>
                                </px:PXToolBarButton>
                                <px:PXToolBarButton Text="Send To Printer">
                                    <AutoCallBack Command="SendToPrinter" Target="ds"></AutoCallBack>
                                </px:PXToolBarButton>
                                <px:PXToolBarButton Text="Delete From Printer">
                                    <AutoCallBack Command="DeleteFromPrinter" Target="ds"></AutoCallBack>
                                </px:PXToolBarButton>
                                <px:PXToolBarButton Text="Assign Letter To Font">
                                    <AutoCallBack Command="AssignLetterToFont" Target="ds"></AutoCallBack>
                                </px:PXToolBarButton>
                                <px:PXToolBarButton Text="Print Drive Dir.">
                                    <AutoCallBack Command="PrintDirectoryForDrive" Target="ds"></AutoCallBack>
                                </px:PXToolBarButton>
                                <px:PXToolBarButton Text="Print Ext. Dir.">
                                    <AutoCallBack Command="PrintDirectoryForExtension" Target="ds"></AutoCallBack>
                                </px:PXToolBarButton>
                            </CustomItems>
                        </ActionBar>
                        <Mode AllowAddNew="False" AllowUpdate="True" AllowDelete="True" />
                        <AutoSize Enabled="True" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="Capabilities" LoadOnDemand="true" BindingContext="VisibilityForm" VisibleExp="DataControls[&quot;chkShowCapabilities&quot;].Value == true">
                <Template>
                    <px:PXFormView ID="capabilitiesDataForm" runat="server" CaptionVisible="False" DataMember="CurrentPrinter" DataSourceID="ds" Height="95%" SkinID="Transparent" Width="100%">
                        <Template>
                            <px:PXTextEdit ID="capabilitiesDataBox" runat="server" AlreadyLocalized="False" DataField="Capabilities" DisableSpellcheck="True" Height="500" IsClientControl="True" SuppressLabel="True" TextMode="MultiLine" Width="100%" />
                        </Template>
                    </px:PXFormView>
                </Template>
            </px:PXTabItem>
            <%--<px:PXTabItem Text="Printing Pipeline" BindingContext="VisibilityForm" VisibleExp="DataControls[&quot;chkShowChildren&quot;].Value == true">
                <Template>
                    <px:PXGrid ID="pipelineGrid" runat="server" DataSourceID="ds" Height="150px" Style="border: 0px;"
                        Width="100%" ActionsPosition="Top" SkinID="Details" MatrixMode="True">
                        <Levels>
                            <px:PXGridLevel DataMember="PrinterChildren">
                                <RowTemplate>
                                    <px:PXSelector ID="edChildPrinterID" runat="server" DataField="ChildPrinterID" />
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="PrinterID" Width="200px" MatrixMode="False" CommitChanges="true" />
                                    <px:PXGridColumn DataField="Active" AllowNull="False" TextAlign="Center" Type="CheckBox" CommitChanges="true" />
                                    <px:PXGridColumn DataField="LineNbr" />
                                    <px:PXGridColumn DataField="SortOrder" />
                                    <px:PXGridColumn DataField="ChildPrinterID" Width="250px" MatrixMode="False" LinkCommand="ViewPrinter" AllowDragDrop="true" CommitChanges="true" />
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <AutoSize Enabled="True" MinHeight="150" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>--%>
        </Items>
        <AutoSize Container="Window" Enabled="True" MinHeight="180" />
    </px:PXTab>
    <!--#include file="~\Pages\AL\Includes\LabelsChangeIDDialog.inc"-->
    <px:PXSmartPanel ID="pnlSendCommand" runat="server" Caption="Send Command" CaptionVisible="true" LoadOnDemand="true" Key="PrinterCommandFilter"
        AutoCallBack-Enabled="true" AutoCallBack-Target="formSendCommand" AutoCallBack-Command="Refresh" CallBackMode-CommitChanges="True"
        CallBackMode-PostData="Page">
        <div style="padding: 5px">
            <px:PXFormView ID="formSendCommand" runat="server" DataSourceID="ds" CaptionVisible="False" DataMember="PrinterCommandFilter">
                <Activity Height="" HighlightColor="" SelectedColor="" Width="" />
                <ContentStyle BackColor="Transparent" BorderStyle="None" />
                <Template>
                    <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="S" ControlSize="XM" />
                    <px:PXDropDown ID="edCommand" runat="server" DataField="Command" CommitChanges="true" />
                    <px:PXTextEdit ID="edContent" runat="server" DataField="Content" CommitChanges="true" />
                </Template>
            </px:PXFormView>
        </div>
        <px:PXPanel ID="pnlSendCommandButton" runat="server" SkinID="Buttons">
            <px:PXButton ID="btnDoSendCommand" runat="server" DialogResult="OK" Text="OK" CommandSourceID="ds" />
        </px:PXPanel>
    </px:PXSmartPanel>
</asp:Content>
