<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="TC103010.aspx.cs" Inherits="Page_TC103010" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="TCAddon.TCReportSettingEntry"
        PrimaryView="ReportSetting"
        >
		<CallbackCommands>

		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
	<px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="ReportSetting" Width="100%" Height="" AllowAutoHide="false">
		<Template>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule57" StartRow="True" LabelsWidth="M" ControlSize="M" ></px:PXLayoutRule>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule46" StartColumn="True" ></px:PXLayoutRule>
			<px:PXSelector runat="server" ID="CstPXSelector52" DataField="ReportID" ></px:PXSelector>
			<px:PXLayoutRule ColumnWidth="500" LabelsWidth="" runat="server" ID="CstPXLayoutRule55" StartColumn="True" ></px:PXLayoutRule>
			<px:PXTextEdit Enabled="False" runat="server" ID="CstPXTextEdit49" DataField="DashBoardMenuName" ></px:PXTextEdit>
			<px:PXLayoutRule ControlSize="M" LabelsWidth="M" runat="server" ID="CstPXLayoutRule54" StartRow="True" ></px:PXLayoutRule>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule47" StartColumn="True" ></px:PXLayoutRule>
			<px:PXTextEdit runat="server" ID="CstPXTextEdit48" DataField="CompanyName" ></px:PXTextEdit>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule53" StartColumn="True" ></px:PXLayoutRule>
			<px:PXTextEdit runat="server" ID="CstPXTextEdit51" DataField="ProductName" ></px:PXTextEdit>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule56" StartColumn="True" ></px:PXLayoutRule>
			<px:PXTextEdit runat="server" ID="CstPXTextEdit50" DataField="KPIViewName" ></px:PXTextEdit></Template>
	
		<ClientEvents AfterRepaint="" Initialize="" ></ClientEvents>
		<AutoSize Enabled="True" MinHeight="200" Container="Parent" ></AutoSize>
		<AutoSize Enabled="True" ></AutoSize>
		<AutoSize MinHeight="100" ></AutoSize></px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" Runat="Server">
	<px:PXGrid ID="grid" runat="server" DataSourceID="ds" Width="100%" Height="300px" SkinID="Details" AllowAutoHide="false">
		<Levels>
			<px:PXGridLevel DataMember="ReportDetails">
			    <Columns>
				<px:PXGridColumn TextField="" CommitChanges="True" DataField="KPI" Width="220" ></px:PXGridColumn>
				<px:PXGridColumn DataField="PanelTitle" Width="220" ></px:PXGridColumn>
				<px:PXGridColumn DataField="TxnType" Width="280" ></px:PXGridColumn>
				<px:PXGridColumn DataField="TradingPartner" Width="280" ></px:PXGridColumn>
				<px:PXGridColumn DataField="DateFrom" Width="90" ></px:PXGridColumn>
				<px:PXGridColumn DataField="DateTo" Width="90" ></px:PXGridColumn></Columns>
			
			</px:PXGridLevel>
		</Levels>
		</px:PXGrid>
</asp:Content>