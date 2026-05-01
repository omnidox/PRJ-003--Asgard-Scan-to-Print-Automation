<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="TC502030.aspx.cs" Inherits="Page_TC502030" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="TCAddon.TCProcessInvoice"
        PrimaryView="TransactionStatus"
        >
		<CallbackCommands>

		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
	<px:PXFormView Height="" SyncPosition="True" TabIndex="100" CaptionVisible="False" Caption="Process Invoice AR" RenderStyle="Normal" ID="form" runat="server" DataSourceID="ds" DataMember="TransactionStatus" Width="100%" AllowAutoHide="false">
		<Template>
			<px:PXLayoutRule LabelsWidth="200" ID="PXLayoutRule1" runat="server" StartRow="True"></px:PXLayoutRule>
			<px:PXLayoutRule LabelsWidth="" runat="server" ID="CstPXLayoutRule1" StartColumn="True" ></px:PXLayoutRule>
			<px:PXLayoutRule runat="server" ID="CstLayoutRule1" ColumnWidth="SM" LabelsWidth="SM" ></px:PXLayoutRule>
			<px:PXSelector Size="SM" LabelWidth="" Width="" runat="server" ID="CstPXSelector4" DataField="OrderNbrFrom" SkinID="" CommitChanges="True" ></px:PXSelector>
			<px:PXDateTimeEdit LabelWidth="" Width="" runat="server" ID="CstPXDateTimeEdit3" DataField="DateFrom" CommitChanges="True" Size="SM" ></px:PXDateTimeEdit>
			<px:PXLayoutRule LabelsWidth="" runat="server" ID="CstPXLayoutRule2" StartColumn="True" ></px:PXLayoutRule>
			<px:PXLayoutRule runat="server" ID="CstLayoutRule2" ControlSize="SM" LabelsWidth="SM" />
			<px:PXSelector Size="SM" LabelWidth="" Width="" runat="server" ID="CstPXSelector6" DataField="OrderNbrTo" CommitChanges="True" SkinID="" ></px:PXSelector>
			<px:PXDateTimeEdit LabelWidth="" Width="" runat="server" ID="CstPXDateTimeEdit5" DataField="DateTo" CommitChanges="True" Size="SM" ></px:PXDateTimeEdit></Template>
		<AutoSize Container="Parent" Enabled="True" MinHeight="100" ></AutoSize>
	</px:PXFormView>
	<px:PXGrid AutoAdjustColumns="True" FilesIndicator="False" NoteIndicator="False" SkinID="PrimaryInquire" Width="100%" Height="500px" runat="server" ID="CstPXGrid7" AdjustPageSize="Auto" AllowPaging="True" Caption="" CaptionVisible="False" SyncPosition="True" TabIndex="1100" PageSize="20">
		<Levels>
			<px:PXGridLevel DataMember="Orders" >
				<Columns>
					<px:PXGridColumn DataField="Selected" Width="60" Type="CheckBox" CommitChanges="False" ></px:PXGridColumn>
					<px:PXGridColumn DataField="DocType" Width="70" ></px:PXGridColumn>
					<px:PXGridColumn DataField="RefNbr" Width="140" CommitChanges="True" ></px:PXGridColumn>
					<px:PXGridColumn DataField="DocDate" Width="90" ></px:PXGridColumn></Columns></px:PXGridLevel></Levels></px:PXGrid></asp:Content>

