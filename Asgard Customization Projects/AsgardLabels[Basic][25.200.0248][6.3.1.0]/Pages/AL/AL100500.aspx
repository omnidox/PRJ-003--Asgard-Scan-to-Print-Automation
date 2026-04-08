<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="AL100500.aspx.cs" Inherits="Page_AL100500" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" PrimaryView="License" TypeName="AA.Objects.License.ALLicenseMaint">
        <CallbackCommands>
            <px:PXDSCallbackCommand CommitChanges="True" Name="Save" />
            <px:PXDSCallbackCommand CommitChanges="True" Name="ValidateLabelary" Visible="false" />
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXTab ID="tab" runat="server" DataSourceID="ds" Height="500px" Width="100%" DataMember="License">
        <%--<Activity HighlightColor="" SelectedColor="" Width="" Height=""></Activity>--%>
        <Items>
            <px:PXTabItem Text="License Settings">
                <Template>
                    <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="XM" ControlSize="M" />
                    <px:PXPanel ID="pnlAPI" runat="server" Caption="API" RenderStyle="Fieldset">
                        <px:PXLayoutRule runat="server" Merge="true" />
                        <%--<px:PXLinkEdit ID="edBaseUrl" runat="server" DataField="BaseUrl" />--%>
                        <%--<px:PXButton ID="btnValidateSpring" Width="100px" runat="server" Text="Validate">
                            <AutoCallBack Command="ValidateSpring" Target="ds" />
                        </px:PXButton>--%>
                        <px:PXLayoutRule runat="server" />
                        <px:PXTextEdit runat="server" ID="edApiKey" DataField="ApiKey" />
                        <px:PXTextEdit runat="server" ID="edSharedKey" DataField="SharedKey" />
                    </px:PXPanel>
                    <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="XM" ControlSize="XXL" />
                    <px:PXPanel ID="pnlInfo" runat="server" Caption="Info" RenderStyle="Fieldset">
                        <px:PXNumberEdit runat="server" ID="edCurrentCompany" DataField="CurrentCompany" />
                        <px:PXTextEdit runat="server" ID="edDatabaseID" DataField="DatabaseID" />
                        <px:PXTextEdit runat="server" ID="edLicenseManager" DataField="LicenseManager" Width="300px"/>
                        <px:PXTextEdit runat="server" ID="edDatabaseName" DataField="DatabaseName" Width="300px" />
                        <px:PXTextEdit runat="server" ID="edFullServerName" DataField="FullServerName" Width="300px" />
                        <px:PXTextEdit runat="server" ID="edHostName" DataField="HostName" Width="300px" />
                        <px:PXTextEdit runat="server" ID="edInstanceID" DataField="InstanceID" Width="300px" />
                        <px:PXTextEdit runat="server" ID="edIPAddress" DataField="IPAddress" Width="300px" />
                        <px:PXTextEdit runat="server" ID="edInstallationID" DataField="InstallationID" Width="400px" />
                        <px:PXDateTimeEdit runat="server" ID="edInstallationDate" DataField="InstallationDate" Width="300px" />
                        <px:PXTextEdit runat="server" ID="edPrinterInfo" DataField="PrinterInfo" TextMode="MultiLine" Height="400px" Width="400px" />
                    </px:PXPanel>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="License Products">
                <Template>
                    <px:PXSplitContainer runat="server" ID="sp1" SplitterPosition="200"
                        SkinID="Horizontal" Height="500px" SavePosition="True">
                        <AutoSize Enabled="True" />
                        <Template1>
                            <px:PXGrid ID="gridProducts" runat="server" SkinID="DetailsInTab" Width="100%" DataSourceID="ds" Height="150px" Caption="Products"
                                AdjustPageSize="Auto" AllowPaging="True">
                                <AutoCallBack Target="gridFeatures" Command="Refresh" />
                                <Levels>
                                    <px:PXGridLevel DataMember="LicenseProducts" DataKeyNames="ProductID">
                                        <RowTemplate>
                                            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="M" ControlSize="XM" />
                                            <px:PXTextEdit runat="server" ID="edCode" DataField="Code" />
                                            <px:PXTextEdit runat="server" ID="edLicenseID" DataField="LicenseID" Width="150px" />
                                            <px:PXTextEdit runat="server" ID="edDescription" DataField="Description" />
                                            <px:PXTextEdit runat="server" ID="edLicType" DataField="LicType" />
                                            <px:PXTextEdit runat="server" ID="edStatus" DataField="Status" />
                                            <px:PXDateTimeEdit runat="server" ID="edStartDate" DataField="StartDate" />
                                            <px:PXDateTimeEdit runat="server" ID="edEndDate" DataField="EndDate" />
                                            <px:PXDateTimeEdit runat="server" ID="edLastCheckDate" DataField="LastCheckDate" />
                                            <px:PXNumberEdit runat="server" ID="edDaysRemaining" DataField="DaysRemaining" />
                                            <px:PXTextEdit runat="server" ID="edFirstName" DataField="FirstName" />
                                            <px:PXTextEdit runat="server" ID="edLastName" DataField="LastName" />
                                            <px:PXTextEdit runat="server" ID="edEmail" DataField="Email" />
                                            <px:PXTextEdit runat="server" ID="edCompany" DataField="Company" />
                                            <px:PXTextEdit runat="server" ID="edAddress" DataField="Address" />
                                            <px:PXTextEdit runat="server" ID="edPhone" DataField="Phone" />
                                            <px:PXTextEdit runat="server" ID="edMetadata" DataField="Metadata" />
                                            <px:PXTextEdit runat="server" ID="edReference" DataField="Reference" />
                                            <px:PXTextEdit runat="server" ID="edCustomFields" DataField="CustomFields" />
                                        </RowTemplate>
                                        <Columns>
                                            <px:PXGridColumn DataField="Code" />
                                            <px:PXGridColumn DataField="LicenseID" Width="150px" />
                                            <px:PXGridColumn DataField="Description" />
                                            <px:PXGridColumn DataField="LicType" />
                                            <px:PXGridColumn DataField="Status" />
                                            <px:PXGridColumn DataField="StartDate" />
                                            <px:PXGridColumn DataField="EndDate" />
                                            <px:PXGridColumn DataField="LastCheckDate" />
                                            <px:PXGridColumn DataField="DaysRemaining" />
                                            <px:PXGridColumn DataField="FirstName" />
                                            <px:PXGridColumn DataField="LastName" />
                                            <px:PXGridColumn DataField="Email" />
                                            <px:PXGridColumn DataField="Company" />
                                            <px:PXGridColumn DataField="Address" />
                                            <px:PXGridColumn DataField="Phone" />
                                            <px:PXGridColumn DataField="Metadata" />
                                            <px:PXGridColumn DataField="Reference" />
                                            <px:PXGridColumn DataField="CustomFields" />
                                        </Columns>
                                    </px:PXGridLevel>
                                </Levels>
                                <Mode AllowFormEdit="True"/>
                                <AutoSize Enabled="True" />
                            </px:PXGrid>
                        </Template1>
                        <Template2>
                            <px:PXGrid ID="gridFeatures" runat="server" SkinID="Details" DataSourceID="ds" Width="100%" Caption="Features" CaptionVisible="true">
                                <Parameters>
                                    <px:PXSyncGridParam ControlID="gridProducts" />
                                </Parameters>
                                <CallbackCommands>
                                    <Save CommitChangesIDs="gridFeatures" RepaintControls="None" RepaintControlsIDs="ds" />
                                    <FetchRow RepaintControls="None" />
                                </CallbackCommands>
                                <Levels>
                                    <px:PXGridLevel DataMember="LicenseProductFeatures" DataKeyNames="Code">
                                        <Columns>
                                            <px:PXGridColumn DataField="FeatureType" Width="100px" />
                                            <px:PXGridColumn DataField="Code" Width="300px" />
                                            <px:PXGridColumn DataField="Description" Width="300px" />
                                            <px:PXGridColumn DataField="ExpiryDate" Width="100px" />
                                            <px:PXGridColumn DataField="AllowUnlimitedConsumptions" Type="CheckBox" TextAlign="Center" Width="80px" />
                                            <px:PXGridColumn DataField="MaxConsumption" TextAlign="Right" Width="100px" />
                                            <px:PXGridColumn DataField="AllowOverages" Type="CheckBox" TextAlign="Center" Width="80px" />
                                            <px:PXGridColumn DataField="MaxOverages" Width="100px" TextAlign="Right" />
                                            <px:PXGridColumn DataField="LocalConsumption" TextAlign="Right" Width="100px" />
                                            <px:PXGridColumn DataField="TotalConsumption" TextAlign="Right" Width="100px" />
                                            <px:PXGridColumn DataField="ConsumptionPeriod" Width="100px" />
                                        </Columns>
                                        <RowTemplate>
                                            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="M" ControlSize="XM" />
                                            <%--<px:PXSelector ID="edContactID" runat="server" DataField="ContactID" AutoRefresh="True" ValueField="DisplayName" AllowEdit="True">
                                                <Parameters>
                                                    <px:PXSyncGridParam ControlID="gridFeatures" />
                                                </Parameters>
                                            </px:PXSelector>--%>
                                        </RowTemplate>
                                        <Layout FormViewHeight="" />
                                    </px:PXGridLevel>
                                </Levels>
                                <AutoSize Enabled="True" MinHeight="150" />
                            </px:PXGrid>
                        </Template2>
                    </px:PXSplitContainer>
                </Template>
            </px:PXTabItem>
        </Items>
        <AutoSize MinHeight="480" Container="Window" Enabled="True" />
    </px:PXTab>
</asp:Content>
