<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="AL207000.aspx.cs" Inherits="Page_AL207000" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>
<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" TypeName="AA.Objects.AL.ALFontMaint" PrimaryView="Document" PageLoadBehavior="SearchSavedKeys">
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
            <px:PXSelector ID="edFontFileID" runat="server" DataField="FontFileID" CommitChanges="True" AllowEdit="True"/>
            <px:PXTextEdit ID="edFontType" runat="server" DataField="FontType" Width="80px" CommitChanges="True" />
            <px:PXDropDown ID="edSizeUnit" runat="server" DataField="SizeUnit" Width="80px" CommitChanges="True" />
            <px:PXNumberEdit ID="edHeight" runat="server" DataField="Height" CommitChanges="True" Width="80px" />
            <px:PXNumberEdit ID="edWidth" runat="server" DataField="Width" CommitChanges="True" Width="80px" />
            <px:PXSelector ID="edFormatID" runat="server" DataField="FormatID" CommitChanges="True" AllowEdit="True"/>
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="S" ControlSize="M" />
            <px:PXCheckBox ID="chkActive" runat="server" Checked="True" DataField="Active" CommitChanges="True" />
            <px:PXCheckBox ID="chkIsSystem" runat="server" Checked="True" DataField="IsSystem" CommitChanges="true" />
            <px:PXCheckBox ID="chkAllowExport" runat="server" Checked="True" DataField="AllowExport" CommitChanges="True" />
            <px:PXDropDown ID="edLanguage" runat="server" DataField="Language" Width="300px" Enabled="false" />
            <px:PXTextEdit ID="edSampleValue" runat="server" DataField="SampleValue" Width="300px" />
            <px:PXTextEdit ID="edMessage" runat="server" DataField="Message" TextMode="MultiLine" Height="50px" Width="300px" AlreadyLocalized="False" IsClientControl="True"></px:PXTextEdit>
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="S" ControlSize="M" />
            <px:PXFormView ID="imageForm" runat="server" DataMember="CurrentDocument" DataSourceID="ds" TabIndex="200" SkinID="Transparent">
                <Template>
                    <px:PXImageView ID="edImageUrl" runat="server" DataField="ImageUrl" Style="max-height: 300px; max-width: 800px;" AlreadyLocalized="False" CallbackUpdatable="True" />
                </Template>
            </px:PXFormView>
            <px:PXUploadDialog ID="uploadFileDialog" runat="server" AutoSaveFile="true" Caption="Upload XML Zip" Key="Document"
                           Height="110px" SessionKey="XmlUploadAllEntities" AllowedFileTypes=".zip;"/>
        </Template>
    </px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXTab ID="tab" runat="server" Height="580px" Width="100%" DataMember="UsedByModels" DataSourceID="ds" >
        <Items>
            <px:PXTabItem Text="Used By Model Expressions">
                <Template>
                    <px:PXGrid ID="labelFormatGrid" runat="server" DataSourceID="ds" Height="150px" Style="border: 0px;"
                        Width="100%" ActionsPosition="Top" SkinID="Inquire" MatrixMode="true" SyncPosition="true" KeepPosition="true">
                        <Levels>
                            <px:PXGridLevel DataMember="UsedByModels">
                                <RowTemplate>
                                    <px:PXSelector ID="edModelName" runat="server" DataField="Name" />
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="ALModel__Name" Width="300px" LinkCommand="ViewModel" />
                                    <px:PXGridColumn DataField="ALModel__Description" Width="300px" />
                                    <px:PXGridColumn DataField="LineNbr" Width="100px" />
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
        </Items>
        <AutoSize Enabled="True" Container="Window" MinWidth="300" MinHeight="250"/>
    </px:PXTab>
    <!--#include file="~\Pages\AL\Includes\LabelsChangeIDDialog.inc"-->
</asp:Content>
