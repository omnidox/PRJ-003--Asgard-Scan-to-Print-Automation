<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true"
    ValidateRequest="false" CodeFile="AL204500.aspx.cs" Inherits="Page_AL204500" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" TypeName="AA.Objects.AL.ALPrintStationMaint" PrimaryView ="Document">
        <CallbackCommands>
            <px:PXDSCallbackCommand Name="ImportAll" Visible="False" />
            <px:PXDSCallbackCommand Name="ExportAll" Visible="False" />
            <px:PXDSCallbackCommand Name="ToggleExport" Visible="False" />
            <px:PXDSCallbackCommand Name="ImportFiles" Visible="False" />
        </CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" Style="z-index: 100" Width="100%" DataMember="Document" TabIndex="2000">
		<Template>
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" />
            <px:PXSelector ID="edName" runat="server" DataField="Name" DataSourceID="ds" />
            <px:PXTextEdit ID="edDescription" runat="server" DataField="Description" Width="300px" AlreadyLocalized="False" IsClientControl="True" />
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="S" />
            <px:PXCheckBox ID="chkActive" runat="server" Checked="True" DataField="Active" CommitChanges="True" AlreadyLocalized="False" IsClientControl="True" />
            <px:PXCheckBox ID="chkAllowExport" runat="server" Checked="True" DataField="AllowExport" CommitChanges="True" />
            <px:PXUploadDialog ID="uploadFileDialog" runat="server" AutoSaveFile="true" Caption="Upload XML Zip" Key="Document"
                           Height="110px" SessionKey="XmlUploadAllEntities" AllowedFileTypes=".zip;"/>
		</Template>
	</px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXTab ID="tab" runat="server" Height="300px" Width="100%" DataMember="CurrentDocument" DataSourceID="ds">
        <AutoSize Enabled="True" Container="Window" MinWidth="300" MinHeight="500" />
		<Items>
			<px:PXTabItem Text="Used By Printers">
			    <Template>
                    <px:PXGrid ID="UsedByPrinterGrid" runat="server" DataSourceID="ds" AllowPaging="False" SkinID="Inquire" Width="100%"
                        Height="450px" MatrixMode="True">
                        <Levels>
                            <px:PXGridLevel DataKeyNames="Name" DataMember="UsedByPrinters">
                                <RowTemplate>
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="Name" Width="180px"/>
                                    <px:PXGridColumn DataField="Description" Width="280px"/>
                                    <px:PXGridColumn DataField="Active" TextAlign="Center" Type="CheckBox" Width="60px"/>
                                    <px:PXGridColumn DataField="LastModifiedByID" Width="280px"/>
                                    <px:PXGridColumn DataField="LastModifiedDateTime" Width="90px"/>
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                    </px:PXGrid>
                </Template>
			</px:PXTabItem>
			<px:PXTabItem Text="Used By Model Printers">
			    <Template>
                    <px:PXGrid ID="UsedByModelGrid" runat="server" DataSourceID="ds" AllowPaging="False" SkinID="Inquire" Width="100%"
                        Height="450px" MatrixMode="True">
                        <Levels>
                            <px:PXGridLevel DataKeyNames="Name" DataMember="UsedByModelPrinters">
                                <RowTemplate>
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="ALModel__Name" Width="180px" />
                                    <px:PXGridColumn DataField="ALModel__Description" Width="280px"/>
                                    <px:PXGridColumn DataField="Active" TextAlign="Center" Type="CheckBox" Width="60px"/>
                                    <px:PXGridColumn DataField="LastModifiedByID" Width="280px"/>
                                    <px:PXGridColumn DataField="LastModifiedDateTime" Width="90px"/>
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                    </px:PXGrid>
                </Template>
			</px:PXTabItem>
			<px:PXTabItem Text="Used By Users">
			    <Template>
                    <px:PXGrid ID="UsedByUserGrid" runat="server" DataSourceID="ds" AllowPaging="False" SkinID="Inquire" Width="100%"
                        Height="450px" MatrixMode="True">
                        <Levels>
                            <px:PXGridLevel DataKeyNames="Name" DataMember="UsedByUsers">
                                <RowTemplate>
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="Users__Username" Width="180px"/>
                                    <px:PXGridColumn DataField="Users__DisplayName" Width="280px"/>
                                    <px:PXGridColumn DataField="Users__State" Type="DropDownList"/>
                                    <px:PXGridColumn DataField="LastModifiedByID" Width="280px"/>
                                    <px:PXGridColumn DataField="LastModifiedDateTime" Width="90px"/>
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                    </px:PXGrid>
                </Template>
			</px:PXTabItem>
		</Items>
		<AutoSize Container="Window" Enabled="True" MinHeight="150" />
	</px:PXTab>
    <!--#include file="~\Pages\AL\Includes\LabelsChangeIDDialog.inc"-->
</asp:Content>
