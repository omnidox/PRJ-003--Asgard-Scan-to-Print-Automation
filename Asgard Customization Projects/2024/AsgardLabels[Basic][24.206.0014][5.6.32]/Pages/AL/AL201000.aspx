<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true"
    ValidateRequest="false" CodeFile="AL201000.aspx.cs" Inherits="Pages_AL201000" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>
<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" Width="100%" runat="server" Visible="True" PrimaryView="Model" TypeName="AA.Objects.AL.ALModelMaint">
        <CallbackCommands>
            <px:PXDSCallbackCommand Name="LoadChildren" Visible="False" />
            <px:PXDSCallbackCommand Name="MoveUp" Visible="False" DependOnGrid="exprGrid" />
            <px:PXDSCallbackCommand Name="MoveDown" Visible="False" DependOnGrid="exprGrid"/>
            <px:PXDSCallbackCommand Name="MoveLeft" Visible="False" DependOnGrid="exprGrid"/>
            <px:PXDSCallbackCommand Name="MoveRight" Visible="False" DependOnGrid="exprGrid"/>
            <px:PXDSCallbackCommand Name="GenComponents" Visible="False" />
            <px:PXDSCallbackCommand Name="ClearCache" Visible="False" />
            <px:PXDSCallbackCommand Name="LoadDataElements" Visible="False" />
            <px:PXDSCallbackCommand Name="FindDataElements" Visible="False" />
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXSplitContainer runat="server" SplitterPosition="900" ID="splitRender" Orientation="Vertical">
        <AutoSize Enabled="true" Container="Window" />
        <Template1>
            <px:PXFormView ID="scrollForm" runat="server" Width="100%" RenderStyle="Simple" DataMember="CurrentModel">
                <Template>
                    <px:PXFormView ID="form" runat="server" DataSourceID="ds" Style="z-index: 100" Width="100%"
                        Caption="Asgard Labels" DataMember="Model" AllowCollapse="false">
                        <ContentStyle BorderStyle="None" />
                        <Template>
                            <px:PXLayoutRule runat="server" LabelsWidth="S" ControlSize="XM" StartColumn="True" StartRow="True" />
                            <px:PXSelector ID="edName" runat="server" DataField="Name" DataSourceID="ds" />
                            <px:PXTextEdit ID="hiddenLabelID" runat="server" DataField="LabelID" />
                            <px:PXTextEdit ID="hiddenImageUrl" runat="server" DataField="ImageUrl" />
                            <px:PXTextEdit ID="edDescription" runat="server" DataField="Description" />
                            <px:PXDropDown ID="edModelType" runat="server" DataField="ModelType" CommitChanges="true" />
                            <px:PXSelector ID="edCloudID" runat="server" DataField="CloudID" CommitChanges="True" />
                            <px:PXSelector ID="edScreenID" runat="server" DataField="ScreenID" FilterByAllFields="true" CommitChanges="True" />
                            <px:PXTextEdit ID="edGraphType" runat="server" DataField="GraphType" CommitChanges="true" />
                            <px:PXSelector ID="edBasedOnView" runat="server" DataField="BasedOnView" />
                            <px:PXLayoutRule runat="server" ColumnSpan="2" />
                            <px:PXTextEdit ID="edMessage" runat="server" DataField="Message" TextMode="MultiLine" Height="50px" AlreadyLocalized="False" IsClientControl="True" />
                            <px:PXLayoutRule runat="server" LabelsWidth="S" ControlSize="XM" StartColumn="True" />
                            <px:PXLayoutRule runat="server" Merge="true" />
                            <px:PXCheckBox ID="chkActive" runat="server" DataField="Active" CommitChanges="true" />
                            <px:PXCheckBox ID="chkAllowExport" runat="server" Checked="True" DataField="AllowExport" CommitChanges="True" />
                            <px:PXCheckBox ID="chkIsSystem" runat="server" DataField="IsSystem" CommitChanges="true" />
                            <px:PXLayoutRule runat="server" Merge="true" />
                            <px:PXCheckBox ID="chkHideWhenInGroup" runat="server" DataField="HideWhenInGroup" CommitChanges="true" />
                            <px:PXCheckBox ID="chkIgnoreRotationOnRender" runat="server" DataField="IgnoreRotationOnRender" CommitChanges="true" />
                            <px:PXLayoutRule runat="server" />
                            <px:PXSelector ID="edFormatID" runat="server" DataField="FormatID" CommitChanges="True" AllowEdit="true" />
                            <px:PXSelector ID="edMarginID" runat="server" DataField="MarginID" CommitChanges="True" AllowEdit="true" />
                            <px:PXDropDown ID="edLayoutType" runat="server" DataField="LayoutType" CommitChanges="True" />
                            <px:PXLayoutRule runat="server" />
                            <px:PXLayoutRule runat="server" LabelsWidth="SM" ControlSize="L" StartColumn="True" />
                            <px:PXFormView ID="VisibilityForm" runat="server" DataMember="CurrentModel" DataSourceID="ds" Caption="Hidden Form needed for VisibleExp of TabItems"
                                Visible="False" TabIndex="300">
                                <Template>
                                    <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="M" ControlSize="XM" />
                                    <px:PXCheckBox ID="chkShowTemplate" runat="server" DataField="ShowTemplate" AlreadyLocalized="False" IsClientControl="True" />
                                    <px:PXCheckBox ID="chkShowExprs" runat="server" DataField="ShowExprs" AlreadyLocalized="False" IsClientControl="True" />
                                    <px:PXCheckBox ID="chkShowSetup" runat="server" DataField="ShowSetup" AlreadyLocalized="False" IsClientControl="True" />
                                    <px:PXCheckBox ID="chkShowRendered" runat="server" DataField="ShowRendered" AlreadyLocalized="False" IsClientControl="True" />
                                    <px:PXCheckBox ID="chkShowPrinters" runat="server" DataField="ShowPrinters" AlreadyLocalized="False" IsClientControl="True" />
                                    <px:PXCheckBox ID="chkShowPrintLog" runat="server" DataField="ShowPrintLog" AlreadyLocalized="False" IsClientControl="True" />
                                    <%--<px:PXCheckBox ID="chkShowDev" runat="server" DataField="ShowDev" AlreadyLocalized="False" IsClientControl="True" />--%>
                                    <px:PXCheckBox ID="chkShowChildren" runat="server" DataField="ShowChildren" AlreadyLocalized="False" IsClientControl="True" />
                                    <px:PXCheckBox ID="chkShowAutomation" runat="server" DataField="ShowAutomation" AlreadyLocalized="False" IsClientControl="True" />
                                    <px:PXCheckBox ID="chkShowUsedBy" runat="server" DataField="ShowUsedBy" AlreadyLocalized="False" IsClientControl="True" />
                                </Template>
                            </px:PXFormView>
                            <px:PXUploadDialog ID="uploadFileDialog" runat="server" AutoSaveFile="true" Caption="Upload XML Zip" Key="Model"
                                Height="110px" SessionKey="XmlUploadAllEntities" AllowedFileTypes=".zip;" />
                        </Template>
                    </px:PXFormView>
                    <px:PXTab ID="tab" runat="server" Height="500px" Width="100%" DataSourceID="ds" DataMember="CurrentModel">
                        <Activity HighlightColor="" SelectedColor="" Width="" Height=""></Activity>
                        <Items>
                            <px:PXTabItem Text="Template" BindingContext="VisibilityForm" VisibleExp="DataControls[&quot;chkShowTemplate&quot;].Value == true">
                                <Template>
                                    <px:PXFormView ID="templateDataForm" runat="server" DataSourceID="ds" Style="left: 18px; top: 36px;" Width="100%" Height="95%" DataMember="CurrentModel" CaptionVisible="False" SkinID="Transparent" TabIndex="3500">
                                        <Template>
                                            <px:PXTextEdit SuppressLabel="True" ID="templateDataBox" DisableSpellcheck="True" runat="server" DataField="Body" TextMode="MultiLine" Width="100%" Height="100%" AlreadyLocalized="False" IsClientControl="True">
                                                <AutoSize Enabled="True" />
                                            </px:PXTextEdit>
                                        </Template>
                                    </px:PXFormView>
                                </Template>
                            </px:PXTabItem>
                            <px:PXTabItem Text="Expressions" BindingContext="VisibilityForm" VisibleExp="DataControls[&quot;chkShowExprs&quot;].Value == true">
                                <Template>
                                    <px:PXFormView ID="ShowHide" runat="server" DataMember="CurrentModel" RenderStyle="Simple" SkinID="Transparent" DataSourceID="ds">
                                        <Template>
                                            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="S" ControlSize="S" Merge="True" />
                                            <px:PXNumberEdit ID="edMoveBy" runat="server" DataField="MoveBy" CommitChanges="True" AlreadyLocalized="False" IsClientControl="True" />
                                            <px:PXDropDown ID="edSizeUnit" runat="server" DataField="SizeUnit" CommitChanges="True" AllowEdit="True" IsClientControl="True" />
                                        </Template>
                                    </px:PXFormView>
                                    <px:PXGrid ID="exprGrid" runat="server" DataSourceID="ds" Height="150px" Style="border: 0px;"
                                        Width="100%" ActionsPosition="Top" SkinID="DetailsInTab" KeepPosition="True" SyncPosition="True" FilesIndicator="False"
                                        RepaintColumns="True" AutoRepaint="True" MatrixMode="true" SyncPositionWithGraph="true">
                                        <EmptyMsg AnonFilteredAddMessage="No records found.
