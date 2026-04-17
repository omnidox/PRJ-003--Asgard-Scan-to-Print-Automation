using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using PX.Common;
using PX.Data;

namespace AA.Objects.AL
{
	// Token: 0x02000209 RID: 521
	public sealed class Models : IPrefetchable, IPXCompanyDependent
	{
		// Token: 0x0600164E RID: 5710 RVA: 0x00051668 File Offset: 0x0004F868
		private IEnumerable<PXDataRecord> GetData()
		{
			return PXDatabase.SelectMulti<ALModel>(new PXDataField[]
			{
				new PXDataField<ALModel.name>(Models.ALIAS),
				new PXDataField<ALModel.labelID>(Models.ALIAS),
				new PXDataField<ALModel.screenID>(Models.ALIAS),
				new PXDataField<ALModel.modelType>(Models.ALIAS),
				new PXDataField<ALModel.graphType>(Models.ALIAS),
				new PXDataField<ALModel.description>(Models.ALIAS),
				new PXDataField<ALModel.tooltip>(Models.ALIAS),
				new PXDataField<ALModel.active>(Models.ALIAS),
				new PXDataField<ALModel.hideWhenInGroup>(Models.ALIAS),
				new PXDataField<ALModel.filterRuleID>(Models.ALIAS),
				new PXDataField<ALModel.reverseFilter>(Models.ALIAS),
				new PXDataField<ALModel.hideInstead>(Models.ALIAS),
				new PXDataField<ALModel.printRuleID>(Models.ALIAS),
				new PXDataField<ALModel.reversePrint>(Models.ALIAS),
				new PXDataField<ALModel.basedOnView>(Models.ALIAS)
			});
		}

		// Token: 0x0600164F RID: 5711 RVA: 0x00051754 File Offset: 0x0004F954
		public void Prefetch()
		{
			this.contentByName.Clear();
			this.contentByID.Clear();
			IEnumerable<PXDataRecord> data = this.GetData();
			foreach (PXDataRecord record in data)
			{
				Models.Model model = new Models.Model(record);
				string name = model.Name;
				Guid? modelID = model.ModelID;
				bool flag = !this.contentByName.ContainsKey(name);
				if (flag)
				{
					this.contentByName.Add(name, model);
				}
				bool flag2 = !this.contentByID.ContainsKey(modelID);
				if (flag2)
				{
					this.contentByID.Add(modelID, model);
				}
			}
		}

		// Token: 0x06001650 RID: 5712 RVA: 0x00051820 File Offset: 0x0004FA20
		public static bool TryGetModelByName(string name, out Models.Model value)
		{
			bool flag = string.IsNullOrEmpty(name);
			bool result;
			if (flag)
			{
				value = null;
				result = false;
			}
			else
			{
				Models slot = PXDatabase.GetSlot<Models>(typeof(Models).Name, new Type[]
				{
					typeof(ALModel)
				});
				Models.Model model;
				bool flag2 = slot.contentByName.TryGetValue(name, out model);
				value = model;
				result = flag2;
			}
			return result;
		}

		// Token: 0x06001651 RID: 5713 RVA: 0x00051884 File Offset: 0x0004FA84
		public static bool TryGetModelByID(Guid? modelID, out Models.Model value)
		{
			bool flag = modelID == null;
			bool result;
			if (flag)
			{
				value = null;
				result = false;
			}
			else
			{
				Models slot = PXDatabase.GetSlot<Models>(typeof(Models).Name, new Type[]
				{
					typeof(ALModel)
				});
				Models.Model model;
				bool flag2 = slot.contentByID.TryGetValue(modelID, out model);
				value = model;
				result = flag2;
			}
			return result;
		}

		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x06001652 RID: 5714 RVA: 0x000518EA File Offset: 0x0004FAEA
		public static Func<Models.Model, bool> IS_SINGLE_OR_GROUP
		{
			get
			{
				return (Models.Model m) => Models.IsSingleOrGroup(m);
			}
		}

		// Token: 0x06001653 RID: 5715 RVA: 0x0005190C File Offset: 0x0004FB0C
		public static bool IsSingleOrGroup(Models.Model model)
		{
			return model != null && (model.ModelType == "S" || model.ModelType == "G");
		}

		// Token: 0x06001654 RID: 5716 RVA: 0x0005194C File Offset: 0x0004FB4C
		public static IEnumerable<Models.Model> GetModelsByScreenID(string screenID, Func<Models.Model, bool> predicate = null)
		{
			Models slot = PXDatabase.GetSlot<Models>(typeof(Models).Name, new Type[]
			{
				typeof(ALModel)
			});
			return (from model in slot.contentByID.Values
			where model.ScreenID == screenID && model.Active.GetValueOrDefault() && (predicate == null || predicate(model))
			select model).ToArray<Models.Model>();
		}

		// Token: 0x06001655 RID: 5717 RVA: 0x000519C0 File Offset: 0x0004FBC0
		public static IEnumerable<Models.Model> GetModelsByGraphType(string graphType, Func<Models.Model, bool> predicate = null)
		{
			Models slot = PXDatabase.GetSlot<Models>(typeof(Models).Name, new Type[]
			{
				typeof(ALModel)
			});
			return (from model in slot.contentByID.Values
			where model.GraphType == graphType && model.Active.GetValueOrDefault() && (predicate == null || predicate(model))
			select model).ToArray<Models.Model>();
		}

