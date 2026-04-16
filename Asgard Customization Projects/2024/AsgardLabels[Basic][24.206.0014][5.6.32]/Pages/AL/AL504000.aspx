<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true"
    ValidateRequest="false" CodeFile="AL504000.aspx.cs" Inherits="Page_AL504000" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" TypeName="AA.Objects.AL.ALPrintLogEnq" PrimaryView="Filter">
        <CallbackCommands>
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" Style="z-index: 100" Width="100%" Caption="Selection" CaptionAlign="Justify"
        DataMember="Filter" >
        <Template>
            <px:PXLayoutRule runat="server" LabelsWidth="SM" StartColumn="True" />
            <px:PXDateTimeEdit ID="edStartDate" runat="server" DataField="StartDate" IsClientControl="True" CommitChanges="True" />
            <px:PXDateTimeEdit ID="edEndDate" runat="server" DataField="EndDate" IsClientControl="True" CommitChanges="True" />
            <px:PXDropDown ID="edAggregatePeriod" runat="server" DataField="AggregatePeriod" IsClientControl="True" CommitChanges="True" />
            <px:PXDropDown ID="edAggregateBy" runat="server" DataField="AggregateBy" IsClientControl="True" CommitChanges="True" />
            <px:PXLayoutRule runat="server" LabelsWidth="SM" StartColumn="True" />
            <px:PXSelector ID="edPrinterID" runat="server" DataField="PrinterID" CommitChanges="True" />
            <px:PXSelector ID="edModelID" runat="server" DataField="ModelID" CommitChanges="True" />
            <px:PXSelector ID="edScreenID" runat="server" DataField="ScreenID" CommitChanges="True" />
            <px:PXSelector ID="edUserID" runat="server" DataField="UserID" CommitChanges="True" />
            <px:PXLayoutRule runat="server" LabelsWidth="SM" StartColumn="True" />
            <px:PXSelector ID="edPrintStationID" runat="server" DataField="PrintStationID" CommitChanges="True" />
            <px:PXSelector ID="edBAccountID" runat="server" DataField="BAccountID" CommitChanges="True" />
            <px:PXDropDown ID="edContentType" runat="server" DataField="ContentType" IsClientControl="True" CommitChanges="True" />
        </Template>
    </px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXGrid ID="grid" runat="server" DataSourceID="ds" TabIndex="3900" SkinID="Inquire" Height="400px" Width="100%">
        <Levels>
            <px:PXGridLevel DataMember="Records">
                <RowTemplate>
<%--                    <px:PXSelector ID="edBAccountID2" runat="server" DataField="BAccountID" />
                    <px:PXSelector ID="edScreenID2" runat="server" DataField="ScreenID" />--%>
                </RowTemplate>
                <Columns>
                    <%--<px:PXGridColumn DataField="GridLineNbr" TextAlign="Right"/>--%>
                    <px:PXGridColumn DataField="Year" />
                    <px:PXGridColumn DataField="Period" />
                    <px:PXGridColumn DataField="Count" />
                    <%--<px:PXGridColumn DataField="BAccountID" Width="150px" LinkCommand="ViewBAccount" DisplayMode="Text" />--%>
                    <%--<px:PXGridColumn DataField="ModelID" Width="200px" LinkCommand="ViewModel" DisplayMode="Text" />--%>
                    <%--<px:PXGridColumn DataField="ScreenID" Width="200px" DisplayMode="Text" LinkCommand="ViewScreen" />--%>
                    <%--<px:PXGridColumn DataField="PrintStationID" Width="150px" LinkCommand="ViewPrintStation" DisplayMode="Text" />--%>
<%--                    <px:PXGridColumn DataField="PrinterID" Width="200px" LinkCommand="ViewPrinter" DisplayMode="Text" />
                    <px:PXGridColumn DataField="UserID" Width="200px" DisplayMode="Text" />
                    <px:PXGridColumn DataField="ContentType" Width="200px" Type="DropDownList" />--%>
                </Columns>
            </px:PXGridLevel>
        </Levels>
        <AutoSize Container="Window" Enabled="True" MinHeight="150" />
        <Mode AllowAddNew="False" AllowDelete="False" AllowUpdate="False" />
    </px:PXGrid>
</asp:Content>

