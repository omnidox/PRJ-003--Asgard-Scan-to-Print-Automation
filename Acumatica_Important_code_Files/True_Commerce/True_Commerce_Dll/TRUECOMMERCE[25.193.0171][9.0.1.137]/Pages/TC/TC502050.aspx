<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="TC502050.aspx.cs" Inherits="Page_TC502050" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="TCAddon.TCProcessWarehouseShippingOrder"
        PrimaryView="TransactionStatus"
        >
		<CallbackCommands>

		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
	<px:PXFormView Caption="Process Warehouse Shipping Order" RenderStyle="Normal" CaptionVisible="False" ID="form" runat="server" DataSourceID="ds" DataMember="TransactionStatus" Width="100%" AllowAutoHide="false">
		<Template>
			<px:PXLayoutRule runat="server" ID="PXLayoutRule1" StartRow="True" ></px:PXLayoutRule>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule1" StartColumn="True" LabelsWidth="" ></px:PXLayoutRule>
			<px:PXLayoutRule runat="server" ID="CstLayoutRule1" LabelsWidth="SM" ControlSize="SM" />
			<px:PXSelector runat="server" ID="CstPXSelector8" DataField="OrderNbrFrom" Size="SM" CommitChanges="True" ></px:PXSelector>
			<px:PXDateTimeEdit runat="server" CommitChanges="True" Size="SM" DataField="DateFrom" ID="CstPXDateTimeEdit3" ></px:PXDateTimeEdit>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule2" StartColumn="True" LabelsWidth="30" ></px:PXLayoutRule>
			<px:PXLayoutRule runat="server" ID="CstLayoutRule3" ControlSize="XS" LabelsWidth="XS" />
			<px:PXSelector runat="server" ID="CstPXSelector9" DataField="OrderNbrTo" Size="SM" CommitChanges="True" ></px:PXSelector>
			<px:PXDateTimeEdit runat="server" CommitChanges="True" Size="SM" DataField="DateTo" ID="CstPXDateTimeEdit5" ></px:PXDateTimeEdit></Template>
		<AutoSize Container="Parent" Enabled="True" MinHeight="100" ></AutoSize>
	</px:PXFormView>
	<px:PXGrid AutoAdjustColumns="True" runat="server" ID="CstPXGrid30" SyncPosition="True" Height="500px" SkinID="PrimaryInquire" TabIndex="1100" Width="100%" Caption="Orders" DataSourceID="ds" AllowPaging="True" AdjustPageSize="Auto" PageSize="20" NoteIndicator="False" FilesIndicator="False">
		<Levels>
			<px:PXGridLevel DataMember="Orders" DataKeyNames="Type,DocNo">
				<RowTemplate>
					<px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="M" ControlSize="M" ></px:PXLayoutRule>
					<px:PXLayoutRule runat="server" Merge="True" ></px:PXLayoutRule></RowTemplate>
				<Columns>
					<px:PXGridColumn DataField="Selected" Type="CheckBox" Width="60" CommitChanges="False" ></px:PXGridColumn>
					<px:PXGridColumn DataField="OrderType" Width="70" ></px:PXGridColumn>
					<px:PXGridColumn DataField="OrderNbr" Width="140" CommitChanges="True" ></px:PXGridColumn>
					<px:PXGridColumn DataField="OrderDate" Width="90" ></px:PXGridColumn></Columns></px:PXGridLevel></Levels></px:PXGrid></asp:Content>