Try to change filter to see records here."
                                            AnonFilteredMessage="No records found.
Try to change filter to see records here."
                                            ComboAddMessage="No records found.
Try to change filter or modify parameters above to see records here."
                                            FilteredAddMessage="No records found.
Try to change filter to see records here."
                                            FilteredMessage="No records found.
Try to change filter to see records here."
                                            NamedComboAddMessage="No records found as &#39;{0}&#39;.
Try to change filter or modify parameters above to see records here."
                                            NamedComboMessage="No records found as &#39;{0}&#39;.
Try to change filter or modify parameters above to see records here."
                                            NamedFilteredAddMessage="No records found as &#39;{0}&#39;.
Try to change filter to see records here."
                                            NamedFilteredMessage="No records found as &#39;{0}&#39;.
Try to change filter to see records here."></EmptyMsg>
                                        <Levels>
                                            <px:PXGridLevel DataMember="Expressions" DataKeyNames="LabelID,LineNbr">
                                                <RowTemplate>
                                                    <px:PXSelector ID="edDataElementID" runat="server" DataField="DataElementID" CommitChanges="True" AllowEdit="True" edit="1" AutoComplete="true" />
                                                    <px:PXSelector ID="edJustificationID" runat="server" DataField="JustificationID" CommitChanges="True" AllowEdit="True" edit="1" AutoComplete="true" />
                                                    <px:PXDropDown ID="edOrientation" runat="server" DataField="Orientation" CommitChanges="True" IsClientControl="True" />
                                                    <px:PXNumberEdit ID="edPosX" runat="server" DataField="PosX" CommitChanges="True" AlreadyLocalized="False" IsClientControl="True" />
                                                    <px:PXNumberEdit ID="edPosY" runat="server" DataField="PosY" CommitChanges="True" AlreadyLocalized="False" IsClientControl="True" />
                                                    <px:PXDropDown ID="edExprType" runat="server" DataField="ExprType" CommitChanges="True" IsClientControl="True" />
                                                    <px:PXSelector ID="edFontID" runat="server" DataField="FontID" CommitChanges="True" AllowEdit="True" edit="1" />
                                                    <px:PXSelector ID="edForeColorID" runat="server" DataField="ForeColorID" CommitChanges="True" AllowEdit="True" edit="1" />
                                                    <px:PXSelector ID="edBackColorID" runat="server" DataField="BackColorID" CommitChanges="True" AllowEdit="True" edit="1" />
                                                    <px:PXSelector ID="edRuleID" runat="server" DataField="RuleID" CommitChanges="True" AllowEdit="True" edit="1" />
                                                    <%-- all field from here will be soon removed/>--%>
                                                    <px:PXNumberEdit ID="edToX" runat="server" DataField="ToX" CommitChanges="True" AlreadyLocalized="False" IsClientControl="True" />
                                                    <px:PXDropDown ID="edSampleType" runat="server" DataField="SampleType" CommitChanges="True" IsClientControl="True" />
                                                    <px:PXSelector ID="edContentID" runat="server" DataField="ContentID" CommitChanges="True" AllowEdit="True" edit="1" />
                                                    <px:PXSelector ID="edBarcodeID" runat="server" DataField="BarcodeID" CommitChanges="True" AllowEdit="True" edit="1" />
                                                    <px:PXSelector ID="edPrinterFileID" runat="server" DataField="PrinterFileID" CommitChanges="True" AllowEdit="True" edit="1" />
                                                    <px:PXSelector ID="edSubstitutionID" runat="server" DataField="SubstitutionID" CommitChanges="True" AllowEdit="True" edit="1" />
                                                </RowTemplate>
                                                <Columns>
                                                    <px:PXGridColumn DataField="LabelID" />
                                                    <px:PXGridColumn DataField="Active" TextAlign="Center" Type="CheckBox" />
                                                    <px:PXGridColumn DataField="LineNbr" Width="80px" TextAlign="Right" />
                                                    <px:PXGridColumn DataField="ExprCode" Width="140px" CommitChanges="True" AllowDragDrop="True" />
                                                    <px:PXGridColumn DataField="DataElementID" Width="250px" CommitChanges="True" AllowDragDrop="True" LinkCommand="ViewDataElement" />
                                                    <px:PXGridColumn DataField="ExprType" Width="90px" CommitChanges="True" Type="DropDownList" AllowDragDrop="True" />
                                                    <px:PXGridColumn DataField="ExprValue" Width="150px" CommitChanges="True" AllowDragDrop="True" />
                                                    <px:PXGridColumn DataField="PosX" Width="80px" CommitChanges="True" />
                                                    <px:PXGridColumn DataField="PosY" Width="80px" CommitChanges="True" />
                                                    <px:PXGridColumn DataField="FontID" Width="80px" CommitChanges="True" LinkCommand="ViewFont" />
                                                    <px:PXGridColumn DataField="JustificationID" Width="120px" CommitChanges="True" LinkCommand="ViewJustification" />
                                                    <px:PXGridColumn DataField="Orientation" Width="90px" CommitChanges="True" Type="DropDownList" />
                                                    <px:PXGridColumn DataField="ReverseDots" TextAlign="Center" Type="CheckBox" />
                                                    <px:PXGridColumn DataField="RuleID" Width="150px" CommitChanges="True" />
                                                    <px:PXGridColumn DataField="ReverseRule" Width="90px" CommitChanges="True" Type="CheckBox" TextAlign="Center" />
                                                    <px:PXGridColumn DataField="ForeColorID" CommitChanges="True" />
                                                    <px:PXGridColumn DataField="BackColorID" CommitChanges="True" />
                                                    <px:PXGridColumn DataField="HexEncoding" Width="90px" CommitChanges="True" Type="DropDownList" />
                                                    <%-- all field from here will be soon removed/>--%>
                                                    <px:PXGridColumn DataField="ToX" Width="80px" CommitChanges="True" />
                                                    <px:PXGridColumn DataField="BarcodeID" Width="150px" MatrixMode="False" CommitChanges="True" />
                                                    <px:PXGridColumn DataField="ContentID" Width="150px" CommitChanges="True" />
                                                    <px:PXGridColumn DataField="PrinterFileID" Width="150px" CommitChanges="True" LinkCommand="ViewPrinterFile" />
                                                    <px:PXGridColumn DataField="SampleType" Width="90px" CommitChanges="True" Type="DropDownList" />
                                                    <px:PXGridColumn DataField="SampleValue" Width="200px" CommitChanges="True" />
                                                    <px:PXGridColumn DataField="DoSubstitute" AllowNull="False" TextAlign="Center" Type="CheckBox" Width="120px" CommitChanges="True" />
                                                    <px:PXGridColumn DataField="SubstitutionID" Width="150px" MatrixMode="False" CommitChanges="True" />
                                                </Columns>
                                            </px:PXGridLevel>
                                        </Levels>
                                        <ActionBar>
                                            <Actions>
                                                <FilterBar Order="0" GroupIndex="3"></FilterBar>
                                            </Actions>
                                            <CustomItems>
                                                <px:PXToolBarButton Text="Find Data Elements">
                                                    <AutoCallBack Command="FindDataElements" Target="ds"></AutoCallBack>
                                                </px:PXToolBarButton>
                                                <px:PXToolBarButton CommandSourceID="ds" CommandName="MoveUp" Text="Pos" Tooltip="Position Up">
                                                    <AutoCallBack Command="MoveUp" Target="ds"></AutoCallBack>
                                                    <Images Normal="main@ArrowUp"></Images>
                                                </px:PXToolBarButton>
                                                <px:PXToolBarButton CommandSourceID="ds" CommandName="MoveDown" Text="Pos" Tooltip="Position Down">
                                                    <AutoCallBack Command="MoveDown" Target="ds"></AutoCallBack>
                                                    <Images Normal="main@ArrowDown"></Images>
                                                </px:PXToolBarButton>
                                                <px:PXToolBarButton CommandSourceID="ds" CommandName="MoveLeft" Text="Pos" Tooltip="Position Left">
                                                    <AutoCallBack Command="MoveLeft" Target="ds"></AutoCallBack>
                                                    <Images Normal="main@ArrowLeft"></Images>
                                                </px:PXToolBarButton>
                                                <px:PXToolBarButton CommandSourceID="ds" CommandName="MoveRight" Text="Pos" Tooltip="Position Right">
                                                    <AutoCallBack Command="MoveRight" Target="ds"></AutoCallBack>
                                                    <Images Normal="main@ArrowRight"></Images>
                                                </px:PXToolBarButton>
