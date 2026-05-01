<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="TC101000.aspx.cs" Inherits="Page_TC101000" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" PrimaryView="TCItemPackageRecord" TypeName="TCAddon.TCItemPackageSettingsSetupMaint">
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" Style="z-index: 100" 
		Width="100%" DataMember="TCItemPackageRecord" TabIndex="1100">
		<Template>
			<px:PXLayoutRule runat="server" StartRow="True"/>
		    <px:PXSegmentMask ID="edInventoryID" runat="server" DataField="InventoryID" CommitChanges="True">
            </px:PXSegmentMask>
		</Template>
	</px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" Runat="Server">
    <px:PXGrid ID="grid" runat="server" DataSourceID="ds" Style="z-index: 100" 
		Width="100%" Height="150px" SkinID="Inquire" TabIndex="1300" AdjustPageSize="Auto" AllowPaging="True">
<EmptyMsg ComboAddMessage="No records found.
Try to change filter or modify parameters above to see records here." NamedComboMessage="No records found as &#39;{0}&#39;.
Try to change filter or modify parameters above to see records here." NamedComboAddMessage="No records found as &#39;{0}&#39;.
Try to change filter or modify parameters above to see records here." FilteredMessage="No records found.
Try to change filter to see records here." FilteredAddMessage="No records found.
Try to change filter to see records here." NamedFilteredMessage="No records found as &#39;{0}&#39;.
Try to change filter to see records here." NamedFilteredAddMessage="No records found as &#39;{0}&#39;.
Try to change filter to see records here." AnonFilteredMessage="No records found.
Try to change filter to see records here." AnonFilteredAddMessage="No records found.
Try to change filter to see records here."></EmptyMsg>
		<Levels>
			<px:PXGridLevel DataMember="TCItemList">
			    <RowTemplate>
                    <px:PXSegmentMask ID="edInventoryCD" runat="server" DataField="InventoryCD">
                    </px:PXSegmentMask>
                    <px:PXTextEdit ID="edDescr" runat="server" AlreadyLocalized="False" DataField="Descr" DefaultLocale="">
                    </px:PXTextEdit>
                    <px:PXNumberEdit ID="edUsrTCPackageSize" runat="server" AlreadyLocalized="False" DataField="UsrTCPackageSize" DefaultLocale="">
                    </px:PXNumberEdit>
                    <px:PXSelector ID="edSalesUnit" runat="server" DataField="SalesUnit">
                    </px:PXSelector>
                    <px:PXNumberEdit ID="edBasePrice" runat="server" AlreadyLocalized="False" DataField="BasePrice" DefaultLocale="">
                    </px:PXNumberEdit>
                    <px:PXNumberEdit ID="edBaseItemWeight" runat="server" AlreadyLocalized="False" DataField="BaseItemWeight" DefaultLocale="">
                    </px:PXNumberEdit>
                </RowTemplate>
			    <Columns>
                    <px:PXGridColumn DataField="InventoryCD" Width="100px">
                    </px:PXGridColumn>
                    <px:PXGridColumn DataField="Descr" Width="200px">
                    </px:PXGridColumn>
                    <px:PXGridColumn DataField="UsrTCPackageSize" TextAlign="Right" Width="150px">
                    </px:PXGridColumn>
                    <px:PXGridColumn DataField="SalesUnit" Width="120px">
                    </px:PXGridColumn>
                    <px:PXGridColumn DataField="BasePrice" Width="100px" TextAlign="Right">
                    </px:PXGridColumn>
                    <px:PXGridColumn DataField="BaseItemWeight" TextAlign="Right" Width="100px">
                    </px:PXGridColumn>
                </Columns>
			</px:PXGridLevel>
		</Levels>
		<AutoSize Container="Window" Enabled="True" MinHeight="150" />
	    <Mode AllowAddNew="False" AllowDelete="False" />
	</px:PXGrid>
</asp:Content>
