<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="TC103040.aspx.cs" Inherits="Page_TC103040" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
  <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="TCAddon.TCLabelSettingsSetupMaint"
        PrimaryView="TCLabelSettingsRecord"
        >
    <CallbackCommands>

    </CallbackCommands>
  </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
  <px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="TCLabelSettingsRecord" Width="100%" AllowAutoHide="false">
    <Template>
      <px:PXLayoutRule LabelsWidth="400" runat="server" StartRow="True" ></px:PXLayoutRule>
	<px:PXLayoutRule runat="server" ID="CstPXLayoutRule4" StartGroup="True" GroupCaption="General Settings" ></px:PXLayoutRule>
	<px:PXNumberEdit LabelWidth="300" Width="200" runat="server" ID="CstPXNumberEdit4" DataField="LabelPrintPageSize" ></px:PXNumberEdit>
	<px:PXLabel runat="server" ID="CstLabel2" Text="* Labels are split into batches of maximum 100 labels per PDF file." ></px:PXLabel>
	<px:PXCheckBox Text="Label Log Tracing" AlignLeft="True" TextAlign="Right" runat="server" LabelWidth="200" DataField="EnableLogLabelXml" ID="CstPXCheckBox1" Width="200" ></px:PXCheckBox>
	<px:PXNumberEdit runat="server" ID="CstPXNumberEdit5" LabelWidth="300" Width="200" DataField="KeepLogInDays" ></px:PXNumberEdit>
	<px:PXLayoutRule runat="server" ID="PXLayoutRule1" StartRow="True" ></px:PXLayoutRule>
	<px:PXLayoutRule runat="server" ID="CstPXLayoutRule63" StartGroup="True" GroupCaption="GS1-128" ></px:PXLayoutRule>
	<px:PXLabel runat="server" ID="NoteB" Text="* The combined entries for Company # and Next Serial # cannot exceed 16 characters." Size="XXL" Width="100%" ></px:PXLabel>
	<px:PXTextEdit runat="server" ID="edUCCNumber" DataField="UCCNumber" ></px:PXTextEdit>
	<px:PXTextEdit CommitChanges="True" runat="server" ID="edUCCExtension" DataField="UCCExtension" ></px:PXTextEdit>
	<px:PXTextEdit CommitChanges="True" runat="server" ID="edUCCCompany" DataField="UCCCompany" ></px:PXTextEdit>
	<px:PXTextEdit CommitChanges="True" runat="server" ID="edUCCNextSerialNo" DataField="UCCNextSerialNo" ></px:PXTextEdit>
	<px:PXCheckBox AlignLeft="True" TextAlign="Right" runat="server" DataField="UCCAutoCreate" ID="edUCCAutoCreate" ></px:PXCheckBox></Template>
    <AutoSize Container="Window" Enabled="True" MinHeight="200" ></AutoSize>
  </px:PXFormView></asp:Content>