<%@ Page Language="C#" MasterPageFile="~/MasterPages/ListView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="TC106000.aspx.cs" Inherits="Page_TC106000" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/ListView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="TCAddon.TCAPIUrlsEntry"
        PrimaryView="LabelField"
        >
		<CallbackCommands>

		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phL" runat="Server">
	<px:PXGrid ID="grid" runat="server" DataSourceID="ds" Width="100%" Height="150px" SkinID="Primary" AllowAutoHide="false">
		<Levels>
			<px:PXGridLevel DataMember="LabelField">
			    <Columns>
				<px:PXGridColumn DataField="ServerName" Width="140" ></px:PXGridColumn>
				<px:PXGridColumn DataField="APIName" Width="180" ></px:PXGridColumn>
				<px:PXGridColumn DataField="APIServiceUrl" Width="280" ></px:PXGridColumn></Columns>
			</px:PXGridLevel>
		</Levels>
		<AutoSize Container="Window" Enabled="True" MinHeight="150" ></AutoSize>
		<ActionBar >
		
			<Actions>
				<AddNew Enabled="False" /></Actions>
			<Actions>
				<Delete Enabled="False" /></Actions></ActionBar>
	</px:PXGrid>
</asp:Content>
