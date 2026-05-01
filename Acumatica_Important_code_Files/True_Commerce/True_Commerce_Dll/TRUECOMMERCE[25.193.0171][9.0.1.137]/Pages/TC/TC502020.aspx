<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="TC502020.aspx.cs" Inherits="Page_TC502020" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="TCAddon.TCProcessShipments"
        PrimaryView="TransactionStatus"
        >
		<CallbackCommands>

		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
	<px:PXFormView SyncPosition="True" Caption="Process Shipment" CaptionVisible="False" RenderStyle="Normal" TabIndex="100" ID="form" runat="server" DataSourceID="ds" DataMember="TransactionStatus" Width="100%" Height="" AllowAutoHide="false">
		<Template>
			<px:PXLayoutRule ID="PXLayoutRule1" runat="server" StartRow="True"></px:PXLayoutRule>
			<px:PXLayoutRule LabelsWidth="" runat="server" ID="CstPXLayoutRule6" StartColumn="True" ></px:PXLayoutRule>
			<px:PXLayoutRule runat="server" ID="CstLayoutRule1" LabelsWidth="SM" ControlSize="SM" ></px:PXLayoutRule>
			<px:PXSelector CommitChanges="True" Size="SM" SkinID="" runat="server" ID="CstPXSelector3" DataField="OrderNbrFrom" ></px:PXSelector>
			<px:PXDateTimeEdit CommitChanges="True" Size="SM" SkinID="" runat="server" ID="CstPXDateTimeEdit2" DataField="DateFrom" ></px:PXDateTimeEdit>
			<px:PXLayoutRule LabelsWidth="30" runat="server" ID="CstPXLayoutRule7" StartColumn="True" ></px:PXLayoutRule>
			<px:PXLayoutRule runat="server" ID="CstLayoutRule2" LabelsWidth="XS" ControlSize="XS" />
			<px:PXSelector CommitChanges="True" Size="SM" SkinID="" runat="server" ID="CstPXSelector5" DataField="OrderNbrTo" ></px:PXSelector>
			<px:PXDateTimeEdit CommitChanges="True" Size="SM" SkinID="" runat="server" ID="CstPXDateTimeEdit4" DataField="DateTo" ></px:PXDateTimeEdit></Template>
	
		<AutoSize Enabled="True" MinHeight="100" Container="Parent" ></AutoSize>
		<AutoSize Enabled="True" ></AutoSize>
		<AutoSize MinHeight="100" ></AutoSize></px:PXFormView>
	<px:PXGrid AutoAdjustColumns="True" runat="server" ID="CstPXGrid30" SyncPosition="True" Height="500px" SkinID="PrimaryInquire" TabIndex="1100" Width="100%" Caption="Orders" DataSourceID="ds" AllowPaging="True" AdjustPageSize="Auto" PageSize="20" NoteIndicator="False" FilesIndicator="False">
		<Levels>
			<px:PXGridLevel DataMember="Orders" >
				<Columns>
					<px:PXGridColumn Type="CheckBox" CommitChanges="False" DataField="Selected" Width="60" ></px:PXGridColumn>
					<px:PXGridColumn CommitChanges="True" DataField="ShipmentNbr" Width="140" ></px:PXGridColumn>
					<px:PXGridColumn DataField="ShipmentType" Width="70" ></px:PXGridColumn>
					<px:PXGridColumn DataField="ShipDate" Width="90" ></px:PXGridColumn></Columns></px:PXGridLevel></Levels></px:PXGrid></asp:Content>