<%--                                                <px:PXToolBarButton Text="Add Data Elements" Key="cmdADE">
                                                    <AutoCallBack Command="AddDataElements" Target="ds">
                                                        <Behavior CommitChanges="True" PostData="Page" />
                                                    </AutoCallBack>
                                                </px:PXToolBarButton>--%>
                                            </CustomItems>
                                        </ActionBar>
                                        <AutoSize Enabled="True" MinHeight="250" />
                                        <Mode AllowDragRows="True" AllowFormEdit="True" AllowUpload="True" InitNewRow="True" AllowDelete="True" />
                                        <CallbackCommands PasteCommand="ExprPasteLine">
                                            <Save PostData="Container" />
                                        </CallbackCommands>
                                    </px:PXGrid>
                                </Template>
                            </px:PXTabItem>
                            <px:PXTabItem Text="Graphics" BindingContext="VisibilityForm" VisibleExp="DataControls[&quot;chkShowExprs&quot;].Value == true">
                                <Template>
                                    <px:PXGrid ID="graphicGrid" runat="server" DataSourceID="ds" Height="150px" SkinID="Details" TabIndex="6800" Width="100%" ActionsPosition="Top"
                                        KeepPosition="True" SyncPosition="True" FilesIndicator="False"
                                        RepaintColumns="True" AutoRepaint="True">
                                        <EmptyMsg AnonFilteredAddMessage="No records found.
Try to change filter to see records here."
                                            AnonFilteredMessage="No records found.
Try to change filter to see records here."
                                            ComboAddMessage="No records found.
Try to change filter or modify parameters above to see records here."
                                            FilteredAddMessage="No records found.
Try to change filter to see records here."
                                            FilteredMessage="No records found.
