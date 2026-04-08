<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="AL203010.aspx.cs" Inherits="Page_AL203010" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server" >
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" TypeName="AA.Objects.Labels.ALPrintLogMaint" 
        PrimaryView="Document" PageLoadBehavior="SearchSavedKeys">
		<CallbackCommands>
            <px:PXDSCallbackCommand Visible="true" Name="ReprintToOther" CommitChanges="true" />
            <px:PXDSCallbackCommand Visible="false" Name="ViewPrintJob"/>
		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
	<px:PXFormView ID="form" runat="server" DataSourceID="ds" Style="z-index: 100" Width="100%" DataMember="Document" TabIndex="3900">
		<Template>
			<px:PXLayoutRule runat="server" StartRow="True" StartColumn="True" LabelsWidth="S" ControlSize="XM"/>
		    <px:PXSelector ID="edRecordID" runat="server" DataField="RecordID" />
            <px:PXSelector ID="edUserID" runat="server" DataField="UserID" AllowEdit="true" DisplayMode="Hint"/>
            <px:PXSelector ID="edOwnerID" runat="server" DataField="OwnerID" AllowEdit="true" DisplayMode="Hint"/>
            <px:PXSelector ID="edScreenID" runat="server" DataField="ScreenID" DisplayMode="Hint"/>
            <px:PXSelector ID="edModelID" runat="server" DataField="ModelID" AllowEdit="true" DisplayMode="Hint"/>
            <px:PXSelector ID="edModelFormatID" runat="server" DataField="ModelFormatID" AllowEdit="true"/>
            <px:PXSelector ID="edModelMarginID" runat="server" DataField="ModelMarginID" AllowEdit="true"/>
            <px:PXSelector ID="edGraphType" runat="server" DataField="GraphType" DisplayMode="Hint"/>
            <px:PXSelector ID="edBasedOnView" runat="server" DataField="BasedOnView" DisplayMode="Hint"/>
            <px:PXSelector ID="edEntityType" runat="server" DataField="EntityType" DisplayMode="Hint"/>
            <px:PXTextEdit ID="edLabelFilename" runat="server"  DataField="LabelFilename"/>
            <px:PXTextEdit ID="edLabelKey" runat="server" DataField="LabelKey" />
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" ControlSize="M"/>
            <px:PXSelector ID="edBAccountID" runat="server" DataField="BAccountID" AllowEdit="true" DisplayMode="Hint"/>
            <px:PXSelector ID="edInventoryID" runat="server" DataField="InventoryID" AllowEdit="true" DisplayMode="Hint"/>
            <px:PXSelector ID="edLotSerialNbr" runat="server" DataField="LotSerialNbr" DisplayMode="Hint"/>
            <px:PXSelector ID="edPrintStationID" runat="server" DataField="PrintStationID" AllowEdit="true" DisplayMode="Hint"/>
            <px:PXSelector ID="edPrinterID" runat="server" DataField="PrinterID" AllowEdit="true" DisplayMode="Hint"/>
            <px:PXSelector ID="edPrinterFormatID" runat="server" DataField="PrinterFormatID" AllowEdit="true" DisplayMode="Hint"/>
            <px:PXSelector ID="edPrinterMarginID" runat="server" DataField="PrinterMarginID" AllowEdit="true" DisplayMode="Hint"/>
            <px:PXDropDown ID="edContentType" runat="server" DataField="ContentType"/>
            <px:PXNumberEdit ID="edNbCopies" runat="server" DataField="NbCopies" />
            <px:PXDateTimeEdit ID="edCreatedDateTime" runat="server" DataField="CreatedDateTime" Width="250px" />
            <px:PXDateTimeEdit ID="edLastModifiedDateTime" runat="server" DataField="LastModifiedDateTime" Width="250px" />
            <px:PXTextEdit ID="edPrintJobID" runat="server" DataField="PrintJobID">
                <LinkCommand Target="ds" Command="ViewPrintJob" />
            </px:PXTextEdit>
            <px:PXLayoutRule runat="server" StartColumn="True"/>
            <px:PXFormView ID="templateDataForm" runat="server" DataMember="CurrentDocument" DataSourceID="ds" SkinID="Transparent">
                <Template>
                    <px:PXImageView ID="edImageUrl" runat="server" DataField="ImageUrl" Width="20%" Height="20%" AlreadyLocalized="True" CallbackUpdatable="True" />
                </Template>
            </px:PXFormView>
		</Template>
		<AutoSize Container="Window" Enabled="True" MinHeight="200" />
	</px:PXFormView>
    <px:PXSmartPanel ID="pnlChangePrinterID" runat="server" Caption="Specify New Printer" CaptionVisible="true" DesignView="Hidden" LoadOnDemand="true"
      Key="Filter" CreateOnDemand="false" AutoCallBack-Enabled="true" AcceptButtonID="btnOK"
      AutoCallBack-Target="formChangePrinterID" AutoCallBack-Command="Refresh" CallBackMode-CommitChanges="True" CallBackMode-PostData="Page">
      <px:PXFormView ID="formChangePrinterID" runat="server" DataSourceID="ds" Style="z-index: 100" Width="100%" CaptionVisible="False" DataMember="Filter">
        <ContentStyle BackColor="Transparent" BorderStyle="None" ></ContentStyle>
        <Template>
          <px:PXLayoutRule ID="rlAcctCD" runat="server" StartColumn="True" LabelsWidth="S" ControlSize="XM" ></px:PXLayoutRule>
            <px:PXSelector ID="edPrinterID2" runat="server" DataField="PrinterID" Width="350px" AutoRefresh="true" CommitChanges="true"/>
        </Template>
      </px:PXFormView>
      <px:PXPanel ID="pnlChangePrinterIDButton" runat="server" SkinID="Buttons">
        <px:PXButton ID="btnOK" runat="server" DialogResult="OK" Text="OK">
          <AutoCallBack Target="formChangePrinterID" Command="Save" ></AutoCallBack>
        </px:PXButton>
        <px:PXButton ID="PXButton3" runat="server" DialogResult="Cancel" Text="Cancel" ></px:PXButton>
      </px:PXPanel>
    </px:PXSmartPanel>	
</asp:Content>
