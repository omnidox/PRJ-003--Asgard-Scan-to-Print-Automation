

/* ---------------------------------------------------------------------*
*                             Acumatica Inc.                            *

*              Copyright (c) 2005-2025 All rights reserved.             *

*                                                                       *

*                                                                       *

* This file and its contents are protected by United States and         *

* International copyright laws.  Unauthorized reproduction and/or       *

* distribution of all or any portion of the code contained herein       *

* is strictly prohibited and will result in severe civil and criminal   *

* penalties.  Any violations of this copyright will be prosecuted       *

* to the fullest extent possible under law.                             *

*                                                                       *

* UNDER NO CIRCUMSTANCES MAY THE SOURCE CODE BE USED IN WHOLE OR IN     *

* PART, AS THE BASIS FOR CREATING A PRODUCT THAT PROVIDES THE SAME, OR  *

* SUBSTANTIALLY THE SAME, FUNCTIONALITY AS ANY ACUMATICA PRODUCT.       *

*                                                                       *

* THIS COPYRIGHT NOTICE MAY NOT BE REMOVED FROM THIS FILE.              *

* --------------------------------------------------------------------- */

using PX.Data;
using System;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.IN;

namespace PX.Objects.SO
{
	[PXCacheName(Messages.SOShipLineSplitPackage, PXDacType.Details)]
	public class SOShipLineSplitPackage : PXBqlTable, IBqlTable
	{
		#region Keys
		public class PK : PrimaryKeyOf.By
		{
			public static SOShipLineSplitPackage Find(PXGraph graph, string shipmentNbr, int? shipmentLineNbr, int? shipmentSplitLineNbr, int? packageLineNbr, PKFindOptions options = PKFindOptions.None) => FindBy(graph, shipmentNbr, shipmentLineNbr, shipmentSplitLineNbr, packageLineNbr, options);
		}

		public static class FK
		{
			public class Shipment : SOShipment.PK.ForeignKeyOf.By { }
			public class ShipmentLine : SOShipLine.PK.ForeignKeyOf.By { }
			public class ShipmentLineSplit : Objects.SO.SOShipLineSplit.PK.ForeignKeyOf.By { }
			public class PackageDetail : Objects.SO.SOPackageDetail.PK.ForeignKeyOf.By { }
			public class InventoryItem : IN.InventoryItem.PK.ForeignKeyOf.By { }
			public class SubItem : INSubItem.PK.ForeignKeyOf.By { }
			//todo public class UnitOfMeasure : INUnit.UK.ByInventory.ForeignKeyOf.By { }
		}
		#endregion

