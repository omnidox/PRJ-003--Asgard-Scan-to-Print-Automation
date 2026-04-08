<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true"
    ValidateRequest="false" CodeFile="AL505000.aspx.cs" Inherits="Page_AL505000" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" TypeName="AA.Objects.Labels.ALExportProcess" PrimaryView="Filter">
        <CallbackCommands>
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" Style="z-index: 100" Width="100%" DataMember="Filter" TabIndex="2000">
        <Template>
            <px:PXLayoutRule runat="server" LabelsWidth="S" ColumnWidth="XL" StartColumn="True" />
            <px:PXDropDown ID="edAction" runat="server" DataField="Action" IsClientControl="True" CommitChanges="True" />
            <px:PXDateTimeEdit ID="edStartDate" runat="server" DataField="StartDate" IsClientControl="True" CommitChanges="True" Width="120" />
            <px:PXDateTimeEdit ID="edEndDate" runat="server" DataField="EndDate" IsClientControl="True" CommitChanges="True" Width="120" />
            <px:PXSelector ID="edScreenID" runat="server" DataField="ScreenID" CommitChanges="True" FilterByAllFields="true"/>
            <px:PXTextEdit ID="edGraphType" runat="server" DataField="GraphType" CommitChanges="true" />
            <px:PXSelector ID="edBasedOnView" runat="server" DataField="BasedOnView" CommitChanges="true" AutoRefresh="true"/>
            <px:PXLayoutRule runat="server" Merge="true" />
            <px:PXCheckBox ID="chkExportFiles" runat="server" DataField="ExportFiles" AutoRefresh="true"/>
            <px:PXCheckBox ID="chkForceUpdate" runat="server" DataField="ForceUpdate"/>
        </Template>
    </px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXGrid ID="grid" runat="server" AllowPaging="true" PageSize="15" DataSourceID="ds" TabIndex="3900" SkinID="Inquire" Height="400px" Width="100%" SyncPosition="true">
        <Levels>
            <px:PXGridLevel DataKeyNames="RecordID" DataMember="Records">
                <RowTemplate>
                    <px:PXSelector ID="edID" runat="server" DataField="ID" />
                    <px:PXSelector ID="edParentID" runat="server" DataField="ParentID" />
                    <px:PXSelector ID="edChildID" runat="server" DataField="ChildID" />
                    <px:PXSelector ID="edScreenID2" runat="server" DataField="ScreenID" />
                    <px:PXSelector ID="edCategoryID" runat="server" DataField="CategoryID" />
                </RowTemplate>
                <Columns>
                    <px:PXGridColumn DataField="Selected" TextAlign="Center" AllowCheckAll="True" Type="CheckBox" Width="60px" />
                    <px:PXGridColumn DataField="UniqueID" TextAlign="Right" />
                    <%--<px:PXGridColumn DataField="SourceEntityType" Width="150px" />--%>
                    <px:PXGridColumn DataField="ID" Width="150px" LinkCommand="ViewMain" />
                    <px:PXGridColumn DataField="ParentID" Width="150px" LinkCommand="ViewParent" />
                    <px:PXGridColumn DataField="ChildID" Width="150px" />
                    <px:PXGridColumn DataField="LineNbr" Width="70px" />
                    <px:PXGridColumn DataField="SortOrder" Width="70px" />
                    <px:PXGridColumn DataField="Name" Width="150px" />
                    <px:PXGridColumn DataField="ScreenID" Width="96px" DisplayMode="Text" />
                    <px:PXGridColumn DataField="CategoryID" Width="150px" DisplayMode="Text" />
                    <px:PXGridColumn DataField="Description" Width="150px" />
                    <px:PXGridColumn DataField="CreatedDateTime" Width="140px" DisplayFormat="g" />
                    <px:PXGridColumn DataField="LastModifiedDateTime" Width="140px" DisplayFormat="g" />
                </Columns>
            </px:PXGridLevel>
        </Levels>
    </px:PXGrid>
</asp:Content>
