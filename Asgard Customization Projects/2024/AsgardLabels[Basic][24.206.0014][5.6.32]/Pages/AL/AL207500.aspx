<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true"
    ValidateRequest="false" CodeFile="AL207500.aspx.cs" Inherits="Page_AL207500" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server" >
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" TypeName="AA.Objects.AL.ALCategoryMaint" PrimaryView="Document" PageLoadBehavior="SearchSavedKeys">
		<CallbackCommands>
            <px:PXDSCallbackCommand Name="Insert" PostData="Self" />
            <px:PXDSCallbackCommand CommitChanges="True" Name="Save" />
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
            <px:PXCheckBox ID="chkIsSystem" runat="server" Checked="True" DataField="IsSystem" CommitChanges="true" />
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
            <px:PXTabItem Text="Models">
				<Template>
                    <px:PXGrid ID="gridUsedByModels" runat="server" AllowPaging="False" DataSourceID="ds" SkinID="Inquire" Width="100%"
                        Height="150px" MatrixMode="True">
                        <Levels>
                            <px:PXGridLevel DataMember="UsedByModels">
                                <RowTemplate>
<%--                                    <px:PXSelector ID="edName3" runat="server" DataField="Name" />
                                    <px:PXTextEdit ID="edDescription3" runat="server" DataField="Description" />--%>
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
            <px:PXTabItem Text="Data Elements">
				<Template>
                    <px:PXGrid ID="gridDataElements" runat="server" AllowPaging="False" DataSourceID="ds" SkinID="Inquire" Width="100%"
                        Height="150px" MatrixMode="True">
                        <Levels>
                            <px:PXGridLevel DataMember="UsedByDataElements">
                                <RowTemplate>
<%--                                    <px:PXSelector ID="edDataElementName" runat="server" DataField="Name" />
                                    <px:PXTextEdit ID="edDataElementDescription" runat="server" DataField="Description" />--%>
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
            <px:PXTabItem Text="Contents">
				<Template>
                    <px:PXGrid ID="gridUsedByContents" runat="server" AllowPaging="False" DataSourceID="ds" SkinID="Inquire" Width="100%"
                        Height="150px" MatrixMode="True">
                        <Levels>
                            <px:PXGridLevel DataMember="UsedByContents">
                                <RowTemplate>
<%--                                    <px:PXSelector ID="edName3" runat="server" DataField="Name" />
                                    <px:PXTextEdit ID="edDescription3" runat="server" DataField="Description" />--%>
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="Name" Width="300px" LinkCommand="ViewContent" />
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
            <px:PXTabItem Text="Substitutions">
				<Template>
                    <px:PXGrid ID="gridUsedBySubstitutions" runat="server" AllowPaging="False" DataSourceID="ds" SkinID="Inquire" Width="100%"
                        Height="150px" MatrixMode="True">
                        <Levels>
                            <px:PXGridLevel DataMember="UsedBySubstitutions">
                                <RowTemplate>
<%--                                    <px:PXSelector ID="edName3" runat="server" DataField="Name" />
                                    <px:PXTextEdit ID="edDescription3" runat="server" DataField="Description" />--%>
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="Name" Width="300px" LinkCommand="ViewSubstitution" />
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
            <px:PXTabItem Text="Colors">
				<Template>
                    <px:PXGrid ID="gridUsedByColors" runat="server" AllowPaging="False" DataSourceID="ds" SkinID="Inquire" Width="100%"
                        Height="150px" MatrixMode="True">
                        <Levels>
                            <px:PXGridLevel DataMember="UsedByColors">
                                <RowTemplate>
<%--                                    <px:PXSelector ID="edName3" runat="server" DataField="Name" />
                                    <px:PXTextEdit ID="edDescription3" runat="server" DataField="Description" />--%>
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="Name" Width="300px" LinkCommand="ViewColor" />
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
            <px:PXTabItem Text="Rules">
				<Template>
                    <px:PXGrid ID="gridUsedByRules" runat="server" AllowPaging="False" DataSourceID="ds" SkinID="Inquire" Width="100%"
                        Height="150px" MatrixMode="True">
                        <Levels>
                            <px:PXGridLevel DataMember="UsedByRules">
                                <RowTemplate>
