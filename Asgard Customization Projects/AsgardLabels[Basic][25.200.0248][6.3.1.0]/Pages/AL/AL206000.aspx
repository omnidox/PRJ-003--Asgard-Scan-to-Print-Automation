<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true"
    ValidateRequest="false" CodeFile="AL206000.aspx.cs" Inherits="Page_AL206000" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" PrimaryView="Document" TypeName="AA.Objects.Labels.ALContentMaint">
		<CallbackCommands>
            <px:PXDSCallbackCommand Name="Insert" PostData="Self" />
            <px:PXDSCallbackCommand CommitChanges="True" Name="Save" />
            <px:PXDSCallbackCommand Name="First" PostData="Self" StartNewGroup="true" />
            <px:PXDSCallbackCommand Name="Last" PostData="Self" />
		</CallbackCommands>
        <DataTrees>
            <px:PXTreeDataMember TreeView="EntityItems" TreeKeys="Key" />
        </DataTrees>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="form" runat="server" Width="100%" DataMember="Document" Caption="Content" DataSourceID="ds" TabIndex="1300">
        <AutoSize Container="Window" Enabled="True" />
		<Template>
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" />
            <px:PXSelector ID="edName" runat="server" DataField="Name" DataSourceID="ds" />
            <px:PXTextEdit ID="edDescription" runat="server" DataField="Description" Width="300px" AlreadyLocalized="False" IsClientControl="True" />
            <px:PXSelector ID="edScreenID" runat="server" DataField="ScreenID" FilterByAllFields="True" CommitChanges="True" />
            <px:PXTextEdit ID="edGraphType" runat="server" DataField="GraphType" CommitChanges="True" AlreadyLocalized="False" IsClientControl="True" />
            <px:PXSelector ID="edStandardID" runat="server" DataField="StandardID" FilterByAllFields="True" CommitChanges="True" AllowEdit="True" edit="1" />
            <px:PXSelector ID="edFormatID" runat="server" DataField="FormatID" CommitChanges="True" AllowEdit="True"/>
            <px:PXSelector ID="edBarcodeID" runat="server" DataField="BarcodeID" CommitChanges="True" AllowEdit="True"/>
            <px:PXSelector ID="edCategoryID" runat="server" DataField="CategoryID" CommitChanges="True" AllowEdit="True"/>
            <px:PXLayoutRule runat="server" ColumnSpan="2"/>
            <px:PXTextEdit ID="edMessage" runat="server" DataField="Message" TextMode="MultiLine" Height="50px" AlreadyLocalized="False" IsClientControl="True" /> 
            <px:PXLayoutRule runat="server" LabelsWidth="XS" ControlSize="XS" StartColumn="True" />
            <px:PXCheckBox ID="chkActive" runat="server" Checked="True" DataField="Active" CommitChanges="True" AlreadyLocalized="False" IsClientControl="True" />
            <px:PXCheckBox ID="chkIsSystem" runat="server" DataField="IsSystem" CommitChanges="true" />
            <px:PXCheckBox ID="chkAllowExport" runat="server" Checked="True" DataField="AllowExport" CommitChanges="True" />
            <px:PXLayoutRule runat="server" LabelsWidth="SM" ControlSize="L" StartColumn="True" />
            <px:PXFormView ID="templateDataForm" runat="server" DataMember="CurrentDocument" DataSourceID="ds" TabIndex="200" SkinID="Transparent">
                <Template>
                    <px:PXImageView ID="edImageUrl" runat="server" DataField="ImageUrl" Style="max-height: 300px; max-width: 800px;" AlreadyLocalized="False" CallbackUpdatable="True" />
                </Template>
            </px:PXFormView>
            <px:PXUploadDialog ID="uploadFileDialog" runat="server" AutoSaveFile="true" Caption="Upload XML Zip" Key="Document"
                           Height="110px" SessionKey="XmlUploadAllEntities" AllowedFileTypes=".zip;"/>
		</Template>
	</px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXTab ID="tab" runat="server" Height="580px" Width="100%" DataMember="CurrentDocument" DataSourceID="ds" >
        <AutoSize Enabled="True" Container="Window" MinWidth="300" MinHeight="250"/>
		<Items>
			<px:PXTabItem Text="Elements">
                <Template>
                    <px:PXGrid ID="gridElements" runat="server" DataSourceID="ds" SkinID="DetailsInTab" Width="100%" 
                        Height="150px" MatrixMode="True" TabIndex="2200" KeepPosition="True" SyncPosition="True"> 
                        <EmptyMsg AnonFilteredAddMessage="No records found. 
