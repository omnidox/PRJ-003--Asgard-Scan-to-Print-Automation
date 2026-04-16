<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true"
    ValidateRequest="false" CodeFile="AL303000.aspx.cs" Inherits="Page_AL303000" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" TypeName="AA.Objects.AL.ALDataSourceMaint" PrimaryView="Document">
        <CallbackCommands>
            <px:PXDSCallbackCommand Name="ImportAll" Visible="False" />
            <px:PXDSCallbackCommand Name="ExportAll" Visible="False" />
            <px:PXDSCallbackCommand Name="ToggleExport" Visible="False" />
            <px:PXDSCallbackCommand Name="ImportFiles" Visible="False" />
        </CallbackCommands>
        <DataTrees>
            <px:PXTreeDataMember TreeView="EntityItemsScreenBasedOn" TreeKeys="Key" />
            <px:PXTreeDataMember TreeView="EntityItemsFunctionBasedOn" TreeKeys="Key" />
            <px:PXTreeDataMember TreeView="EntityItemsScreenChildren" TreeKeys="Key" />
            <px:PXTreeDataMember TreeView="EntityItemsFunctionChildren" TreeKeys="Key" />
        </DataTrees>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" Style="z-index: 100" Width="100%" DataMember="Document">
        <Template>
            <px:PXLayoutRule runat="server" LabelsWidth="S" ControlSize="XM" StartColumn="True" StartRow="True" />
            <px:PXSelector ID="edName" runat="server" DataField="Name" DataSourceID="ds" />
            <px:PXTextEdit ID="edDescription" runat="server" DataField="Description" />
            <px:PXSelector ID="edScreenID" runat="server" DataField="ScreenID" FilterByAllFields="true" CommitChanges="True" />
            <px:PXTextEdit ID="edGraphType" runat="server" DataField="GraphType" CommitChanges="true" />
            <px:PXLayoutRule runat="server" StartColumn="True" />
            <px:PXCheckBox ID="chkActive" runat="server" DataField="Active" CommitChanges="true" />
            <px:PXCheckBox ID="chkIsSystem" runat="server" Checked="True" DataField="IsSystem" CommitChanges="true" />
            <px:PXCheckBox ID="chkExport" runat="server" DataField="AllowExport" CommitChanges="true" />
            <px:PXFormView ID="VisibilityForm" runat="server" DataMember="CurrentDocument" DataSourceID="ds" Caption="Hidden Form needed for VisibleExp of TabItems"
                Visible="False" TabIndex="300">
                <Template>
                    <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="M" ControlSize="XM" />
                    <px:PXCheckBox ID="chkIsGlobal" runat="server" DataField="IsGlobal" AlreadyLocalized="False" IsClientControl="True" />
                    <px:PXCheckBox ID="chkIsScreenBased" runat="server" DataField="IsScreenBased" AlreadyLocalized="False" IsClientControl="True" />
                    <px:PXCheckBox ID="chkShowImages" runat="server" DataField="ShowImages" AlreadyLocalized="False" IsClientControl="True" />
                    <px:PXCheckBox ID="chkShowFixed" runat="server" DataField="ShowFixed" AlreadyLocalized="False" IsClientControl="True" />
                    <px:PXCheckBox ID="chkShowFunctions" runat="server" DataField="ShowFunctions" AlreadyLocalized="False" IsClientControl="True" />
                    <px:PXCheckBox ID="chkShowScreens" runat="server" DataField="ShowScreens" AlreadyLocalized="False" IsClientControl="True" />
                    <%--<px:PXCheckBox ID="chkShowContents" runat="server" DataField="ShowContents" AlreadyLocalized="False" IsClientControl="True" />--%>
                    <px:PXCheckBox ID="chkShowIterators" runat="server" DataField="ShowIterators" AlreadyLocalized="False" IsClientControl="True" />
                    <px:PXCheckBox ID="chkShowScripts" runat="server" DataField="ShowScripts" AlreadyLocalized="False" IsClientControl="True" />
                </Template>
            </px:PXFormView>
            <px:PXUploadDialog ID="uploadFileDialog" runat="server" AutoSaveFile="true" Caption="Upload XML Zip" Key="Document"
                Height="110px" SessionKey="XmlUploadAllEntities" AllowedFileTypes=".zip;" />
        </Template>
    </px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXTab ID="tab" runat="server" Width="100%" Height="150px" DataSourceID="ds">
        <Items>
            <px:PXTabItem Text="Contents">
                <Template>
                    <px:PXGrid ID="contentElementsGrid" runat="server" DataSourceID="ds" Height="600px" Style="border: 0px;"
                        Width="100%" ActionsPosition="Top" SkinID="Details" KeepPosition="True" SyncPosition="True" FilesIndicator="False"
                        RepaintColumns="True" AutoRepaint="True">
                        <Levels>
                            <px:PXGridLevel DataMember="ContentElements" DataKeyNames="SourceID,LineNbr">
                                <RowTemplate>
                                    <px:PXSelector ID="edContentID" runat="server" DataField="ContentID" CommitChanges="True" AllowEdit="True" edit="1" />
                                    <px:PXSelector ID="edBarcodeIDForContent" runat="server" DataField="BarcodeID" CommitChanges="True" AllowEdit="True" edit="1" />
                                    <px:PXSelector ID="edSubstitutionIDForContent" runat="server" DataField="SubstitutionID" CommitChanges="True" AllowEdit="True" edit="1" />
                                    <px:PXSelector ID="edCategoryIDForContent" runat="server" DataField="CategoryID" CommitChanges="True" AllowEdit="True" edit="1" />
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="SourceID" />
                                    <px:PXGridColumn DataField="Active" TextAlign="Center" Type="CheckBox" Width="60px" CommitChanges="True"/>
                                    <px:PXGridColumn DataField="IsSystem" TextAlign="Center" Type="CheckBox" Width="70px" CommitChanges="True"/>
                                    <px:PXGridColumn DataField="GenName" TextAlign="Center" Type="CheckBox" Width="60px" CommitChanges="True"/>
                                    <px:PXGridColumn DataField="LineNbr" TextAlign="Right" />
                                    <px:PXGridColumn DataField="CategoryID" Width="150px" MatrixMode="False" CommitChanges="True" AllowDragDrop="True" />
                                    <px:PXGridColumn DataField="Name" Width="300px" AllowDragDrop="True" LinkCommand="ViewDataElement"/>
                                    <px:PXGridColumn DataField="ContentID" Width="300px" CommitChanges="True" AllowDragDrop="True" />
                                    <px:PXGridColumn DataField="BarcodeID" Width="250px" MatrixMode="False" CommitChanges="True" AllowDragDrop="True" />
                                    <px:PXGridColumn DataField="LastModifiedByID" Width="150px" />
                                    <px:PXGridColumn DataField="LastModifiedDateTime" Width="150px" />
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <ActionBar>
                            <CustomItems>
                                <px:PXToolBarButton Text="Duplicate" >
                                    <AutoCallBack Command="DuplicateRowContent" Target="ds"></AutoCallBack>
                                </px:PXToolBarButton>
                            </CustomItems>
                        </ActionBar>
                        <Mode AllowDragRows="True" InitNewRow="True" AllowUpload="True" />
                        <CallbackCommands PasteCommand="DataElementContentPasteLine" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="Functions" BindingContext="VisibilityForm" VisibleExp="DataControls[&quot;chkShowFunctions&quot;].Value == true">
                <Template>
                    <px:PXGrid ID="functionElementsGrid" runat="server" DataSourceID="ds" Height="600px" Style="border: 0px;"
                        Width="100%" ActionsPosition="Top" SkinID="Details" KeepPosition="True" SyncPosition="True" FilesIndicator="False"
                        RepaintColumns="True" AutoRepaint="True" MatrixMode="true">
                        <Levels>
                            <px:PXGridLevel DataMember="FunctionElements">
                                <RowTemplate>
                                    <px:PXTreeSelector ID="edBasedOnForFunction" runat="server" Width="90px" DataField="BasedOn" CommitChanges="true"
                                        TreeDataSourceID="ds" PopulateOnDemand="True" InitialExpandLevel="0"
                                        ShowRootNode="False" MinDropWidth="468" MaxDropWidth="600" AllowEditValue="True" AutoRefresh="True" TreeDataMember="EntityItemsFunctionBasedOn" AlreadyLocalized="False">
                                        <DataBindings>
                                            <px:PXTreeItemBinding DataMember="EntityItemsFunctionBasedOn" TextField="Name" ValueField="Path" ImageUrlField="Icon" ToolTipField="Path" />
                                        </DataBindings>
                                    </px:PXTreeSelector>
                                    <px:PXTreeSelector ID="edExprValueForFunction" runat="server" DataField="ExprValue" CommitChanges="true"
                                        TreeDataSourceID="ds" PopulateOnDemand="True" InitialExpandLevel="0"
                                        ShowRootNode="False" MinDropWidth="468" MaxDropWidth="600" AllowEditValue="True" AutoRefresh="True" TreeDataMember="EntityItemsFunctionChildren" AlreadyLocalized="False">
                                        <DataBindings>
                                            <px:PXTreeItemBinding DataMember="EntityItemsFunctionChildren" TextField="Name" ValueField="SubKey" ImageUrlField="Icon" ToolTipField="Path" />
                                        </DataBindings>
                                    </px:PXTreeSelector>
                                    <px:PXSelector ID="edBarcodeIDForFunction" runat="server" DataField="BarcodeID" CommitChanges="True" AllowEdit="True" edit="1" />
                                    <px:PXSelector ID="edSubstitutionIDForFunction" runat="server" DataField="SubstitutionID" CommitChanges="True" AllowEdit="True" edit="1" />
                                    <px:PXTextEdit ID="edArg1ForFunction" runat="server" DataField="Arg1" CommitChanges="true" />
                                    <px:PXTextEdit ID="edArg2ForFunction" runat="server" DataField="Arg2" CommitChanges="true" />
                                    <px:PXTextEdit ID="edArg3ForFunction" runat="server" DataField="Arg3" CommitChanges="true" />
                                    <px:PXTextEdit ID="edArg4ForFunction" runat="server" DataField="Arg4" CommitChanges="true" />
                                    <px:PXTextEdit ID="edArg5ForFunction" runat="server" DataField="Arg5" CommitChanges="true" />
                                    <px:PXTextEdit ID="edArg6ForFunction" runat="server" DataField="Arg6" CommitChanges="true" />
                                    <px:PXSelector ID="edCategoryIDForFunction" runat="server" DataField="CategoryID" CommitChanges="True" AllowEdit="True" edit="1" />
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="SourceID" />
                                    <px:PXGridColumn DataField="Active" TextAlign="Center" Type="CheckBox" Width="60px" CommitChanges="True"/>
                                    <px:PXGridColumn DataField="IsSystem" TextAlign="Center" Type="CheckBox" Width="70px" CommitChanges="True"/>
                                    <px:PXGridColumn DataField="GenName" TextAlign="Center" Type="CheckBox" Width="60px" CommitChanges="True"/>
                                    <px:PXGridColumn DataField="LineNbr" TextAlign="Right" />
                                    <px:PXGridColumn DataField="CategoryID" Width="150px" MatrixMode="False" CommitChanges="True" AllowDragDrop="True" />
                                    <px:PXGridColumn DataField="Name" Width="350px" AllowDragDrop="True" LinkCommand="ViewDataElement"/>
                                    <px:PXGridColumn DataField="BasedOn" Width="90px" CommitChanges="True" AllowDragDrop="True" />
                                    <px:PXGridColumn DataField="ExprValue" Width="120px" CommitChanges="True" AllowDragDrop="True" />
                                    <px:PXGridColumn DataField="Arg1" Width="170px" CommitChanges="True" AllowDragDrop="True" />
                                    <px:PXGridColumn DataField="Arg2" Width="170px" CommitChanges="True" AllowDragDrop="True" />
                                    <px:PXGridColumn DataField="Arg3" Width="170px" CommitChanges="True" AllowDragDrop="True" />
                                    <px:PXGridColumn DataField="Arg4" Width="100px" CommitChanges="True" AllowDragDrop="True" />
                                    <px:PXGridColumn DataField="Arg5" Width="100px" CommitChanges="True" AllowDragDrop="True" />
                                    <px:PXGridColumn DataField="Arg6" Width="100px" CommitChanges="True" AllowDragDrop="True" />
                                    <px:PXGridColumn DataField="SampleType" Width="90px" CommitChanges="True" Type="DropDownList" AllowDragDrop="True" />
                                    <px:PXGridColumn DataField="SampleBasedOn" Width="100px" CommitChanges="True" AllowDragDrop="True" />
                                    <px:PXGridColumn DataField="SampleValue" Width="100px" CommitChanges="True" AllowDragDrop="True" />
                                    <px:PXGridColumn DataField="DoSubstitute" Width="120px" AllowNull="False" TextAlign="Center" Type="CheckBox" AllowDragDrop="True" CommitChanges="True" />
                                    <px:PXGridColumn DataField="SubstitutionID" Width="150px" MatrixMode="False" CommitChanges="True" AllowDragDrop="True" />
                                    <px:PXGridColumn DataField="BarcodeID" Width="150px" MatrixMode="False" CommitChanges="True" AllowDragDrop="True" />
                                    <px:PXGridColumn DataField="LastModifiedByID" Width="150px" />
                                    <px:PXGridColumn DataField="LastModifiedDateTime" Width="150px" />
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <ActionBar>
                            <CustomItems>
                                <px:PXToolBarButton Text="Duplicate" >
                                    <AutoCallBack Command="DuplicateRowFunction" Target="ds"></AutoCallBack>
                                </px:PXToolBarButton>
                            </CustomItems>
                        </ActionBar>
                        <Mode AllowDragRows="True" InitNewRow="True" AllowUpload="True" />
                        <CallbackCommands PasteCommand="DataElementFunctionPasteLine" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="Screens" BindingContext="VisibilityForm" VisibleExp="DataControls[&quot;chkShowScreens&quot;].Value == true">
                <Template>
                    <px:PXGrid ID="screenElementGrid" runat="server" DataSourceID="ds" Height="600px" Style="border: 0px;"
                        Width="100%" ActionsPosition="Top" SkinID="Details" KeepPosition="True" SyncPosition="True" FilesIndicator="False"
                        RepaintColumns="True" AutoRepaint="True">
                        <Levels>
                            <px:PXGridLevel DataMember="ScreenElements" DataKeyNames="SourceID,LineNbr">
                                <RowTemplate>
                                    <px:PXTreeSelector ID="edBasedOnForScreen" runat="server" DataField="BasedOn" CommitChanges="True"
                                        TreeDataSourceID="ds" PopulateOnDemand="True" InitialExpandLevel="0"
                                        ShowRootNode="False" MinDropWidth="468" MaxDropWidth="600" AllowEditValue="True" AutoRefresh="True" TreeDataMember="EntityItemsScreenBasedOn" AlreadyLocalized="False">
                                        <DataBindings>
                                            <px:PXTreeItemBinding DataMember="EntityItemsScreenBasedOn" TextField="Name" ValueField="Path" ImageUrlField="Icon" ToolTipField="Path"></px:PXTreeItemBinding>
                                        </DataBindings>
                                    </px:PXTreeSelector>
                                    <px:PXTreeSelector ID="edExprValueForScreen" runat="server" DataField="ExprValue"
                                        TreeDataSourceID="ds" PopulateOnDemand="True" InitialExpandLevel="0" CommitChanges="true"
                                        ShowRootNode="False" MinDropWidth="468" MaxDropWidth="600" AllowEditValue="True" AutoRefresh="True" TreeDataMember="EntityItemsScreenChildren" AlreadyLocalized="False">
                                        <DataBindings>
                                            <px:PXTreeItemBinding DataMember="EntityItemsScreenChildren" TextField="Name" ValueField="Path" ImageUrlField="Icon" ToolTipField="Path"></px:PXTreeItemBinding>
                                        </DataBindings>
                                    </px:PXTreeSelector>
                                    <px:PXSelector ID="edBarcodeIDForScreen" runat="server" DataField="BarcodeID" CommitChanges="True" AllowEdit="True" edit="1" />
                                    <px:PXSelector ID="edSubstitutionIDForScreen" runat="server" DataField="SubstitutionID" CommitChanges="True" AllowEdit="True" edit="1" />
                                    <px:PXSelector ID="edCategoryIDForScreen" runat="server" DataField="CategoryID" CommitChanges="True" AllowEdit="True" edit="1" />
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="SourceID" />
                                    <px:PXGridColumn DataField="Active" TextAlign="Center" Type="CheckBox" Width="60px" CommitChanges="True"/>
                                    <px:PXGridColumn DataField="IsSystem" TextAlign="Center" Type="CheckBox" Width="70px" CommitChanges="True"/>
                                    <px:PXGridColumn DataField="GenName" TextAlign="Center" Type="CheckBox" Width="60px" CommitChanges="True"/>
                                    <px:PXGridColumn DataField="LineNbr" TextAlign="Right" />
                                    <px:PXGridColumn DataField="CategoryID" Width="150px" MatrixMode="False" CommitChanges="True" AllowDragDrop="True" />
                                    <px:PXGridColumn DataField="Name" Width="300px" AllowDragDrop="True" LinkCommand="ViewDataElement"/>
                                    <px:PXGridColumn DataField="BasedOn" Width="150px" CommitChanges="True" AllowDragDrop="True" />
                                    <px:PXGridColumn DataField="ExprValue" Width="300px" CommitChanges="True" AllowDragDrop="True" />
                                    <px:PXGridColumn DataField="SampleType" Width="90px" CommitChanges="True" Type="DropDownList" AllowDragDrop="True" />
                                    <px:PXGridColumn DataField="SampleBasedOn" Width="150px" CommitChanges="True" AllowDragDrop="True" />
                                    <px:PXGridColumn DataField="SampleValue" Width="200px" CommitChanges="True" AllowDragDrop="True" />
                                    <px:PXGridColumn DataField="BarcodeID" Width="150px" MatrixMode="False" CommitChanges="True" AllowDragDrop="True" />
                                    <px:PXGridColumn DataField="DoSubstitute" Width="120px" AllowNull="False" TextAlign="Center" Type="CheckBox" AllowDragDrop="True" CommitChanges="True" />
                                    <px:PXGridColumn DataField="SubstitutionID" Width="150px" MatrixMode="False" CommitChanges="True" AllowDragDrop="True" />
                                    <px:PXGridColumn DataField="LastModifiedByID" Width="150px" />
                                    <px:PXGridColumn DataField="LastModifiedDateTime" Width="150px" />
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <ActionBar>
                            <CustomItems>
                                <px:PXToolBarButton Text="Duplicate" >
                                    <AutoCallBack Command="DuplicateRowScreen" Target="ds"></AutoCallBack>
                                </px:PXToolBarButton>
                            </CustomItems>
                        </ActionBar>
                        <Mode AllowDragRows="True" InitNewRow="True" AllowUpload="True" />
                        <CallbackCommands PasteCommand="DataElementScreenPasteLine" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="Hardcoded" BindingContext="VisibilityForm" VisibleExp="DataControls[&quot;chkShowFixed&quot;].Value == true">
                <Template>
                    <px:PXGrid ID="fixedElementGrid" runat="server" DataSourceID="ds" Height="600px" Style="border: 0px;"
                        Width="100%" ActionsPosition="Top" SkinID="Details" KeepPosition="True" SyncPosition="True" FilesIndicator="False"
                        RepaintColumns="True" AutoRepaint="True">
                        <Levels>
                            <px:PXGridLevel DataMember="FixedElements" DataKeyNames="SourceID,LineNbr">
                                <RowTemplate>
                                    <px:PXSelector ID="edCategoryIDForFixed" runat="server" DataField="CategoryID" Width="200px" CommitChanges="True" AllowEdit="True" edit="1" />
                                    <px:PXSelector ID="edBarcodeIDForFixed" runat="server" DataField="BarcodeID" Width="200px" CommitChanges="True" AllowEdit="True" edit="1" />
                                    <px:PXSelector ID="edSubstitutionIDForFixed" runat="server" DataField="SubstitutionID" Width="200px" CommitChanges="True" AllowEdit="True" edit="1" />
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="SourceID" />
                                    <px:PXGridColumn DataField="Active" TextAlign="Center" Type="CheckBox" Width="60px" CommitChanges="True"/>
                                    <px:PXGridColumn DataField="IsSystem" TextAlign="Center" Type="CheckBox" Width="70px" CommitChanges="True"/>
                                    <px:PXGridColumn DataField="GenName" TextAlign="Center" Type="CheckBox" Width="60px" CommitChanges="True"/>
                                    <px:PXGridColumn DataField="LineNbr" TextAlign="Right" />
                                    <px:PXGridColumn DataField="CategoryID" Width="150px" MatrixMode="False" CommitChanges="True" AllowDragDrop="True" />
                                    <px:PXGridColumn DataField="ExprValue" Width="300px" CommitChanges="True" AllowDragDrop="True" />
                                    <px:PXGridColumn DataField="Name" Width="140px" AllowDragDrop="True" LinkCommand="ViewDataElement"/>
                                    <px:PXGridColumn DataField="DoSubstitute" AllowNull="False" TextAlign="Center" Type="CheckBox" Width="120px" AllowDragDrop="True" CommitChanges="True" />
                                    <px:PXGridColumn DataField="SubstitutionID" Width="150px" MatrixMode="False" CommitChanges="True" AllowDragDrop="True" />
                                    <px:PXGridColumn DataField="BarcodeID" Width="150px" MatrixMode="False" CommitChanges="True" AllowDragDrop="True" />
                                    <px:PXGridColumn DataField="LastModifiedByID" Width="150px" />
                                    <px:PXGridColumn DataField="LastModifiedDateTime" Width="150px" />
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <ActionBar>
                            <CustomItems>
                                <px:PXToolBarButton Text="Duplicate" >
                                    <AutoCallBack Command="DuplicateRowFixed" Target="ds"></AutoCallBack>
                                </px:PXToolBarButton>
                            </CustomItems>
                        </ActionBar>
                        <Mode AllowDragRows="True" InitNewRow="True" AllowUpload="True" />
                        <CallbackCommands PasteCommand="DataElementFixedPasteLine" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="Images" BindingContext="VisibilityForm" VisibleExp="DataControls[&quot;chkShowImages&quot;].Value == true">
                <Template>
                    <px:PXGrid ID="imageElementsGrid" runat="server" DataSourceID="ds" Height="600px" Style="border: 0px;"
                        Width="100%" ActionsPosition="Top" SkinID="Details" KeepPosition="True" SyncPosition="True" FilesIndicator="False"
                        RepaintColumns="True" AutoRepaint="True">
                        <Levels>
                            <px:PXGridLevel DataMember="ImageElements" DataKeyNames="SourceID,LineNbr">
                                <RowTemplate>
                                    <px:PXSelector ID="edPrinterFileGUID" runat="server" DataField="PrinterFileGUID" Width="200px" CommitChanges="True" AllowEdit="True" edit="1" />
                                    <px:PXSelector ID="edSubstitutionIDForImage" runat="server" DataField="SubstitutionID" Width="200px" CommitChanges="True" AllowEdit="True" edit="1" />
                                    <px:PXSelector ID="edCategoryIDForImage" runat="server" DataField="CategoryID" Width="200px" CommitChanges="True" AllowEdit="True" edit="1" />
                                    <px:PXDropDown ID="edArg2ForImage" runat="server" DataField="Arg2" Width="200px" CommitChanges="True" AllowEdit="True" edit="1" />
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="SourceID" />
                                    <px:PXGridColumn DataField="Active" TextAlign="Center" Type="CheckBox" Width="60px" CommitChanges="True"/>
                                    <px:PXGridColumn DataField="IsSystem" TextAlign="Center" Type="CheckBox" Width="70px" CommitChanges="True"/>
                                    <px:PXGridColumn DataField="GenName" TextAlign="Center" Type="CheckBox" Width="60px" CommitChanges="True"/>
                                    <px:PXGridColumn DataField="LineNbr" TextAlign="Right" />
                                    <px:PXGridColumn DataField="CategoryID" Width="150px" MatrixMode="False" CommitChanges="True" AllowDragDrop="True" />
                                    <px:PXGridColumn DataField="PrinterFileGUID" Width="250px" CommitChanges="True" AllowDragDrop="True" LinkCommand="ViewPrinterFile" />
                                    <px:PXGridColumn DataField="Arg1" Width="200px" CommitChanges="True" AllowDragDrop="True" />
                                    <px:PXGridColumn DataField="Arg2" Width="200px" CommitChanges="True" AllowDragDrop="True" />
                                    <px:PXGridColumn DataField="Name" Width="250px" AllowDragDrop="True" LinkCommand="ViewDataElement"/>
                                    <px:PXGridColumn DataField="LastModifiedByID" Width="150px" />
                                    <px:PXGridColumn DataField="LastModifiedDateTime" Width="150px" />
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <ActionBar>
                            <CustomItems>
                                <px:PXToolBarButton Text="Duplicate" >
                                    <AutoCallBack Command="DuplicateRowImage" Target="ds"></AutoCallBack>
                                </px:PXToolBarButton>
                            </CustomItems>
                        </ActionBar>
                        <Mode AllowDragRows="True" InitNewRow="True" AllowUpload="True" />
                        <CallbackCommands PasteCommand="DataElementImagePasteLine" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="Iterators" BindingContext="VisibilityForm" VisibleExp="DataControls[&quot;chkShowIterators&quot;].Value == true">
                <Template>
                    <px:PXGrid ID="iteratorElementGrid" runat="server" DataSourceID="ds" Height="600px" Style="border: 0px;"
                        Width="100%" ActionsPosition="Top" SkinID="Details" KeepPosition="True" SyncPosition="True" FilesIndicator="False"
                        RepaintColumns="True" AutoRepaint="True">
                        <Levels>
                            <px:PXGridLevel DataMember="IteratorElements" DataKeyNames="SourceID,LineNbr">
                                <RowTemplate>
                                    <px:PXSelector ID="edCategoryIDForIterator" runat="server" DataField="CategoryID" CommitChanges="True" AllowEdit="True" edit="1" />
                                    <px:PXSelector ID="edSnippetIDForIterator" runat="server" DataField="SnippetID" CommitChanges="True" AllowEdit="True" edit="1" />
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="SourceID" />
                                    <px:PXGridColumn DataField="Active" TextAlign="Center" Type="CheckBox" Width="60px" CommitChanges="True"/>
                                    <px:PXGridColumn DataField="IsSystem" TextAlign="Center" Type="CheckBox" Width="70px" CommitChanges="True"/>
                                    <px:PXGridColumn DataField="GenName" TextAlign="Center" Type="CheckBox" Width="60px" CommitChanges="True"/>
                                    <px:PXGridColumn DataField="LineNbr" TextAlign="Right" />
                                    <px:PXGridColumn DataField="CategoryID" Width="150px" MatrixMode="False" CommitChanges="True" AllowDragDrop="True" />
                                    <px:PXGridColumn DataField="Name" Width="300px" AllowDragDrop="True" LinkCommand="ViewDataElement"/>
                                    <px:PXGridColumn DataField="SnippetID" Width="150px" MatrixMode="False" CommitChanges="True" AllowDragDrop="True" />
                                    <px:PXGridColumn DataField="Arg1" Width="130px" MatrixMode="False" CommitChanges="True" TextAlign="Right" AllowDragDrop="True" />
                                    <px:PXGridColumn DataField="Arg2" Width="170px" MatrixMode="False" CommitChanges="True" TextAlign="Right" AllowDragDrop="True" />
                                    <px:PXGridColumn DataField="Arg3" Width="130px" MatrixMode="False" CommitChanges="True" TextAlign="Right" AllowDragDrop="True" />
                                    <px:PXGridColumn DataField="Arg4" Width="200px" MatrixMode="False" CommitChanges="True" TextAlign="Right" AllowDragDrop="True" />
                                    <px:PXGridColumn DataField="Arg5" Width="170px" MatrixMode="False" CommitChanges="True" TextAlign="Right" AllowDragDrop="True" />
                                    <px:PXGridColumn DataField="LastModifiedByID" Width="150px" />
                                    <px:PXGridColumn DataField="LastModifiedDateTime" Width="150px" />
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <ActionBar>
                            <CustomItems>
                                <px:PXToolBarButton Text="Duplicate" >
                                    <AutoCallBack Command="DuplicateRowIterator" Target="ds"></AutoCallBack>
                                </px:PXToolBarButton>
                            </CustomItems>
                        </ActionBar>
                        <Mode AllowDragRows="True" InitNewRow="True" AllowUpload="True" />
                        <CallbackCommands PasteCommand="DataElementIteratorPasteLine" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="Scripts" BindingContext="VisibilityForm" VisibleExp="DataControls[&quot;chkShowScripts&quot;].Value == true">
                <Template>
                    <px:PXGrid ID="scriptElementGrid" runat="server" DataSourceID="ds" Height="600px" Style="border: 0px;"
                        Width="100%" ActionsPosition="Top" SkinID="Details" KeepPosition="True" SyncPosition="True" FilesIndicator="False"
                        RepaintColumns="True" AutoRepaint="True">
                        <Levels>
                            <px:PXGridLevel DataMember="ScriptElements" DataKeyNames="SourceID,LineNbr">
                                <RowTemplate>
                                    <px:PXSelector ID="edCategoryIDForScript" runat="server" DataField="CategoryID" Width="200px" CommitChanges="True" AllowEdit="True" edit="1" />
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="SourceID" />
                                    <px:PXGridColumn DataField="Active" TextAlign="Center" Type="CheckBox" Width="60px" CommitChanges="True"/>
                                    <px:PXGridColumn DataField="IsSystem" TextAlign="Center" Type="CheckBox" Width="70px" CommitChanges="True"/>
                                    <px:PXGridColumn DataField="GenName" TextAlign="Center" Type="CheckBox" Width="60px" CommitChanges="True"/>
                                    <px:PXGridColumn DataField="LineNbr" TextAlign="Right" />
                                    <px:PXGridColumn DataField="CategoryID" Width="150px" MatrixMode="False" CommitChanges="True" AllowDragDrop="True" />
                                    <px:PXGridColumn DataField="ExprValue" Width="800px" CommitChanges="True" AllowDragDrop="True" />
                                    <px:PXGridColumn DataField="Name" Width="140px" AllowDragDrop="True" LinkCommand="ViewDataElement"/>
                                    <px:PXGridColumn DataField="LastModifiedByID" Width="150px" />
                                    <px:PXGridColumn DataField="LastModifiedDateTime" Width="150px" />
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <ActionBar>
                            <CustomItems>
                                <px:PXToolBarButton Text="Duplicate" >
                                    <AutoCallBack Command="DuplicateRowScript" Target="ds"></AutoCallBack>
                                </px:PXToolBarButton>
                            </CustomItems>
                        </ActionBar>
                        <Mode AllowDragRows="True" InitNewRow="True" AllowUpload="True" />
                        <CallbackCommands PasteCommand="DataElementScriptPasteLine" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
        </Items>
        <AutoSize Container="Window" Enabled="True" MinHeight="150" />
    </px:PXTab>
    <!--#include file="~\Pages\AL\Includes\LabelsChangeIDDialog.inc"-->
</asp:Content>