		// Token: 0x06001656 RID: 5718 RVA: 0x00051A34 File Offset: 0x0004FC34
		public static IEnumerable<Models.Model> GetModelsByType(string modelType)
		{
			Models slot = PXDatabase.GetSlot<Models>(typeof(Models).Name, new Type[]
			{
				typeof(ALModel)
			});
			return (from model in slot.contentByID.Values
			where model.ModelType == modelType && model.Active.GetValueOrDefault()
			select model).ToArray<Models.Model>();
		}

		// Token: 0x06001657 RID: 5719 RVA: 0x00051A9E File Offset: 0x0004FC9E
		public static void Reset()
		{
			PXDatabase.ResetSlot<Models>(typeof(Models).Name, new Type[]
			{
				typeof(ALModel)
			});
		}

		// Token: 0x04000936 RID: 2358
		private static readonly string ALIAS = typeof(ALModel).Name;

		// Token: 0x04000937 RID: 2359
		private readonly IDictionary<string, Models.Model> contentByName = new Dictionary<string, Models.Model>();

		// Token: 0x04000938 RID: 2360
		private readonly IDictionary<Guid?, Models.Model> contentByID = new Dictionary<Guid?, Models.Model>();

		// Token: 0x0200096B RID: 2411
		[DebuggerDisplay("Model: {Name} ({Description})")]
		public class Model
		{
			// Token: 0x17000B77 RID: 2935
			// (get) Token: 0x0600293E RID: 10558 RVA: 0x0007D69B File Offset: 0x0007B89B
			public string Name { get; }

			// Token: 0x17000B78 RID: 2936
			// (get) Token: 0x0600293F RID: 10559 RVA: 0x0007D6A3 File Offset: 0x0007B8A3
			public Guid? ModelID { get; }

			// Token: 0x17000B79 RID: 2937
			// (get) Token: 0x06002940 RID: 10560 RVA: 0x0007D6AB File Offset: 0x0007B8AB
			public string ScreenID { get; }

			// Token: 0x17000B7A RID: 2938
			// (get) Token: 0x06002941 RID: 10561 RVA: 0x0007D6B3 File Offset: 0x0007B8B3
			public string ModelType { get; }

			// Token: 0x17000B7B RID: 2939
			// (get) Token: 0x06002942 RID: 10562 RVA: 0x0007D6BB File Offset: 0x0007B8BB
			public string GraphType { get; }

			// Token: 0x17000B7C RID: 2940
			// (get) Token: 0x06002943 RID: 10563 RVA: 0x0007D6C3 File Offset: 0x0007B8C3
			public string Description { get; }

			// Token: 0x17000B7D RID: 2941
			// (get) Token: 0x06002944 RID: 10564 RVA: 0x0007D6CB File Offset: 0x0007B8CB
			public string Tooltip { get; }

			// Token: 0x17000B7E RID: 2942
			// (get) Token: 0x06002945 RID: 10565 RVA: 0x0007D6D3 File Offset: 0x0007B8D3
			public bool? Active { get; }

			// Token: 0x17000B7F RID: 2943
			// (get) Token: 0x06002946 RID: 10566 RVA: 0x0007D6DB File Offset: 0x0007B8DB
			public bool? HideWhenInGroup { get; }

			// Token: 0x17000B80 RID: 2944
			// (get) Token: 0x06002947 RID: 10567 RVA: 0x0007D6E3 File Offset: 0x0007B8E3
			public Guid? FilterRuleID { get; }

			// Token: 0x17000B81 RID: 2945
			// (get) Token: 0x06002948 RID: 10568 RVA: 0x0007D6EB File Offset: 0x0007B8EB
			public bool? ReverseFilter { get; }

			// Token: 0x17000B82 RID: 2946
			// (get) Token: 0x06002949 RID: 10569 RVA: 0x0007D6F3 File Offset: 0x0007B8F3
			public bool? HideInstead { get; }

			// Token: 0x17000B83 RID: 2947
			// (get) Token: 0x0600294A RID: 10570 RVA: 0x0007D6FB File Offset: 0x0007B8FB
			public Guid? PrintRuleID { get; }

			// Token: 0x17000B84 RID: 2948
			// (get) Token: 0x0600294B RID: 10571 RVA: 0x0007D703 File Offset: 0x0007B903
			public bool? ReversePrint { get; }

			// Token: 0x17000B85 RID: 2949
			// (get) Token: 0x0600294C RID: 10572 RVA: 0x0007D70B File Offset: 0x0007B90B
			public string BasedOnView { get; }

			// Token: 0x0600294D RID: 10573 RVA: 0x0007D714 File Offset: 0x0007B914
			public Model(PXDataRecord record)
			{
				int num = 0;
				this.Name = record.GetString(num++);
				this.ModelID = record.GetGuid(num++);
				this.ScreenID = record.GetString(num++);
				this.ModelType = record.GetString(num++);
				this.GraphType = record.GetString(num++);
				this.Description = record.GetString(num++);
				this.Tooltip = record.GetString(num++);
				this.Active = record.GetBoolean(num++);
				this.HideWhenInGroup = record.GetBoolean(num++);
				this.FilterRuleID = record.GetGuid(num++);
				this.ReverseFilter = record.GetBoolean(num++);
				this.HideInstead = record.GetBoolean(num++);
				this.PrintRuleID = record.GetGuid(num++);
				this.ReversePrint = record.GetBoolean(num++);
				this.BasedOnView = record.GetString(num++);
			}
		}
	}
}
