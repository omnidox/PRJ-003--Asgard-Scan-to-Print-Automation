public class ALRSSSOShipmentEntryExt:PXGraphExtension<SOShipmentEntry>, IExtends<SOShipmentEntry>
	PXSelectJoin<SOPackageDetail,LeftJoin`3,Where`2,OrderBy`1> ALiStarPackages;
	Boolean IsActive();
	void Initialize();
	void Configure(PXScreenConfiguration configuration);