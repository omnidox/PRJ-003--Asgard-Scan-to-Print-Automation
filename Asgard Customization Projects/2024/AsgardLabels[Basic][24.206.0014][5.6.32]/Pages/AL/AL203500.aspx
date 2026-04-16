<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="AL203500.aspx.cs" Inherits="Page_AL203500" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>
<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" TypeName="AA.Objects.AL.ALRuleMaint" PrimaryView="Document" PageLoadBehavior="SearchSavedKeys">
        <CallbackCommands>
            <px:PXDSCallbackCommand Name="Insert" PostData="Self" />
            <px:PXDSCallbackCommand CommitChanges="True" Name="Save" />
            <px:PXDSCallbackCommand Name="PasteLine" Visible="False" />
            <px:PXDSCallbackCommand Name="ResetOrder" Visible="False" />
            <px:PXDSCallbackCommand Name="First" PostData="Self" StartNewGroup="true" />
            <px:PXDSCallbackCommand Name="Last" PostData="Self" />
            <px:PXDSCallbackCommand Name="ImportAll" Visible="False" />
            <px:PXDSCallbackCommand Name="ExportAll" Visible="False" />
            <px:PXDSCallbackCommand Name="ToggleExport" Visible="False" />
            <px:PXDSCallbackCommand Name="ImportFiles" Visible="False" />
            <px:PXDSCallbackCommand Name="RefreshComposite" Visible="True" />
            <px:PXDSCallbackCommand Name="Duplicate" Visible="True" />
            <px:PXDSCallbackCommand Name="DeepDuplicate" Visible="True" />
        </CallbackCommands>
        <DataTrees>
            <px:PXTreeDataMember TreeView="EntityItems" TreeKeys="Key" />
        </DataTrees>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="form" runat="server" Width="100%" DataMember="Document" Caption="Rule">
        <AutoSize Container="Window" Enabled="True" />
        <Template>
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" ControlSize="L" />
            <px:PXSelector ID="edName" runat="server" DataField="Name" DataSourceID="ds" />
            <px:PXTextEdit ID="edDescription" runat="server" DataField="Description" />
            <px:PXSelector ID="edScreenID" runat="server" DataField="ScreenID" FilterByAllFields="true" CommitChanges="True"/>
            <px:PXTextEdit ID="edGraphType" runat="server" DataField="GraphType" CommitChanges="true" />
            <px:PXSelector ID="edCategoryID" runat="server" DataField="CategoryID" DataSourceID="ds" AllowEdit="true" />
            <px:PXTreeSelector ID="edExpression" runat="server" DataField="Expression"
                TreeDataSourceID="ds" PopulateOnDemand="True" InitialExpandLevel="0" 
                ShowRootNode="false" MinDropWidth="468" MaxDropWidth="600" AllowEditValue="true" Width="1000" Height="100"
                AppendSelectedValue="true" AutoRefresh="true" TreeDataMember="EntityItems" CommitChanges="true">
                <DataBindings>
                    <px:PXTreeItemBinding DataMember="EntityItems" TextField="Name" ValueField="Path" ImageUrlField="Icon" ToolTipField="Path" />
                </DataBindings>
            </px:PXTreeSelector>
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="S" ControlSize="M" />
            <px:PXCheckBox ID="chkActive" runat="server" Checked="True" DataField="Active" CommitChanges="True" />
            <px:PXCheckBox ID="chkIsSystem" runat="server" DataField="IsSystem" CommitChanges="true" />
            <px:PXCheckBox ID="chkAllowExport" runat="server" Checked="True" DataField="AllowExport" CommitChanges="True" />
            <px:PXCheckBox ID="chkIsComposite" runat="server" Checked="True" DataField="IsComposite" CommitChanges="true" />
            <px:PXFormView ID="VisibilityForm" runat="server" DataMember="CurrentDocument" DataSourceID="ds" Caption="Hidden Form needed for VisibleExp of TabItems"
                Visible="False" TabIndex="300">
                <Template>
                    <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="M" ControlSize="XM" />
                    <px:PXCheckBox ID="chkShowExpr" runat="server" DataField="ShowExpr" />
                    <px:PXCheckBox ID="chkShowUsedBy" runat="server" DataField="ShowUsedBy" />
                    <px:PXCheckBox ID="chkShowChildren" runat="server" DataField="ShowChildren" />
                </Template>
            </px:PXFormView>
            <px:PXUploadDialog ID="uploadFileDialog" runat="server" AutoSaveFile="true" Caption="Upload XML Zip" Key="Document"
                           Height="110px" SessionKey="XmlUploadAllEntities" AllowedFileTypes=".zip;"/>
        </Template>
    </px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXTab ID="tab" runat="server" Height="580px" Width="100%" DataMember="CurrentDocument" DataSourceID="ds" >
        <AutoSize Enabled="True" Container="Window" MinWidth="300" MinHeight="250"/>
        <Items>
            <px:PXTabItem Text="Rule Details" BindingContext="VisibilityForm" VisibleExp="DataControls[&quot;chkShowChildren&quot;].Value == true">
                <Template>
                    <px:PXGrid ID="subGrid" runat="server" DataSourceID="ds" Height="150px" Style="border: 0px;"
                        Width="100%" ActionsPosition="Top" SkinID="Details" MatrixMode="True">
                        <Levels>
                            <px:PXGridLevel DataMember="Details">
                                <RowTemplate>
                                    <px:PXCheckBox ID="chkReverse" runat="server" DataField="Reverse" />
                                    <px:PXSelector ID="edChildRuleID" runat="server" DataField="ChildRuleID" />
                                    <px:PXDropDown ID="edOpenBracket" runat="server" DataField="OpenBracket" />
                                    <px:PXDropDown ID="edCloseBracket" runat="server" DataField="CloseBracket" />
                                    <px:PXDropDown ID="edOperation" runat="server" DataField="Operation" />
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="Active" AllowNull="False" TextAlign="Center" Type="CheckBox" CommitChanges="true" />
                                    <px:PXGridColumn DataField="LineNbr" />
                                    <px:PXGridColumn DataField="SortOrder" />
                                    <px:PXGridColumn DataField="OpenBracket" />
                                    <px:PXGridColumn DataField="Reverse" Type="CheckBox" TextAlign="Center"/>
                                    <px:PXGridColumn DataField="ChildRuleID" Width="250px" MatrixMode="False" LinkCommand="ViewRule" AllowDragDrop="true" CommitChanges="true" />
                                    <px:PXGridColumn DataField="CloseBracket" />
                                    <px:PXGridColumn DataField="Operation" />
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <Mode InitNewRow="True" AllowDragRows="true" />
                        <AutoSize Enabled="True" MinHeight="150" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="Used by Models" BindingContext="VisibilityForm" VisibleExp="DataControls[&quot;chkShowUsedBy&quot;].Value == true">
                <Template>
                    <px:PXGrid ID="gridUsedByModels" runat="server" AllowPaging="False" DataSourceID="ds" SkinID="Inquire" Width="100%"
                        Height="150px" MatrixMode="True">
                        <Levels>
                            <px:PXGridLevel DataMember="UsedByModels">
                                <RowTemplate>
                                    <px:PXSelector ID="edName" runat="server" DataField="Name" />
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="Name" Width="300px" LinkCommand="ViewModel" />
                                    <px:PXGridColumn DataField="Description" Width="300px" />
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
            <px:PXTabItem Text="Used by Expressions" BindingContext="VisibilityForm" VisibleExp="DataControls[&quot;chkShowUsedBy&quot;].Value == true">
                <Template>
                    <px:PXGrid ID="gridUsedByExpressions" runat="server" AllowPaging="False" DataSourceID="ds" SkinID="Inquire" Width="100%"
                        Height="150px" MatrixMode="True">
                        <Levels>
                            <px:PXGridLevel DataMember="UsedByExprs">
                                <Columns>
                                    <px:PXGridColumn DataField="ALModel__Name" Width="300px" LinkCommand="ViewModel" />
                                    <px:PXGridColumn DataField="ALModel__Description" Width="300px" />
                                    <px:PXGridColumn DataField="ExprCode" Width="300px" LinkCommand="ViewModelExpr" />
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
            <px:PXTabItem Text="Used by Composites" BindingContext="VisibilityForm" VisibleExp="DataControls[&quot;chkShowExpr&quot;].Value == true">
                <Template>
                    <px:PXGrid ID="gridUsedByComposites" runat="server" AllowPaging="False" DataSourceID="ds" SkinID="Inquire" Width="100%"
                        Height="150px" MatrixMode="True">
                        <Levels>
                            <px:PXGridLevel DataMember="UsedByComposites">
                                <RowTemplate>
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="ALRule__Name" Width="300px" LinkCommand="ViewModel" />
                                    <px:PXGridColumn DataField="ALRule__LastModifiedByID" Width="150px" />
                                    <px:PXGridColumn DataField="ALRule__LastModifiedDateTime" Width="150px" />
                                </Columns>
                                <Layout FormViewHeight="" />
                            </px:PXGridLevel>
                        </Levels>
                        <AutoSize Enabled="True" MinHeight="150" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="Used by Colors" BindingContext="VisibilityForm" VisibleExp="DataControls[&quot;chkShowUsedBy&quot;].Value == true">
                <Template>
                    <px:PXGrid ID="gridUsedByColors" runat="server" AllowPaging="False" DataSourceID="ds" SkinID="Inquire" Width="100%"
                        Height="150px" MatrixMode="True">
                        <Levels>
                            <px:PXGridLevel DataMember="UsedByColors">
                                <Columns>
                                    <px:PXGridColumn DataField="ALColor__Name" Width="300px" LinkCommand="ViewModel" />
                                    <px:PXGridColumn DataField="ALColor__Description" Width="300px" />
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
            <px:PXTabItem Text="Used by Formats" BindingContext="VisibilityForm" VisibleExp="DataControls[&quot;chkShowUsedBy&quot;].Value == true">
                <Template>
                    <px:PXGrid ID="gridUsedByFormats" runat="server" AllowPaging="False" DataSourceID="ds" SkinID="Inquire" Width="100%"
                        Height="150px" MatrixMode="True">
                        <Levels>
                            <px:PXGridLevel DataMember="UsedByFormats">
                                <Columns>
                                    <px:PXGridColumn DataField="ALFormat__Name" Width="300px" LinkCommand="ViewModel" />
                                    <px:PXGridColumn DataField="ALFormat__Description" Width="300px" />
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
            <px:PXTabItem Text="Used by Content Elements" BindingContext="VisibilityForm" VisibleExp="DataControls[&quot;chkShowUsedBy&quot;].Value == true">
                <Template>
                    <px:PXGrid ID="gridUsedByContentElements" runat="server" AllowPaging="False" DataSourceID="ds" SkinID="Inquire" Width="100%"
                        Height="150px" MatrixMode="True">
                        <Levels>
                            <px:PXGridLevel DataMember="UsedByContentElements">
                                <Columns>
                                    <px:PXGridColumn DataField="ALContent__Name" Width="300px" LinkCommand="ViewModel" />
                                    <px:PXGridColumn DataField="ALContent__Description" Width="300px" />
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
            <px:PXTabItem Text="Used by Auto Prints" BindingContext="VisibilityForm" VisibleExp="DataControls[&quot;chkShowUsedBy&quot;].Value == true">
    <Template>
        <px:PXGrid ID="gridUsedByAutoPrints" runat="server" AllowPaging="False" DataSourceID="ds" SkinID="Inquire" Width="100%"
            Height="150px" MatrixMode="True">
            <Levels>
                <px:PXGridLevel DataMember="UsedByAutoPrints">
                    <Columns>
                        <px:PXGridColumn DataField="Name" Width="300px" LinkCommand="ViewAutoPrint" />
                        <px:PXGridColumn DataField="Description" Width="300px" />
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
    </px:PXTab>
     <!--#include file="~\Pages\AL\Includes\LabelsChangeIDDialog.inc"-->
</asp:Content>
