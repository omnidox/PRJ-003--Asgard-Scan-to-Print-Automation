<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="TC102000.aspx.cs" Inherits="Page_TC102000" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" PrimaryView="TCCustomerRecord" TypeName="TCAddon.TCCustomerSettingsSetupMaint">
	
	<CallbackCommands></CallbackCommands></px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" Style="z-index: 100" Width="100%" DataMember="TCCustomerRecord" TabIndex="900">
		<Template>
			<px:PXLayoutRule runat="server" StartRow="True" StartColumn="True"></px:PXLayoutRule>
		    <px:PXLayoutRule runat="server" StartColumn="True">
            </px:PXLayoutRule>
			<px:PXLayoutRule GroupCaption="General Settings" runat="server" ID="CstPXLayoutRule56" StartGroup="True" ></px:PXLayoutRule>
			<px:PXLayoutRule runat="server" ID="CstLayoutRule74" LabelsWidth="M" ControlSize="M" ></px:PXLayoutRule>
			<px:PXSegmentMask CommitChanges="True" runat="server" ID="edCustomerID" DataField="CustomerID" ></px:PXSegmentMask>
			<px:PXDropDown runat="server" ID="edAsnType" DataField="AsnType" ></px:PXDropDown>
			<px:PXDropDown CommitChanges="True" runat="server" ID="edAutoPackType" DataField="AutoPackType" ></px:PXDropDown>
			<px:PXSelector CommitChanges="True" AutoRefresh="True" runat="server" ID="edTemplateID" DataField="TemplateID" ></px:PXSelector></Template>
		<AutoSize Container="Window" Enabled="True" MinHeight="200" ></AutoSize>
	</px:PXFormView></asp:Content>