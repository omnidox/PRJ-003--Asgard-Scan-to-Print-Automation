<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="TC103000.aspx.cs" Inherits="Page_TC103000" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="TCAddon.TCReportLogon"
        PrimaryView="ReportLink"
        >
		<CallbackCommands></CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
	<px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="ReportLink" Width="100%" AllowAutoHide="false">
		<Template>
			<px:PXLayoutRule ControlSize="SM" ColumnWidth="" LabelsWidth="SM" ID="PXLayoutRule1" runat="server" StartRow="True"></px:PXLayoutRule>
			<px:PXDropDown runat="server" ID="CstPXDropDown11" DataField="ServerName" ></px:PXDropDown>
			<px:PXTextEdit runat="server" ID="CstPXTextEdit3" DataField="UserName" ></px:PXTextEdit>
			<px:PXTextEdit runat="server" ID="CstPXTextEdit1" DataField="Password" TextMode="Password" ></px:PXTextEdit>
			<px:PXTextEdit runat="server" ID="CstPXTextEdit2" DataField="Tenant" ></px:PXTextEdit>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule5" StartRow="True" ></px:PXLayoutRule>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule6" StartColumn="True" ></px:PXLayoutRule>
			<px:PXCheckBox AlignLeft="True" runat="server" ID="CstPXCheckBox4" DataField="ReportAuth" ></px:PXCheckBox>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule7" StartColumn="True" ></px:PXLayoutRule>
			<px:PXCheckBox AlignLeft="True" runat="server" ID="CstPXCheckBox8" DataField="LabelAuth" ></px:PXCheckBox>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule9" StartColumn="True" ></px:PXLayoutRule>
			<px:PXCheckBox AlignLeft="True" runat="server" ID="CstPXCheckBox10" DataField="IndicatorAuth" ></px:PXCheckBox>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule11" StartRow="True" ></px:PXLayoutRule>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule15" StartColumn="True" ></px:PXLayoutRule>
			<px:PXTextEdit Enabled="False" LabelWidth="200" runat="server" ID="CstPXTextEdit14" DataField="ReportHistory" TextMode="SingleLine" Width="800" ></px:PXTextEdit>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule16" StartColumn="True" ></px:PXLayoutRule>
			<px:PXButton StateColumn="" DependOnGrid="" AlignLeft="True" Text="Disconnect" runat="server" ID="btDiscon_Report" >
				<AutoCallBack Target="ds" Command="tCReportDisconnectConfirm" ></AutoCallBack></px:PXButton>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule23" StartRow="True" ></px:PXLayoutRule>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule25" StartColumn="True" ></px:PXLayoutRule>
			<px:PXTextEdit Enabled="False" LabelWidth="200" runat="server" ID="CstPXTextEdit13" DataField="LabelHistory" TextMode="SingleLine" Width="800" ></px:PXTextEdit>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule26" StartColumn="True" ></px:PXLayoutRule>
			<px:PXButton AlignLeft="True" Text="Disconnect" runat="server" ID="btDiscon_Label" >
				<AutoCallBack Target="ds" Command="tCLabelDisconnectConfirm" ></AutoCallBack></px:PXButton>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule24" StartRow="True" ></px:PXLayoutRule>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule27" StartColumn="True" ></px:PXLayoutRule>
			<px:PXTextEdit Enabled="False" LabelWidth="200" runat="server" ID="CstPXTextEdit12" DataField="IndicatorHistory" TextMode="SingleLine" Width="800" ></px:PXTextEdit>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule28" StartColumn="True" ></px:PXLayoutRule>
			<px:PXButton AlignLeft="True" Text="Disconnect" runat="server" ID="btDiscon_Indicator" >
				<AutoCallBack Target="ds" Command="tCTransactionDisconnectConfirm" ></AutoCallBack></px:PXButton></Template>
		<AutoSize Container="Window" Enabled="True" MinHeight="200" ></AutoSize>
	</px:PXFormView>
	<px:PXSmartPanel AutoReload="True" runat="server" ID="PanelRptDisconnect" Height="140px" Width="400px" LoadOnDemand="True" AutoRepaint="True" CaptionVisible="True" Caption="Report Disconnect" Key="TCReportConnectFilterView" CommandSourceID="ds" CommandName="TCReportDisconnectConfirm">
		<px:PXPanel runat="server" ID="CstPanel39" Height="80">
			<px:PXLabel runat="server" Text="Continue to disconnect &quot;Report&quot;? This will unlink all related Report Dashboard(s)." ID="CstLabel40" ></px:PXLabel></px:PXPanel>
		<px:PXPanel runat="server" ID="CstPanel35" SkinID="Buttons">
			<px:PXButton runat="server" Text="Continue" DialogResult="OK" SyncVisible="False" CommandSourceID="ds" CommandName="AutoPack" ID="CstButton36">
				<AutoCallBack Command="TCReportDisconnect" Target="ds" ></AutoCallBack></px:PXButton>
			<px:PXButton runat="server" Text="Cancel" DialogResult="Cancel" ID="CstButton37" ></px:PXButton></px:PXPanel></px:PXSmartPanel>
	<px:PXSmartPanel AutoReload="True" runat="server" ID="PanelTransactionDisconnect" Height="140px" Width="400px" LoadOnDemand="True" AutoRepaint="True" CaptionVisible="True" Caption="Transaction Status Disconnect" Key="TCTransactionConnectFilterView" CommandSourceID="ds" CommandName="TCTransactionDisconnectConfirm">
		<px:PXPanel runat="server" ID="CstPanel38" Height="80">
			<px:PXLabel runat="server" ID="CstLabel39" Text="Continue to disconnect &quot;Transaction Status&quot;? This will also set all related Automation Schedule(s) to &quot;Inactive&quot;." ></px:PXLabel></px:PXPanel>
		<px:PXPanel runat="server" ID="CstPanel34" SkinID="Buttons">
			<px:PXButton Enabled="True" runat="server" ID="CstButton35" Text="Continue" DialogResult="OK" SyncVisible="False" CommandSourceID="ds" CommandName="TCTransactionDisconnect">
				<AutoCallBack Enabled="True" Target="ds" Command="TCTransactionDisconnect" ></AutoCallBack></px:PXButton>
			<px:PXButton runat="server" ID="CstButton34" Text="Cancel" DialogResult="Cancel" ></px:PXButton></px:PXPanel></px:PXSmartPanel>
	<px:PXSmartPanel AutoReload="True" runat="server" ID="PanelLabelDisconnect" Height="140px" Width="400px" LoadOnDemand="True" AutoRepaint="True" CaptionVisible="True" Caption="Label Disconnect" Key="TCLabelConnectFilterView" CommandSourceID="ds" CommandName="TCLabelDisconnectConfirm">
		<px:PXPanel runat="server" ID="CstPanel37" Height="80">
			<px:PXLabel runat="server" ID="CstLabel38" Text="Continue to disconnect &quot;Label&quot;? You will need to log on Foundry platform again if you want to print label(s)." ></px:PXLabel></px:PXPanel>
		<px:PXPanel runat="server" ID="CstPanel33" SkinID="Buttons">
			<px:PXButton runat="server" ID="CstButton33" Text="Continue" DialogResult="OK" SyncVisible="False" CommandSourceID="ds" CommandName="TCLabelDisconnect">
				<AutoCallBack Target="ds" Command="TCLabelDisconnect" ></AutoCallBack></px:PXButton>
			<px:PXButton runat="server" ID="CstButton32" Text="Cancel" DialogResult="Cancel" ></px:PXButton></px:PXPanel></px:PXSmartPanel></asp:Content>