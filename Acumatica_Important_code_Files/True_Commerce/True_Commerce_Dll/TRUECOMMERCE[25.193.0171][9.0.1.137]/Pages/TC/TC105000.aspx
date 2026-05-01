<%@ Page Language="C#" MasterPageFile="~/MasterPages/ListView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="TC105000.aspx.cs" Inherits="Page_TC105000" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/ListView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="TCAddon.TCShipFromAddressEntry"
        PrimaryView="TCShipFromAddress">
		<CallbackCommands></CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phL" runat="Server">
	<px:PXFormView TabIndex="900" Width="100%" AllowCollapse="False" RenderStyle="Normal" DataMember="TCShipFromAddress" runat="server" ID="tcShipFromAddressID" DataSourceID="ds" >
		<Template>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule11" StartColumn="True" ></px:PXLayoutRule>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule16" StartGroup="True" GroupCaption="Ship-from Address" ></px:PXLayoutRule>		
			<px:PXSelector runat="server" ID="CstPXSelector13" DataField="TCSFAddressID" ></px:PXSelector>
			<px:PXTextEdit runat="server" ID="edTCShipFromName" DataField="TCShipFromName" ></px:PXTextEdit>
			<px:PXTextEdit runat="server" ID="edTCShipFromAddress1" DataField="TCShipFromAddress1" ></px:PXTextEdit>
			<px:PXTextEdit runat="server" ID="edTCShipFromAddress2" DataField="TCShipFromAddress2" ></px:PXTextEdit>
			<px:PXTextEdit runat="server" ID="edTCShipFromCity" DataField="TCShipFromCity" ></px:PXTextEdit>
			<px:PXSelector CommitChanges="True" AllowAddNew="True" AutoRefresh="True" runat="server" ID="edTCShipFromState" DataField="TCShipFromState" ></px:PXSelector>
			<px:PXSelector CommitChanges="True" AllowAddNew="True" runat="server" ID="edTCShipFromCountry" DataField="TCShipFromCountry" ></px:PXSelector>
			<px:PXTextEdit runat="server" ID="edTCShipFromPostCode" DataField="TCShipFromPostCode" ></px:PXTextEdit>
			<px:PXTextEdit runat="server" ID="edTCShipFromPhoneNo" DataField="TCShipFromPhoneNo" ></px:PXTextEdit>
			<px:PXCheckBox AlignLeft="False" TextAlign="Right" runat="server" ID="CstPXCheckBox13" DataField="TCDefaultAddress" ></px:PXCheckBox></Template>
		<AutoSize Container="Window" ></AutoSize>
		<AutoSize Enabled="True" ></AutoSize>
		<AutoSize MinHeight="200" ></AutoSize></px:PXFormView></asp:Content>
