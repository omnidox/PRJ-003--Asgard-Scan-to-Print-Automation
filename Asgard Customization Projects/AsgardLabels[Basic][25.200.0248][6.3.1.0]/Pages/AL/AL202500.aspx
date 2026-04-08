<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="AL202500.aspx.cs" Inherits="Page_AL202500" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>
<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" TypeName="AA.Objects.Labels.ALColorMaint" PrimaryView="Document" PageLoadBehavior="SearchSavedKeys">
        <CallbackCommands>
            <px:PXDSCallbackCommand Name="Insert" PostData="Self" />
            <px:PXDSCallbackCommand CommitChanges="True" Name="Save" />
            <px:PXDSCallbackCommand Name="First" PostData="Self" StartNewGroup="true" />
            <px:PXDSCallbackCommand Name="Last" PostData="Self" />
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="form" runat="server" Width="100%" DataMember="Document" Caption="Rule" DataSourceID="ds" TabIndex="2600">
        <AutoSize Container="Window" Enabled="True" />
        <Template>
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" />
            <px:PXSelector ID="edName" runat="server" DataField="Name" DataSourceID="ds" />
            <px:PXTextEdit ID="edDescription" runat="server" DataField="Description" Width="300px" AlreadyLocalized="False" IsClientControl="True" />
            <px:PXTextEdit ID="edPrimaryColor" runat="server" DataField="PrimaryColor" CommitChanges="True" TextMode="Color" Width="300px" AlreadyLocalized="False" IsClientControl="True" />
            <px:PXSelector ID="edCategoryID" runat="server" DataField="CategoryID" DataSourceID="ds" />
            <px:PXLayoutRule runat="server" LabelsWidth="XXS" ControlSize="XXS" StartColumn="True"/>
            <px:PXCheckBox ID="chkActive" runat="server" Checked="True" DataField="Active" CommitChanges="True" AlreadyLocalized="False" IsClientControl="True" />
            <px:PXCheckBox ID="chkIsSystem" runat="server" DataField="IsSystem" CommitChanges="true" />
            <px:PXCheckBox ID="chkIsComposite" runat="server" Checked="True" DataField="IsComposite" CommitChanges="True" AlreadyLocalized="False" IsClientControl="True" />
            <px:PXCheckBox ID="chkAllowExport" runat="server" Checked="True" DataField="AllowExport" CommitChanges="True" AlreadyLocalized="False" IsClientControl="True" />
            <px:PXFormView ID="VisibilityForm" runat="server" DataMember="CurrentDocument" DataSourceID="ds" Caption="Hidden Form needed for VisibleExp of TabItems"
                Visible="False" TabIndex="300">
                <Template>
                    <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="M" ControlSize="XM" />
                    <px:PXCheckBox ID="chkShowChildren" runat="server" DataField="ShowChildren" AlreadyLocalized="False" IsClientControl="True" />
                </Template>
            </px:PXFormView>
            <px:PXLayoutRule runat="server" LabelsWidth="XXS" ControlSize="XXS" StartColumn="True"/>
            <px:PXNumberEdit ID="edAlpha" runat="server" AlreadyLocalized="False" DataField="Alpha" Enabled="False" IsClientControl="True"/>
            <px:PXNumberEdit ID="edRed" runat="server" AlreadyLocalized="False" DataField="Red" Enabled="False" IsClientControl="True"/>
            <px:PXNumberEdit ID="edGreen" runat="server" AlreadyLocalized="False" DataField="Green" Enabled="False" IsClientControl="True"/>
            <px:PXNumberEdit ID="edBlue" runat="server" AlreadyLocalized="False" DataField="Blue" Enabled="False" IsClientControl="True"/>
            <px:PXUploadDialog ID="uploadFileDialog" runat="server" AutoSaveFile="true" Caption="Upload XML Zip" Key="Document"
                           Height="110px" SessionKey="XmlUploadAllEntities" AllowedFileTypes=".zip;"/>
        </Template>
    </px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXTab ID="tab" runat="server" Height="580px" Width="100%" DataMember="Rules" DataSourceID="ds" >
        <AutoSize Enabled="True" Container="Window" MinWidth="300" MinHeight="250"/>
        <Items>
            <px:PXTabItem Text="Rule Details" BindingContext="VisibilityForm" VisibleExp="DataControls[&quot;chkShowChildren&quot;].Value == true">
                <Template>
                    <px:PXGrid ID="subGrid" runat="server" DataSourceID="ds" Height="150px" Style="border: 0px;"
                        Width="100%" ActionsPosition="Top" SkinID="Details" MatrixMode="True">
                        <Levels>
                            <px:PXGridLevel DataMember="Rules">
                                <RowTemplate>
                                    <%--<px:PXCheckBox ID="edReverse" runat="server" DataField="Reverse" />--%>
                                    <px:PXSelector ID="edBAccountID" runat="server" DataField="BAccountID" />
                                    <px:PXSelector ID="edChildColorID" runat="server" DataField="ChildColorID" />
                                    <px:PXSelector ID="edRuleID" runat="server" DataField="RuleID" />
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="Active" AllowNull="False" TextAlign="Center" Type="CheckBox" CommitChanges="true" />
                                    <px:PXGridColumn DataField="LineNbr" />
                                    <px:PXGridColumn DataField="SortOrder" />
                                    <px:PXGridColumn DataField="BAccountID" Width="250px" MatrixMode="False" LinkCommand="ViewBAccount" AllowDragDrop="true" CommitChanges="true" />
                                    <px:PXGridColumn DataField="RuleID" Width="250px" MatrixMode="False" LinkCommand="ViewRule" AllowDragDrop="true" CommitChanges="true" />
                                    <px:PXGridColumn DataField="ReverseRule" Type="CheckBox" TextAlign="Center" AllowDragDrop="true" />
                                    <px:PXGridColumn DataField="ChildColorID" Width="250px" MatrixMode="False" LinkCommand="ViewColor" AllowDragDrop="true" CommitChanges="true" />
                                    <px:PXGridColumn DataField="DoThrow" Type="CheckBox" TextAlign="Center" AllowDragDrop="true" />
                                    <px:PXGridColumn DataField="Message" AllowDragDrop="true" Width="500px" />
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <Mode InitNewRow="True" AllowDragRows="true" AllowUpload="True" />
                        <AutoSize Enabled="True" MinHeight="150" />
                        <CallbackCommands PasteCommand="PasteLine"></CallbackCommands>
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="Used by Expressions">
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
            <px:PXTabItem Text="Used by Rules">
                <Template>
                    <px:PXGrid ID="gridUsedByRules" runat="server" AllowPaging="False" DataSourceID="ds" SkinID="Inquire" Width="100%"
                        Height="150px" MatrixMode="True">
                        <Levels>
                            <px:PXGridLevel DataMember="UsedByRules">
                                <RowTemplate>
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="ALColor__Name" Width="300px" LinkCommand="ViewColor" />
                                    <px:PXGridColumn DataField="ALRule__Name" Width="300px" LinkCommand="ViewRule" />
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
