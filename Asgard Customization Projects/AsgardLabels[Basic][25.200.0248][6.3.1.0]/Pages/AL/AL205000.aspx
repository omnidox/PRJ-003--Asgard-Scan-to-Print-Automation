<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="AL205000.aspx.cs" Inherits="Page_AL205000" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>
<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" TypeName="AA.Objects.Labels.ALMarginMaint" PrimaryView="Document" PageLoadBehavior="SearchSavedKeys">
        <CallbackCommands>
            <px:PXDSCallbackCommand Name="Insert" PostData="Self" />
            <px:PXDSCallbackCommand CommitChanges="True" Name="Save" />
            <px:PXDSCallbackCommand Name="First" PostData="Self" StartNewGroup="true" />
            <px:PXDSCallbackCommand Name="Last" PostData="Self" />
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="form" runat="server" Width="100%" DataMember="Document" Caption="Rule">
        <AutoSize Container="Window" Enabled="True" />
        <Template>
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" />
            <px:PXSelector ID="edName" runat="server" DataField="Name" DataSourceID="ds" />
            <px:PXTextEdit ID="edDescription" runat="server" DataField="Description" Width="300px" />
            <px:PXSelector ID="edCategoryID" runat="server" DataField="CategoryID" DataSourceID="ds" />
            <px:PXDropDown ID="edSizeUnit" runat="server" DataField="SizeUnit" CommitChanges="true" Width="100"/>
            <px:PXNumberEdit ID="edLeft" runat="server" DataField="Left" CommitChanges="true"/>
            <px:PXNumberEdit ID="edRight" runat="server" DataField="Right" CommitChanges="true"/>
            <px:PXNumberEdit ID="edTop" runat="server" DataField="Top" CommitChanges="true"/>
            <px:PXNumberEdit ID="edBottom" runat="server" DataField="Bottom" />
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="S" ControlSize="M" />
            <px:PXCheckBox ID="chkActive" runat="server" Checked="True" DataField="Active" CommitChanges="True" />
            <px:PXCheckBox ID="chkIsSystem" runat="server" DataField="IsSystem" CommitChanges="true" />
            <px:PXCheckBox ID="chkAllowExport" runat="server" Checked="True" DataField="AllowExport" CommitChanges="True" />
            <px:PXUploadDialog ID="uploadFileDialog" runat="server" AutoSaveFile="true" Caption="Upload XML Zip" Key="Document"
                           Height="110px" SessionKey="XmlUploadAllEntities" AllowedFileTypes=".zip;"/>
        </Template>
    </px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXTab ID="tab" runat="server" Height="580px" Width="100%" DataMember="CurrentDocument" DataSourceID="ds" >
        <AutoSize Enabled="True" Container="Window" MinWidth="300" MinHeight="250"/>
        <Items>
            <px:PXTabItem Text="Used by Models">
                <Template>
                    <px:PXGrid ID="gridUsedByModels" runat="server" AllowPaging="False" DataSourceID="ds" SkinID="Inquire" Width="100%"
                        Height="150px" MatrixMode="True">
                        <Levels>
                            <px:PXGridLevel DataMember="UsedByModels">
                                <RowTemplate>
                                    <px:PXSelector ID="edName" runat="server" DataField="Name" />
                                    <px:PXTextEdit ID="edDescription" runat="server" DataField="Description" />
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
        </Items>
    </px:PXTab>
     <!--#include file="~\Pages\AL\Includes\LabelsChangeIDDialog.inc"-->
</asp:Content>
