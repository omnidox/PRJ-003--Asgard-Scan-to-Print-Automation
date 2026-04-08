<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true"
    ValidateRequest="false" CodeFile="AL303500.aspx.cs" Inherits="Page_AL303500" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" TypeName="AA.Objects.Labels.ALDataElementMaint" PrimaryView="Document">
        <CallbackCommands>
            <px:PXDSCallbackCommand Name="Insert" PostData="Self" />
            <px:PXDSCallbackCommand CommitChanges="True" Name="Save" />
            <px:PXDSCallbackCommand Name="First" PostData="Self" StartNewGroup="true" />
            <px:PXDSCallbackCommand Name="Last" PostData="Self" />
        </CallbackCommands>
        <DataTrees>
            <px:PXTreeDataMember TreeView="EntityItemsBasedOn" TreeKeys="Key" />
            <px:PXTreeDataMember TreeView="EntityItemsExprValue" TreeKeys="Key" />
            <px:PXTreeDataMember TreeView="EntityItemsSampleBasedOn" TreeKeys="Key" />
            <px:PXTreeDataMember TreeView="EntityItemsSampleValue" TreeKeys="Key" />
        </DataTrees>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" Style="z-index: 100" Width="100%" DataMember="Document">
        <Template>
            <px:PXLayoutRule runat="server" StartRow="True" ColumnSpan="3" />
            <px:PXSelector runat="server" ID="edName" DataField="Name" CommitChanges="true" />
            <px:PXLayoutRule runat="server" Merge="true" />
            <px:PXCheckBox runat="server" ID="chkGenName" DataField="GenName" CommitChanges="true" />
            <px:PXCheckBox runat="server" ID="chkActive" DataField="Active" CommitChanges="true" />
            <px:PXCheckBox runat="server" ID="chkIsSystem" DataField="IsSystem" CommitChanges="true" />
            <px:PXLayoutRule runat="server" ColumnSpan="1" ControlSize="L"/>
            <px:PXDropDown runat="server" ID="edExprType" DataField="ExprType" CommitChanges="true" Width="120" />
            <px:PXSelector runat="server" ID="edSourceID" DataField="SourceID" CommitChanges="true" AllowEdit="true" AutoRefresh="true" />
            <px:PXTreeSelector ID="edBasedOn" runat="server" DataField="BasedOn" CommitChanges="true"
                TreeDataSourceID="ds" PopulateOnDemand="True" InitialExpandLevel="0"
                ShowRootNode="False" MinDropWidth="468" MaxDropWidth="600" AllowEditValue="True" AutoRefresh="True" TreeDataMember="EntityItemsBasedOn" AlreadyLocalized="False">
                <DataBindings>
                    <px:PXTreeItemBinding DataMember="EntityItemsBasedOn" TextField="Name" ValueField="Path" ImageUrlField="Icon" ToolTipField="Path" />
                </DataBindings>
            </px:PXTreeSelector>
            <px:PXTreeSelector ID="edExprValue" runat="server" DataField="ExprValue" CommitChanges="true"
                TreeDataSourceID="ds" PopulateOnDemand="True" InitialExpandLevel="0"
                ShowRootNode="False" MinDropWidth="468" MaxDropWidth="600" AllowEditValue="True" AutoRefresh="True" TreeDataMember="EntityItemsExprValue" AlreadyLocalized="False">
                <DataBindings>
                    <px:PXTreeItemBinding DataMember="EntityItemsExprValue" TextField="Name" ValueField="Path" ImageUrlField="Icon" ToolTipField="Path" />
                </DataBindings>
            </px:PXTreeSelector>
            <%--<px:PXTextEdit runat="server" ID="edDefaultValue" DataField="DefaultValue" CommitChanges="true" />--%>
            <px:PXSelector runat="server" ID="edSnippetID" DataField="SnippetID" CommitChanges="true" AllowEdit="true" />
            <px:PXSelector runat="server" ID="edContentID" DataField="ContentID" CommitChanges="true" AllowEdit="true" />
            <px:PXSelector runat="server" ID="edPrinterFileGUID" DataField="PrinterFileGUID" CommitChanges="true" AllowEdit="true" />
            <px:PXLayoutRule runat="server" StartColumn="True" ControlSize="L" />
            <px:PXLayoutRule runat="server" Merge="True" />
            <px:PXTextEdit runat="server" ID="lblArgName1" DataField="ArgName1" SuppressLabel="True" Enabled="False" CommitChanges="True" />
            <px:PXTextEdit runat="server" ID="edArg1" DataField="Arg1" SuppressLabel="True" CommitChanges="true" />
            <px:PXLayoutRule runat="server" StartColumn="True" ControlSize="L" />
            <px:PXLayoutRule runat="server" Merge="True" />
            <px:PXTextEdit runat="server" ID="lblArgName2" DataField="ArgName2" SuppressLabel="True" Enabled="False" CommitChanges="True" />
            <px:PXTextEdit runat="server" ID="edArg2" DataField="Arg2" SuppressLabel="True" CommitChanges="true" />
            <px:PXLayoutRule runat="server" StartColumn="True" ControlSize="L" />
            <px:PXLayoutRule runat="server" Merge="True" />
            <px:PXTextEdit runat="server" ID="lblArgName3" DataField="ArgName3" SuppressLabel="True" Enabled="False" CommitChanges="True" />
            <px:PXTextEdit runat="server" ID="edArg3" DataField="Arg3" SuppressLabel="True" CommitChanges="true" />
            <px:PXLayoutRule runat="server" StartColumn="True" ControlSize="L" />
            <px:PXLayoutRule runat="server" Merge="True" />
            <px:PXTextEdit runat="server" ID="lblArgName4" DataField="ArgName4" SuppressLabel="True" Enabled="False" CommitChanges="True" />
            <px:PXTextEdit runat="server" ID="edArg4" DataField="Arg4" SuppressLabel="True" CommitChanges="true" />
            <px:PXLayoutRule runat="server" StartColumn="True" ControlSize="L" />
            <px:PXLayoutRule runat="server" Merge="True" />
            <px:PXTextEdit runat="server" ID="lblArgName5" DataField="ArgName5" SuppressLabel="True" Enabled="False" CommitChanges="True" />
            <px:PXTextEdit runat="server" ID="edArg5" DataField="Arg5" SuppressLabel="True" CommitChanges="true" />
            <px:PXLayoutRule runat="server" StartColumn="True" ControlSize="L" />
            <px:PXLayoutRule runat="server" Merge="True" />
            <px:PXTextEdit runat="server" ID="lblArgName6" DataField="ArgName6" SuppressLabel="True" Enabled="False" CommitChanges="True" />
            <px:PXTextEdit runat="server" ID="edArg6" DataField="Arg6" SuppressLabel="True" CommitChanges="true" />
            <px:PXLayoutRule runat="server" StartColumn="True" ControlSize="L" />
            <px:PXCheckBox runat="server" ID="edDoSubstitute" DataField="DoSubstitute" CommitChanges="true" />
            <px:PXSelector runat="server" ID="edSubstitutionID" DataField="SubstitutionID" CommitChanges="true" AllowEdit="true" />
            <px:PXSelector runat="server" ID="edBarcodeID" DataField="BarcodeID" CommitChanges="true" AllowEdit="true" />
            <px:PXSelector runat="server" ID="edCategoryID" DataField="CategoryID" CommitChanges="true" AllowEdit="true" />
            <px:PXLayoutRule runat="server" StartColumn="True" ControlSize="L" />
            <px:PXDropDown runat="server" ID="edSampleType" DataField="SampleType" CommitChanges="true" AutoRefresh="true"/>
            <px:PXTreeSelector ID="edSampleBasedOn" runat="server" DataField="SampleBasedOn" CommitChanges="true"
                TreeDataSourceID="ds" PopulateOnDemand="True" InitialExpandLevel="0"
                ShowRootNode="False" MinDropWidth="468" MaxDropWidth="600" AllowEditValue="True" AutoRefresh="True" TreeDataMember="EntityItemsSampleBasedOn" AlreadyLocalized="False">
                <DataBindings>
                    <px:PXTreeItemBinding DataMember="EntityItemsSampleBasedOn" TextField="Name" ValueField="Path" ImageUrlField="Icon" ToolTipField="Path" />
                </DataBindings>
            </px:PXTreeSelector>
            <px:PXTreeSelector ID="edSampleValue" runat="server" DataField="SampleValue" CommitChanges="true"
                TreeDataSourceID="ds" PopulateOnDemand="True" InitialExpandLevel="0"
                ShowRootNode="False" MinDropWidth="468" MaxDropWidth="600" AllowEditValue="True" AutoRefresh="True" TreeDataMember="EntityItemsSampleValue" AlreadyLocalized="False">
                <DataBindings>
                    <px:PXTreeItemBinding DataMember="EntityItemsSampleValue" TextField="Name" ValueField="Path" ImageUrlField="Icon" ToolTipField="Path" />
                </DataBindings>
            </px:PXTreeSelector>
            <%--<px:PXTextEdit runat="server" ID="edDescription" DataField="Description" />--%>
            <px:PXUploadDialog ID="uploadFileDialog" runat="server" AutoSaveFile="true" Caption="Upload XML Zip" Key="Document"
                Height="110px" SessionKey="XmlUploadAllEntities" AllowedFileTypes=".zip;" />
        </Template>
    </px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXTab ID="tab" runat="server" Width="100%" Height="150px" DataSourceID="ds">
        <Items>
            <px:PXTabItem Text="Used By Models" BindingContext="form">
                <Template>
                    <px:PXGrid ID="gridModels" runat="server" AllowPaging="False" DataSourceID="ds" SkinID="Inquire" Width="100%"
                        Height="150px" MatrixMode="True">
                        <Levels>
                            <px:PXGridLevel DataMember="UsedByModels">
                                <Columns>
                                    <px:PXGridColumn DataField="Active" AllowNull="False" TextAlign="Center" Type="CheckBox" />
                                    <px:PXGridColumn DataField="ALModel__Name" Width="350px" LinkCommand="ViewModel" />
                                    <px:PXGridColumn DataField="ALModel__Description" Width="300px" />
                                    <px:PXGridColumn DataField="LineNbr" Width="150px" />
                                    <px:PXGridColumn DataField="ALDataElement__Name" Width="300px" LinkCommand="ViewDataElement" />
                                    <px:PXGridColumn DataField="ExprValue" Width="300px" />
                                    <px:PXGridColumn DataField="LastModifiedByID" Width="150px" />
                                    <px:PXGridColumn DataField="LastModifiedDateTime" Width="150px" />
                                </Columns>
                                <Layout FormViewHeight="" />
                            </px:PXGridLevel>
                        </Levels>
                        <AutoSize Enabled="True" MinHeight="150" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="Used By Contents" BindingContext="form">
                <Template>
                    <px:PXGrid ID="gridContents" runat="server" AllowPaging="False" DataSourceID="ds" SkinID="Inquire" Width="100%"
                        Height="150px" MatrixMode="True">
                        <Levels>
                            <px:PXGridLevel DataMember="UsedByContents">
                                <Columns>
                                    <px:PXGridColumn DataField="ALContent__Name" Width="350px" LinkCommand="ViewModel" />
                                    <px:PXGridColumn DataField="ALContent__Description" Width="300px" />
                                    <px:PXGridColumn DataField="Active" AllowNull="False" TextAlign="Center" Type="CheckBox" />
                                    <px:PXGridColumn DataField="LastModifiedByID" Width="150px" />
                                    <px:PXGridColumn DataField="LastModifiedDateTime" Width="150px" />
                                </Columns>
                                <Layout FormViewHeight="" />
                            </px:PXGridLevel>
                        </Levels>
                        <AutoSize Enabled="True" MinHeight="150" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
        </Items>
        <AutoSize Container="Window" Enabled="True" MinHeight="150" />
    </px:PXTab>
</asp:Content>