Try to change filter to see records here."
                                            NamedComboAddMessage="No records found as '{0}'.
Try to change filter or modify parameters above to see records here."
                                            NamedComboMessage="No records found as '{0}'.
Try to change filter or modify parameters above to see records here."
                                            NamedFilteredAddMessage="No records found as '{0}'.
Try to change filter to see records here."
                                            NamedFilteredMessage="No records found as '{0}'.
Try to change filter to see records here." />
                                        <Levels>
                                            <px:PXGridLevel DataMember="Graphics" DataKeyNames="ModelID,LineNbr">
                                                <RowTemplate>
                                                    <px:PXDropDown ID="edGraphicType" runat="server" DataField="GraphicType" CommitChanges="True" AllowEdit="True" IsClientControl="True" />
                                                    <px:PXSelector ID="edForeColorID2" runat="server" DataField="ForeColorID" CommitChanges="True" AllowEdit="True" edit="1" />
                                                    <px:PXSelector ID="edBackColorID2" runat="server" DataField="BackColorID" CommitChanges="True" AllowEdit="True" edit="1" />
                                                    <%--<px:PXTextEdit ID="edForeColor2" runat="server" DataField="ForeColor" TextMode="Color" AlreadyLocalized="False" IsClientControl="True" />--%>
                                                    <%--<px:PXTextEdit ID="edBackColor2" runat="server" DataField="BackColor" TextMode="Color" AlreadyLocalized="False" IsClientControl="True" />--%>
                                                </RowTemplate>
                                                <Columns>
                                                    <px:PXGridColumn DataField="ModelID" />
                                                    <px:PXGridColumn DataField="Active" Width="60px" TextAlign="Center" Type="CheckBox" />
                                                    <px:PXGridColumn DataField="LineNbr" TextAlign="Right" />
                                                    <px:PXGridColumn DataField="GraphicType" Type="DropDownList" Width="100px" AllowDragDrop="True" CommitChanges="True" />
                                                    <px:PXGridColumn DataField="FromX" TextAlign="Right" Width="75px" AllowDragDrop="True" CommitChanges="True" />
                                                    <px:PXGridColumn DataField="FromY" TextAlign="Right" Width="75px" AllowDragDrop="True" CommitChanges="True" />
                                                    <px:PXGridColumn DataField="ToX" TextAlign="Right" Width="65px" AllowDragDrop="True" CommitChanges="True" />
                                                    <px:PXGridColumn DataField="ToY" TextAlign="Right" Width="65px" AllowDragDrop="True" CommitChanges="True" />
                                                    <px:PXGridColumn DataField="Thickness" Width="90px" CommitChanges="True" />
                                                    <px:PXGridColumn DataField="Rounding" Width="100px" Type="DropDownList" CommitChanges="True" />
                                                    <px:PXGridColumn DataField="ForeColorID" TextAlign="Right" Width="130px" CommitChanges="True" />
                                                    <%--<px:PXGridColumn DataField="ForeColor" Width="50px" AllowDragDrop="True"/>--%>
                                                    <px:PXGridColumn DataField="BackColorID" TextAlign="Right" Width="130px" CommitChanges="True" />
                                                    <%--<px:PXGridColumn DataField="BackColor" Width="50px" />--%>
                                                </Columns>
                                            </px:PXGridLevel>
                                        </Levels>
                                        <AutoSize Enabled="True" MinHeight="250"></AutoSize>
                                        <Mode AllowDragRows="True" AllowUpload="True" InitNewRow="True"></Mode>
                                        <CallbackCommands PasteCommand="GraphicPasteLine"></CallbackCommands>
                                    </px:PXGrid>
                                </Template>
                            </px:PXTabItem>
                            <px:PXTabItem Text="Printers" BindingContext="VisibilityForm" VisibleExp="DataControls[&quot;chkShowPrinters&quot;].Value == true">
                                <Template>
                                    <px:PXGrid ID="printersGrid" runat="server" DataSourceID="ds" Height="150px" Style="border: 0px;"
                                        Width="100%" ActionsPosition="Top" SkinID="Details" MatrixMode="True" NoteIndicator="False" FilesIndicator="False">
                                        <EmptyMsg AnonFilteredAddMessage="No records found.
Try to change filter to see records here."
                                            AnonFilteredMessage="No records found.
Try to change filter to see records here."
                                            ComboAddMessage="No records found.
Try to change filter or modify parameters above to see records here."
                                            FilteredAddMessage="No records found.
Try to change filter to see records here."
                                            FilteredMessage="No records found.
Try to change filter to see records here."
                                            NamedComboAddMessage="No records found as &#39;{0}&#39;.
Try to change filter or modify parameters above to see records here."
                                            NamedComboMessage="No records found as &#39;{0}&#39;.
Try to change filter or modify parameters above to see records here."
                                            NamedFilteredAddMessage="No records found as &#39;{0}&#39;.
Try to change filter to see records here."
                                            NamedFilteredMessage="No records found as &#39;{0}&#39;.
