<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="TC302000.aspx.cs" Inherits="Page_TC302000" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="TCAddon.TCLabelLogEntry"
        PrimaryView="TCLabelLogHeaderRecord"
        >
		<CallbackCommands>

		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
	<px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="TCLabelLogHeaderRecord" Width="100%" AllowAutoHide="false">
		<Template>
			<px:PXLayoutRule runat="server" ID="PXLayoutRule1" StartRow="True" ></px:PXLayoutRule>
			<px:PXTextEdit runat="server" DataField="LogName" Enabled="False" ID="TeLogName" ></px:PXTextEdit>
			<px:PXDateTimeEdit EditFormat="MM/dd/yyyy hh:mm tt" Enabled="False" runat="server" DataField="LogDate" ID="DteLogDate" Width="300px"></px:PXDateTimeEdit>
			<px:PXTextEdit Enabled="False" runat="server" ID="CstPXTextEdit1" DataField="LogPrintStatus" ></px:PXTextEdit>
			<px:PXTextEdit TextAlign="Justify" TextMode="MultiLine" Height="100" runat="server" DataField="ErrorMessage" Enabled="False" ID="TeError" ></px:PXTextEdit>
			<px:PXTextEdit TextAlign="Justify" TextMode="MultiLine" CommitChanges="True" Enabled="False" runat="server" ID="TeLogFile" Width="1200px" Height="900px" DataField="LabelXml" >
				<LinkCommand Enabled="" Command="" ></LinkCommand>
				<AutoCallBack Command="" ></AutoCallBack>
				<AutoCallBack Enabled="True" ></AutoCallBack></px:PXTextEdit></Template>
		<AutoSize Container="Window" Enabled="True" MinHeight="200" ></AutoSize>
	</px:PXFormView>
</asp:Content>