<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="TC502040.aspx.cs" Inherits="Page_TC502040" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
  <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="TCAddon.TCProcessPurchaseOrders"
        PrimaryView="TransactionStatus"
        >
    <CallbackCommands>

    </CallbackCommands>
  </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
  <px:PXFormView SyncPosition="True" TabIndex="100" RenderStyle="Normal" CaptionVisible="False" Caption="Process Purchase Orders" ID="form" runat="server" DataSourceID="ds" DataMember="TransactionStatus" Width="100%" AllowAutoHide="false">
    <Template>
      <px:PXLayoutRule ID="PXLayoutRule1" runat="server" StartRow="True"></px:PXLayoutRule>
      <px:PXLayoutRule runat="server" ID="CstPXLayoutRule1" StartColumn="True" LabelsWidth="" ></px:PXLayoutRule>
      <px:PXLayoutRule runat="server" ID="CstLayoutRule1" ControlSize="SM" LabelsWidth="SM" ></px:PXLayoutRule>
      <px:PXSelector CommitChanges="True" runat="server" ID="CstPXSelector4" DataField="OrderNbrFrom" Size="SM" ></px:PXSelector>
      <px:PXDateTimeEdit CommitChanges="True" runat="server" ID="CstPXDateTimeEdit3" DataField="DateFrom" Size="SM" ></px:PXDateTimeEdit>
      <px:PXLayoutRule runat="server" ID="CstPXLayoutRule2" StartColumn="True" LabelsWidth="30" ></px:PXLayoutRule>
      <px:PXLayoutRule runat="server" ID="CstLayoutRule2" ControlSize="SM" LabelsWidth="SM" ></px:PXLayoutRule>
      <px:PXSelector CommitChanges="True" runat="server" ID="CstPXSelector6" DataField="OrderNbrTo" Size="SM" ></px:PXSelector>
      <px:PXDateTimeEdit CommitChanges="True" runat="server" ID="CstPXDateTimeEdit5" DataField="DateTo" Size="SM" ></px:PXDateTimeEdit>
      <px:PXLayoutRule runat="server" ID="CstPXLayoutRule12" StartColumn="True" />
      <px:PXLayoutRule runat="server" ID="CstLayoutRule14" />
      <px:PXSelector CommitChanges="True" runat="server" ID="CstPXSelector18" DataField="VendorID" Size="SM" ></px:PXSelector>
      <px:PXDropDown Size="SM" runat="server" ID="CstPXDropDown17" DataField="POStatus" CommitChanges="True" ></px:PXDropDown>
      <px:PXLayoutRule runat="server" ID="CstPXLayoutRule13" StartColumn="True" />
      <px:PXLayoutRule runat="server" ID="CstLayoutRule15" />
      <px:PXCheckBox CommitChanges="True" runat="server" ID="CstPXCheckBox16" DataField="IsUpdateTransactionStatus" ></px:PXCheckBox></Template>
    <AutoSize Container="Parent" Enabled="True" MinHeight="100" ></AutoSize>
  </px:PXFormView>
  <px:PXGrid AutoAdjustColumns="True" SkinID="PrimaryInquire" FilesIndicator="False" NoteIndicator="False" Height="500" runat="server" ID="CstPXGrid7" AdjustPageSize="Auto" AllowPaging="True" Width="100%" AllowFilter="True" AllowSearch="True" DataSourceID="ds" PageSize="20" TabIndex="1100" SyncPosition="True">
    <Levels>
      <px:PXGridLevel DataMember="Orders" >
        <Columns>
          <px:PXGridColumn Type="CheckBox" DataField="Selected" Width="60" ></px:PXGridColumn>
          <px:PXGridColumn DataField="OrderType" Width="70" ></px:PXGridColumn>
          <px:PXGridColumn DataField="OrderNbr" Width="140" ></px:PXGridColumn>
          <px:PXGridColumn DataField="OrderDate" Width="90" ></px:PXGridColumn></Columns></px:PXGridLevel></Levels></px:PXGrid></asp:Content>