Try to change filter to see records here." AnonFilteredMessage="No records found. 
Try to change filter to see records here." ComboAddMessage="No records found. 
Try to change filter or modify parameters above to see records here." FilteredAddMessage="No records found. 
Try to change filter to see records here." FilteredMessage="No records found. 
Try to change filter to see records here." NamedComboAddMessage="No records found as '{0}'. 
Try to change filter or modify parameters above to see records here." NamedComboMessage="No records found as '{0}'. 
Try to change filter or modify parameters above to see records here." NamedFilteredAddMessage="No records found as '{0}'. 
Try to change filter to see records here." NamedFilteredMessage="No records found as '{0}'. 
Try to change filter to see records here." /> 
                        <Levels>
                            <px:PXGridLevel DataMember="Elements">
                                <RowTemplate>
                                    <px:PXSelector ID="edIdentifier" runat="server" DataField="Identifier" CommitChanges="True" AutoComplete="true"/> 
                                    <%--<px:PXTreeSelector ID="edExprValue" runat="server" DataField="ExprValue" 
                                        TreeDataSourceID="ds" PopulateOnDemand="True" InitialExpandLevel="0" 
                                        ShowRootNode="False" MinDropWidth="468" MaxDropWidth="600" AllowEditValue="True" 
                                        AppendSelectedValue="True" AutoRefresh="True" TreeDataMember="EntityItems" AlreadyLocalized="False"> 
                                        <DataBindings>
                                            <px:PXTreeItemBinding DataMember="EntityItems" TextField="Name" ValueField="Path" ImageUrlField="Icon" ToolTipField="Path" />
                                        </DataBindings>
                                    </px:PXTreeSelector>--%>
                                    <%--<px:PXSelector ID="edSubstitutionID" runat="server" DataField="SubstitutionID" CommitChanges="True" AllowEdit="true" AutoComplete="true"/>--%>
                                    <px:PXSelector ID="edPreHumanSequenceID" runat="server" DataField="PreHumanSequenceID" CommitChanges="True" AutoComplete="true" DisplayMode="Text"/> 
                                    <px:PXSelector ID="edPostHumanSequenceID" runat="server" DataField="PostHumanSequenceID" CommitChanges="True" AutoComplete="true" DisplayMode="Text"/> 
                                    <px:PXSelector ID="edDataElementID" runat="server" DataField="DataElementID" CommitChanges="True" AllowEdit="True" AutoComplete="true"/>
                                    <px:PXSelector ID="edBarcodeSequenceID" runat="server" DataField="BarcodeSequenceID" CommitChanges="True" AutoComplete="true"/> 
                                    <px:PXSelector ID="edPreExprSequenceID" runat="server" DataField="PreExprSequenceID" CommitChanges="True" AutoComplete="true" DisplayMode="Text"/> 
                                    <px:PXSelector ID="edPostExprSequenceID" runat="server" DataField="PostExprSequenceID" CommitChanges="True" AutoComplete="true" DisplayMode="Text"/> 
                                    <px:PXSelector ID="edRuleID" runat="server" DataField="RuleID" CommitChanges="True" AutoComplete="true"/> 
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="ContentID" Width="200px" MatrixMode="False" />
                                    <px:PXGridColumn DataField="LineNbr" />
                                    <px:PXGridColumn DataField="SortOrder" />
                                    <px:PXGridColumn DataField="Active" Type="CheckBox" Width="60px" TextAlign="Center" />
                                    <px:PXGridColumn DataField="HriUsage" Type="DropDownList" Width="100px" CommitChanges="True"/>
                                    <px:PXGridColumn DataField="PreHumanSequenceID" Width="80px" CommitChanges="True" LinkCommand="ViewPreHumanSequence" DisplayMode="Text"/>
                                    <px:PXGridColumn DataField="Identifier" Width="80px" CommitChanges="True" LinkCommand="ViewIdentifier"/>
                                    <px:PXGridColumn DataField="PostHumanSequenceID" Width="80px" CommitChanges="True" LinkCommand="ViewPostHumanSequence" DisplayMode="Text"/>
                                    <px:PXGridColumn DataField="ALStandardIdentifier__ShortName" Width="100px"/>
                                    <px:PXGridColumn DataField="RuleID" Width="150px" CommitChanges="True" AllowDragDrop="true" LinkCommand="ViewRule" DisplayMode="Text"/>
                                    <px:PXGridColumn DataField="ReverseRule" Width="70px" CommitChanges="True" AllowDragDrop="true" Type="CheckBox" TextAlign="Center" />
                                    <px:PXGridColumn DataField="PrePostUsage" Type="DropDownList" Width="120px" CommitChanges="True"/>
                                    <px:PXGridColumn DataField="PreExprSequenceID" Width="150px" CommitChanges="True" LinkCommand="ViewPreExprSequence" DisplayMode="Text"/>
                                    <px:PXGridColumn DataField="DataElementID" Width="200px" CommitChanges="True" AllowDragDrop="True" LinkCommand="ViewDataElement" />
                                    <px:PXGridColumn DataField="ExprType" Width="90px" CommitChanges="True" Type="DropDownList"/>
                                    <px:PXGridColumn DataField="ExprValue" Width="200px" CommitChanges="True" />
                                    <px:PXGridColumn DataField="PostExprSequenceID" Width="150px" CommitChanges="True" LinkCommand="ViewPostExprSequence" DisplayMode="Text"/>
                                    <px:PXGridColumn DataField="BarcodeSequenceID" Width="150px" CommitChanges="True" LinkCommand="ViewBarcodeSequence"/>
