<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="TC106010.aspx.cs" Inherits="Page_TC106010" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="TCAddon.TCVersionEntry"
        PrimaryView="TCVersionNumber"
        >
		<CallbackCommands>

		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
	<px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="TCVersionNumber" Width="100%" AllowAutoHide="false">
		<Template>
			<px:PXLayoutRule ID="PXLayoutRule1" runat="server" StartRow="True"></px:PXLayoutRule>
			<px:PXTextEdit Enabled="True" runat="server" ID="CstPXTextEdit1" DataField="ProjectName" ></px:PXTextEdit>
			<px:PXTextEdit Enabled="True" runat="server" ID="CstPXTextEdit2" DataField="VersionNumber" ></px:PXTextEdit></Template>
		<AutoSize Container="Window" Enabled="True" MinHeight="200" ></AutoSize>
	</px:PXFormView>
</asp:Content>

