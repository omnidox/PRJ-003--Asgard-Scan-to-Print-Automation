using System;
using Asgard.Labels.Abstractions.Interface;
using PX.Data;

namespace AA.Objects.Labels.Integration
{
	// Token: 0x020001B9 RID: 441
	public class ModelAction
	{
		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x0600131A RID: 4890 RVA: 0x000423CD File Offset: 0x000405CD
		public PXAction Action { get; }

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x0600131B RID: 4891 RVA: 0x000423D5 File Offset: 0x000405D5
		public IAcuModel Model { get; }

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x0600131C RID: 4892 RVA: 0x000423DD File Offset: 0x000405DD
		public IPrinter Printer { get; }

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x0600131D RID: 4893 RVA: 0x000423E5 File Offset: 0x000405E5
		public Guid? RuleID
		{
			get
			{
				return this.Model.FilterRuleID;
			}
		}

		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x0600131E RID: 4894 RVA: 0x000423F4 File Offset: 0x000405F4
		public bool ReverseRule
		{
			get
			{
				return this.Model.ReverseFilter.GetValueOrDefault();
			}
		}

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x0600131F RID: 4895 RVA: 0x00042414 File Offset: 0x00040614
		public bool HideInstead
		{
			get
			{
				return this.Model.HideInstead.GetValueOrDefault();
			}
		}

		// Token: 0x06001320 RID: 4896 RVA: 0x00042434 File Offset: 0x00040634
		public ModelAction(PXAction action, IAcuModel model, IPrinter printer)
		{
			this.Action = action;
			this.Model = model;
			this.Printer = printer;
		}

		// Token: 0x06001321 RID: 4897 RVA: 0x00042453 File Offset: 0x00040653
		public void SetEnabled(bool isEnabled)
		{
			this.Action.SetEnabled(isEnabled);
		}

		// Token: 0x06001322 RID: 4898 RVA: 0x00042463 File Offset: 0x00040663
		public void SetVisible(bool isVisible)
		{
			this.Action.SetVisible(isVisible);
		}

		// Token: 0x06001323 RID: 4899 RVA: 0x00042473 File Offset: 0x00040673
		public void SetTooltip(string tooltip)
		{
			this.Action.SetTooltip(tooltip);
		}
	}
}
