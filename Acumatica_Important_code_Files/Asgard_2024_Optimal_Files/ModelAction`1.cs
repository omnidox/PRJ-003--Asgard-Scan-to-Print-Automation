using System;
using System.Collections.Generic;
using PX.Data;

namespace AA.Objects.AL
{
	// Token: 0x0200011C RID: 284
	public class ALModelActionName<ModelID> : BqlFormulaEvaluator<ModelID> where ModelID : IBqlField
	{
		// Token: 0x06000940 RID: 2368 RVA: 0x00021260 File Offset: 0x0001F460
		public override object Evaluate(PXCache cache, object item, Dictionary<Type, object> pars)
		{
			Guid? modelID = (Guid?)pars[typeof(ModelID)];
			return BasicLabelUtils.GetActionName(modelID, null);
		}
	}
}
