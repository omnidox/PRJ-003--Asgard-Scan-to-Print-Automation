using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AA.Objects.Core;
using Asgard.Labels.Abstractions.Interface;
using Asgard.Labels.Abstractions.Service;
using Asgard.Labels.Impl.Context;
using PX.BarcodeProcessing;
using PX.Data;

namespace AA.Objects.Labels.Mobile
{
	// Token: 0x020001AC RID: 428
	public class LabelConductor<TSelf, TGraph> : LabelConductor<TGraph> where TSelf : BarcodeDrivenStateMachine<TSelf, TGraph> where TGraph : PXGraph, new()
	{
		// Token: 0x060012B2 RID: 4786 RVA: 0x00040F45 File Offset: 0x0003F145
		internal LabelConductor(ILabelHandler<TSelf, TGraph> labelHandler) : base(labelHandler)
		{
			this._selfLabelHandler = labelHandler;
		}

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x060012B3 RID: 4787 RVA: 0x00040F57 File Offset: 0x0003F157
		public object PrimaryRow
		{
			get
			{
				return this._selfLabelHandler.PrimaryRow;
			}
		}

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x060012B4 RID: 4788 RVA: 0x00040F64 File Offset: 0x0003F164
		public IEnumerable Details
		{
			get
			{
				return this._selfLabelHandler.Details;
			}
		}

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x060012B5 RID: 4789 RVA: 0x00040F71 File Offset: 0x0003F171
		public TSelf Basis
		{
			get
			{
				return this._selfLabelHandler.ScanBasis;
			}
		}

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x060012B6 RID: 4790 RVA: 0x00040F7E File Offset: 0x0003F17E
		public ILabelGenerator<IAcuLabelContext> LabelGenerator
		{
			get
			{
				return this._selfLabelHandler.LabelGenerator;
			}
		}

		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x060012B7 RID: 4791 RVA: 0x00040F8B File Offset: 0x0003F18B
		public IModelProvider ModelProvider
		{
			get
			{
				return this._selfLabelHandler.ModelProvider;
			}
		}

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x060012B8 RID: 4792 RVA: 0x00040F98 File Offset: 0x0003F198
		public IEntityContextFactory EntityContextFactory
		{
			get
			{
				return this._selfLabelHandler.EntityContextFactory;
			}
		}

		// Token: 0x060012B9 RID: 4793 RVA: 0x00040FA8 File Offset: 0x0003F1A8
		public IEnumerable<BarcodeDrivenStateMachine<TSelf, TGraph>.ScanCommand> GetCommands(Func<IModel, bool> predicate = null)
		{
			IEnumerable<IAcuModel> models = base.GetModels(predicate);
			return models.Select(new Func<IAcuModel, BarcodeDrivenStateMachine<TSelf, TGraph>.ScanCommand>(this.ToCommand)).ToArray<BarcodeDrivenStateMachine<TSelf, TGraph>.ScanCommand>();
		}

		// Token: 0x060012BA RID: 4794 RVA: 0x00040FDC File Offset: 0x0003F1DC
		private BarcodeDrivenStateMachine<TSelf, TGraph>.ScanCommand ToCommand(IAcuModel model)
		{
			return new LabelPrintCommand<TSelf, TGraph>(this, model);
		}

		// Token: 0x0400078E RID: 1934
		private readonly ILabelHandler<TSelf, TGraph> _selfLabelHandler;
	}
}
