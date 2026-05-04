


	public class CarrierRates : CarrierRatesExtension<SOShipmentEntry, SOShipment>
		{
			protected override DocumentMapping GetDocumentMapping() => new DocumentMapping(typeof(SOShipment)) { DocumentDate = typeof(SOShipment.shipDate) };
			protected override DocumentPackageMapping GetDocumentPackageMapping() => new DocumentPackageMapping(typeof(SOPackageDetailEx)) { };

			protected override void CalculateFreightCost(Document doc)
			{
				Base.CalculateFreightCost(true);
			}

			protected override void UpdatePackageWeightFromScale(decimal? weight)
			{
				Base.Packages.Current.Weight = weight;
				Base.Packages.Update(Base.Packages.Current);
			}

			public virtual CarrierRequest BuildRateRequest(SOShipment order) => base.BuildRateRequest(Documents.Cache.GetExtension<Document>(order));
			protected override CarrierRequest GetCarrierRequest(Document doc, UnitsType unit, List<string> methods, List<CarrierBoxEx> boxes)
			{
				var shipment = (SOShipment)Documents.Cache.GetMain(doc);

				SOShipmentAddress shipAddress = Base.Shipping_Address.Select();
				BAccount companyAccount = PXSelectJoin<BAccountR, InnerJoin<GL.Branch, On<GL.Branch.bAccountID, Equal<BAccountR.bAccountID>>>, Where<GL.Branch.branchID, Equal<Required<GL.Branch.branchID>>>>.Select(Base, Base.Accessinfo.BranchID);
				Address companyAddress = PXSelect<Address, Where<Address.addressID, Equal<Required<Address.addressID>>>>.Select(Base, companyAccount.DefAddressID);
				SOShipmentContact shipContact = Base.Shipping_Contact.Select();
				Contact companyContact = PXSelect<Contact, Where<Contact.contactID, Equal<Required<Contact.contactID>>>>.Select(Base, companyAccount.DefContactID);

				CarrierRequest cr = new CarrierRequest(unit, shipment.CuryID);
				cr.Shipper = companyAddress;
				cr.Origin = null;
				cr.Destination = shipAddress;
				cr.PackagesEx = boxes;
				cr.Resedential = shipment.Resedential == true;
				cr.SaturdayDelivery = shipment.SaturdayDelivery == true;
				cr.Insurance = shipment.Insurance == true;
				cr.ShipDate = Tools.Max(Base.Accessinfo.BusinessDate.Value.Date, shipment.ShipDate.Value);
				cr.Methods = methods;
				cr.Attributes = new List<string>();
				cr.InvoiceLineTotal = Base.Document.Current.LineTotal.GetValueOrDefault();
				cr.ShipperContact = companyContact;
				cr.DestinationContact = shipContact;

				if (shipment.GroundCollect == true && Base.CanUseGroundCollect(shipment))
					cr.Attributes.Add("COLLECT");

				return cr;
			}

			protected override IEnumerable<Tuple<ILineInfo, InventoryItem>> GetLines(Document doc)
			{
				var shipment = (SOShipment)Documents.Cache.GetMain(doc);

				return
					PXSelectJoin<SOShipLine,
					InnerJoin<InventoryItem, On<SOShipLine.FK.InventoryItem>>,
					Where<SOShipLine.FK.Shipment.SameAsCurrent>,
					OrderBy<Asc<SOShipLine.shipmentType, Asc<SOShipLine.shipmentNbr, Asc<SOShipLine.lineNbr>>>>>
					.SelectMultiBound(Base, new object[] { shipment }).AsEnumerable()
					.Cast<PXResult<SOShipLine, InventoryItem>>()
					.Select(r => Tuple.Create<ILineInfo, InventoryItem>(new LineInfo(r), r));
			}

			protected override IList<SOPackageEngine.PackSet> GetPackages(Document doc, bool suppressRecalc = false)
			{
				var shipment = (SOShipment)Documents.Cache.GetMain(doc);

				SOPackageEngine.PackSet set = new SOPackageEngine.PackSet(shipment.SiteID.Value);
				foreach (SOPackageDetailEx package in Base.Packages.View.SelectMultiBound(new object[] { shipment }))
					set.Packages.Add(package.ToPackageInfo(shipment.SiteID.Value));

				return set.AsSingleEnumerable().ToList();
			}

			protected override void ClearPackages(Document doc)
			{
				foreach (SOPackageDetailEx package in Base.Packages.View.SelectMultiBound(new object[] { Documents.Cache.GetMain(doc) }))
					Base.Packages.Delete(package);
			}

			protected override void InsertPackages(IEnumerable<SOPackageInfoEx> packages)
			{
				foreach (SOPackageInfoEx package in packages)
					Base.Packages.Insert(package.ToPackageDetail(SOPackageType.Auto).Apply(d => d.ShipmentNbr = Base.Document.Current.ShipmentNbr));
			}

			protected override void RecalculatePackagesForOrder(Document doc)
			{
				if (Base.Document.Current != null
					&& Base.Document.Current.UnlimitedPackages != true)
				{
					if (Base.Document.Current.Released == true || Base.Document.Current.Confirmed == true)
						throw new PXException(Messages.PackagesRecalcErrorReleasedDocument);

					if (Base.Document.Current.SiteID == null)
						throw new PXException(Messages.PackagesRecalcErrorWarehouseIdNotSpecified);

					Base.PackageDetailExt.OnBeforeRecalculatePackages(doc);

					PXRowDeleted packageContentDeleted = (s,e) =>
						Base.PackageDetailExt.OnAutoPackageContentDeleted((SOShipLineSplitPackage)e.Row);

					int packageCount = 0;
					decimal weightTotal = 0;
					SOPackageEngine.PackSet manualPackSet;
					IList<SOPackageEngine.PackSet> packsets = CalculatePackages(Base.Document.Current, out manualPackSet);

					try
					{
						Base.RowDeleted.AddHandler<SOShipLineSplitPackage>(packageContentDeleted);

						foreach (SOPackageDetailEx package in Base.Packages.Select())
						{
							if (manualPackSet.Packages.Count == 0 && package.PackageType != SOPackageType.Auto)
							{
								weightTotal += package.Weight.GetValueOrDefault();
								packageCount++;
								continue;
							}
							Base.Packages.Delete(package);
						}
					}
					finally
					{
						Base.RowDeleted.RemoveHandler<SOShipLineSplitPackage>(packageContentDeleted);
					}

					foreach (SOPackageEngine.PackSet ps in packsets)
					{
						foreach (SOPackageInfoEx package in ps.Packages)
						{
							weightTotal += package.GrossWeight.GetValueOrDefault();

							SOPackageDetailEx detail = new SOPackageDetailEx();
							detail.PackageType = SOPackageType.Auto;
							detail.ShipmentNbr = Base.Document.Current.ShipmentNbr;
							detail.BoxID = package.BoxID;
							detail.Weight = package.GrossWeight;
							detail.WeightUOM = package.WeightUOM;
							detail.Qty = package.Qty;
							detail.QtyUOM = package.QtyUOM;
							detail.InventoryID = package.InventoryID;
							detail.DeclaredValue = package.DeclaredValue;

							detail = Base.Packages.Insert(detail);
							detail.Confirmed = false;
							packageCount++;
						}
					}

					foreach (SOPackageInfoEx package in manualPackSet.Packages)
					{
						weightTotal += package.GrossWeight.GetValueOrDefault();

						SOPackageDetailEx detail = new SOPackageDetailEx();
						detail.PackageType = SOPackageType.Manual;
						detail.ShipmentNbr = Base.Document.Current.ShipmentNbr;
						detail.BoxID = package.BoxID;
						detail.Weight = package.GrossWeight;
						detail.WeightUOM = package.WeightUOM;
						detail.Qty = package.Qty;
						detail.QtyUOM = package.QtyUOM;
						detail.InventoryID = package.InventoryID;
						detail.DeclaredValue = package.DeclaredValue;
						detail.Height = package.Height;
						detail.Width = package.Width;
						detail.Length = package.Length;

						detail = Base.Packages.Insert(detail);
						detail.Confirmed = false;
						packageCount++;
					}

					Base.Document.Current.IsPackageValid = true;
					Base.Document.Current.RecalcPackagesReason = SOShipment.recalcPackagesReason.None;
					Base.Document.Current.PackageWeight = weightTotal;
					Base.Document.Current.PackageCount = packageCount;

					Base.Document.Update(Base.Document.Current);
				}
			}

			protected virtual IList<SOPackageEngine.PackSet> CalculatePackages(SOShipment shipment, out SOPackageEngine.PackSet manualPackSet)
			{
				SOOrderExtension ext = Base.GetExtension<SOOrderExtension>();

				Dictionary<string, SOPackageEngine.ItemStats> stats = new Dictionary<string, SOPackageEngine.ItemStats>();

				PXSelectBase<SOPackageInfoEx> selectManual = new PXSelect<SOPackageInfoEx,
						Where<SOPackageInfoEx.orderType, Equal<Required<SOOrder.orderType>>,
						And<SOPackageInfoEx.orderNbr, Equal<Required<SOOrder.orderNbr>>,
						And<SOPackageInfoEx.siteID, Equal<Required<SOPackageInfoEx.siteID>>>>>>(Base);

				SOPackageEngine.OrderInfo orderInfo = new SOPackageEngine.OrderInfo(shipment.ShipVia);

				manualPackSet = new SOPackageEngine.PackSet(shipment.SiteID.Value);
				List<string> processedManualPackageOrders = new List<string>();
				foreach (SOShipLine line in Base.Transactions.View.SelectMultiBound(new object[] { shipment }))
				{
					SOOrder order = PXParentAttribute.SelectParent<SOOrder>(Base.Transactions.Cache, line);
					bool manualPackaging =
						PXAccess.FeatureInstalled<FeaturesSet.autoPackaging>() == false
						|| order?.IsManualPackage == true
						|| Base.Document?.Current?.UnlimitedPackages == true;
					if (manualPackaging)
					{
						string key = string.Format("{0}.{1}.{2}", order.OrderType, order.OrderNbr, shipment.SiteID);
						if (!processedManualPackageOrders.Contains(key))
						{
							foreach (SOPackageInfoEx box in selectManual.Select(order.OrderType, order.OrderNbr, shipment.SiteID))
							{
								// DeclaredValue from Sales Order should be converted to base currency.
								decimal baseCuryDeclaredValue;
								PXDBCurrencyAttribute.CuryConvBase<SOOrder.curyInfoID>(
									ext.soorder.Cache, order, box.DeclaredValue ?? 0m, out baseCuryDeclaredValue);

								box.DeclaredValue = baseCuryDeclaredValue;
								manualPackSet.Packages.Add(box);
							}
							processedManualPackageOrders.Add(key);
						}
					}
					else
					{
						InventoryItem item = InventoryItem.PK.Find(Base, line.InventoryID);

						if (item.PackageOption == INPackageOption.Manual)
							continue;

						orderInfo.AddLine(item, line.BaseQty);


						int inventoryID = item.PackSeparately == true
							? item.InventoryID.Value
							: SOPackageEngine.ItemStats.Mixed;

						string key = string.Format("{0}.{1}.{2}.{3}", line.SiteID, inventoryID, item.PackageOption, line.Operation);

						SOPackageEngine.ItemStats stat;
						if (stats.ContainsKey(key))
						{
							stat = stats[key];
							stat.BaseQty += line.BaseQty.GetValueOrDefault();
							stat.BaseWeight += line.ExtWeight.GetValueOrDefault();
							stat.DeclaredValue += line.LineAmt ?? 0m;
							stat.AddLine(item, line.BaseQty);
						}
						else
						{
							stat = new SOPackageEngine.ItemStats();
							stat.SiteID = line.SiteID;
							stat.InventoryID = inventoryID;
							stat.Operation = line.Operation;
							stat.PackOption = item.PackageOption;
							stat.BaseQty += line.BaseQty.GetValueOrDefault();
							stat.BaseWeight += line.ExtWeight.GetValueOrDefault();
							stat.DeclaredValue += line.LineAmt ?? 0m;
							stat.AddLine(item, line.BaseQty);
							stats.Add(key, stat);
						}
					}
				}
				orderInfo.Stats.AddRange(stats.Values);

				SOPackageEngine engine = CreatePackageEngine();
				return engine.Pack(orderInfo);
			}


			protected virtual IList<CarrierBox> GetPackages(SOShipment shiporder, Carrier carrier, CarrierPlugin plugin)
			{
				List<CarrierBox> list = new List<CarrierBox>();

				List<SOPackageDetailEx> packages = PXSelect<SOPackageDetailEx,
					Where<SOPackageDetailEx.shipmentNbr, Equal<Required<SOShipment.shipmentNbr>>>>
					.Select(Base, shiporder.ShipmentNbr).RowCast<SOPackageDetailEx>().ToList();

				bool failed = false;
				List<SOCarrierPackageDetailEx> carrierPackages = GetCarrierPackageDetail(packages, carrier.CarrierID);

				foreach (SOCarrierPackageDetailEx pkgDetail in carrierPackages)
				{
					SOPackageDetailEx detail = pkgDetail.Package;
					if (carrier.ConfirmationRequired == true)
					{
						if (detail.Confirmed != true)
						{
							failed = true;

							Base.Packages.Cache.RaiseExceptionHandling<SOPackageDetail.confirmed>(detail, detail.Confirmed,
								new PXSetPropertyException(Messages.ConfirmationIsRequired, PXErrorLevel.Error));
						}
					}

					list.Add(BuildCarrierPackage(pkgDetail, plugin));
				}

				if (failed)
				{
					throw new PXException(Messages.ConfirmationIsRequired);
				}

				return list;
			}

			public virtual CarrierBox BuildCarrierPackage(SOCarrierPackageDetailEx pkgDetail, CarrierPlugin plugin)
			{
				SOPackageDetailEx detail = pkgDetail.Package;
				CarrierBox box = new CarrierBox(detail.LineNbr.Value, ConvertWeightValue(detail.Weight ?? 0, plugin));
				box.Description = detail.Description;
				box.DeclaredValue = detail.DeclaredValue ?? 0;
				box.COD = detail.COD ?? 0;
				box.Length = ConvertLinearValue(detail.Length ?? 0, plugin);
				box.Width = ConvertLinearValue(detail.Width ?? 0, plugin);
				box.Height = ConvertLinearValue(detail.Height ?? 0, plugin);
				box.CarrierPackage = pkgDetail.CarrierBoxName;
				box.CustomRefNbr1 = detail.CustomRefNbr1;
				box.CustomRefNbr2 = detail.CustomRefNbr2;

				return box;
			}

			private List<SOCarrierPackageDetailEx> GetCarrierPackageDetail(List<SOPackageDetailEx> packages, string carrierID)
			{
				List<SOCarrierPackageDetailEx> sOCarrierPackages = new List<SOCarrierPackageDetailEx>();
				var carrierPackages = PXSelect<CarrierPackage, Where<CarrierPackage.carrierID, Equal<Required<CarrierPackage.carrierID>>>>
					.Select(Base, carrierID).RowCast<CarrierPackage>().AsEnumerable();

				foreach (var package in packages)
				{
					SOCarrierPackageDetailEx box = new SOCarrierPackageDetailEx();
					box.CarrierID = carrierID;
					box.CarrierBoxName = carrierPackages.Where(x => x.BoxID.Equals(package.BoxID)).Select(y => y.CarrierBox).FirstOrDefault();
					box.Package = package;

					sOCarrierPackages.Add(box);
				}

				return sOCarrierPackages;
			}

			public virtual CarrierRequest BuildRequest(SOShipment shiporder)
			{
				INSite warehouse = INSite.PK.Find(Base, shiporder.SiteID);
				if (warehouse == null)
				{
					Base.Document.Cache.RaiseExceptionHandling<SOShipment.siteID>(shiporder, shiporder.SiteID,
								new PXSetPropertyException(Messages.WarehouseIsRequired, PXErrorLevel.Error));

					throw new PXException(Messages.WarehouseIsRequired);
				}

				SOShipmentAddress shipAddress = PXSelect<SOShipmentAddress, Where<SOShipmentAddress.addressID, Equal<Required<SOShipment.shipAddressID>>>>.Select(Base, shiporder.ShipAddressID);
				SOShipmentContact shipToContact = PXSelect<SOShipmentContact, Where<SOShipmentContact.contactID, Equal<Required<SOShipment.shipContactID>>>>.Select(Base, shiporder.ShipContactID);
				Address warehouseAddress = PXSelect<Address, Where<Address.addressID, Equal<Required<Address.addressID>>>>.Select(Base, warehouse.AddressID);
				Contact warehouseContact = PXSelect<Contact, Where<Contact.contactID, Equal<Required<Contact.contactID>>>>.Select(Base, warehouse.ContactID);
				PXResult<BAccountR, GL.Branch, GL.DAC.Organization> result = (PXResult<BAccountR, GL.Branch, GL.DAC.Organization>)
																			PXSelectJoin<BAccountR,
																			InnerJoin<GL.Branch, On<GL.Branch.bAccountID, Equal<BAccountR.bAccountID>>,
																			InnerJoin<GL.DAC.Organization, On<GL.DAC.Organization.organizationID, Equal<GL.Branch.organizationID>>>>,
																			Where<GL.Branch.branchID, Equal<Required<GL.Branch.branchID>>>>.Select(Base, warehouse.BranchID);
				BAccount companyAccount = result;
				GL.Branch branch = result;
				GL.DAC.Organization organization = result;

				Address shipperAddress = PXSelect<Address, Where<Address.addressID, Equal<Required<Address.addressID>>>>.Select(Base, companyAccount.DefAddressID);
				Contact shipperContact = PXSelect<Contact, Where<Contact.contactID, Equal<Required<Contact.contactID>>>>.Select(Base, companyAccount.DefContactID);

				Carrier carrier = Carrier.PK.Find(Base, shiporder.ShipVia);
				CarrierPlugin plugin = CarrierPlugin.PK.Find(Base, carrier.CarrierPluginID);
				ValidatePlugin(plugin);

				CarrierRequest cr = new CarrierRequest(GetUnitsType(plugin), shiporder.CuryID);
				cr.Attributes = new List<string>();

				Location customerLocation = PXSelect<Location, Where<Location.bAccountID, Equal<Required<Location.bAccountID>>, And<Location.locationID, Equal<Required<Location.locationID>>>>>.Select(Base, shiporder.CustomerID, shiporder.CustomerLocationID);

				bool useGroundCollect = (shiporder.GroundCollect == true && Base.CanUseGroundCollect(shiporder));
				if (useGroundCollect || shiporder.UseCustomerAccount == true)
				{
					//by customer and location
					CarrierPluginCustomer cpc = PXSelect<CarrierPluginCustomer,
						Where<CarrierPluginCustomer.carrierPluginID, Equal<Required<CarrierPluginCustomer.carrierPluginID>>,
							And<CarrierPluginCustomer.customerID, Equal<Required<CarrierPluginCustomer.customerID>>,
							And<CarrierPluginCustomer.isActive, Equal<True>,
							And<Where<CarrierPluginCustomer.customerLocationID, Equal<Required<CarrierPluginCustomer.customerLocationID>>, Or<CarrierPluginCustomer.customerLocationID, IsNull>>>>>>,
						OrderBy<Desc<CarrierPluginCustomer.customerLocationID>>>
						.Select(Base, plugin.CarrierPluginID, shiporder.CustomerID, shiporder.CustomerLocationID);

					if (!string.IsNullOrEmpty(cpc?.CarrierAccount))
					{
						cr.ThirdPartyAccountID = cpc.CarrierAccount;

						Address customerAddress = PXSelect<Address, Where<Address.addressID, Equal<Required<Address.addressID>>>>.Select(Base, customerLocation.DefAddressID);
						cr.ThirdPartyPostalCode = cpc.PostalCode ?? customerAddress.PostalCode;
						cr.ThirdPartyCountryCode = cpc.CountryID ?? customerAddress.CountryID;
					}
					else if (shiporder.UseCustomerAccount == true)
					{
						throw new PXException(Messages.CustomeCarrierAccountIsNotSetup);
					}

					if (shiporder.UseCustomerAccount == true && cpc?.CarrierBillingType == CarrierBillingTypes.Receiver)
					{
						cr.Attributes.Add("RECEIVER");
					}
				}

				decimal freightCharge = 0m;
				if (shiporder.FreightAmountSource == FreightAmountSourceAttribute.OrderBased)
				{
					SOOrderExtension ext = Base.GetExtension<SOOrderExtension>();
					IEnumerable<SOOrderShipment> sOOrderShipments = ext.OrderListSimple.Select().RowCast<SOOrderShipment>();

					//if the freight amount is based on Sales Order and the shipment has multiple SO or if it is a partial shipment, then add attribute "SKIPFREIGHTCHARGE"
					//don't send the overriden freight price field in this case. Otherwise send FreightAmt + PremiumFreightAmt from SO
					if (sOOrderShipments.Count() == 1)
					{
						SOOrderShipment soOrderShipment = sOOrderShipments.FirstOrDefault();
						SOOrder order = ext.soorder.Select(soOrderShipment?.OrderType, soOrderShipment?.OrderNbr);

						if (order?.OrderQty == soOrderShipment?.ShipmentQty)
						{
							freightCharge = (order?.FreightAmt ?? 0m) + (order?.PremiumFreightAmt ?? 0m);
						}
						else
						{
							cr.Attributes.Add("SKIPFREIGHTCHARGE");
						}
					}
					else
					{
						cr.Attributes.Add("SKIPFREIGHTCHARGE");
					}
				}
				else
				{
					freightCharge = shiporder.FreightAmt ?? 0m;
				}

				cr.FreightCharge = freightCharge;

				(SOAddress billToAddress, SOContact billToContact) = Base.GetBillToAddressContact();

				if (billToAddress != null)
				{
					cr.BillToAddress = billToAddress;
				}
				if (billToContact != null)
				{
					cr.BillToContact = billToContact;
				}

				cr.Shipper = shipperAddress;
				cr.ShipperContact = shipperContact;
				cr.Origin = warehouseAddress;
				cr.OriginContact = warehouseContact;
				cr.Destination = shipAddress;
				cr.DestinationContact = shipToContact;
				cr.Packages = GetPackages(shiporder, carrier, plugin);
				cr.Resedential = shiporder.Resedential == true;
				cr.SaturdayDelivery = shiporder.SaturdayDelivery == true;
				cr.Insurance = shiporder.Insurance == true;
				cr.ShipDate = Tools.Max(Base.Accessinfo.BusinessDate.Value.Date, shiporder.ShipDate.Value.Date);
				cr.ReceiverTaxID = customerLocation?.TaxRegistrationID;
				cr.ShipperTaxID = companyAccount.TaxRegistrationID;

				if (useGroundCollect)
				{
					cr.Attributes.Add("COLLECT");
				}
				cr.InvoiceLineTotal = shiporder.LineTotal.GetValueOrDefault();

				if (!string.IsNullOrWhiteSpace(warehouse.CarrierFacility))
				{
					cr.Attributes.Add(string.Concat("CarrierFacilityW:", warehouse.CarrierFacility));
				}

				if (!string.IsNullOrWhiteSpace(branch.CarrierFacility))
				{
					cr.Attributes.Add(string.Concat("CarrierFacilityB:", branch.CarrierFacility));
				}
				else if (!string.IsNullOrWhiteSpace(organization.CarrierFacility))
				{
					cr.Attributes.Add(string.Concat("CarrierFacilityB:", organization.CarrierFacility));
				}

				return cr;
			}

			protected override WebDialogResult AskForRateSelection() => Base.CurrentDocument.AskExt();

			protected virtual void _(Events.FieldUpdated<SOPackageDetailEx, SOPackageDetailEx.boxID> e)
			{
				if (e.Row != null)
				{
					Base.Packages.Cache.SetDefaultExt<SOPackageDetailEx.boxDescription>(e.Row);
					Base.Packages.Cache.SetDefaultExt<SOPackageDetailEx.length>(e.Row);
					Base.Packages.Cache.SetDefaultExt<SOPackageDetailEx.width>(e.Row);
					Base.Packages.Cache.SetDefaultExt<SOPackageDetailEx.height>(e.Row);
					Base.Packages.Cache.SetDefaultExt<SOPackageDetailEx.boxWeight>(e.Row);
					Base.Packages.Cache.SetDefaultExt<SOPackageDetailEx.maxWeight>(e.Row);
				}
			}

			protected virtual void _(Events.RowSelected<SOShipment> e)
			{
				if (!(e.Row is SOShipment row))
					return;

				if (row.UnlimitedPackages == true)
				{
					shopRates.SetEnabled(false);
					shopRates.SetTooltip(Messages.ShopForRatesDisabledDueToUnlimitedPackages);
				}
				else
				{
					shopRates.SetEnabled(true);
					shopRates.SetTooltip(Messages.ShopForRatesButtonCaption);
				}
			}

			/// Overrides <seealso cref="PXGraph.Persist()"/>
			[PXOverride]
			public void Persist( // TODO: override PrePersist instead
				Action base_Persist)
			{
				if (Base.Document.Current != null && Base.Document.Current.IsPackageValid != true &&
					Base.Document.Current.Released != true && Base.Document.Current.Confirmed != true && Base.Document.Current.SiteID != null)
				{
					if (Base.Document.Current.RecalcPackagesReason == SOShipment.recalcPackagesReason.ShipVia && Base.ValidateAvailablePackages())
					{
						foreach (SOPackageDetail package in Base.Packages.Select())
						{
							if (package.PackageType == SOPackageType.Auto)
								package.Confirmed = false;
						}

						Base.Document.Current.IsPackageValid = true;
					}
					else
					{
					recalculatePackages.Press();
				}
				}

				base_Persist();
			}

			protected override IEnumerable<CarrierPlugin> GetApplicableCarrierPlugins()
			{
				return PXSelectReadonly<CarrierPlugin,
					Where<CarrierPlugin.isActive, Equal<True>, And<CarrierPlugin.siteID, IsNull, Or<CarrierPlugin.siteID, Equal<Current<SOShipment.siteID>>>>>>
					.Select(Base)
					.RowCast<CarrierPlugin>();
			}

			private class LineInfo : ILineInfo
			{
				private SOShipLine _line;
				public LineInfo(SOShipLine line) { _line = line; }

				public decimal? BaseQty => _line.BaseQty;
				public decimal? CuryLineAmt => _line.LineAmt;
				public decimal? ExtWeight => _line.ExtWeight;
				public int? SiteID => _line.SiteID;
				public string Operation => _line.Operation;
			}
		}
