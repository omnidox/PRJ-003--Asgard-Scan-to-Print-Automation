<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true"
    ValidateRequest="false" CodeFile="AL205500.aspx.cs" Inherits="Page_AL205500" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server" >
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" TypeName="AA.Objects.AL.ALStandardMaint" PrimaryView="Document" PageLoadBehavior="SearchSavedKeys">
		<CallbackCommands>
            <px:PXDSCallbackCommand Name="Insert" PostData="Self" />
            <px:PXDSCallbackCommand CommitChanges="True" Name="Save" />
            <px:PXDSCallbackCommand Name="PasteLine" Visible="False" />
            <px:PXDSCallbackCommand Name="ResetOrder" Visible="False" />
            <px:PXDSCallbackCommand Name="First" PostData="Self" StartNewGroup="true" />
            <px:PXDSCallbackCommand Name="Last" PostData="Self" />
            <px:PXDSCallbackCommand Name="ImportAll" Visible="False" />
            <px:PXDSCallbackCommand Name="ExportAll" Visible="False" />
            <px:PXDSCallbackCommand Name="ToggleExport" Visible="False" />
            <px:PXDSCallbackCommand Name="ImportFiles" Visible="False" />
		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
	<px:PXFormView ID="form" runat="server" Width="100%" DataMember="Document" Caption="Standard">
        <AutoSize Container="Window" Enabled="True" />
		<Template>
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" />
            <px:PXSelector ID="edName" runat="server" DataField="Name" DataSourceID="ds" />
            <px:PXTextEdit ID="edDescription" runat="server" DataField="Description" Width="300px" />
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="S" ControlSize="M" />
            <px:PXCheckBox ID="chkActive" runat="server" Checked="True" DataField="Active" CommitChanges="True" />            
            <px:PXCheckBox ID="chkIsSystem" runat="server" DataField="IsSystem" CommitChanges="true" />
            <px:PXCheckBox ID="chkAllowExport" runat="server" Checked="True" DataField="AllowExport" CommitChanges="True" />
            <px:PXUploadDialog ID="uploadFileDialog" runat="server" AutoSaveFile="true" Caption="Upload XML Zip" Key="Document"
                           Height="110px" SessionKey="XmlUploadAllEntities" AllowedFileTypes=".zip;"/>
		</Template>
	</px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
	<px:PXTab ID="tab" runat="server" Height="580px" Width="100%" DataMember="CurrentDocument" DataSourceID="ds" >
        <AutoSize Enabled="True" Container="Window" MinWidth="300" MinHeight="250"/>
		<Items>
			<px:PXTabItem Text="Identifiers">
                <Template>
                    <px:PXGrid ID="gridIdentifiers" runat="server" DataSourceID="ds" SkinID="DetailsInTab" Width="100%"
                        Height="150px" MatrixMode="True" TabIndex="1000">
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
                            <px:PXGridLevel DataMember="Details" DataKeyNames="StandardID,LineNbr">
                                <RowTemplate>
                                    <px:PXSelector ID="edCategoryID" runat="server" DataField="CategoryID" AlreadyLocalized="False" IsClientControl="True" AutoComplete="True" />
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="Identifier" Width="100px"/>
                                    <px:PXGridColumn DataField="Description" Width="400px" />
                                    <px:PXGridColumn DataField="ShortName" Width="200px" />
                                    <px:PXGridColumn DataField="Regex" Width="200px" />
                                    <px:PXGridColumn DataField="FixedLength" Width="80px" Type="CheckBox" TextAlign="Center" />
                                    <px:PXGridColumn DataField="CategoryID" Width="200px" />
                                    <px:PXGridColumn DataField="LastModifiedByID" Width="150px" />
                                    <px:PXGridColumn DataField="LastModifiedDateTime" Width="150px" />
                                </Columns>
                                <Layout FormViewHeight="" />
                            </px:PXGridLevel>
                        </Levels>
                        <AutoSize Enabled="True" MinHeight="150" />
                        <Mode AllowUpload="True" />
                    </px:PXGrid>
                </Template>
			</px:PXTabItem>
			<px:PXTabItem Text="Used By Contents">
				<Template>
                    <px:PXGrid ID="gridUsedByContents" runat="server" AllowPaging="False" DataSourceID="ds" SkinID="Inquire" Width="100%"
                        Height="150px" MatrixMode="True">
                        <Levels>
                            <px:PXGridLevel DataMember="UsedByContents">
                                <Columns>
                                    <px:PXGridColumn DataField="Active" AllowNull="False" TextAlign="Center" Type="CheckBox"/>
                                    <px:PXGridColumn DataField="Name" Width="350px" LinkCommand="ViewModel"/>
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
            <px:PXTabItem Text="Categories">
                <Template>
                    <px:PXGrid ID="gridCategories" runat="server" DataSourceID="ds" SkinID="DetailsInTab" Width="100%"
                        Height="150px" MatrixMode="True" TabIndex="1000">
                        <Levels>
                            <px:PXGridLevel DataMember="Categories" DataKeyNames="StandardID">
                                <RowTemplate>
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="CategoryID" Width="100px"/>
                                    <px:PXGridColumn DataField="Description" Width="400px" />
                                    <px:PXGridColumn DataField="LastModifiedByID" Width="150px" />
                                    <px:PXGridColumn DataField="LastModifiedDateTime" Width="150px" />
                                </Columns>
                                <Layout FormViewHeight="" />
                            </px:PXGridLevel>
                        </Levels>
                        <AutoSize Enabled="True" MinHeight="150" />
                        <Mode AllowUpload="True" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
		</Items>
	</px:PXTab>
     <!--#include file="~\Pages\AL\Includes\LabelsChangeIDDialog.inc"-->
</asp:Content>