<%--                                    <px:PXGridColumn DataField="SampleType" Width="90px" CommitChanges="True" Type="DropDownList"/>
                                    <px:PXGridColumn DataField="SampleValue" Width="200px" CommitChanges="True" />
                                    <px:PXGridColumn DataField="DoSubstitute" AllowNull="False" TextAlign="Center" Type="CheckBox" Width="120px" AllowDragDrop="true" CommitChanges="true" />
                                    <px:PXGridColumn DataField="SubstitutionID" Width="150px" MatrixMode="False" CommitChanges="true" AllowDragDrop="true" LinkCommand="ViewSubstitution" />--%>
                                    <px:PXGridColumn DataField="LastModifiedByID" Width="150px" />
                                    <px:PXGridColumn DataField="LastModifiedDateTime" Width="150px" />
                                </Columns>
                                <Layout FormViewHeight="" />
                            </px:PXGridLevel>
                        </Levels>
                        <AutoSize Enabled="True" MinHeight="150" />
                        <Mode AllowDragRows="True" AllowUpload="True" InitNewRow="True" /> 
                        <CallbackCommands PasteCommand="PasteLine" /> 
                    </px:PXGrid>
                </Template>
			</px:PXTabItem>
			<px:PXTabItem Text="Used By Data Elements">
				<Template>
                    <px:PXGrid ID="gridUsedByDataElements" runat="server" AllowPaging="False" DataSourceID="ds" SkinID="Inquire" Width="100%"
                        Height="150px" MatrixMode="True">
                        <Levels>
                            <px:PXGridLevel DataMember="UsedByDataElements">
                                <Columns>
                                    <px:PXGridColumn DataField="Name" Width="350px" LinkCommand="ViewModel"/>
                                    <px:PXGridColumn DataField="Active" AllowNull="False" TextAlign="Center" Type="CheckBox"/>
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
	</px:PXTab>
     <!--#include file="~\Pages\AL\Includes\LabelsChangeIDDialog.inc"-->
</asp:Content>