<%--                                    <px:PXSelector ID="edName3" runat="server" DataField="Name" />
                                    <px:PXTextEdit ID="edDescription3" runat="server" DataField="Description" />--%>
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="Name" Width="300px" LinkCommand="ViewRule" />
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
            <px:PXTabItem Text="Formats">
				<Template>
                    <px:PXGrid ID="gridUsedByFormats" runat="server" AllowPaging="False" DataSourceID="ds" SkinID="Inquire" Width="100%"
                        Height="150px" MatrixMode="True">
                        <Levels>
                            <px:PXGridLevel DataMember="UsedByFormats">
                                <RowTemplate>
<%--                                    <px:PXSelector ID="edName3" runat="server" DataField="Name" />
                                    <px:PXTextEdit ID="edDescription3" runat="server" DataField="Description" />--%>
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="Name" Width="300px" LinkCommand="ViewFormat" />
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
            <px:PXTabItem Text="Margins">
				<Template>
                    <px:PXGrid ID="gridUsedByMargins" runat="server" AllowPaging="False" DataSourceID="ds" SkinID="Inquire" Width="100%"
                        Height="150px" MatrixMode="True">
                        <Levels>
                            <px:PXGridLevel DataMember="UsedByMargins">
                                <RowTemplate>
<%--                                    <px:PXSelector ID="edName3" runat="server" DataField="Name" />
                                    <px:PXTextEdit ID="edDescription3" runat="server" DataField="Description" />--%>
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="Name" Width="300px" LinkCommand="ViewMargin" />
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
            <px:PXTabItem Text="Justifications">
				<Template>
                    <px:PXGrid ID="gridUsedByJustifications" runat="server" AllowPaging="False" DataSourceID="ds" SkinID="Inquire" Width="100%"
                        Height="150px" MatrixMode="True">
                        <Levels>
                            <px:PXGridLevel DataMember="UsedByJustifications">
                                <RowTemplate>
<%--                                    <px:PXSelector ID="edName3" runat="server" DataField="Name" />
                                    <px:PXTextEdit ID="edDescription3" runat="server" DataField="Description" />--%>
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="Name" Width="300px" LinkCommand="ViewJustification" />
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
            <px:PXTabItem Text="Barcodes">
				<Template>
                    <px:PXGrid ID="gridUsedByBarcodes" runat="server" AllowPaging="False" DataSourceID="ds" SkinID="Inquire" Width="100%"
                        Height="150px" MatrixMode="True">
                        <Levels>
                            <px:PXGridLevel DataMember="UsedByBarcodes">
                                <RowTemplate>
<%--                                    <px:PXSelector ID="edName3" runat="server" DataField="Name" />
                                    <px:PXTextEdit ID="edDescription3" runat="server" DataField="Description" />--%>
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="Name" Width="300px" LinkCommand="ViewBarcode" />
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
            <px:PXTabItem Text="Fonts">
				<Template>
                    <px:PXGrid ID="gridUsedByFonts" runat="server" AllowPaging="False" DataSourceID="ds" SkinID="Inquire" Width="100%"
                        Height="150px" MatrixMode="True">
                        <Levels>
                            <px:PXGridLevel DataMember="UsedByFonts">
                                <RowTemplate>
<%--                                    <px:PXSelector ID="edName3" runat="server" DataField="Name" />
                                    <px:PXTextEdit ID="edDescription3" runat="server" DataField="Description" />--%>
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="Name" Width="300px" LinkCommand="ViewFont" />
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
		</Items>
	</px:PXTab>
     <!--#include file="~\Pages\AL\Includes\LabelsChangeIDDialog.inc"-->
</asp:Content>
