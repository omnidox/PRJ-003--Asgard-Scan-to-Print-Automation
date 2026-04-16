<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="AL101000.aspx.cs" Inherits="Page_AL101000" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" PrimaryView="Setup" TypeName="AA.Objects.AL.ALSetupMaint">
        <CallbackCommands>
            <px:PXDSCallbackCommand CommitChanges="True" Name="Save" />
            <px:PXDSCallbackCommand CommitChanges="True" Name="ValidateLabelary" Visible="false"/>
            <px:PXDSCallbackCommand CommitChanges="True" Name="ValidateLabelZoom" Visible="false"/>
            <px:PXDSCallbackCommand CommitChanges="True" Name="ValidatePrintNode" Visible="false"/>
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXTab ID="tab" runat="server" DataSourceID="ds" Height="500px" Width="100%" DataMember="Setup">
        <Activity HighlightColor="" SelectedColor="" Width="" Height=""></Activity>
        <Items>
            <px:PXTabItem Text="Basic Settings">
                <Template>
                    <%--<px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="XM" ControlSize="XM"/>--%>
                    <px:PXPanel ID="pnlRendering" runat="server" Caption="Labelary Integration" RenderStyle="Fieldset">
                        <px:PXLayoutRule runat="server" LabelsWidth="M" ControlSize="XM"/>
                        <px:PXLayoutRule runat="server" Merge="true" />
                        <px:PXLinkEdit ID="edLabelaryAPI" runat="server" DataField="LabelaryAPI"/>
                        <px:PXButton ID="btnValidateLabelary" Width="100px" runat="server" Text="Validate"> 
                            <AutoCallBack Command="ValidateLabelary" Target="ds" /> 
                        </px:PXButton> 
                        <px:PXLayoutRule runat="server" /> 
                        <px:PXTextEdit ID="edLabelaryAPIKey" runat="server" DataField="LabelaryAPIKey"/>
                    </px:PXPanel>
                    <px:PXPanel runat="server" ID="pnlPrintNodeSettings" Caption="Cloud Print Integration" RenderStyle="Fieldset">
                        <px:PXLayoutRule runat="server" LabelsWidth="M" ControlSize="XM" />
                        <px:PXLayoutRule runat="server" Merge="true" /> 
                        <px:PXLinkEdit runat="server" ID="edPrintNodeAPI" DataField="PrintNodeAPI" />
                        <px:PXButton ID="btnValidatePrintNode" Width="100px" runat="server" Text="Validate"> 
                            <AutoCallBack Command="ValidatePrintNode" Target="ds" /> 
                        </px:PXButton> 
                        <px:PXLayoutRule runat="server" /> 
                        <px:PXTextEdit runat="server" ID="edPrintNodeAPIKey" DataField="PrintNodeAPIKey" />
                    </px:PXPanel>
<%--                    <px:PXPanel ID="pnlLabelZoom" runat="server" Caption="LabelZoom Integration" RenderStyle="Fieldset">
                        <px:PXLayoutRule runat="server" LabelsWidth="M" ControlSize="XM"/>
                        <px:PXLayoutRule runat="server" Merge="true" /> 
                        <px:PXLinkEdit ID="edLabelZoomAPI" runat="server" DataField="LabelZoomAPI"/>
                        <px:PXButton ID="btnValidateLabelZoom" Width="100px" runat="server" Text="Validate"> 
                            <AutoCallBack Command="ValidateLabelZoom" Target="ds" /> 
                        </px:PXButton> 
                        <px:PXLayoutRule runat="server" /> 
                        <px:PXTextEdit ID="edLabelZoomAPIKey" runat="server" DataField="LabelZoomAPIKey"/>
                    </px:PXPanel>--%>
                    <%--<px:PXLayoutRule runat="server" LabelsWidth="XM" ControlSize="XM"/>--%>
                    <px:PXPanel ID="pnlModelSettings" runat="server" Caption="Model Settings" RenderStyle="Fieldset">
                        <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="XM" ControlSize="XM"/>
                        <px:PXSelector ID="edRenderingPrinterID" runat="server" DataField="RenderingPrinterID" AllowEdit="true"/>
                        <px:PXSelector ID="edZplGraphicCreator" runat="server" DataField="ZplGraphicCreator" DisplayMode="Text"/>
                        <px:PXDropDown ID="edDefaultLanguage" runat="server" DataField="DefaultLanguage"/>
                        <px:PXSelector ID="edDefaultFormatID" runat="server" DataField="DefaultFormatID" AllowEdit="true"/>
                        <px:PXSelector ID="edDefaultMarginID" runat="server" DataField="DefaultMarginID" AllowEdit="true"/>
                        <px:PXSelector ID="edDefaultCategoryID" runat="server" DataField="DefaultCategoryID" AllowEdit="true"/>
                        <px:PXNumberEdit ID="edNbDaysToKeep" runat="server" DataField="NbDaysToKeep"/>
                        <px:PXDropDown ID="edDevMode" runat="server" DataField="DevMode" CommitChanges="true" AllowMultiSelect="true"/>
                        <px:PXDropDown ID="edRecordImportMode" runat="server" DataField="RecordImportMode"/>
                    </px:PXPanel>
                    <px:PXLayoutRule runat="server" StartColumn="True"/>
