<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="TC501000.aspx.cs" Inherits="Page_TC501000" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" PrimaryView="Filter" TypeName="TCAddon.TCProcessShipmentAutomation">
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" Style="z-index: 100" 
		Width="100%" DataMember="Filter" TabIndex="100" Caption="Selection" DefaultControlID="edAction">
		<Template>
			<px:PXLayoutRule runat="server" StartRow="True" LabelsWidth="M" StartColumn="True" ControlSize="XM"/>
            <px:PXSelector ID="edOrderNbrFrom" runat="server" DataField="OrderNbrFrom" CommitChanges="True" AutoRefresh="True">
            </px:PXSelector>
            <px:PXDateTimeEdit ID="edDateFrom" runat="server" AlreadyLocalized="False" DataField="DateFrom" CommitChanges="True">
            </px:PXDateTimeEdit>
		    <px:PXDropDown ID="edAutoPack" runat="server" DataField="AutoPack">
            </px:PXDropDown>
            <px:PXLayoutRule runat="server" LabelsWidth="M" StartColumn="True" ControlSize="SM">
            </px:PXLayoutRule>
            <px:PXSelector ID="edOrderNbrTo" runat="server" DataField="OrderNbrTo" CommitChanges="True" AutoRefresh="True">
            </px:PXSelector>
            <px:PXDateTimeEdit ID="edDateTo" runat="server" AlreadyLocalized="False" DataField="DateTo" CommitChanges="True">
            </px:PXDateTimeEdit>
            <px:PXCheckBox ID="edShipmentConfirm" runat="server" AlreadyLocalized="False" DataField="ShipmentConfirm" Text="Confirm Shipment">
            </px:PXCheckBox>
		</Template>
	</px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" Runat="Server">
    <px:PXGrid AutoAdjustColumns="True" ID="grid" runat="server" DataSourceID="ds" Style="z-index: 100" ActionsPosition="Top"
        Caption="Orders" Width="100%" Height="150px" SkinID="PrimaryInquire" BatchUpdate="True" TabIndex="1100" AllowPaging="True" AdjustPageSize="Auto" SyncPosition="True"
        FastFilterFields="OrderNbr,UsrTCPONumber,CustomerID,edOrderDate" NoteIndicator="False" FilesIndicator="False">
		<Levels>
			<px:PXGridLevel DataKeyNames="OrderType,OrderNbr" DataMember="Orders">
			    <RowTemplate>
                    <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="M" ControlSize="M" ></px:PXLayoutRule>
                    <px:PXLayoutRule runat="server" Merge="True" ></px:PXLayoutRule>
                    <px:PXCheckBox ID="edSelected" runat="server" DataField="Selected" AlreadyLocalized="False">
                    </px:PXCheckBox>
                    <px:PXSelector ID="edOrderNbr" runat="server" DataField="OrderNbr" Enabled="False" AllowEdit="True">
                    </px:PXSelector>
                    <px:PXTextEdit ID="edUsrTCPONumber" runat="server" DataField="UsrTCPONumber" Enable="False" AlreadyLocalized="False" DefaultLocale="">
                    </px:PXTextEdit>
                    <px:PXSegmentMask ID="edCustomerID" runat="server" DataField="CustomerID" Enable="False">
                    </px:PXSegmentMask>
                    <px:PXDateTimeEdit ID="edOrderDate" runat="server" AlreadyLocalized="False" DataField="OrderDate" DefaultLocale="">
                    </px:PXDateTimeEdit>
                </RowTemplate>
                <Columns>
                    <px:PXGridColumn AllowNull="False" DataField="Selected" TextAlign="Center" Type="CheckBox" Width="60px" AllowCheckAll="True" AllowFilter="False" AllowMove="False">
                    </px:PXGridColumn>
	<px:PXGridColumn DataField="OrderType" Width="70" />
                    <px:PXGridColumn AllowUpdate="False" DataField="OrderNbr" Width="100px" LinkCommand="ViewDocument">
                    </px:PXGridColumn>
                    <px:PXGridColumn AllowUpdate="False" DataField="UsrTCPONumber" Width="200px">
                    </px:PXGridColumn>
                    <px:PXGridColumn AllowUpdate="False" DataField="CustomerID" Width="120px">
                    </px:PXGridColumn>
                    <px:PXGridColumn DataField="OrderDate" Width="90px">
                    </px:PXGridColumn></Columns>
			</px:PXGridLevel>
		</Levels>
		<AutoSize Container="Window" Enabled="True" MinHeight="150" ></AutoSize>
        <ActionBar DefaultAction="ViewDocument" ></ActionBar>
	</px:PXGrid>
</asp:Content>
