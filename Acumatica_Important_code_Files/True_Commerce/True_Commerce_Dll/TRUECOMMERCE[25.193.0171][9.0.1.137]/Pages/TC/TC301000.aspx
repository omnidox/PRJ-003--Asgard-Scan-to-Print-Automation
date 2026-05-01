<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="TC301000.aspx.cs" Inherits="Page_TC301000" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" PrimaryView="TemplateHeader" TypeName="TCAddon.TCAutoPackTemplateEntry">
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" Style="z-index: 100" 
		Width="100%" DataMember="TemplateHeader" TabIndex="3100">
		<Template>
			<px:PXLayoutRule runat="server" StartRow="True" StartColumn="True" LabelsWidth="SM"/>
		    <px:PXSelector ID="edTemplateID" runat="server" CommitChanges="True" DataField="TemplateID" AutoRefresh="True">
            </px:PXSelector>
            <px:PXTextEdit ID="edTemplateDesc" runat="server" AlreadyLocalized="False" DataField="TemplateDesc" DefaultLocale="">
            </px:PXTextEdit>
		</Template>
	</px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" Runat="Server">
    <px:PXGrid ID="grid" runat="server" DataSourceID="ds" Style="z-index: 100" 
		Width="100%" Height="150px" SkinID="Details" TabIndex="3300">
		<Levels>
			<px:PXGridLevel DataKeyNames="TemplateID" DataMember="TemplateDetails">
			    <RowTemplate>
                    <px:PXSelector ID="edInventoryID" runat="server" AutoRefresh="True" CommitChanges="True" DataField="InventoryID">
                    </px:PXSelector>
                    <px:PXNumberEdit ID="edPackageSize" runat="server" AlreadyLocalized="False" DataField="PackageSize" CommitChanges="True" DefaultLocale="">
                    </px:PXNumberEdit>
                    <px:PXNumberEdit ID="edBoxNo" runat="server" AlreadyLocalized="False" DataField="BoxNo" CommitChanges="True" DefaultLocale="">
                    </px:PXNumberEdit>
                    <px:PXTextEdit ID="edSalesUnit" runat="server" AlreadyLocalized="False" DataField="SalesUnit" DefaultLocale="">
                    </px:PXTextEdit>
                </RowTemplate>
                <Columns>
                    <px:PXGridColumn CommitChanges="True" DataField="InventoryID" Width="120px">
                    </px:PXGridColumn>
                    <px:PXGridColumn DataField="PackageSize" TextAlign="Left" Width="110px">
                    </px:PXGridColumn>
                    <px:PXGridColumn CommitChanges="True" DataField="BoxNo" TextAlign="Left">
                    </px:PXGridColumn>
                    <px:PXGridColumn CommitChanges="True" DataField="SalesUnit" Width="120px">
                    </px:PXGridColumn>
                </Columns>
			</px:PXGridLevel>
		</Levels>
		<AutoSize Container="Window" Enabled="True" MinHeight="150" />
	</px:PXGrid>
</asp:Content>
