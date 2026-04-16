<px:PXSmartPanel ID="pnlChangeID" runat="server" Caption="Specify New ID" CaptionVisible="true" DesignView="Hidden" LoadOnDemand="true"
      Key="ChangeIDDialog" CreateOnDemand="false" AutoCallBack-Enabled="true" AcceptButtonID="btnOK"
      AutoCallBack-Target="formChangeID" AutoCallBack-Command="Refresh" CallBackMode-CommitChanges="True" CallBackMode-PostData="Page">
      <px:PXFormView ID="formChangeID" runat="server" DataSourceID="ds" Style="z-index: 100" Width="100%" CaptionVisible="False" DataMember="ChangeIDDialog">
        <ContentStyle BackColor="Transparent" BorderStyle="None" ></ContentStyle>
        <Template>
          <px:PXLayoutRule ID="rlAcctCD" runat="server" StartColumn="True" LabelsWidth="S" ControlSize="XM" ></px:PXLayoutRule>
          <px:PXTextEdit ID="edAcctCD" runat="server" DataField="Name" ></px:PXTextEdit>
        </Template>
      </px:PXFormView>
      <px:PXPanel ID="pnlChangeIDButton" runat="server" SkinID="Buttons">
        <px:PXButton ID="btnOK" runat="server" DialogResult="OK" Text="OK">
          <AutoCallBack Target="formChangeID" Command="Save" ></AutoCallBack>
        </px:PXButton>
        <px:PXButton ID="PXButton3" runat="server" DialogResult="Cancel" Text="Cancel" ></px:PXButton>
      </px:PXPanel>
    </px:PXSmartPanel>	