using System;
using System.Collections.Generic;
using PX.Data;

namespace AA.Objects.Labels
{
	// Token: 0x0200011D RID: 285
	public class ALModelActionName<ModelID> : BqlFormulaEvaluator<ModelID> where ModelID : IBqlField
	{
		// Token: 0x06000CCC RID: 3276 RVA: 0x0001FEBC File Offset: 0x0001E0BC
		public override object Evaluate(PXCache cache, object item, Dictionary<Type, object> pars)
		{
			Guid? modelID = (Guid?)pars[typeof(ModelID)];
			return BasicLabelUtils.GetActionName(modelID, null);
		}
	}
}