Try to change filter to see records here."></EmptyMsg>
                                        <Levels>
                                            <px:PXGridLevel DataMember="Printers" DataKeyNames="LabelID,LineNbr">
                                                <RowTemplate>
                                                    <px:PXSelector ID="edUserID" DataField="UserID" CommitChanges="True" runat="server" AllowEdit="True" FilterByAllFields="True" AutoRefresh="True" edit="1" />
                                                    <px:PXSelector ID="edWorkGroupID" DataField="WorkGroupID" CommitChanges="True" runat="server" AllowEdit="True" FilterByAllFields="True" AutoRefresh="True" edit="1" />
                                                    <px:PXSelector ID="edOwnerID" DataField="OwnerID" CommitChanges="True" runat="server" AllowEdit="True" FilterByAllFields="True" AutoRefresh="True" edit="1" />
                                                    <px:PXSelector ID="edPrintStationID" DataField="PrintStationID" CommitChanges="True" runat="server" AllowEdit="True" FilterByAllFields="True" AutoRefresh="True" edit="1" />
                                                    <px:PXSelector ID="edPrinterID" DataField="PrinterID" CommitChanges="True" runat="server" AllowEdit="True" FilterByAllFields="True" AutoRefresh="True" edit="1" />
                                                </RowTemplate>
                                                <Columns>
                                                    <px:PXGridColumn DataField="LabelID" />
                                                    <px:PXGridColumn DataField="LineNbr" TextAlign="Right" />
                                                    <px:PXGridColumn DataField="Active" TextAlign="Center" Type="CheckBox" AllowCheckAll="True" />
                                                    <px:PXGridColumn DataField="UserID" Width="100px" CommitChanges="True" LinkCommand="ViewUser" />
                                                    <px:PXGridColumn DataField="WorkGroupID" Width="150px" CommitChanges="True" />
                                                    <px:PXGridColumn DataField="OwnerID" Width="150px" CommitChanges="True" />
                                                    <px:PXGridColumn DataField="PrintStationID" Width="150px" LinkCommand="ViewPrintStation" CommitChanges="True" />
                                                    <px:PXGridColumn DataField="PrinterID" Width="150px" LinkCommand="ViewPrinter" CommitChanges="True" />
                                                    <px:PXGridColumn DataField="CreatedByID" Width="100px" />
                                                    <px:PXGridColumn DataField="CreatedDateTime" Width="100px" />
                                                    <px:PXGridColumn DataField="LastModifiedByID" Width="100px" />
                                                    <px:PXGridColumn DataField="LastModifiedDateTime" Width="100px" />
                                                </Columns>
                                            </px:PXGridLevel>
                                        </Levels>
                                        <AutoSize Enabled="True" MinHeight="250"></AutoSize>
                                        <Mode AllowDragRows="True" AllowUpload="True" InitNewRow="True"></Mode>
                                        <CallbackCommands PasteCommand="PrinterPasteLine"></CallbackCommands>
                                    </px:PXGrid>
                                </Template>
                            </px:PXTabItem>
                            <px:PXTabItem Text="Child Labels" BindingContext="VisibilityForm" VisibleExp="DataControls[&quot;chkShowChildren&quot;].Value == true">
                                <Template>
                                    <px:PXGrid ID="childrenGrid" runat="server" DataSourceID="ds" Height="150px" Style="border: 0px;"
                                        Width="100%" ActionsPosition="Top" SkinID="Details" MatrixMode="True" KeepPosition="true" SyncPosition="true" 
                                               NoteIndicator="False" FilesIndicator="False">
                                        <EmptyMsg AnonFilteredAddMessage="No records found.
Try to change filter to see records here."
                                            AnonFilteredMessage="No records found.
Try to change filter to see records here."
                                            ComboAddMessage="No records found.
Try to change filter or modify parameters above to see records here."
                                            FilteredAddMessage="No records found.
Try to change filter to see records here."
                                            FilteredMessage="No records found.
Try to change filter to see records here."
                                            NamedComboAddMessage="No records found as &#39;{0}&#39;.
Try to change filter or modify parameters above to see records here."
                                            NamedComboMessage="No records found as &#39;{0}&#39;.
Try to change filter or modify parameters above to see records here."
                                            NamedFilteredAddMessage="No records found as &#39;{0}&#39;.
Try to change filter to see records here."
                                            NamedFilteredMessage="No records found as &#39;{0}&#39;.