		#region RecordID
		[PXDBIdentity(IsKey = true)]
		public virtual Int32? RecordID { get; set; }
		public abstract class recordID : PX.Data.BQL.BqlInt.Field { }
		#endregion
		#region ShipmentNbr
		[PXDBString(15, IsUnicode = true, InputMask = "")]
		[PXDBDefault(typeof(SOShipment.shipmentNbr))]
		public virtual String ShipmentNbr { get; set; }
		public abstract class shipmentNbr : PX.Data.BQL.BqlString.Field { }
		#endregion
		#region ShipmentLineNbr
		[PXDBInt]
		[PXFormula(typeof(Selector))]
		public virtual Int32? ShipmentLineNbr { get; set; }
		public abstract class shipmentLineNbr : PX.Data.BQL.BqlInt.Field { }
		#endregion
		#region ShipmentSplitLineNbr
		[PXDBInt]
		[PXUIField(DisplayName = "Shipment Split Line Nbr.")]
		[PXParent(typeof(FK.ShipmentLineSplit))]
		[PXSelector(typeof(Search>,
			And>>>),
			new[] {
				typeof(SOShipLineSplit.lineNbr),
				typeof(SOShipLineSplit.splitLineNbr),
				typeof(SOShipLineSplit.origOrderType),
				typeof(SOShipLineSplit.origOrderNbr),
				typeof(SOShipLineSplit.inventoryID),
				typeof(SOShipLineSplit.lotSerialNbr),
				typeof(SOShipLineSplit.qty),
				typeof(SOShipLineSplit.packedQty),
				typeof(SOShipLineSplit.uOM) }, DirtyRead = true)]
		public virtual Int32? ShipmentSplitLineNbr { get; set; }
		public abstract class shipmentSplitLineNbr : PX.Data.BQL.BqlInt.Field { }
		#endregion
		#region PackageLineNbr
		[PXDBInt]
		[PXDBDefault(typeof(SOPackageDetail.lineNbr))]
		[PXParent(typeof(FK.PackageDetail))]
		public virtual Int32? PackageLineNbr { get; set; }
		public abstract class packageLineNbr : PX.Data.BQL.BqlInt.Field { }
		#endregion
		#region InventoryID
		[Inventory(Enabled = false)]
		[PXFormula(typeof(Selector))]
		public virtual int? InventoryID { get; set; }
		public abstract class inventoryID : PX.Data.BQL.BqlInt.Field { }
		#endregion
		#region SubItemID
		[SubItem(typeof(inventoryID), Enabled = false)]
		[PXFormula(typeof(Selector))]
		public virtual Int32? SubItemID { get; set; }
		public abstract class subItemID : PX.Data.BQL.BqlInt.Field { }
		#endregion
		#region LotSerialNbr
		[PXDBString(INLotSerialStatusByCostCenter.lotSerialNbr.Length, IsUnicode = true, InputMask = "")]
		[PXUIField(DisplayName = "Lot/Serial Nbr.", FieldClass = "LotSerial", Enabled = false)]
		[PXFormula(typeof(Selector))]
		public virtual String LotSerialNbr { get; set; }
		public abstract class lotSerialNbr : PX.Data.BQL.BqlString.Field { }
		#endregion
		#region UOM
		[INUnit(typeof(inventoryID), DisplayName = "UOM", Enabled = false)]
		[PXFormula(typeof(Selector))]
		public virtual String UOM { get; set; }
		public abstract class uOM : PX.Data.BQL.BqlString.Field { }
		#endregion
		#region Qty
		[PXDBQuantity(typeof(uOM), typeof(basePackedQty))]
		[PXDefault(TypeCode.Decimal, "0.0")]
		[PXUIField(DisplayName = "Quantity")]
		public virtual Decimal? PackedQty { get; set; }
		public abstract class packedQty : PX.Data.BQL.BqlDecimal.Field { }
		#endregion
		#region BaseQty
		[PXDBDecimal(6)]
		public virtual Decimal? BasePackedQty { get; set; }
		public abstract class basePackedQty : PX.Data.BQL.BqlDecimal.Field { }
		#endregion
		#region UnitPriceFactor
		[PXDBDecimal(6)]
		[PXDefault(TypeCode.Decimal, "0.0")]
		public virtual Decimal? UnitPriceFactor { get; set; }
		public abstract class unitPriceFactor : PX.Data.BQL.BqlDecimal.Field { }
		#endregion
		#region WeightFactor
		[PXDBDecimal(6)]
		[PXDefault(TypeCode.Decimal, "1.0")]
		public virtual Decimal? WeightFactor { get; set; }
		public abstract class weightFactor : PX.Data.BQL.BqlDecimal.Field { }
		#endregion

		#region System Columns
		[PXDBCreatedByID]
		public virtual Guid? CreatedByID
		{
			get;
			set;
		}
		public abstract class createdByID : PX.Data.BQL.BqlGuid.Field { }

		[PXDBCreatedByScreenID]
		public virtual String CreatedByScreenID
		{
			get;
			set;
		}
		public abstract class createdByScreenID : PX.Data.BQL.BqlString.Field { }

		[PXDBCreatedDateTime]
		public virtual DateTime? CreatedDateTime
		{
			get;
			set;
		}
		public abstract class createdDateTime : PX.Data.BQL.BqlDateTime.Field { }

		[PXDBLastModifiedByID]
		public virtual Guid? LastModifiedByID
		{
			get;
			set;
		}
		public abstract class lastModifiedByID : PX.Data.BQL.BqlGuid.Field { }

		[PXDBLastModifiedByScreenID]
		public virtual String LastModifiedByScreenID
		{
			get;
			set;
		}
		public abstract class lastModifiedByScreenID : PX.Data.BQL.BqlString.Field { }

		[PXDBLastModifiedDateTime]
		public virtual DateTime? LastModifiedDateTime
		{
			get;
			set;
		}
		public abstract class lastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field { }

		[PXDBTimestamp(VerifyTimestamp = VerifyTimestampOptions.BothFromGraphAndRecord)]
		public virtual Byte[] tstamp
		{
			get;
			set;
		}
		public abstract class Tstamp : PX.Data.BQL.BqlByteArray.Field { }
		#endregion
	}
}
