<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true"
    ValidateRequest="false" CodeFile="AL503500.aspx.cs" Inherits="Page_AL503500" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" TypeName="AA.Objects.Labels.ALPrintJobProcess" PrimaryView="Filter">
        <CallbackCommands>
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" Style="z-index: 100" Width="100%" DataMember="Filter" TabIndex="2000">
        <Template>
            <px:PXLayoutRule runat="server" LabelsWidth="S" StartColumn="True" />
            <px:PXDropDown ID="edAction" runat="server" DataField="Action" IsClientControl="True" CommitChanges="True" Width="120" />
            <px:PXDateTimeEdit ID="edStartDate" runat="server" AlreadyLocalized="False" DataField="StartDate" IsClientControl="True" CommitChanges="True" Width="120" />
            <px:PXDateTimeEdit ID="edEndDate" runat="server" AlreadyLocalized="False" DataField="EndDate" IsClientControl="True" CommitChanges="True" Width="120" />
            <px:PXDropDown ID="edContentType" runat="server" DataField="ContentType" IsClientControl="True" CommitChanges="True" Width="120" />
            <px:PXSelector ID="edUserID" runat="server" DataField="UserID" CommitChanges="True" Width="120" />
            <px:PXLayoutRule runat="server" LabelsWidth="S" StartColumn="True" />
            <px:PXSelector ID="edPrintNodeComputerID" runat="server" DataField="PrintNodeComputerID" CommitChanges="True" />
            <px:PXSelector ID="edPrintNodePrinterID" runat="server" DataField="PrintNodePrinterID" CommitChanges="True" AutoRefresh="true" />
        </Template>
    </px:PXFormView>
    <px:PXTab ID="tab" runat="server" Width="100%" Height="200px" DataSourceID="ds" >
        <Items>
            <px:PXTabItem Text="Jobs">
                <Template>
                    <px:PXGrid ID="grid" runat="server" DataSourceID="ds" TabIndex="3900" SkinID="Inquire" Height="400px" Width="100%" SyncPosition="true" FilesIndicator="False" NoteIndicator="False">
                        <CallbackCommands>
                            <Refresh RepaintControlsIDs="form" />
                        </CallbackCommands>
                        <Levels>
                            <px:PXGridLevel DataKeyNames="RecordID" DataMember="Records">
                                <RowTemplate>
                                    <px:PXSelector ID="edRecordID2" runat="server" DataField="RecordID" />
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="Selected" TextAlign="Center" AllowCheckAll="True" Type="CheckBox" Width="60px" />
                                    <px:PXGridColumn DataField="RecordID" TextAlign="Right" LinkCommand="ViewJob" Width="90px"/>
                                    <px:PXGridColumn DataField="PrintJobID" TextAlign="Right" Width="100px"/>
                                    <px:PXGridColumn DataField="State" Width="100px" />
<%--                                    <px:PXGridColumn DataField="UserID" Width="100px" DisplayMode="Text" />
                                    <px:PXGridColumn DataField="ContentType" Type="DropDownList" Width="100px"/>--%>
                                    <px:PXGridColumn DataField="Title" Width="400px"/>
                                    <px:PXGridColumn DataField="Source" Width="250px"/>
                                    <px:PXGridColumn DataField="StateDate" Width="160px" DisplayFormat="g" />
                                    <px:PXGridColumn DataField="ReceivedAt" Width="160px" DisplayFormat="g" />
                                    <px:PXGridColumn DataField="ExpiresAt" Width="160px" DisplayFormat="g" />
                                    <px:PXGridColumn DataField="SentToClientAt" Width="160px" DisplayFormat="g" />
                                    <px:PXGridColumn DataField="InProgressAt" Width="160px" DisplayFormat="g" />
                                    <px:PXGridColumn DataField="DoneAt" Width="160px" DisplayFormat="g" />
                                    <px:PXGridColumn DataField="ExpiredAt" Width="160px" DisplayFormat="g" />
                                    <px:PXGridColumn DataField="PrintLogID" TextAlign="Right" LinkCommand="ViewLog" />
<%--                                    <px:PXGridColumn DataField="PrinterID" Width="150px" LinkCommand="ViewPrinter" />
                                    <px:PXGridColumn DataField="PrintStationID" Width="150px" LinkCommand="ViewPrintStation" />--%>
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
        </Items>
        <AutoSize Container="Window" Enabled="True" MinHeight="150" />
    </px:PXTab>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
</asp:Content>
