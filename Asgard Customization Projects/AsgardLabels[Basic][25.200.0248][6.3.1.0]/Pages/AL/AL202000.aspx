<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="AL202000.aspx.cs" Inherits="Page_AL202000" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>
<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" TypeName="AA.Objects.Labels.ALFormatMaint" PrimaryView="Document" PageLoadBehavior="SearchSavedKeys">
        <CallbackCommands>
            <px:PXDSCallbackCommand Name="Insert" PostData="Self" />
            <px:PXDSCallbackCommand CommitChanges="True" Name="Save" />
            <px:PXDSCallbackCommand Name="First" PostData="Self" StartNewGroup="true" />
            <px:PXDSCallbackCommand Name="Last" PostData="Self" />
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="form" runat="server" Width="100%" TabIndex="5500" DataSourceID="ds" DataMember="Document">
        <AutoSize Container="Window" Enabled="True" />
        <Template>
            <px:PXLayoutRule runat="server" LabelsWidth="S" ControlSize="L" StartRow="True" StartColumn="True" />
            <px:PXSelector ID="edName" runat="server" DataField="Name"/>
            <px:PXLayoutRule runat="server" Merge="true" />
            <px:PXCheckBox ID="chkActive" runat="server" DataField="Active" CommitChanges="true" />
            <px:PXCheckBox ID="chkIsSystem" runat="server" DataField="IsSystem" CommitChanges="true" />
            <px:PXLayoutRule runat="server" Merge="true" />
            <px:PXCheckBox ID="chkAllowExport" runat="server" Checked="True" DataField="AllowExport" CommitChanges="True" />
            <px:PXCheckBox ID="chkIsComposite" runat="server" Checked="True" DataField="IsComposite" CommitChanges="True" />
            <px:PXLayoutRule runat="server" />
            <px:PXFormView ID="VisibilityForm" runat="server" DataMember="CurrentDocument" DataSourceID="ds" Caption="Hidden Form needed for VisibleExp of TabItems"
                Visible="False" TabIndex="300">
                <Template>
                    <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="M" ControlSize="XM" />
                    <px:PXCheckBox ID="chkShowChildren" runat="server" DataField="ShowChildren" AlreadyLocalized="False" IsClientControl="True" />
                </Template>
            </px:PXFormView>
            <px:PXTextEdit ID="edDescription" runat="server" AlreadyLocalized="False" DataField="Description" />
            <px:PXDropDown ID="edPrintDensityType" runat="server" DataField="PrintDensityType" CommitChanges="true"/>
            <px:PXDropDown ID="edPrintDensity" runat="server" DataField="PrintDensity" CommitChanges="true"/>
            <%--<px:PXDropDown ID="edPrintQuality" runat="server" DataField="PrintQuality" />--%>
            <px:PXDropDown ID="edRotation" runat="server" DataField="Rotation" />
            <px:PXLayoutRule runat="server" LabelsWidth="S" ControlSize="L" StartColumn="True" />
            <px:PXNumberEdit ID="edWidth" runat="server" AlreadyLocalized="False" DataField="Width" />
            <px:PXNumberEdit ID="edHeight" runat="server" AlreadyLocalized="False" DataField="Height" />
            <px:PXDropDown ID="edSizeUnit" runat="server" DataField="SizeUnit" Width="70px"/>
            <px:PXNumberEdit ID="edWidthDots" runat="server" AlreadyLocalized="False" DataField="WidthDots" />
            <px:PXNumberEdit ID="edHeightDots" runat="server" AlreadyLocalized="False" DataField="HeightDots" />
            <px:PXSelector ID="edMarginID" runat="server" DataField="MarginID" CommitChanges="True" AllowEdit="true" />
            <px:PXSelector ID="edCategoryID" runat="server" DataField="CategoryID" DataSourceID="ds" />
            <px:PXLayoutRule runat="server" LabelsWidth="SM" ControlSize="L" StartColumn="True" />
            <px:PXCheckBox ID="chkUseWithPdf" runat="server" DataField="UseWithPdf" CommitChanges="true" />
            <px:PXDropDown ID="edPageSize" runat="server" DataField="PageSize" CommitChanges="true" />
            <px:PXDropDown ID="edPageOrientation" runat="server" DataField="PageOrientation" CommitChanges="true" />
            <px:PXDropDown ID="edPageHAlign" runat="server" DataField="PageHAlign" CommitChanges="true" />
            <px:PXDropDown ID="edPageVAlign" runat="server" DataField="PageVAlign" CommitChanges="true" />
            <px:PXUploadDialog ID="uploadFileDialog" runat="server" AutoSaveFile="true" Caption="Upload XML Zip" Key="Document"
                           Height="110px" SessionKey="XmlUploadAllEntities" AllowedFileTypes=".zip;"/>
        </Template>
    </px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXTab ID="tab" runat="server" Height="580px" Width="100%" DataMember="UsedByModels" DataSourceID="ds" >
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
                                    <px:PXSelector ID="edChildFormatID" runat="server" DataField="ChildFormatID" />
                                    <px:PXSelector ID="edRuleID" runat="server" DataField="RuleID" />
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="Active" AllowNull="False" TextAlign="Center" Type="CheckBox" CommitChanges="true" />
                                    <px:PXGridColumn DataField="LineNbr" />
                                    <px:PXGridColumn DataField="SortOrder" />
                                    <px:PXGridColumn DataField="BAccountID" Width="250px" MatrixMode="False" LinkCommand="ViewBAccount" AllowDragDrop="true" CommitChanges="true" />
                                    <px:PXGridColumn DataField="RuleID" Width="250px" MatrixMode="False" LinkCommand="ViewRule" AllowDragDrop="true" CommitChanges="true" />
                                    <px:PXGridColumn DataField="ReverseRule" Type="CheckBox" TextAlign="Center" AllowDragDrop="true" />
                                    <px:PXGridColumn DataField="ChildFormatID" Width="250px" MatrixMode="False" LinkCommand="ViewFormat" AllowDragDrop="true" CommitChanges="true" />
                                    <px:PXGridColumn DataField="DoThrow" Type="CheckBox" TextAlign="Center" AllowDragDrop="true" />
                                    <px:PXGridColumn DataField="Message" AllowDragDrop="true" Width="500px"/>
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <Mode InitNewRow="True" AllowDragRows="true" AllowUpload="True" />
                        <AutoSize Enabled="True" MinHeight="150" />
                        <CallbackCommands PasteCommand="PasteLine"></CallbackCommands>
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="Used By Models">
                <Template>
                    <px:PXGrid ID="labelFormatGrid" runat="server" DataSourceID="ds" Height="150px" Style="border: 0px;"
                        Width="100%" ActionsPosition="Top" SkinID="Inquire" MatrixMode="true" SyncPosition="true" KeepPosition="true">
                        <Levels>
                            <px:PXGridLevel DataMember="UsedByModels">
                                <RowTemplate>
                                    <px:PXSelector ID="edName2" runat="server" DataField="Name" />
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
            <px:PXTabItem Text="Used by Rules">
                <Template>
                    <px:PXGrid ID="gridUsedByRules" runat="server" AllowPaging="False" DataSourceID="ds" SkinID="Inquire" Width="100%"
                        Height="150px" MatrixMode="True">
                        <Levels>
                            <px:PXGridLevel DataMember="UsedByRules">
                                <RowTemplate>
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="ALFormat__Name" Width="300px" LinkCommand="ViewFormat" />
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
        <AutoSize Enabled="True" Container="Window" MinWidth="300" MinHeight="250"/>
    </px:PXTab>
     <!--#include file="~\Pages\AL\Includes\LabelsChangeIDDialog.inc"-->
</asp:Content>
