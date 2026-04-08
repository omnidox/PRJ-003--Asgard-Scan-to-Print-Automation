<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="AL204000.aspx.cs" Inherits="Page_AL204000" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>
<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" TypeName="AA.Objects.Labels.ALBarcodeMaint" PrimaryView="Document" PageLoadBehavior="SearchSavedKeys">
        <CallbackCommands>
            <px:PXDSCallbackCommand Name="Insert" PostData="Self" />
            <px:PXDSCallbackCommand CommitChanges="True" Name="Save" />
            <px:PXDSCallbackCommand Name="First" PostData="Self" StartNewGroup="true" />
            <px:PXDSCallbackCommand Name="Last" PostData="Self" />
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="form" runat="server" Height="100px" Width="100%" DataMember="Document" Caption="Rule" DataSourceID="ds" TabIndex="18600">
        <AutoSize Container="Window" Enabled="True" />
        <Template>
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" />
            <px:PXSelector ID="edName" runat="server" DataField="Name" DataSourceID="ds" />
            <px:PXTextEdit ID="edDescription" runat="server" DataField="Description" Width="300px" AlreadyLocalized="False" IsClientControl="True" />
            <px:PXDropDown ID="edBarcodeType" runat="server" DataField="BarcodeType" Width="300px" AutoComplete="true" CommitChanges="True" IsClientControl="True" />
            <px:PXTextEdit ID="edSampleValue" runat="server" DataField="SampleValue" Width="300px" AlreadyLocalized="False" IsClientControl="True" />
            <px:PXSelector ID="edCategoryID" runat="server" DataField="CategoryID" DataSourceID="ds" />
            <px:PXTextEdit ID="edMessage" runat="server" DataField="Message" TextMode="MultiLine" Height="50px" AlreadyLocalized="False" IsClientControl="True"></px:PXTextEdit>
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" ControlSize="M" />
            <px:PXCheckBox ID="chkActive" runat="server" Checked="True" DataField="Active" CommitChanges="True" AlreadyLocalized="False" IsClientControl="True" />
            <px:PXCheckBox ID="chkIsSystem" runat="server" DataField="IsSystem" CommitChanges="true" />
            <px:PXCheckBox ID="chkAllowExport" runat="server" Checked="True" DataField="AllowExport" CommitChanges="True" />
            <px:PXDropDown ID="edLanguage" runat="server" DataField="Language" Width="300px" Enabled="False" IsClientControl="True" />
            <px:PXDropDown ID="edDimension" runat="server" DataField="Dimension" Width="300px" Enabled="False" IsClientControl="True" />
            <px:PXSelector ID="edFormatID" runat="server" DataField="FormatID" CommitChanges="True" AllowEdit="True"/>
            <px:PXLayoutRule runat="server" StartColumn="True" />
            <px:PXFormView ID="templateDataForm" runat="server" DataMember="CurrentDocument" DataSourceID="ds" TabIndex="200" SkinID="Transparent">
                <Template>
                    <px:PXImageView ID="edImageUrl" runat="server" DataField="ImageUrl" Height="250px" Width="650px" AlreadyLocalized="False" CallbackUpdatable="True" />
                </Template>
            </px:PXFormView>
            <px:PXUploadDialog ID="uploadFileDialog" runat="server" AutoSaveFile="true" Caption="Upload XML Zip" Key="Document"
                           Height="110px" SessionKey="XmlUploadAllEntities" AllowedFileTypes=".zip;"/>
        </Template>
    </px:PXFormView>
    <px:PXTab ID="PXTab1" runat="server" Height="300px" Width="100%" DataMember="CurrentDocument" DataSourceID="ds">
        <AutoSize Enabled="True" Container="Window" MinWidth="300" MinHeight="500" />
        <Items>
            <px:PXTabItem Text="Options">
                <Template>
                    <px:PXGrid ID="subGridOptions" runat="server" DataSourceID="ds" Height="500px" Style="border: 0px;"
                        Width="100%" ActionsPosition="Top" SkinID="Details" MatrixMode="True">
                        <Levels>
                            <px:PXGridLevel DataMember="Options">
                                <RowTemplate>
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="BarcodeID" Width="200px" MatrixMode="False" />
                                    <px:PXGridColumn DataField="LineNbr" />
                                    <px:PXGridColumn DataField="SortOrder" />
                                    <px:PXGridColumn DataField="Option" Width="100px" />
                                    <px:PXGridColumn DataField="Description" Width="300px" />
                                    <px:PXGridColumn DataField="Constraint" Width="200px" />
                                    <px:PXGridColumn DataField="Value" />
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <Mode InitNewRow="False" AllowDragRows="true" AllowDelete="False" AllowAddNew="false" />
                        <AutoSize Enabled="True" MinHeight="200" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="Used By Data Elements">
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
        </Items>
    </px:PXTab>
    <!--#include file="~\Pages\AL\Includes\LabelsChangeIDDialog.inc"-->
</asp:Content>
