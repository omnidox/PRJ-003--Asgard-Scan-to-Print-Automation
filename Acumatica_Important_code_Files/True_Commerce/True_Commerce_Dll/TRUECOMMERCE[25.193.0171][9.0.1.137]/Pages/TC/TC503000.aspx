<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="TC503000.aspx.cs" Inherits="Page_TC503000" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
  <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="TCAddon.TCProcessBatchLabelPrinting"
        PrimaryView="Filter"
        >
    <CallbackCommands>

    </CallbackCommands>
  </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
  <px:PXFormView Caption="" Height="" RenderStyle="Normal" ID="form" runat="server" DataSourceID="ds" DataMember="Filter" Width="100%" AllowAutoHide="false">
    <Template>
	<px:PXLayoutRule runat="server" ID="CstPXLayoutRule16" StartRow="True" ></px:PXLayoutRule>
	<px:PXLayoutRule runat="server" ID="CstPXLayoutRule18" StartColumn="True" ></px:PXLayoutRule>
	<px:PXFormView RenderStyle="Fieldset" Caption="Filters" runat="server" ID="formFilter" DataMember="Filter">
		<Template>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule20" StartRow="True" ></px:PXLayoutRule>
			<px:PXLayoutRule ControlSize="XS" LabelsWidth="SM" runat="server" ID="CstPXLayoutRule21" StartColumn="True" ></px:PXLayoutRule>
			<px:PXDateTimeEdit Size="SM" CommitChanges="True" runat="server" ID="CstPXDateTimeEdit24" DataField="DateFrom" ></px:PXDateTimeEdit>
			<px:PXSelector AutoRefresh="True" Size="SM" CommitChanges="True" runat="server" ID="CstPXSelector30" DataField="ShipmentNbrFrom" ></px:PXSelector>
			<px:PXDropDown CommitChanges="True" Size="SM" runat="server" ID="CstPXDropDown25" DataField="DocStatus" ></px:PXDropDown>
			<px:PXTextEdit CommitChanges="True" Size="SM" runat="server" ID="CstPXTextEdit29" DataField="PurchaseOrderNumber" ></px:PXTextEdit>
			<px:PXTextEdit CommitChanges="True" Size="SM" runat="server" ID="CstPXTextEdit26" DataField="EDIDistributionCenter" ></px:PXTextEdit>
			<px:PXSegmentMask AutoRefresh="True" CommitChanges="True" Size="SM" runat="server" ID="CstPXSegmentMask23" DataField="Customer" ></px:PXSegmentMask>
			<px:PXSelector CommitChanges="True" Size="SM" AutoRefresh="True" runat="server" ID="CstPXSelector27" DataField="LabelPartner" ></px:PXSelector>
			<px:PXDropDown CommitChanges="True" Size="SM" runat="server" ID="CstPXDropDown28" DataField="Level" ></px:PXDropDown>
			<px:PXLayoutRule ControlSize="XS" LabelsWidth="SM" runat="server" ID="CstPXLayoutRule22" StartColumn="True" ></px:PXLayoutRule>
			<px:PXDateTimeEdit CommitChanges="True" Size="SM" runat="server" ID="CstPXDateTimeEdit31" DataField="DateTo" ></px:PXDateTimeEdit>
			<px:PXSelector Size="SM" CommitChanges="True" AutoRefresh="True" runat="server" ID="CstPXSelector32" DataField="ShipmentNbrTo" ></px:PXSelector></Template></px:PXFormView>
	<px:PXLayoutRule ColumnWidth="100%" runat="server" ID="CstPXLayoutRule7" StartRow="True" ></px:PXLayoutRule>
	<px:PXLayoutRule ColumnWidth="100%" LabelsWidth="" runat="server" ID="CstPXLayoutRule8" StartColumn="True" ControlSize="" ></px:PXLayoutRule>
	<px:PXFormView Width="100%" Caption="Options" RenderStyle="Fieldset" DataMember="Filter" runat="server" ID="formOpt" >
		<Template>
			<px:PXLayoutRule runat="server" ID="CstLayoutRule3" LabelsWidth="SM" ControlSize="XL" ></px:PXLayoutRule>
			<px:PXLabel Size="XXXL" runat="server" ID="CstLabel4" Text="*If you select Label Template or ASN Type, the selected value will be updated to the shipments once the labels are printed successfully." ></px:PXLabel>
			<px:PXSelector AutoRefresh="True" CommitChanges="True" runat="server" ID="CstPXSelector10" DataField="LabelTemplate" Size="" ></px:PXSelector>
			<px:PXDropDown runat="server" ID="CstPXDropDown1" DataField="ASNType" CommitChanges="True" Size="" ></px:PXDropDown>
			<px:PXCheckBox runat="server" ID="CstPXCheckBox2" DataField="ValidateRequiredField" CommitChanges="True" Size="" ></px:PXCheckBox>
			<px:PXSelector CommitChanges="True" runat="server" ID="CstPXSelector2" DataField="PrintDestination" ></px:PXSelector></Template></px:PXFormView></Template>
    <AutoSize Container="Parent" Enabled="False" MinHeight="360" ></AutoSize>
  </px:PXFormView>
  <px:PXGrid AutoAdjustColumns="True" runat="server" ID="CstPXGrid12" AdjustPageSize="Auto" AllowPaging="True" BatchUpdate="True" Caption="Shipment Packages" Height="400" SkinID="PrimaryInquire" Width="100%" DataSourceID="ds" FilesIndicator="False" NoteIndicator="False" SyncPosition="True" TabIndex="1100">
    <Levels>
      <px:PXGridLevel DataMember="Packs" >
        <Columns>
          <px:PXGridColumn AllowSort="False" CommitChanges="False" Type="CheckBox" AllowCheckAll="True" AllowFilter="False" AllowMove="False" TextAlign="Center" DataField="Selected" Width="60" ></px:PXGridColumn>
          <px:PXGridColumn DataField="ShipmentNbr" Width="70" ></px:PXGridColumn>
	<px:PXGridColumn DataField="Customer" Width="70" ></px:PXGridColumn>
	<px:PXGridColumn DataField="LabelPartner" Width="70" ></px:PXGridColumn>
          <px:PXGridColumn DataField="UCC128" Width="70" ></px:PXGridColumn>
          <px:PXGridColumn DataField="Level" Width="70" ></px:PXGridColumn>
          <px:PXGridColumn DataField="Location" Width="70" ></px:PXGridColumn>
	<px:PXGridColumn DataField="PONbr" Width="70" ></px:PXGridColumn>
	<px:PXGridColumn DataField="DCCode" Width="70" ></px:PXGridColumn>
	<px:PXGridColumn DataField="StoreNumber" Width="70" ></px:PXGridColumn>
	<px:PXGridColumn DataField="ShipDate" Width="90" ></px:PXGridColumn>
	<px:PXGridColumn DataField="PrintStatus" Width="140" ></px:PXGridColumn>
	<px:PXGridColumn DisplayFormat="MM/dd/yyyy hh:mm tt" DataField="PrintDate" Width="200" ></px:PXGridColumn></Columns>
        <RowTemplate>
          <px:PXLayoutRule runat="server" ID="CstPXLayoutRule13" Merge="True" ></px:PXLayoutRule>
          <px:PXCheckBox runat="server" ID="edSelected" DataField="Selected" AlignLeft="False" ></px:PXCheckBox>
          <px:PXTextEdit runat="server" ID="CstPXTextEdit15" DataField="ShipmentNbr" ></px:PXTextEdit>
          <px:PXTextEdit runat="server" ID="CstPXTextEdit16" DataField="UCC128" ></px:PXTextEdit>
          <px:PXTextEdit runat="server" ID="CstPXTextEdit14" DataField="Level" ></px:PXTextEdit></RowTemplate></px:PXGridLevel></Levels>
	<AutoSize Container="Parent" Enabled="True" MinHeight="400" ></AutoSize></px:PXGrid></asp:Content>