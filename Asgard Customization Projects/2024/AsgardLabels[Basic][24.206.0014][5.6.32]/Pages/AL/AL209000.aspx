<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="AL209000.aspx.cs" Inherits="Page_AL209000" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>
<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" TypeName="AA.Objects.AL.ALAlternateTypeMaint" PrimaryView="Document" PageLoadBehavior="SearchSavedKeys">
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
            <px:PXSelector ID="edAlternateType" runat="server" DataField="AlternateType" DataSourceID="ds" />
            <px:PXTextEdit ID="edDescription" runat="server" DataField="Description" Width="300px" />
            <px:PXTextEdit ID="edRegex" runat="server" DataField="Regex" Width="300px" />
            <px:PXDropDown ID="edRegexValidation" runat="server" DataField="RegexValidation" Width="100px" />
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="S" ControlSize="M" />
            <px:PXCheckBox ID="chkActive" runat="server" Checked="True" DataField="Active" CommitChanges="True" />
        </Template>
    </px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXTab ID="tab" runat="server" Height="580px" Width="100%" DataMember="UsedByInventoryItems" DataSourceID="ds" >
        <Items>
            <px:PXTabItem Text="Used By Items">
                <Template>
                    <px:PXGrid ID="itemsGrid" runat="server" DataSourceID="ds" Height="150px" Style="border: 0px;"
                        Width="100%" ActionsPosition="Top" SkinID="Inquire" MatrixMode="true" SyncPosition="true" KeepPosition="true">
                        <Levels>
                            <px:PXGridLevel DataMember="UsedByInventoryItems">
                                <RowTemplate>
                                    <%--<px:PXSelector ID="edModelName" runat="server" DataField="Name" />--%>
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="InventoryItem__InventoryCD" Width="150px" LinkCommand="ViewItem" />
                                    <px:PXGridColumn DataField="InventoryItem__Descr" Width="300px" />
                                    <px:PXGridColumn DataField="INItemXRef__BAccountID" Width="150px" LinkCommand="ViewBAccount"/>
                                    <px:PXGridColumn DataField="INItemXRef__AlternateID" Width="300px" />
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
</asp:Content>
