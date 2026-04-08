<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormView.master" AutoEventWireup="true"
    ValidateRequest="false" CodeFile="AL209500.aspx.cs" Inherits="Page_AL209500" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" TypeName="AA.Objects.Labels.ALAutoPrintMaint" PrimaryView="Document" PageLoadBehavior="SearchSavedKeys">
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
            <px:PXLayoutRule runat="server" StartRow="True" LabelsWidth="SM" ControlSize="XL" />
            <px:PXSelector ID="edName" runat="server" DataField="Name" DataSourceID="ds" />
            <px:PXTextEdit ID="edDescription" runat="server" AlreadyLocalized="False" DataField="Description" IsClientControl="True"/>
            <px:PXSelector ID="edScreenID" runat="server" DataField="ScreenID" FilterByAllFields="true" CommitChanges="True" />
            <px:PXTextEdit ID="edGraphType" runat="server" DataField="GraphType" CommitChanges="true" />
            <px:PXSelector ID="edBAccountID" runat="server" DataField="BAccountID" CommitChanges="true" AllowEdit="true"/>
            <px:PXCheckBox ID="chkReverseRule" runat="server" DataField="ReverseRule" CommitChanges="true" />
            <px:PXSelector ID="edRuleID" runat="server" DataField="RuleID" CommitChanges="true" AllowEdit="true"/>
            <px:PXSelector ID="edModelID" runat="server" DataField="ModelID" CommitChanges="true" AllowEdit="true"/>
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" />
            <px:PXCheckBox ID="chkActive" runat="server" Checked="True" DataField="Active" CommitChanges="True" />
            <px:PXCheckBox ID="chkIsSystem" runat="server" DataField="IsSystem" CommitChanges="true" />
            <px:PXCheckBox ID="chkAllowExport" runat="server" Checked="True" DataField="AllowExport" CommitChanges="True" />
        </Template>
    </px:PXFormView>
    <!--#include file="~\Pages\AL\Includes\LabelsChangeIDDialog.inc"-->
</asp:Content>
