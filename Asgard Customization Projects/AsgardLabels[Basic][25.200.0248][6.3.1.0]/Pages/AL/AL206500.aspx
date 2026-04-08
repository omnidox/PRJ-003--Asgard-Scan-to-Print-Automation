<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="AL206500.aspx.cs" Inherits="Page_AL206500" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>
<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" TypeName="AA.Objects.Labels.ALSubstitutionMaint" PrimaryView="Document" PageLoadBehavior="SearchSavedKeys">
        <CallbackCommands>
            <px:PXDSCallbackCommand Name="Insert" PostData="Self" />
            <px:PXDSCallbackCommand CommitChanges="True" Name="Save" />
            <px:PXDSCallbackCommand Name="First" PostData="Self" StartNewGroup="true" />
            <px:PXDSCallbackCommand Name="Last" PostData="Self" />
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="form" runat="server" Width="100%" DataMember="Document" Caption="Substitution" TemplateContainer="">
        <AutoSize Container="Window" Enabled="True" MinHeight="200" />
        <Template>
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" />
            <px:PXSelector ID="edName" runat="server" DataField="Name" Width="400px" DataSourceID="ds" />
            <px:PXTextEdit ID="edDescription" runat="server" DataField="Description" Width="800px" />
            <px:PXSelector ID="edCategoryID" runat="server" DataField="CategoryID" Width="400px" />
            <px:PXSelector ID="edTypeName" runat="server" DataField="TypeName" Width="800px" Enabled="false" />
            <px:PXSelector ID="edFunctionName" runat="server" DataField="FunctionName" Width="800px" Enabled="false" />
            <px:PXTextEdit ID="edSignature" runat="server" DataField="Signature" Width="800px" Enabled="false" />
            <px:PXTextEdit ID="edInternalName" runat="server" DataField="InternalName" Width="800px" Enabled="false" />
            <px:PXTextEdit ID="edReturnTypeName" runat="server" DataField="ReturnTypeName" Width="800px" Enabled="false" />
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="S" ControlSize="M" />
            <px:PXCheckBox ID="chkActive" runat="server" Checked="True" DataField="Active" CommitChanges="True" />
            <px:PXCheckBox ID="chkIsComposite" runat="server" Checked="True" DataField="IsComposite" CommitChanges="true" />
            <px:PXCheckBox ID="chkIsSystem" runat="server" Checked="True" DataField="IsSystem" CommitChanges="true" />
            <px:PXCheckBox ID="chkAllowExport" runat="server" Checked="True" DataField="AllowExport" CommitChanges="True" />
            <px:PXFormView ID="VisibilityForm" runat="server" DataMember="CurrentDocument" DataSourceID="ds" Caption="Hidden Form needed for VisibleExp of TabItems"
                Visible="False" TabIndex="300">
                <Template>
                    <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="M" ControlSize="XM" />
                    <px:PXCheckBox ID="chkShowChildren" runat="server" DataField="ShowChildren" />
                </Template>
            </px:PXFormView>
            <px:PXUploadDialog ID="uploadFileDialog" runat="server" AutoSaveFile="true" Caption="Upload XML Zip" Key="Document"
                Height="110px" SessionKey="XmlUploadAllEntities" AllowedFileTypes=".zip;" />
        </Template>
    </px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXTab ID="tab" runat="server" Height="580px" Width="100%" DataMember="CurrentDocument" DataSourceID="ds">
        <AutoSize Enabled="True" Container="Window" MinWidth="300" MinHeight="250"></AutoSize>
        <Items>
            <px:PXTabItem Text="Substitution Details" BindingContext="VisibilityForm" VisibleExp="DataControls[&quot;chkShowChildren&quot;].Value == true">
                <Template>
                    <px:PXGrid ID="subGrid" runat="server" DataSourceID="ds" Height="150px" Style="border: 0px;"
                        Width="100%" ActionsPosition="Top" SkinID="Details" MatrixMode="True">
                        <Levels>
                            <px:PXGridLevel DataMember="SubstitutionDetails">
                                <RowTemplate>
                                    <px:PXSelector ID="edChildSubstitutionID" runat="server" DataField="ChildSubstitutionID" />
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="SubstitutionID" Width="200px" MatrixMode="False" CommitChanges="true" />
                                    <px:PXGridColumn DataField="Active" AllowNull="False" TextAlign="Center" Type="CheckBox" CommitChanges="true" />
                                    <px:PXGridColumn DataField="LineNbr" />
                                    <px:PXGridColumn DataField="SortOrder" />
                                    <px:PXGridColumn DataField="ChildSubstitutionID" Width="250px" MatrixMode="False" LinkCommand="ViewSubstitution" AllowDragDrop="true" CommitChanges="true" />
                                    <px:PXGridColumn DataField="Arg1" Width="100px" CommitChanges="true" AllowDragDrop="true" />
                                    <px:PXGridColumn DataField="Arg2" Width="300px" CommitChanges="true" AllowDragDrop="true" />
                                    <px:PXGridColumn DataField="Arg3" Width="300px" CommitChanges="true" AllowDragDrop="true" />
                                    <px:PXGridColumn DataField="Arg4" Width="250px" CommitChanges="true" AllowDragDrop="true" />
                                    <px:PXGridColumn DataField="Arg5" Width="200px" CommitChanges="true" AllowDragDrop="true" />
                                    <px:PXGridColumn DataField="Arg6" Width="150px" CommitChanges="true" AllowDragDrop="true" />
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <AutoSize Enabled="True" MinHeight="150" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="Used By Composites" BindingContext="form">
                <Template>
                    <px:PXGrid ID="gridDataComposites" runat="server" AllowPaging="False" DataSourceID="ds" SkinID="Inquire" Width="100%"
                        Height="150px" MatrixMode="True">
                        <Levels>
                            <px:PXGridLevel DataMember="UsedByComposites">
                                <Columns>
                                    <px:PXGridColumn DataField="ALSubstitution__Name" Width="350px" LinkCommand="ViewModel" />
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
            <px:PXTabItem Text="Used By Data Elements" BindingContext="form">
                <Template>
                    <px:PXGrid ID="gridDataElements" runat="server" AllowPaging="False" DataSourceID="ds" SkinID="Inquire" Width="100%"
                        Height="150px" MatrixMode="True">
                        <Levels>
                            <px:PXGridLevel DataMember="UsedByDataElements">
                                <Columns>
                                    <px:PXGridColumn DataField="ALDataSource__Name" Width="350px" LinkCommand="ViewModel" />
                                    <px:PXGridColumn DataField="Name" Width="350px" LinkCommand="ViewModel" />
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
            <px:PXTabItem Text="Used By Content Elements" BindingContext="form">
                <Template>
                    <px:PXGrid ID="gridContentElements" runat="server" AllowPaging="False" DataSourceID="ds" SkinID="Inquire" Width="100%"
                        Height="150px" MatrixMode="True">
                        <Levels>
                            <px:PXGridLevel DataMember="UsedByContentElements">
                                <Columns>
                                    <px:PXGridColumn DataField="ALContent__Name" Width="350px" LinkCommand="ViewModel" />
                                    <%--<px:PXGridColumn DataField="Name" Width="350px" LinkCommand="ViewModel" />--%>
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
    </px:PXTab>
    <!--#include file="~\Pages\AL\Includes\LabelsChangeIDDialog.inc"-->
</asp:Content>
