<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="AL100000.aspx.cs" Inherits="Page_AL100000" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" TypeName="AA.Objects.Labels.ALAboutMaint" PrimaryView="ALAbout">
        <CallbackCommands>
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="ALAbout" Width="100%" AllowAutoHide="false">
        <Template>
            <px:PXLayoutRule LabelsWidth="SM" ControlSize="XXL" ColumnWidth="100%" runat="server" StartRow="True" />
            <px:PXLabel runat="server" ID="aboutLabel" Text="Asgard Labels Powered by Asgard Alliance" />
            <px:PXTextEdit runat="server" ID="edAcumaticaVersion" DataField="AcumaticaVersion" />
            <px:PXTextEdit runat="server" ID="edBasicVersion" DataField="BasicVersion" />
            <%--<px:PXTextEdit runat="server" ID="edProfessionalVersion" DataField="ProfessionalVersion" />--%>
            <%--<px:PXTextEdit runat="server" ID="edEnterpriseVersion" DataField="EnterpriseVersion" />--%>
            <px:PXTextEdit runat="server" ID="edWikiVersion" DataField="WikiVersion" />
            <px:PXTextEdit runat="server" ID="edIntegrations" DataField="Integrations" />
            <px:PXMailEdit runat="server" ID="edSupportEmail" DataField="SupportEmail" />
            <px:PXLinkEdit runat="server" ID="edSupportLink" DataField="SupportLink" />
            <px:PXTextEdit runat="server" ID="edSupportNum" DataField="SupportNum" />
            <px:PXLinkEdit runat="server" ID="edTermsLink" DataField="TermsLink" />
            <px:PXNumberEdit runat="server" ID="edNbPrinters" DataField="NbPrinters" />
            <px:PXNumberEdit runat="server" ID="edNbPrintStations" DataField="NbPrintStations" />
            <px:PXNumberEdit runat="server" ID="edNbModels" DataField="NbModels" />
            <%--<px:PXNumberEdit runat="server" ID="edNbUsers" DataField="NbUsers" />--%>
            <%--<px:PXNumberEdit runat="server" ID="edNbOwners" DataField="NbOwners" />--%>
        </Template>
        <AutoSize Container="Window" Enabled="True" MinHeight="200" />
    </px:PXFormView>
</asp:Content>