Try to change filter to see records here."></EmptyMsg>
                                        <Levels>
                                            <px:PXGridLevel DataMember="Children" DataKeyNames="LabelID,LineNbr">
                                                <RowTemplate>
                                                    <px:PXSelector ID="edLabelChildID" DataField="LabelChildID" CommitChanges="True" runat="server" AllowEdit="True" FilterByAllFields="True" edit="1"></px:PXSelector>
                                                </RowTemplate>
                                                <Columns>
                                                    <px:PXGridColumn DataField="LabelID" />
                                                    <px:PXGridColumn DataField="LineNbr" TextAlign="Right" />
                                                    <px:PXGridColumn DataField="Active" TextAlign="Center" Type="CheckBox" AllowCheckAll="True" />
                                                    <px:PXGridColumn DataField="LabelChildID" Width="150px" LinkCommand="ViewLabelChild" CommitChanges="True" />
                                                    <px:PXGridColumn DataField="ALModel__HideWhenInGroup" TextAlign="Center" Type="CheckBox" Width="100px" />
                                                    <px:PXGridColumn DataField="ALModel__Description" Width="300px" />
                                                    <px:PXGridColumn DataField="ALModel__FormatID" Width="100px" DisplayMode="Text" LinkCommand="ViewFormat" />
                                                    <px:PXGridColumn DataField="ALModel__ReverseFilter" TextAlign="Center" Type="CheckBox"/>
                                                    <px:PXGridColumn DataField="ALModel__FilterRuleID" Width="120px" DisplayMode="Text" LinkCommand="ViewFilterRule" />
                                                    <px:PXGridColumn DataField="ALModel__ReversePrint" TextAlign="Center" Type="CheckBox" />
                                                    <px:PXGridColumn DataField="ALModel__PrintRuleID" Width="100px" DisplayMode="Text" LinkCommand="ViewPrintRule" />
                                                    <px:PXGridColumn DataField="ALModel__NbCopiesExpr" Width="150px" />
                                                    <px:PXGridColumn DataField="LastModifiedByID" Width="100px" />
                                                    <px:PXGridColumn DataField="LastModifiedDateTime" Width="100px" />
                                                </Columns>
                                            </px:PXGridLevel>
                                        </Levels>
                                        <ActionBar>
                                            <Actions>
                                                <FilterBar Order="0" GroupIndex="3"></FilterBar>
                                            </Actions>
                                            <CustomItems>
                                                <px:PXToolBarButton Text="Load Children" AlreadyLocalized="False" SuppressHtmlEncoding="False" UsesSignalR="False">
                                                    <AutoCallBack Command="LoadChildren" Target="ds" />
                                                </px:PXToolBarButton>
                                            </CustomItems>
                                        </ActionBar>
                                        <AutoSize Enabled="True" MinHeight="250" />
                                        <Mode AllowDragRows="True" AllowUpload="True" InitNewRow="True" />
                                        <CallbackCommands PasteCommand="ChildPasteLine" />
                                    </px:PXGrid>
                                </Template>
                            </px:PXTabItem>
                            <px:PXTabItem Text="Rendered" BindingContext="VisibilityForm" VisibleExp="DataControls[&quot;chkShowRendered&quot;].Value == true">
                                <Template>
                                    <px:PXFormView ID="renderedDataForm" runat="server" CaptionVisible="False" DataMember="CurrentModel" DataSourceID="ds" Height="95%" SkinID="Transparent" Width="100%">
                                        <Template>
                                            <px:PXTextEdit ID="renderedDataBox" runat="server" AlreadyLocalized="False" DataField="Rendered" DisableSpellcheck="True" Height="100%" IsClientControl="True" SuppressLabel="True" TextMode="MultiLine" Width="100%" />
                                        </Template>
                                    </px:PXFormView>
                                </Template>
                            </px:PXTabItem>
                            <px:PXTabItem Text="Setup" BindingContext="VisibilityForm" VisibleExp="DataControls[&quot;chkShowSetup&quot;].Value == true">
                                <Template>
                                    <px:PXLayoutRule runat="server" LabelsWidth="M" ControlSize="L" StartColumn="True" />
                                    <px:PXSelector runat="server" ID="edFilterRuleID" DataField="FilterRuleID" CommitChanges="True" AllowEdit="True" edit="1" />
                                    <px:PXCheckBox runat="server" ID="chkReverseFilter" DataField="ReverseFilter" CommitChanges="True" AlreadyLocalized="False" IsClientControl="True" />
                                    <px:PXCheckBox runat="server" ID="chkHideInstead" DataField="HideInstead" CommitChanges="True" AlreadyLocalized="False" IsClientControl="True" />
                                    <px:PXSelector runat="server" ID="edPrintRuleID" DataField="PrintRuleID" CommitChanges="True" AllowEdit="True" edit="1" />
                                    <px:PXCheckBox runat="server" ID="chkReversePrint" DataField="ReversePrint" CommitChanges="True" AlreadyLocalized="False" IsClientControl="True" />
                                    <px:PXLayoutRule runat="server"></px:PXLayoutRule>
                                    <px:PXDropDown ID="edPrintOnOtherDensity" runat="server" DataField="PrintOnOtherDensity" Width="300px" IsClientControl="True" CommitChanges="True" />
                                    <px:PXTextEdit ID="edTooltip" runat="server" DataField="Tooltip" AlreadyLocalized="False" IsClientControl="True" />
                                    <px:PXCheckBox runat="server" ID="chkDealingMode" DataField="DealingMode" CommitChanges="True" AlreadyLocalized="False" IsClientControl="True" />
                                    <%--<px:PXTreeSelector ID="edDealingCountExpr" runat="server" DataField="DealingCountExpr"
                                        TreeDataSourceID="ds" PopulateOnDemand="True" InitialExpandLevel="0"
                                        ShowRootNode="False" MinDropWidth="468" MaxDropWidth="600" AllowEditValue="True"
                                        AppendSelectedValue="True" AutoRefresh="True" TreeDataMember="DealingEntityItems" AlreadyLocalized="False">
                                        <DataBindings>
                                            <px:PXTreeItemBinding DataMember="DealingEntityItems" TextField="Name" ValueField="Path" ImageUrlField="Icon" ToolTipField="Path" />
                                        </DataBindings>
                                    </px:PXTreeSelector>
                                    <px:PXTreeSelector ID="edNbCopiesExpr" runat="server" DataField="NbCopiesExpr"
                                        TreeDataSourceID="ds" PopulateOnDemand="True" InitialExpandLevel="0"
                                        ShowRootNode="False" MinDropWidth="468" MaxDropWidth="600" AllowEditValue="True"
                                        AppendSelectedValue="True" AutoRefresh="True" TreeDataMember="DealingEntityItems" AlreadyLocalized="False">
                                        <DataBindings>
                                            <px:PXTreeItemBinding DataMember="DealingEntityItems" TextField="Name" ValueField="Path" ImageUrlField="Icon" ToolTipField="Path" />
                                        </DataBindings>
                                    </px:PXTreeSelector>--%>
                                    <px:PXTextEdit ID="edDealingCountExpr" runat="server" DataField="DealingCountExpr" AlreadyLocalized="False" IsClientControl="True" />
                                    <px:PXTextEdit ID="edNbCopiesExpr" runat="server" DataField="NbCopiesExpr" AlreadyLocalized="False" IsClientControl="True" />
                                    <px:PXNumberEdit ID="edSendPauseEvery" runat="server" DataField="SendPauseEvery" AlreadyLocalized="False" IsClientControl="True" />
                                    <px:PXSelector ID="edNumberingID" runat="server" DataField="NumberingID" CommitChanges="True" AllowEdit="True" edit="1" />
                                    <px:PXDropDown ID="edLanguage" runat="server" DataField="Language" Width="300px" IsClientControl="True" CommitChanges="True" />
                                    <px:PXDropDown ID="edEncoding" runat="server" DataField="Encoding" Width="300px" IsClientControl="True" CommitChanges="True" />
                                    <px:PXSelector ID="edCategoryID" runat="server" DataField="CategoryID" CommitChanges="True" AllowEdit="True" edit="1" />
                                </Template>
                            </px:PXTabItem>
                            <px:PXTabItem Text="Used By Data Elements" BindingContext="VisibilityForm" VisibleExp="DataControls[&quot;chkShowUsedBy&quot;].Value == true">
                                <Template>
                                    <px:PXGrid ID="gridUsedByDataElements" runat="server" AllowPaging="False" DataSourceID="ds" SkinID="Inquire" Width="100%"
                                        Height="20px" MatrixMode="True">
                                        <Levels>
                                            <px:PXGridLevel DataMember="UsedByDataElements">
                                                <Columns>
                                                    <px:PXGridColumn DataField="Name" Width="350px" LinkCommand="ViewModel" />
                                                    <px:PXGridColumn DataField="Active" AllowNull="False" TextAlign="Center" Type="CheckBox" />
                                                    <px:PXGridColumn DataField="LastModifiedByID" Width="150px" />
                                                    <px:PXGridColumn DataField="LastModifiedDateTime" Width="150px" />
                                                </Columns>
                                                <Layout FormViewHeight="" />
                                            </px:PXGridLevel>
                                        </Levels>
                                        <AutoSize Enabled="True" MinHeight="20" />
                                    </px:PXGrid>
                                </Template>
                            </px:PXTabItem>
                            <px:PXTabItem Text="Automation" BindingContext="VisibilityForm" VisibleExp="DataControls[&quot;chkShowAutomation&quot;].Value == true">
                                <Template>
                                    <px:PXLayoutRule runat="server" LabelsWidth="SM" ControlSize="XL" StartColumn="True" />
                                    <px:PXTextEdit ID="edActionName" runat="server" DataField="ActionName" AutoRefresh="True" DataSourceID="ds" />
                                    <px:PXDropDown ID="edTriggerField" runat="server" DataField="TriggerField" Width="300px" IsClientControl="True" CommitChanges="true" />
                                    <px:PXDropDown ID="edTriggerValue" runat="server" DataField="TriggerValue" Width="300px" IsClientControl="True" CommitChanges="true" />
                                    <px:PXTextEdit ID="edTriggerDesignID" runat="server" DataField="TriggerDesignID">
                                        <LinkCommand Target="ds" Command="ViewGenInquiry" />
                                    </px:PXTextEdit>
                                    <px:PXTextEdit ID="edTriggerEventID" runat="server" DataField="TriggerEventID">
                                        <LinkCommand Target="ds" Command="ViewBusEvent" />
                                    </px:PXTextEdit>
                                    <px:PXTextEdit ID="edTriggerProviderID" runat="server" DataField="TriggerProviderID">
                                        <LinkCommand Target="ds" Command="ViewProvider" />
                                    </px:PXTextEdit>
                                    <px:PXTextEdit ID="edTriggerMappingID" runat="server" DataField="TriggerMappingID">
                                        <LinkCommand Target="ds" Command="ViewImpScenario" />
                                    </px:PXTextEdit>
                                    <%--<px:PXDropDown ID="edAutoPrintOn" runat="server" DataField="AutoPrintOn" Width="300px" IsClientControl="True" CommitChanges="true" />
                                    <px:PXLayoutRule runat="server" Merge="True" />
                                    <px:PXSelector runat="server" ID="edAutoPrintRuleID" DataField="AutoPrintRuleID" CommitChanges="True" />
                                    <px:PXCheckBox runat="server" ID="chkReverseAutoPrint" DataField="ReverseAutoPrint" CommitChanges="True" AlreadyLocalized="False" IsClientControl="True" />--%>
                                </Template>
                            </px:PXTabItem>
                            <%--<px:PXTabItem Text="Questions" BindingContext="VisibilityForm" VisibleExp="DataControls[&quot;chkShowQuestions&quot;].Value == true">
                                <Template>
                                    <px:PXGrid ID="QuestionGrid" runat="server" DataSourceID="ds" Height="150px" Style="border: 0px;"
                                        Width="100%" ActionsPosition="Top" SkinID="Details" MatrixMode="True">
                                        <EmptyMsg AnonFilteredAddMessage="No records found.
