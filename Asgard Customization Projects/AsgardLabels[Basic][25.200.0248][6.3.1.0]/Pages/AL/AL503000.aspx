<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true"
    ValidateRequest="false" CodeFile="AL503000.aspx.cs" Inherits="Page_AL503000" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" TypeName="AA.Objects.Labels.ALPrintLogProcess" PrimaryView="Filter">
        <CallbackCommands>
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXSplitContainer runat="server" SplitterPosition="1100" ID="splitRender" Orientation="Vertical">
        <AutoSize Enabled="true" Container="Window" />
        <Template1>
            <px:PXFormView ID="form" runat="server" DataSourceID="ds" Style="z-index: 100" Width="100%" DataMember="Filter" TabIndex="2000">
                <Template>
                    <px:PXLayoutRule runat="server" LabelsWidth="S" StartColumn="True" />
                    <px:PXDropDown ID="edAction" runat="server" DataField="Action" IsClientControl="True" CommitChanges="True" Width="120" />
                    <px:PXDateTimeEdit ID="edStartDate" runat="server" DataField="StartDate" IsClientControl="True" CommitChanges="True" Width="120" />
                    <px:PXDateTimeEdit ID="edEndDate" runat="server" DataField="EndDate" IsClientControl="True" CommitChanges="True" Width="120" />
                    <px:PXDropDown ID="edContentType" runat="server" DataField="ContentType" IsClientControl="True" CommitChanges="True" Width="120" />
                    <px:PXSelector ID="edUserID" runat="server" DataField="UserID" CommitChanges="True" Width="120" />
                    <px:PXSelector ID="edOwnerID" runat="server" DataField="OwnerID" CommitChanges="True" Width="120"/>
                    <px:PXLayoutRule runat="server" LabelsWidth="SM" StartColumn="True" />
                    <px:PXSelector ID="edBAccountID" runat="server" DataField="BAccountID" CommitChanges="True" />
                    <px:PXSelector ID="edModelID" runat="server" DataField="ModelID" CommitChanges="True" />
                    <px:PXSelector ID="edScreenID" runat="server" DataField="ScreenID" CommitChanges="True" />
                    <px:PXSelector ID="edInventoryID" runat="server" DataField="InventoryID" CommitChanges="True" />
                    <px:PXTextEdit ID="edLotSerialNbr" runat="server" DataField="LotSerialNbr" IsClientControl="True" CommitChanges="True" />
                    <px:PXTextEdit ID="edLabelKey" runat="server" DataField="LabelKey" IsClientControl="True" CommitChanges="True" />
                    <px:PXLayoutRule runat="server" LabelsWidth="S" StartColumn="True" />
                    <px:PXSelector ID="edPrinterID" runat="server" DataField="PrinterID" CommitChanges="True" />
                    <px:PXSelector ID="edNewPrinterID" runat="server" DataField="NewPrinterID" CommitChanges="True" />
                    <px:PXSelector ID="edPrintStationID" runat="server" DataField="PrintStationID" CommitChanges="True" />
                    <px:PXSelector ID="edFormatID" runat="server" DataField="FormatID" CommitChanges="True" />
                    <px:PXTextEdit ID="edLabelFilename" runat="server" DataField="LabelFilename" IsClientControl="True" CommitChanges="True" />
                </Template>
            </px:PXFormView>
            <px:PXTab ID="tab" runat="server" Width="100%" Height="200px" DataSourceID="ds">
                <Items>
                    <px:PXTabItem Text="Labels">
                        <Template>
                            <px:PXGrid ID="grid" runat="server" AllowPaging="true" PageSize="15" DataSourceID="ds" TabIndex="3900" SkinID="Inquire" Height="400px" Width="100%" SyncPosition="true">
                                <AutoCallBack Target="formPhoto" Command="Refresh" ActiveBehavior="True">
                                    <Behavior CommitChanges="True" RepaintControlsIDs="formPhoto" />
                                </AutoCallBack>
                                <CallbackCommands>
                                    <Refresh RepaintControlsIDs="form" />
                                </CallbackCommands>
                                <Levels>
                                    <px:PXGridLevel DataKeyNames="RecordID" DataMember="Records">
                                        <RowTemplate>
                                            <px:PXSelector ID="edBAccountID2" runat="server" DataField="BAccountID" />
                                            <px:PXSelector ID="edScreenID2" runat="server" DataField="ScreenID" />
                                            <px:PXSelector ID="edRecordID2" runat="server" DataField="RecordID" />
                                            <px:PXLinkEdit ID="edImageUrl2" runat="server" DataField="ImageUrl" />
                                        </RowTemplate>
                                        <Columns>
                                            <px:PXGridColumn DataField="Selected" TextAlign="Center" AllowCheckAll="True" Type="CheckBox" Width="60px" />
                                            <px:PXGridColumn DataField="RecordID" TextAlign="Right" LinkCommand="ViewLog" />
                                            <px:PXGridColumn DataField="CreatedDateTime" Width="140px" DisplayFormat="g" />
                                            <px:PXGridColumn DataField="BAccountID" Width="150px" LinkCommand="ViewBAccount" DisplayMode="Text" />
                                            <px:PXGridColumn DataField="InventoryID" Width="150px" LinkCommand="ViewInventoryItem" DisplayMode="Text" />
                                            <px:PXGridColumn DataField="LotSerialNbr" Width="150px" />
                                            <px:PXGridColumn DataField="LabelKey" Width="120px" />
                                            <px:PXGridColumn DataField="ModelID" Width="120px" LinkCommand="ViewModel" DisplayMode="Text" />
                                            <px:PXGridColumn DataField="LabelFilename" Width="150px" />
                                            <px:PXGridColumn DataField="ScreenID" Width="96px" DisplayMode="Text" LinkCommand="ViewScreen" />
                                            <px:PXGridColumn DataField="PrintStationID" Width="150px" LinkCommand="ViewPrintStation" DisplayMode="Text" />
                                            <px:PXGridColumn DataField="PrinterID" Width="180px" LinkCommand="ViewPrinter" DisplayMode="Text" />
                                            <px:PXGridColumn DataField="ModelFormatID" Width="150px" LinkCommand="ViewModelFormat" DisplayMode="Text" />
                                            <px:PXGridColumn DataField="PrinterFormatID" Width="150px" LinkCommand="ViewPrinterFormat" DisplayMode="Text" />
                                            <px:PXGridColumn DataField="UserID" Width="100px" DisplayMode="Text" />
                                            <px:PXGridColumn DataField="OwnerID" Width="100px" DisplayMode="Text" />
                                            <px:PXGridColumn DataField="ContentType" Type="DropDownList" />
                                            <px:PXGridColumn DataField="NbCopies" TextAlign="Right" />
                                            <px:PXGridColumn DataField="ImageUrl" Width="120px" CommitChanges="true" />
                                        </Columns>
                                    </px:PXGridLevel>
                                </Levels>
                                <ActionBar DefaultAction="cmdDoubleClick">
                                    <CustomItems>
                                        <px:PXToolBarButton Text="Double Click" Visible="False" Key="cmdDoubleClick">
                                            <AutoCallBack Command="DoubleClick" Target="ds" />
                                        </px:PXToolBarButton>
                                    </CustomItems>
                                </ActionBar>
                                <%-- CA303000
                                <ActionBar DefaultAction="cmdDoubleClick">
                                    <CustomItems>
                                        <px:PXToolBarButton Text="Double Click" Visible="False" Key="cmdDoubleClick">
                                            <AutoCallBack Command="DoubleClick" Target="ds" />
                                        </px:PXToolBarButton>
                                    </CustomItems>
                                </ActionBar>
                                --%>
                            </px:PXGrid>
                        </Template>
                    </px:PXTabItem>
                </Items>
                <AutoSize Container="Window" Enabled="True" MinHeight="150" />
            </px:PXTab>
        </Template1>
        <Template2>
            <div class="content">
                <px:PXFormView ID="formPhoto" runat="server" DataMember="ImageViewer" DataSourceID="ds" TabIndex="200" SkinID="Transparent">
                    <Template>
                        <px:PXImageView ID="edImageUrl" runat="server" DataField="ImageUrl" Height="100%" Width="100%" AlreadyLocalized="False" CallbackUpdatable="True" />
                    </Template>
                </px:PXFormView>
            </div>
        </Template2>
    </px:PXSplitContainer>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
</asp:Content>
