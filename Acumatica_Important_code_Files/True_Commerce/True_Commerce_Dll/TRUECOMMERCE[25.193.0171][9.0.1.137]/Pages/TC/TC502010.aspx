<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="TC502010.aspx.cs" Inherits="Page_TC502010" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="TCAddon.TCProcessSalesOrders"
        PrimaryView="TransactionStatus"
        >
		<CallbackCommands>

		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
	<px:PXFormView CaptionVisible="False" Caption="Process Purchase Order Acknowledgement" RenderStyle="Normal" TabIndex="100" Height="" ID="form" runat="server" DataSourceID="ds" DataMember="TransactionStatus" Width="100%" AllowAutoHide="false">
		<Template>
			<px:PXLayoutRule runat="server" ID="PXLayoutRule1" StartRow="True" ></px:PXLayoutRule>
			<px:PXLayoutRule LabelsWidth="150" runat="server" ID="CstPXLayoutRule1" StartColumn="True" ></px:PXLayoutRule>
			<px:PXLayoutRule LabelsWidth="SM" runat="server" ID="CstLayoutRule1" ControlSize="" ColumnWidth="" ></px:PXLayoutRule>
			<px:PXSelector Size="SM" CommitChanges="True" runat="server" ID="CstPXSelector8" DataField="OrderNbrFrom" ></px:PXSelector>
			<px:PXDateTimeEdit Size="SM" CommitChanges="True" runat="server" ID="CstPXDateTimeEdit3" DataField="DateFrom" ></px:PXDateTimeEdit>
			<px:PXLayoutRule LabelsWidth="30" runat="server" ID="CstPXLayoutRule2" StartColumn="True" ></px:PXLayoutRule>
			<px:PXLayoutRule runat="server" ID="CstLayoutRule2" LabelsWidth="XS" ControlSize="XS" ></px:PXLayoutRule>
			<px:PXSelector Size="SM" CommitChanges="True" runat="server" ID="CstPXSelector9" DataField="OrderNbrTo" ></px:PXSelector>
			<px:PXDateTimeEdit Size="SM" CommitChanges="True" runat="server" ID="CstPXDateTimeEdit5" DataField="DateTo" ></px:PXDateTimeEdit></Template>
		<AutoSize Container="Parent" Enabled="True" MinHeight="100" ></AutoSize>
	</px:PXFormView>
	<px:PXGrid AutoAdjustColumns="True" runat="server" ID="CstPXGrid30" SyncPosition="True" Height="500px" SkinID="PrimaryInquire" TabIndex="1100" Width="100%" Caption="Orders" DataSourceID="ds" AllowPaging="True" AdjustPageSize="Auto" PageSize="20" NoteIndicator="False" FilesIndicator="False">
		<Levels>
			<px:PXGridLevel DataMember="Orders" DataKeyNames="Type,DocNo">
				<RowTemplate>
					<px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="M" ControlSize="M" ></px:PXLayoutRule>
					<px:PXLayoutRule runat="server" Merge="True" ></px:PXLayoutRule></RowTemplate>
				<Columns>
					<px:PXGridColumn CommitChanges="False" DataField="Selected" Type="CheckBox" Width="60" ></px:PXGridColumn>
					<px:PXGridColumn DataField="OrderType" Width="70" ></px:PXGridColumn>
					<px:PXGridColumn CommitChanges="True" DataField="OrderNbr" Width="140" ></px:PXGridColumn>
					<px:PXGridColumn DataField="OrderDate" Width="90" ></px:PXGridColumn></Columns></px:PXGridLevel></Levels></px:PXGrid></asp:Content>

