<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="AL503510.aspx.cs" Inherits="Page_AL503510" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" TypeName="AA.Objects.Labels.ALPrintJobMaint"
        PrimaryView="Document" PageLoadBehavior="SearchSavedKeys">
        <CallbackCommands>
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" Style="z-index: 100" Width="100%" DataMember="Document"
        TabIndex="3900" DefaultControlID="edRecordID">
        <Template>
            <px:PXLayoutRule runat="server" StartRow="True" StartColumn="True" LabelsWidth="M" ControlSize="XL" />
            <px:PXSelector ID="edRecordID" runat="server" AutoRefresh="true" DataField="RecordID" FilterByAllFields="True" />
            <px:PXNumberEdit ID="edPrintJobID" runat="server" DataField="PrintJobID" ValueType="Int64" />
            <px:PXSelector ID="edPrintNodeComputerID" runat="server" DataField="PrintNodeComputerID" />
            <px:PXSelector ID="edPrintNodePrinterID" runat="server" DataField="PrintNodePrinterID" />
            <px:PXTextEdit ID="edState" runat="server" DataField="State" />
            <px:PXTextEdit ID="edTitle" runat="server" DataField="Title" />
            <px:PXTextEdit ID="edSource" runat="server" DataField="Source" />
            <px:PXDateTimeEdit ID="edCreatedDateTime" runat="server" DataField="CreatedDateTime" Width="250px" />
            <px:PXDateTimeEdit ID="edReceivedAt" runat="server" DataField="ReceivedAt" Width="250px" />
            <px:PXDateTimeEdit ID="edSentToClientAt" runat="server" DataField="SentToClientAt" Width="250px" />
            <px:PXDateTimeEdit ID="edQueuedAt" runat="server" DataField="QueuedAt" Width="250px" />
            <px:PXDateTimeEdit ID="edInProgressAt" runat="server" DataField="InProgressAt" Width="250px" />
            <px:PXDateTimeEdit ID="edDoneAt" runat="server" DataField="DoneAt" Width="250px" />
            <px:PXDateTimeEdit ID="edExpiredAt" runat="server" DataField="ExpiredAt" Width="250px" />
            <px:PXDateTimeEdit ID="edStateDate" runat="server" DataField="StateDate" Width="250px" />
            <px:PXDateTimeEdit ID="edLastModifiedDateTime" runat="server" DataField="LastModifiedDateTime" Width="250px" />
            <px:PXTextEdit ID="edPrintLogID" runat="server" DataField="PrintLogID">
                <LinkCommand Target="ds" Command="ViewPrintLog" />
            </px:PXTextEdit>
<%--            <px:PXSelector ID="edUserID" runat="server" DataField="UserID" />
            <px:PXSelector ID="edPrinterID" runat="server" DataField="PrinterID" />
            <px:PXSelector ID="edPrintStationID" runat="server" DataField="PrintStationID" />
            <px:PXDropDown ID="edContentType" runat="server" DataField="ContentType" />--%>
        </Template>
        <AutoSize Container="Window" Enabled="True" MinHeight="200" />
    </px:PXFormView>
</asp:Content>
