<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true"
    ValidateRequest="false" CodeFile="AL304000.aspx.cs" Inherits="Page_AL304000" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" TypeName="AA.Objects.Labels.ALJustificationMaint" PrimaryView="Document">
        <CallbackCommands>
            <px:PXDSCallbackCommand Name="Insert" PostData="Self" />
            <px:PXDSCallbackCommand CommitChanges="True" Name="Save" />
            <px:PXDSCallbackCommand Name="First" PostData="Self" StartNewGroup="true" />
            <px:PXDSCallbackCommand Name="Last" PostData="Self" />
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" Style="z-index: 100" Width="100%" DataMember="Document">
        <Template>
            <px:PXLayoutRule runat="server" LabelsWidth="S" ControlSize="XL" StartColumn="True" StartRow="True" />
            <px:PXSelector ID="edName" runat="server" DataField="Name" DataSourceID="ds" />
            <px:PXTextEdit ID="edDescription" runat="server" DataField="Description" />
            <px:PXDropDown ID="edJustification" runat="server" DataField="Justification" CommitChanges="True" />
            <px:PXSelector ID="edCategoryID" runat="server" DataField="CategoryID" DataSourceID="ds" />
            <px:PXLayoutRule runat="server" Merge="true" />
            <px:PXNumberEdit ID="edFromX" runat="server" DataField="FromX" CommitChanges="true" />
            <px:PXNumberEdit ID="edToX" runat="server" DataField="ToX" CommitChanges="true" />
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" />
            <px:PXNumberEdit ID="edMaxLines" runat="server" DataField="MaxLines" CommitChanges="true" />
            <px:PXDropDown ID="edSizeUnit" runat="server" DataField="SizeUnit" CommitChanges="True" />
            <px:PXNumberEdit ID="edSpaceBetweenLines" runat="server" DataField="SpaceBetweenLines" CommitChanges="true" />
            <px:PXNumberEdit ID="edHangingIndent" runat="server" DataField="HangingIndent" CommitChanges="true" />
            <px:PXLayoutRule runat="server" StartColumn="True" />
            <px:PXCheckBox ID="chkActive" runat="server" DataField="Active" CommitChanges="true" />
            <px:PXCheckBox ID="chkIsSystem" runat="server" Checked="True" DataField="IsSystem" CommitChanges="true" />
            <px:PXCheckBox ID="chkExport" runat="server" DataField="AllowExport" CommitChanges="true" />
        </Template>
    </px:PXFormView>
    <px:PXUploadDialog ID="uploadFileDialog" runat="server" AutoSaveFile="true" Caption="Upload XML Zip" Key="Document"
        Height="110px" SessionKey="XmlUploadAllEntities" AllowedFileTypes=".zip;" />
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXTab ID="tab" runat="server" Width="100%" Height="150px" DataSourceID="ds">
        <Items>
            <px:PXTabItem Text="Used By Models">
                <Template>
                    <px:PXGrid ID="gridUsedByModels" runat="server" AllowPaging="False" DataSourceID="ds" SkinID="Inquire" Width="100%"
                        Height="20px" MatrixMode="True">
                        <Levels>
                            <px:PXGridLevel DataMember="UsedByModels">
                                <Columns>
                                    <px:PXGridColumn DataField="Active" AllowNull="False" TextAlign="Center" Type="CheckBox" />
                                    <px:PXGridColumn DataField="ALModel__Name" Width="200px" LinkCommand="ViewModel" />
                                    <px:PXGridColumn DataField="ALModel__Description" Width="300px" />
                                    <px:PXGridColumn DataField="ExprCode" Width="200px" />
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
        </Items>
        <AutoSize Container="Window" Enabled="True" MinHeight="150" />
    </px:PXTab>
    <!--#include file="~\Pages\AL\Includes\LabelsChangeIDDialog.inc"-->
</asp:Content>