Try to change filter to see records here."
                                            AnonFilteredMessage="No records found.
Try to change filter to see records here."
                                            ComboAddMessage="No records found.
Try to change filter or modify parameters above to see records here."
                                            FilteredAddMessage="No records found.
Try to change filter to see records here."
                                            FilteredMessage="No records found.
Try to change filter to see records here."
                                            NamedComboAddMessage="No records found as &#39;{0}&#39;.
Try to change filter or modify parameters above to see records here."
                                            NamedComboMessage="No records found as &#39;{0}&#39;.
Try to change filter or modify parameters above to see records here."
                                            NamedFilteredAddMessage="No records found as &#39;{0}&#39;.
Try to change filter to see records here."
                                            NamedFilteredMessage="No records found as &#39;{0}&#39;.
Try to change filter to see records here."></EmptyMsg>
                                        <Levels>
                                            <px:PXGridLevel DataMember="Question" DataKeyNames="ModelID,LineNbr">
                                                <RowTemplate>
                                                    <px:PXSelector ID="edQuestionID" DataField="QuestionID" CommitChanges="True" runat="server" AllowEdit="True" FilterByAllFields="True" edit="1" />
                                                    <px:PXSelector ID="edAnswerID" DataField="AnswerID" CommitChanges="True" runat="server" AllowEdit="True" FilterByAllFields="True" edit="1" />
                                                    <px:PXSelector ID="edRuleID2" runat="server" DataField="RuleID" CommitChanges="True" AllowEdit="True" edit="1" />
                                                </RowTemplate>
                                                <Columns>
                                                    <px:PXGridColumn DataField="Active" Width="60px" TextAlign="Center" Type="CheckBox" AllowCheckAll="True" />
                                                    <px:PXGridColumn DataField="QuestionID" Width="180px" CommitChanges="True" />
                                                    <px:PXGridColumn DataField="AnswerID" Width="180px" CommitChanges="True" />
                                                    <px:PXGridColumn DataField="ALAnswer__AnswerType" Width="110px" />
                                                    <px:PXGridColumn DataField="ALAnswer__AttributeType" Width="130px" />
                                                    <px:PXGridColumn DataField="DefaultValueType" CommitChanges="True" Type="DropDownList" />
                                                    <px:PXGridColumn DataField="DefaultValue" Width="150px" CommitChanges="True" />
                                                    <px:PXGridColumn DataField="RuleID" Width="150px" CommitChanges="True" />
                                                    <px:PXGridColumn DataField="ReverseRule" Width="70px" CommitChanges="True" Type="CheckBox" TextAlign="Center" />
                                                </Columns>
                                            </px:PXGridLevel>
                                        </Levels>
                                        <AutoSize Enabled="True" MinHeight="250"></AutoSize>
                                        <Mode AllowDragRows="True" AllowUpload="True" InitNewRow="True"></Mode>
                                        <CallbackCommands PasteCommand="QuestionPasteLine"></CallbackCommands>
                                    </px:PXGrid>
                                </Template>
                            </px:PXTabItem>--%>
                            <px:PXTabItem Text="Print Log" BindingContext="VisibilityForm" VisibleExp="DataControls[&quot;chkShowPrintLog&quot;].Value == true">
                                <Template>
                                    <px:PXGrid ID="printLogGrid" runat="server" DataSourceID="ds" SkinID="Inquire" TabIndex="8700" Width="100%">
                                        <EmptyMsg AnonFilteredAddMessage="No records found.
Try to change filter to see records here."
                                            AnonFilteredMessage="No records found.
Try to change filter to see records here."
                                            ComboAddMessage="No records found.
Try to change filter or modify parameters above to see records here."
                                            FilteredAddMessage="No records found.
Try to change filter to see records here."
                                            FilteredMessage="No records found.
Try to change filter to see records here."
                                            NamedComboAddMessage="No records found as '{0}'.
Try to change filter or modify parameters above to see records here."
                                            NamedComboMessage="No records found as '{0}'.
Try to change filter or modify parameters above to see records here."
                                            NamedFilteredAddMessage="No records found as '{0}'.
Try to change filter to see records here."
                                            NamedFilteredMessage="No records found as '{0}'.
