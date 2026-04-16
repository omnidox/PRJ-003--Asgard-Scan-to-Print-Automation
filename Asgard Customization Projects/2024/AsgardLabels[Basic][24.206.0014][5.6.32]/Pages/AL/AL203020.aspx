<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="AL203020.aspx.cs" Inherits="Page_AL203020" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" TypeName="AA.Objects.AL.ALPrinterFileMaint" 
        PrimaryView="Document" PageLoadBehavior="SearchSavedKeys">
		<CallbackCommands>
		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
	<px:PXFormView ID="form" runat="server" DataSourceID="ds" Style="z-index: 100" Width="100%" DataMember="Document">
		<AutoSize Container="Window" Enabled="True" MinHeight="200" />
		<Template>
			<px:PXLayoutRule runat="server" StartRow="True" StartColumn="True" LabelsWidth="S" ControlSize="XXL"/>
            <px:PXSelector ID="edName" runat="server" DataField="Name" DataSourceID="ds" />
		    <%--<px:PXSelector ID="edPrinterFileGUID" runat="server" DataField="PrinterFileGUID"/>--%>
		    <px:PXSelector ID="edPrinterFileID" runat="server" DataField="PrinterFileID" Width="100px"/>
			<px:PXLayoutRule runat="server" Merge="True"/>
            <px:PXCheckBox ID="chkActive" runat="server" DataField="Active" CommitChanges="true"/>
            <px:PXCheckBox ID="chkAllowExport" runat="server" DataField="AllowExport"/>
            <px:PXCheckBox ID="chkIsComposite" runat="server" DataField="IsComposite" CommitChanges="True"/>
			<px:PXLayoutRule runat="server"/>
            <%--<px:PXTextEdit ID="edName" runat="server"  DataField="Name"/>--%>
            <px:PXTextEdit ID="edDescription" runat="server" DataField="Description"/>
            <px:PXDropDown ID="edFontStyle" runat="server" DataField="FontStyle" Width="150px" CommitChanges="true" AllowMultiSelect="true"/>
            <%--<px:PXDropDown ID="edDrive" runat="server" DataField="Drive" Width="200px" CommitChanges="true"/>--%>
            <px:PXTextEdit ID="edFileName" runat="server" DataField="FileName" Width="160px" CommitChanges="true"/>
            <px:PXTextEdit ID="edShortFileName" runat="server" DataField="ShortFileName" Width="100px" CommitChanges="true"/>
            <px:PXDropDown ID="edStatus" runat="server" DataField="Status" Width="100px" CommitChanges="true"/>
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="S" ControlSize="XM"/>
            <px:PXTextEdit ID="edExtension" runat="server" DataField="Extension"/>
            <px:PXNumberEdit ID="edSize" runat="server" DataField="Size" />
            <px:PXNumberEdit ID="edWidth" runat="server" DataField="Width"/>
            <px:PXNumberEdit ID="edHeight" runat="server" DataField="Height"/>
            <px:PXNumberEdit ID="edMaxWidth" runat="server" DataField="MaxWidth"/>
            <px:PXNumberEdit ID="edMaxHeight" runat="server" DataField="MaxHeight"/>
            <px:PXDropDown ID="edPixelFormat" runat="server" DataField="PixelFormat"/>
            <px:PXNumberEdit ID="edAscent" runat="server" DataField="Ascent"/>
            <px:PXNumberEdit ID="edDescent" runat="server" DataField="Descent"/>
            <px:PXNumberEdit ID="edLineSpacing" runat="server" DataField="LineSpacing"/>
            <px:PXUploadDialog ID="uploadFileDialog" runat="server" AutoSaveFile="true" Caption="Upload XML Zip" Key="Document"
                           Height="110px" SessionKey="XmlUploadAllEntities" AllowedFileTypes=".zip;"/>
			<px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="S" ControlSize="M"/>
            <px:PXFormView ID="VisibilityForm" runat="server" DataMember="CurrentDocument" DataSourceID="ds" Caption="Hidden Form needed for VisibleExp of TabItems"
                Visible="False" TabIndex="300">
                <Template>
                    <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="M" ControlSize="XM" />
                    <px:PXCheckBox ID="chkShowChildren" runat="server" DataField="ShowChildren" AlreadyLocalized="False" IsClientControl="True" />
                </Template>
            </px:PXFormView>
            <px:PXFormView ID="templateDataForm" runat="server" DataMember="Document" DataSourceID="ds" TabIndex="200" SkinID="Transparent">
                <Template>
                    <px:PXImageView ID="edImageUrl" runat="server" DataField="ImageUrl" Width="250px" height="300px" AlreadyLocalized="False" CallbackUpdatable="True" />
                </Template>
            </px:PXFormView>
		</Template>
	</px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXTab ID="tab" runat="server" Height="580px" Width="100%" DataSourceID="ds" >
        <Items>
            <px:PXTabItem Text="Used By Data Elements">
                <Template>
                    <px:PXGrid ID="UsedByDataElementsGrid" runat="server" DataSourceID="ds" Height="150px" Style="border: 0px;"
                        Width="100%" ActionsPosition="Top" SkinID="Inquire" MatrixMode="True" SyncPosition="True" KeepPosition="True" TabIndex="900">
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
                            <px:PXGridLevel DataMember="UsedByDataElements" DataKeyNames="RecordID">
                                <RowTemplate>
                                    <px:PXSelector ID="edName2" runat="server" DataField="Name" />
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="Name" Width="180px" LinkCommand="ViewDataElement" />
                                    <px:PXGridColumn DataField="LastModifiedByID" Width="150px" />
                                    <px:PXGridColumn DataField="LastModifiedDateTime" Width="150px" />
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <AutoSize Enabled="True" MinHeight="150" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="Used By Fonts">
                <Template>
                    <px:PXGrid ID="UsedByFontsGrid" runat="server" DataSourceID="ds" Height="150px" Style="border: 0px;"
                        Width="100%" ActionsPosition="Top" SkinID="Inquire" MatrixMode="True" SyncPosition="True" KeepPosition="True" TabIndex="900">
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
                            <px:PXGridLevel DataMember="UsedByFonts" DataKeyNames="RecordID">
                                <RowTemplate>
                                    <px:PXSelector ID="edName3" runat="server" DataField="Name" />
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="Name" Width="180px" LinkCommand="ViewFont" />
                                    <px:PXGridColumn DataField="LastModifiedByID" Width="150px" />
                                    <px:PXGridColumn DataField="LastModifiedDateTime" Width="150px" />
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <AutoSize Enabled="True" MinHeight="150" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="Rule Details" BindingContext="VisibilityForm" VisibleExp="DataControls[&quot;chkShowChildren&quot;].Value == true">
                <Template>
                    <px:PXGrid ID="subGrid" runat="server" DataSourceID="ds" Height="150px" Style="border: 0px;"
                        Width="100%" ActionsPosition="Top" SkinID="Details" MatrixMode="True">
                        <Levels>
                            <px:PXGridLevel DataMember="Rules">
                                <RowTemplate>
                                    <%--<px:PXCheckBox ID="edReverse" runat="server" DataField="Reverse" />--%>
                                    <px:PXSelector ID="edBAccountID" runat="server" DataField="BAccountID" />
                                    <px:PXSelector ID="edChildPrinterFileID" runat="server" DataField="ChildPrinterFileID" />
                                    <px:PXSelector ID="edRuleID" runat="server" DataField="RuleID" />
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="Active" AllowNull="False" TextAlign="Center" Type="CheckBox" CommitChanges="true" />
                                    <px:PXGridColumn DataField="LineNbr" />
                                    <px:PXGridColumn DataField="SortOrder" />
                                    <px:PXGridColumn DataField="BAccountID" Width="250px" MatrixMode="False" LinkCommand="ViewBAccount" AllowDragDrop="true" CommitChanges="true" />
                                    <px:PXGridColumn DataField="RuleID" Width="250px" MatrixMode="False" LinkCommand="ViewRule" AllowDragDrop="true" CommitChanges="true" />
                                    <px:PXGridColumn DataField="Reverse" Type="CheckBox" TextAlign="Center" AllowDragDrop="true" />
                                    <px:PXGridColumn DataField="ChildPrinterFileID" Width="250px" MatrixMode="False" LinkCommand="ViewrinterFile" AllowDragDrop="true" CommitChanges="true" />
                                    <px:PXGridColumn DataField="Height" AllowDragDrop="true"/>
                                    <px:PXGridColumn DataField="Width" AllowDragDrop="true"/>
                                    <px:PXGridColumn DataField="DoThrow" Type="CheckBox" TextAlign="Center" AllowDragDrop="true" />
                                    <px:PXGridColumn DataField="Message" AllowDragDrop="true" Width="500px" />
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <Mode InitNewRow="True" AllowDragRows="true" AllowUpload="True" />
                        <AutoSize Enabled="True" MinHeight="150" />
                        <CallbackCommands PasteCommand="PasteLine"></CallbackCommands>
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
        </Items>
        <AutoSize Enabled="True" Container="Window" MinWidth="300" MinHeight="250"/>
    </px:PXTab>
</asp:Content>
