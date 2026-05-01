<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="TC640000.aspx.cs" Inherits="Page_TC640000" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="TCAddon.TCReportDisplayEntry10"
        PrimaryView="ReportCredential"
        >
		<CallbackCommands>
			<px:PXDSCallbackCommand Name="TCOpenAutoPackConfirm" Visible="False" ></px:PXDSCallbackCommand></CallbackCommands>
	
		<ClientEvents MenuShow="" CommandPerformed="" ButtonClick="" BeforeRedirect="" Initialize="" ></ClientEvents></px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
	<px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="ReportCredential" Width="100%" AllowAutoHide="false">
		<Template>
			<px:PXLayoutRule runat="server" ID="PXLayoutRule1" StartRow="True" />
			<px:PXButton Text="View Report" runat="server" ID="CstButton10">
				<AutoCallBack Target="ds" Command="ViewReport" ></AutoCallBack></px:PXButton></Template>
		<AutoSize Container="Window" Enabled="True" MinHeight="200" ></AutoSize>
	
		<ClientEvents Initialize="" ></ClientEvents></px:PXFormView></asp:Content>