Try to change filter to see records here." />
                                        <Levels>
                                            <px:PXGridLevel DataKeyNames="RecordID" DataMember="PrintLog">
                                                <RowTemplate>
                                                    <px:PXNumberEdit ID="edRecordID" runat="server" AlreadyLocalized="False" DataField="RecordID" IsClientControl="True">
                                                    </px:PXNumberEdit>
                                                </RowTemplate>
                                                <Columns>
                                                    <px:PXGridColumn DataField="RecordID" TextAlign="Right" LinkCommand="ViewPrintLog" />
                                                    <px:PXGridColumn DataField="LabelKey" Width="150px" />
                                                    <px:PXGridColumn DataField="LabelFilename" Width="350px" />
                                                    <px:PXGridColumn DataField="UserID" Width="100px" />
                                                    <px:PXGridColumn DataField="PrinterID" Width="180px" LinkCommand="ViewPrinter" />
                                                    <px:PXGridColumn DataField="PrinterFormatID" Width="120px" LinkCommand="ViewFormat" />
                                                    <px:PXGridColumn DataField="PrintStationID" Width="120px" LinkCommand="ViewPrintStation" />
                                                    <px:PXGridColumn DataField="BAccountID" Width="140px" LinkCommand="ViewBAccount" />
                                                    <px:PXGridColumn DataField="InventoryID" Width="140px" LinkCommand="ViewInventory" />
                                                    <px:PXGridColumn DataField="LotSerialNbr" Width="140px" />
                                                    <px:PXGridColumn DataField="PrintJobID" Width="140px" />
                                                    <px:PXGridColumn DataField="CreatedDateTime" Width="90px" />
                                                </Columns>
                                            </px:PXGridLevel>
                                        </Levels>
                                        <AutoSize Enabled="True" />
                                    </px:PXGrid>
                                </Template>
                            </px:PXTabItem>
                        </Items>
                        <AutoSize Enabled="true" MinHeight="250" />
                    </px:PXTab>
                </Template>
                <AutoSize Enabled="True" />
            </px:PXFormView>
        </Template1>
        <Template2>
            <%--This PXTreeView makes PXDataSource.GetPath crash because no tree model is related to CurrentModel--%>
            <px:PXTreeView ID="tree" runat="server" DataSourceID="ds" DataMember="CurrentModel" Height="0"
                AutoRepaint="True" SyncPosition="True" SyncPositionWithGraph="True" PreserveExpanded="True" ExpandDepth="0" PopulateOnDemand="True"
                Caption="Sample" ShowRootNode="False" AllowCollapse="True">
                <CaptionStyle Height="20px" />
            </px:PXTreeView>
            <div class="content">
                <px:PXFormView ID="templateDataForm2" runat="server" DataMember="CurrentModel" DataSourceID="ds" TabIndex="200" SkinID="Transparent">
                    <ContentStyle CssClass="imgContainer"></ContentStyle>
                    <Template>
                        <px:PXImageView ID="edImageUrl" runat="server" DataField="ImageUrl" Style="width: 100%; height: auto;" AlreadyLocalized="False" CallbackUpdatable="True" />
                    </Template>
                </px:PXFormView>
            </div>
        </Template2>
    </px:PXSplitContainer>
    <style type="text/css">
        td.splitterRC div * {
            overflow: auto !important;
        }

        div.content * {
            display: block !important;
        }
    </style>
    <!--#include file="~\Pages\AL\Includes\LabelsChangeIDDialog.inc"-->
    <px:PXSmartPanel ID="PanelAddDataElements" runat="server" Width="873px" Key="dataElementFilter" Caption="Load Data Elements" CaptionVisible="True"
        LoadOnDemand="True" CallBackMode-CommitChanges="True" CallBackMode-PostData="Page" AutoCallBack-Command="Refresh" Height="400px"
        AutoRepaint="true" DesignView="Content" ShowAfterLoad="true">
        <px:PXFormView runat="server" ID="frmAddDataElementFilter" DataMember="DataElementFilter" DataSourceID="ds" SkinID="Transparent">
            <Template>
                <px:PXDropDown runat="server" ID="edDaElExprType" DataField="ExprType" CommitChanges="True" />
                <px:PXTextEdit runat="server" ID="edDaElBasedOn" DataField="BasedOn" CommitChanges="True" />
                <px:PXTextEdit runat="server" ID="edDaElExprValue" DataField="ExprValue" CommitChanges="True" />
                <px:PXCheckBox runat="server" ID="chkWithBarcode" DataField="WithBarcode" CommitChanges="True" />
                <px:PXLayoutRule runat="server" ID="frmDaElColumn2" StartColumn="True" />
                <px:PXSelector runat="server" ID="edDaElCategoryID" DataField="CategoryID" CommitChanges="True" />
                <px:PXSelector runat="server" ID="edDaElContentID" DataField="ContentID" CommitChanges="True" />
                <px:PXSelector runat="server" ID="edDaElSubstitutionID" DataField="SubstitutionID" CommitChanges="True" />
            </Template>
        </px:PXFormView>
        <px:PXGrid ID="gridDataElements" runat="server" Width="100%" DataSourceID="ds" BatchUpdate="True" Style="height: 250px;"
            AutoAdjustColumns="True" SkinID="Inquire" FilesIndicator="false" NoteIndicator="false">
            <CallbackCommands>
                <Refresh CommitChanges="true"></Refresh>
            </CallbackCommands>
            <%--<ClientEvents AfterCellUpdate="UpdateItemSiteCell" />--%>
            <ActionBar PagerVisible="False">
                <PagerSettings Mode="NextPrevFirstLast" />
            </ActionBar>
            <Levels>
                <px:PXGridLevel DataMember="SelectedDataElements">
                    <Columns>
                        <px:PXGridColumn AllowCheckAll="True" AllowNull="False" DataField="Selected" TextAlign="Center" Type="CheckBox" Width="80" />
                        <px:PXGridColumn DataField="Name" Width="220" />
                        <px:PXGridColumn DataField="Description" Width="280" />
                        <px:PXGridColumn DataField="ALDataSource__Name" Width="180" />
                        <px:PXGridColumn DataField="CategoryID" Width="180" />
                        <px:PXGridColumn DataField="BarcodeID" Width="180" />
                        <px:PXGridColumn DataField="SubstitutionID" Width="180" />
                        <px:PXGridColumn DataField="ContentID" Width="180" />
                        <px:PXGridColumn DataField="PrinterFileID" Width="180" />
                    </Columns>
                    <Layout ColumnsMenu="True" FormViewHeight="" />
                </px:PXGridLevel>
            </Levels>
            <AutoSize Enabled="True" />
            <Mode AllowAddNew="False" AllowDelete="False" />
        </px:PXGrid>
<%--        <px:PXPanel ID="PXPanelDataElementBtn" runat="server" SkinID="Buttons">
            <px:PXButton ID="PXButtonLoadDataElementCallCmd" runat="server" CommandName="AddSelectedDataElements" CommandSourceID="ds" Text="Add" SyncVisible="false" />
            <px:PXButton ID="PXButtonLoadDataElementOK" runat="server" Text="Add &amp; Close" DialogResult="OK" />
            <px:PXButton ID="PXButtonLoadDataElementCancel" runat="server" DialogResult="Cancel" Text="Cancel" />
        </px:PXPanel>--%>
    </px:PXSmartPanel>
</asp:Content>