<%--                    <px:PXPanel runat="server" ID="pnlShipment2DSettings" Caption="Shipment 2D Settings" RenderStyle="Fieldset">
                        <px:PXLayoutRule runat="server" LabelsWidth="XM" ControlSize="XM" />
                        <px:PXCheckBox runat="server" ID="chkGenerateShipment2D" DataField="GenerateShipment2D" CommitChanges="True" />
                        <px:PXSelector runat="server" ID="edShipment2DModelID" DataField="Shipment2DModelID" CommitChanges="True" AllowEdit="true"/>
                        <px:PXSelector runat="server" ID="edPackingListReportID" DataField="PackingListReportID" CommitChanges="True" AllowEdit="true"/>
                    </px:PXPanel>--%>
<%--                    <px:PXPanel runat="server" ID="pnlCarrierLabelsViaCloud" Caption="Carrier Labels Via Cloud" RenderStyle="Fieldset">
                        <px:PXLayoutRule runat="server" LabelsWidth="XM" ControlSize="XM" />
                        <px:PXCheckBox runat="server" ID="chkCarrierLabelsViaCloud" DataField="PrintCarrierLabelsViaCloud" CommitChanges="True" />
                        <px:PXSelector runat="server" ID="edCarrierLabelsModelID" DataField="PrintCarrierLabelsModelID" CommitChanges="True" AllowEdit="true"/>
                    </px:PXPanel>
                    <px:PXPanel runat="server" ID="pnlProductionTicketLabelsViaCloud" Caption="Production Ticket Via Cloud" RenderStyle="Fieldset">
                        <px:PXLayoutRule runat="server" LabelsWidth="XM" ControlSize="XM" />
                        <px:PXCheckBox runat="server" ID="chkPrintProductionTicketViaCloud" DataField="PrintProductionTicketViaCloud" CommitChanges="True" />
                        <px:PXSelector runat="server" ID="edPrintProductionTicketModelID" DataField="PrintProductionTicketModelID" CommitChanges="True" AllowEdit="true"/>
                        <px:PXSelector runat="server" ID="edPrintProductionTicketRuleID" DataField="PrintProductionTicketRuleID" CommitChanges="True" AllowEdit="true"/>
                    </px:PXPanel>--%>
                   <px:PXPanel runat="server" ID="pnlPrinterOverride" Caption="Printer Settings" RenderStyle="Fieldset">
                        <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="XM" ControlSize="XM" />
                        <px:PXDropDown runat="server" ID="edEnablePrinterOverride" DataField="EnablePrinterOverride" CommitChanges="True" AllowMultiSelect="true"/>
<%--                        <px:PXCheckBox runat="server" ID="chkEnablePrinterOverride" DataField="EnablePrinterOverride" CommitChanges="True" />
                        <px:PXCheckBox runat="server" ID="chkPrinterOverrideAM" DataField="PrinterOverrideAM" CommitChanges="True" />--%>
                    </px:PXPanel>
<px:PXPanel runat="server" ID="pnlNbCopies" Caption="Nb Copies Settings" RenderStyle="Fieldset">
                        <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="XM" ControlSize="XM" />
                        <px:PXDropDown runat="server" ID="edEnableCopiesOverride" DataField="EnableCopiesOverride" CommitChanges="True" AllowMultiSelect="true"/>
                    </px:PXPanel>
                    <px:PXPanel runat="server" ID="pnlOtherIntegrations" Caption="Other Integrations" RenderStyle="Fieldset">
                        <px:PXLayoutRule runat="server" LabelsWidth="XM" ControlSize="XM" />
                        <px:PXCheckBox runat="server" ID="chkBoxPrint" DataField="BoxPrint" CommitChanges="True" />
                        <px:PXSelector runat="server" ID="edBoxPrintModelID" DataField="BoxPrintModelID" CommitChanges="True" AllowEdit="true"/>
                        <px:PXCheckBox runat="server" ID="chkOwnShipment" DataField="OwnShipment" CommitChanges="True" />
                        <px:PXCheckBox runat="server" ID="chkPrintOnConfirm" DataField="PrintOnConfirm" CommitChanges="True" />
                        <px:PXSelector runat="server" ID="edPrintOnConfirmModelID" DataField="PrintOnConfirmModelID" CommitChanges="True" AllowEdit="true"/>
                        <px:PXCheckBox runat="server" ID="chkFixPackageLineNbr" DataField="FixPackageLineNbr" CommitChanges="true"/>
                    </px:PXPanel>
                </Template>
            </px:PXTabItem>
        </Items>
        <AutoSize MinHeight="480" Container="Window" Enabled="True" />
    </px:PXTab>
</asp:Content>