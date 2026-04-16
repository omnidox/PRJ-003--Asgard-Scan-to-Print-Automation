<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="AL101010.aspx.cs" Inherits="Page_AL101010" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" PrimaryView="Setup" TypeName="AA.Objects.AL.ALPrinterFileProcess">
		<CallbackCommands>
            <px:PXDSCallbackCommand CommitChanges="True" Name="LoadFiles" Visible="true"/>
            <px:PXDSCallbackCommand CommitChanges="True" Name="ProcessFiles" Visible="true"/>
		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
    <px:PXTab ID="tab" runat="server" DataSourceID="ds" Height="500px" Width="100%" DataMember="Setup">
        <Activity HighlightColor="" SelectedColor="" Width="" Height=""></Activity>
        <Items>
            <px:PXTabItem Text="Images">
                <Template>
                    <px:PXGrid ID="ImagesGrid" runat="server" DataSourceID="ds" SkinID="Details" TabIndex="27900" Width="100%">
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
                            <px:PXGridLevel DataMember="ImageFiles" DataKeyNames="PrinterFileGUID">
                                <RowTemplate>
                                    <px:PXSelector ID="edPrinterFileGUIDImage" runat="server" DataField="PrinterFileGUID"/>
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="Active" Type="CheckBox" Width="60px" TextAlign="Center" />
                                    <px:PXGridColumn DataField="Status"/>
                                    <px:PXGridColumn DataField="PrinterFileGUID" Width="270px" LinkCommand="ViewPrinterFile"/>
                                    <px:PXGridColumn DataField="PrinterFileID" Width="90px" LinkCommand="ViewPrinterFileByID"/>
                                    <px:PXGridColumn DataField="FileName" Width="170px"/>
                                    <px:PXGridColumn DataField="ShortFileName" Width="128px"/>
                                    <px:PXGridColumn DataField="Extension" Width="90px"/>
                                    <px:PXGridColumn DataField="Name" Width="180px"/>
                                    <px:PXGridColumn DataField="Description" Width="280px"/>
                                    <px:PXGridColumn DataField="Size"/>
                                    <px:PXGridColumn DataField="Width"/>
                                    <px:PXGridColumn DataField="Height"/>
                                    <px:PXGridColumn DataField="PixelFormat" Type="DropDownList" Width="120px"/>
                                    <px:PXGridColumn DataField="LastModifiedByID" Width="100px" />
                                    <px:PXGridColumn DataField="LastModifiedDateTime" Width="100px" />
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <AutoSize Enabled="True" />
                    </px:PXGrid>
                    <px:PXUploadDialog ID="uploadFilesDialog" runat="server" AutoSaveFile="true" Caption="Upload Files from Zip" Key="Setup"
                                Height="110px" SessionKey="DoImportFilesFromZip" AllowedFileTypes=".zip;" />
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="Fonts">
                <Template>
                    <px:PXGrid ID="FontsGrid" runat="server" DataSourceID="ds" SkinID="Details" TabIndex="27900" Width="100%" MatrixMode="true">
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
                            <px:PXGridLevel DataMember="FontFiles" DataKeyNames="PrinterFileGUID">
                                <RowTemplate>
                                    <px:PXSelector ID="edPrinterFileGUIDFont" runat="server" DataField="PrinterFileGUID"/>
                                    <px:PXDropDown ID="edFontStyle" runat="server" DataField="FontStyle" AllowMultiSelect="true"/>
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="Active" Type="CheckBox" Width="60px" TextAlign="Center" />
                                    <px:PXGridColumn DataField="Status"/>
                                    <px:PXGridColumn DataField="PrinterFileGUID" Width="270px" LinkCommand="ViewPrinterFile"/>
                                    <px:PXGridColumn DataField="PrinterFileID" Width="90px" LinkCommand="ViewPrinterFileByID"/>
                                    <px:PXGridColumn DataField="FileName" Width="160px"/>
                                    <px:PXGridColumn DataField="ShortFileName" Width="120px"/>
                                    <px:PXGridColumn DataField="Extension" Width="90px"/>
                                    <px:PXGridColumn DataField="Name" Width="180px"/>
                                    <px:PXGridColumn DataField="Description" Width="280px"/>
                                    <px:PXGridColumn DataField="FontStyle"/>
                                    <px:PXGridColumn DataField="Size"/>
                                    <px:PXGridColumn DataField="Width"/>
                                    <px:PXGridColumn DataField="Height"/>
                                    <px:PXGridColumn DataField="Ascent"/>
                                    <px:PXGridColumn DataField="Descent"/>
                                    <px:PXGridColumn DataField="LineSpacing"/>
                                    <px:PXGridColumn DataField="LastModifiedByID" Width="100px" />
                                    <px:PXGridColumn DataField="LastModifiedDateTime" Width="100px" />
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <AutoSize Enabled="True" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="Others">
                <Template>
                    <px:PXGrid ID="OthersGrid" runat="server" DataSourceID="ds" SkinID="Details" TabIndex="27900" Width="100%">
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
                            <px:PXGridLevel DataMember="OtherFiles" DataKeyNames="PrinterFileGUID">
                                <RowTemplate>
                                    <px:PXSelector ID="edPrinterFileGUIDOther" runat="server" DataField="PrinterFileGUID"/>
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="Active" Type="CheckBox" Width="60px" TextAlign="Center" />
                                    <px:PXGridColumn DataField="Status"/>
                                    <px:PXGridColumn DataField="PrinterFileGUID" Width="270px" LinkCommand="ViewPrinterFile"/>
                                    <px:PXGridColumn DataField="PrinterFileID" Width="90px" LinkCommand="ViewPrinterFileByID"/>
                                    <px:PXGridColumn DataField="FileName" Width="170px"/>
                                    <px:PXGridColumn DataField="ShortFileName" Width="120px"/>
                                    <px:PXGridColumn DataField="Extension" Width="90px"/>
                                    <px:PXGridColumn DataField="Name" Width="180px"/>
                                    <px:PXGridColumn DataField="Description" Width="600px"/>
                                    <px:PXGridColumn DataField="Size"/>
                                    <px:PXGridColumn DataField="LastModifiedByID" Width="100px" />
                                    <px:PXGridColumn DataField="LastModifiedDateTime" Width="100px" />
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <AutoSize Enabled="True" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
        </Items>
        <AutoSize MinHeight="480" Container="Window" Enabled="True" />
    </px:PXTab>
</asp:Content>
