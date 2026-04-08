<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="AL101000.aspx.cs" Inherits="Page_AL101000" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" PrimaryView="Setup" TypeName="AA.Objects.Labels.ALSetupMaint">
        <CallbackCommands>
            <px:PXDSCallbackCommand CommitChanges="True" Name="Save" />
            <px:PXDSCallbackCommand CommitChanges="True" Name="ValidateLabelary" Visible="false" />
            <px:PXDSCallbackCommand CommitChanges="True" Name="ValidateLabelZoom" Visible="false" />
            <px:PXDSCallbackCommand CommitChanges="True" Name="ValidatePrintNode" Visible="false" />
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXTab ID="tab" runat="server" DataSourceID="ds" Height="500px" Width="100%" DataMember="Setup">
        <Activity HighlightColor="" SelectedColor="" Width="" Height=""></Activity>
        <Items>
            <px:PXTabItem Text="Basic Settings">
                <Template>
                    <px:PXPanel ID="pnlRendering" runat="server" Caption="Labelary Integration" RenderStyle="Fieldset">
                        <px:PXLayoutRule runat="server" LabelsWidth="SM" ControlSize="XM" />
                        <px:PXLayoutRule runat="server" Merge="true" />
                        <px:PXLinkEdit ID="edLabelaryAPI" runat="server" DataField="LabelaryAPI" />
                        <px:PXButton ID="btnValidateLabelary" Width="100px" runat="server" Text="Validate">
                            <AutoCallBack Command="ValidateLabelary" Target="ds" />
                        </px:PXButton>
                        <px:PXLayoutRule runat="server" />
                        <px:PXTextEdit ID="edLabelaryAPIKey" runat="server" DataField="LabelaryAPIKey" />
                    </px:PXPanel>
                    <px:PXPanel runat="server" ID="pnlPrintNodeSettings" Caption="Cloud Print Integration" RenderStyle="Fieldset">
                        <px:PXLayoutRule runat="server" LabelsWidth="SM" ControlSize="XM" />
                        <px:PXLayoutRule runat="server" Merge="true" />
                        <px:PXLinkEdit runat="server" ID="edPrintNodeAPI" DataField="PrintNodeAPI" />
                        <px:PXButton ID="btnValidatePrintNode" Width="100px" runat="server" Text="Validate">
                            <AutoCallBack Command="ValidatePrintNode" Target="ds" />
                        </px:PXButton>
                        <px:PXLayoutRule runat="server" />
                        <px:PXTextEdit runat="server" ID="edPrintNodeAPIKey" DataField="PrintNodeAPIKey" />
                    </px:PXPanel>
                    <px:PXPanel ID="pnlLabelZoom" runat="server" Caption="LabelZoom Integration" RenderStyle="Fieldset">
                        <px:PXLayoutRule runat="server" LabelsWidth="SM" ControlSize="XM" />
                        <px:PXLayoutRule runat="server" Merge="true" />
                        <px:PXLinkEdit ID="edLabelZoomAPI" runat="server" DataField="LabelZoomAPI" />
                        <px:PXButton ID="btnValidateLabelZoom" Width="100px" runat="server" Text="Validate">
                            <AutoCallBack Command="ValidateLabelZoom" Target="ds" />
                        </px:PXButton>
                        <px:PXLayoutRule runat="server" />
                        <px:PXTextEdit ID="edLabelZoomAPIKey" runat="server" DataField="LabelZoomAPIKey" />
                        <px:PXSelector ID="edLabelZoomCategoryID" runat="server" DataField="LabelZoomCategoryID" AllowEdit="true" />
                        <px:PXSelector ID="edLabelZoomImageSubstitutionID" runat="server" DataField="LabelZoomImageSubstitutionID" AllowEdit="true" />
                    </px:PXPanel>
                    <px:PXPanel ID="pnlMongoDb" runat="server" Caption="Mongo Db Integration" RenderStyle="Fieldset">
                        <px:PXLayoutRule runat="server" LabelsWidth="SM" ControlSize="XM" />
                        <px:PXLayoutRule runat="server" Merge="true" />
                        <px:PXLinkEdit ID="edMongoURL" runat="server" DataField="MongoURL" />
                        <px:PXButton ID="btnValidateMongoDb" Width="100px" runat="server" Text="Validate">
                            <AutoCallBack Command="ValidateMongoDb" Target="ds" />
                        </px:PXButton>
                        <px:PXLayoutRule runat="server" />
                        <px:PXTextEdit ID="edMongoOptions" runat="server" DataField="MongoOptions" />
                    </px:PXPanel>
                    <px:PXPanel ID="pnlModelSettings" runat="server" Caption="Model Settings" RenderStyle="Fieldset">
                        <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" ControlSize="XL" />
                        <%--<px:PXSelector ID="edRenderingPrinterID" runat="server" DataField="RenderingPrinterID" AllowEdit="true" />--%>
                        <px:PXSelector ID="edZplGraphicCreator" runat="server" DataField="ZplGraphicCreator" AutoComplete="true" DisplayMode="Text" />
                        <px:PXDropDown ID="edDefaultLanguage" runat="server" DataField="DefaultLanguage" />
                        <px:PXSelector ID="edDefaultFormatID" runat="server" DataField="DefaultFormatID" AllowEdit="true" />
                        <px:PXSelector ID="edDefaultMarginID" runat="server" DataField="DefaultMarginID" AllowEdit="true" />
                        <px:PXSelector ID="edDefaultCategoryID" runat="server" DataField="DefaultCategoryID" AllowEdit="true" />
                    </px:PXPanel>
                    <px:PXLayoutRule runat="server" StartColumn="True" />
                    <px:PXPanel runat="server" ID="pnlLabelIntegrations" Caption="Label Integrations" RenderStyle="Fieldset">
                        <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" ControlSize="XXL" />
                        <px:PXDropDown runat="server" ID="edEnableIntegration" DataField="EnableIntegration" CommitChanges="True" AllowMultiSelect="true" />
                    </px:PXPanel>
                    <px:PXPanel runat="server" ID="pnlPrinterOverride" Caption="Printer Override" RenderStyle="Fieldset">
                        <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" ControlSize="XXL" />
                        <px:PXDropDown runat="server" ID="edEnablePrinterOverride" DataField="EnablePrinterOverride" CommitChanges="True" AllowMultiSelect="true" />
                    </px:PXPanel>
                    <px:PXPanel runat="server" ID="pnlNbCopies" Caption="Nb Copies Settings" RenderStyle="Fieldset">
                        <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" ControlSize="XXL" />
                        <px:PXDropDown runat="server" ID="edEnableCopiesOverride" DataField="EnableCopiesOverride" CommitChanges="True" AllowMultiSelect="true" />
                    </px:PXPanel>
                    <px:PXPanel runat="server" ID="pnlOtherIntegrations" Caption="Other Options" RenderStyle="Fieldset">
                        <px:PXLayoutRule runat="server" LabelsWidth="SM" />
                        <px:PXNumberEdit ID="edNbDaysToKeep" runat="server" DataField="NbDaysToKeep" />
                        <px:PXDropDown ID="edDevMode" runat="server" DataField="DevMode" CommitChanges="true" AllowMultiSelect="true" />
                        <px:PXDropDown ID="edRecordImportMode" runat="server" DataField="RecordImportMode" Width="150" />
                    </px:PXPanel>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="Renderers">
                <Template>
                    <px:PXGrid ID="gridRenderers" runat="server" DataSourceID="ds"
                        Width="100%" SkinID="Details">
                        <Levels>
                            <px:PXGridLevel DataMember="Renderers">
                                <Columns>
                                    <px:PXGridColumn DataField="Active" Type="CheckBox" Width="60px" TextAlign="Center" />
                                    <px:PXGridColumn DataField="FromContent" Width="150px" />
                                    <px:PXGridColumn DataField="ToContent" Width="150px" />
                                    <px:PXGridColumn DataField="RenderingPrinterID" Width="200px" />
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <AutoSize Enabled="True" Container="Parent" MinHeight="200" />
                        <ActionBar>
                            <Actions>
                                <AddNew ToolBarVisible="Top" />
                                <Delete ToolBarVisible="Top" />
                            </Actions>
                        </ActionBar>
                        <Mode AllowAddNew="True" AllowDelete="True" AllowUpdate="True" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
        </Items>
        <AutoSize MinHeight="480" Container="Window" Enabled="True" />
    </px:PXTab>
</asp:Content>
