<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="TC102010.aspx.cs" Inherits="Page_TC102010" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" PrimaryView="TCCustomerRecord" TypeName="TCAddon.TCLabelCustomerSettingsSetupMaint">
	
	<CallbackCommands></CallbackCommands></px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" Style="z-index: 100" Width="100%" DataMember="TCCustomerRecord" TabIndex="900">
		<Template>
			<px:PXLayoutRule LabelsWidth="" runat="server" StartRow="True" StartColumn="True"></px:PXLayoutRule>
		    <px:PXLayoutRule runat="server" StartColumn="True">
            </px:PXLayoutRule>
			<px:PXLayoutRule GroupCaption="General Settings" runat="server" ID="CstPXLayoutRule56" StartGroup="True" ></px:PXLayoutRule>
			<px:PXLayoutRule runat="server" ID="CstLayoutRule74" LabelsWidth="XXL" ControlSize="XL" ></px:PXLayoutRule>
			<px:PXSegmentMask CommitChanges="True" runat="server" ID="edCustomerID" DataField="CustomerID" ></px:PXSegmentMask>
			<px:PXSelector AutoRefresh="True" CommitChanges="True" runat="server" ID="edTCDefaultShipFromAddress" DataField="TCDefaultShipFromAddress" ></px:PXSelector>
			<px:PXSelector runat="server" DataField="DefaultLabelPartner" AutoRefresh="True" CommitChanges="True" ID="edDefaultParnter" ></px:PXSelector>
			<px:PXSelector AutoRefresh="True" CommitChanges="True" runat="server" DataField="DefaultLabelTemplate" ID="edTCDefaultLabelTemplate" ></px:PXSelector>
			<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown40" DataField="ProcessingUnmatchingLine" ></px:PXDropDown>
			<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown43" DataField="ValidateUCC128" ></px:PXDropDown>
			<px:PXDropDown CommitChanges="True" runat="server" ID="TCCstPXDropDown44" DataField="ComplianceSequence" ></px:PXDropDown>
			<px:PXCheckBox CommitChanges="True" runat="server" ID="CstPXCheckBox42" DataField="ValidateRequiredFields" ></px:PXCheckBox>
			<px:PXCheckBox CommitChanges="True" runat="server" ID="CstPXCheckBox41" DataField="PullAddFromShipment" ></px:PXCheckBox>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule6" StartColumn="True" ></px:PXLayoutRule>
			<px:PXLayoutRule GroupCaption="GS1-128" runat="server" ID="CstPXLayoutRule63" StartGroup="True" ></px:PXLayoutRule>
			<px:PXLabel Size="XXL" runat="server" ID="NoteB" Text="* The combined entries for Company # and Next Serial # cannot exceed 16 characters." Width="100%" ></px:PXLabel>
			<px:PXTextEdit runat="server" ID="edUCCNumber" DataField="UCCNumber" ></px:PXTextEdit>
			<px:PXTextEdit CommitChanges="True" runat="server" ID="edUCCExtension" DataField="UCCExtension" ></px:PXTextEdit>
			<px:PXTextEdit CommitChanges="True" runat="server" ID="edUCCCompany" DataField="UCCCompany" ></px:PXTextEdit>
			<px:PXTextEdit CommitChanges="True" runat="server" ID="edUCCNextSerialNo" DataField="UCCNextSerialNo" ></px:PXTextEdit>
			<px:PXCheckBox runat="server" ID="edUCCAutoCreate" DataField="UCCAutoCreate" ></px:PXCheckBox>
			<px:PXSelector runat="server" ID="CstPXSelector24" DataField="PrintDestination" /></Template>
		<AutoSize Container="Window" Enabled="True" MinHeight="200" ></AutoSize>
	</px:PXFormView>
	<px:PXTab runat="server" ID="FieldMaps" Height="600px" Width="100%" DataSourceID="ds" DataMember="">
		<Items>
			<px:PXTabItem Text="Label Fields Maps">
				<Template>
					<px:PXFormView Style='width:100%;' runat="server" ID="CstFormView15" DataMember="TCCustomerRecord" DataSourceID="ds">
						<Template>
							<px:PXLayoutRule ColumnWidth="100%" ControlSize="XXL" runat="server" ID="CstPXLayoutRule20" StartRow="True" EnableViewState="True" ></px:PXLayoutRule>
							<px:PXLayoutRule runat="server" ID="CstPXLayoutRule18" StartColumn="True" ></px:PXLayoutRule>
							<px:PXLayoutRule runat="server" ID="CstPXLayoutRule23" StartGroup="True" ></px:PXLayoutRule>
							<px:PXLabel Width="100%" runat="server" ID="CstLabel19" Text="*Please clicking on &#39;LOAD LABEL FIELDS&#39; button to get the label fields of default label template and corresponding transaction types which are setup in TM. Then, choose different TM Transactions to update &#39;Misc&#39; fields maps." Size="XXL" ViewStateMode="Inherit" Style='width:100%;height:100;' ></px:PXLabel>
							<px:PXSelector Height="" Style='width:;' AutoRefresh="True" runat="server" ID="CstPXSelector17" DataField="TMTransaction" CommitChanges="True" ></px:PXSelector></Template></px:PXFormView>				
					<px:PXGrid SyncPosition="True" runat="server" ID="mapListGrid" Height="500px" Width="100%">
						<AutoSize Enabled="True" ></AutoSize>
						<ActionBar ActionsVisible="True" Position="Top">
							<CustomItems>
								<px:PXToolBarButton Visible="True" Text="Load Label Fields">
									<AutoCallBack Target="ds" Command="TCLoadFields" ></AutoCallBack></px:PXToolBarButton>
								<px:PXToolBarButton Visible="True" Text="Load Acumatica Fields">
									<AutoCallBack Target="ds" Command="TCRefreshFieldLookups" ></AutoCallBack></px:PXToolBarButton></CustomItems>
							<Actions>
								<Save MenuVisible="False" Enabled="False" ></Save>
								<Search Enabled="False" ></Search>
								<Refresh Enabled="False" ></Refresh>
								<Refresh MenuVisible="False" ></Refresh>
								<Search MenuVisible="False" ></Search></Actions>
							<Actions>
								<Save MenuVisible="False" ></Save></Actions>
							<Actions>
								<EditRecord Enabled="False" ></EditRecord></Actions>
							<Actions>
								<EditRecord MenuVisible="False" ></EditRecord></Actions></ActionBar>
						<Mode AllowFormEdit="True" AllowAddNew="True" ></Mode>
						<Levels>
							<px:PXGridLevel SortOrder="None" DataMember="TCLabelFieldMapRecord" DataKeyNames="CustomerID,LabelFieldLevel,LabelField">
								<RowTemplate>
									<px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="L" ControlSize="XM" ></px:PXLayoutRule>
									<px:PXSelector AutoRefresh="True" AllowEdit="False" Enabled="True" runat="server" ID="sl1" DataField="LabelField" CommitChanges="True" ></px:PXSelector>
									<px:PXNumberEdit LabelWidth="" runat="server" ID="CstPXNumberEdit4" DataField="LabelFieldLength" Enabled="False"></px:PXNumberEdit>
									<px:PXCheckBox runat="server" ID="CstPXCheckBox8" DataField="Used" Width="100px"></px:PXCheckBox>
									<px:PXCheckBox Width="100px" runat="server" ID="CstPXCheckBox5" DataField="Required" Enabled="False"></px:PXCheckBox>
									<px:PXDropDown AllowEdit="False" runat="server" ID="dd1" Enabled="true" DataField="LabelFieldLevel" CommitChanges="True" ></px:PXDropDown>
									<px:PXDropDown AllowEdit="False" runat="server" ID="dd2" Enabled="true" DataField="AcumaticaTable" CommitChanges="True" ></px:PXDropDown>
									<px:PXSelector AutoRefresh="True" AllowEdit="False" runat="server" ID="sl2" DataField="AcumaticaField" Enabled="true" AllowAddNew="" CommitChanges="True" ></px:PXSelector></RowTemplate>
								<Columns>
									<px:PXGridColumn CommitChanges="True" DataField="LabelField" ></px:PXGridColumn>
									<px:PXGridColumn AllowFocus="False" AutoGenerateOption="NotSet" AllowUpdate="False" TextAlign="Left" DataField="LabelFieldLength" Width="140" ></px:PXGridColumn>
									<px:PXGridColumn AllowFocus="False" AutoGenerateOption="NotSet" AllowUpdate="False" DataField="Used" Width="100px" ></px:PXGridColumn>
									<px:PXGridColumn AllowFocus="False" AutoGenerateOption="NotSet" AllowUpdate="False" DataField="Required" Width="100px" ></px:PXGridColumn>
									<px:PXGridColumn CommitChanges="True" DataField="LabelFieldLevel" Width="200" ></px:PXGridColumn>
									<px:PXGridColumn CommitChanges="True" DataField="AcumaticaTable" ></px:PXGridColumn>
									<px:PXGridColumn CommitChanges="True" DataField="AcumaticaField" ></px:PXGridColumn></Columns>
								<Mode AllowFormEdit="True" ></Mode>
								<Layout ShowRowStatus="True" ></Layout>
								<Layout RowSelectorsVisible="True" ></Layout></px:PXGridLevel></Levels>
						<CallbackCommands>
							<Refresh CommitChanges="" ></Refresh></CallbackCommands>
						<CallbackCommands>
							<Refresh PostData="Self" ></Refresh></CallbackCommands></px:PXGrid></Template></px:PXTabItem></Items></px:PXTab></asp:Content>