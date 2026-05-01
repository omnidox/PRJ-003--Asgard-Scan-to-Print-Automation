<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="TC104000.aspx.cs" Inherits="Page_TC104000" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="TCAddon.TCTransactionStatusEntry"
        PrimaryView="TransactionStatus"
        >
		<CallbackCommands>

		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
	<px:PXFormView SyncPosition="True" TabIndex="1100" CaptionVisible="False" Caption="Transaction Service Configuration" RenderStyle="Normal" ID="form" runat="server" DataSourceID="ds" DataMember="TransactionStatus" Width="100%" AllowAutoHide="false">
		<Template>
			<px:PXLayoutRule Merge="True" ColumnSpan="200" LabelsWidth="200" runat="server" ID="CstPXLayoutRule28" StartRow="True" ></px:PXLayoutRule>
			<px:PXLayoutRule StartRow="" Merge="" LabelsWidth="" SuppressLabel="True" ColumnSpan="200" runat="server" ID="CstPXLayoutRule29" StartColumn="True" ></px:PXLayoutRule>
			<px:PXPanel ContentLayout-ColumnsWidth="200" ContentLayout-ControlSize="M" ContentLayout-LabelsWidth="200" ContentLayout-Orientation="Horizontal" ContentLayout-StackLayout="Simple" RenderStyle="Simple" ContentLayout-AutoSizeControls="True" ContentLayout-ContentAlign="Right" ContentLayout-Layout="Cavas" ContentLayout-InnerSpacing="True" ContentLayout-OuterSpacing="Horizontal" ContentLayout-SpacingSize="Small" runat="server" ID="CstPanel40">
				<px:PXLabel Style='left:10px;top:;Position:relative;' runat="server" ID="CstLabel41" Text="* Please go to menu &quot;TrueCommerce&quot; and click &quot;Login&quot; to sign in with your Foundry user for the first time." ></px:PXLabel></px:PXPanel>
			<px:PXLayoutRule LabelsWidth="" ID="PXLayoutRule1" runat="server" StartRow="True"></px:PXLayoutRule>
			<px:PXLayoutRule LabelsWidth="200" runat="server" ID="CstPXLayoutRule8" StartColumn="True" ></px:PXLayoutRule>
			<px:PXNumberEdit Width="50" runat="server" ID="CstPXNumberEdit12" DataField="QueryDocDateRange" ></px:PXNumberEdit>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule43" StartRow="True" />
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule44" StartColumn="True" LabelsWidth="200" />
			<px:PXCheckBox runat="server" ID="CstPXCheckBox45" DataField="IsUpdateTransactionStatus" AlignLeft="True" TextAlign="Right" /></Template>
		<AutoSize Container="Window" Enabled="True" MinHeight="200" ></AutoSize>
	</px:PXFormView>
</asp:Content>

