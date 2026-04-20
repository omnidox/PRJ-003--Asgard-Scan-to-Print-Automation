using System;
using PX.Data;
using Scriban;

namespace AA.Objects.AL
{
	// Token: 0x0200010D RID: 269
	public class ResultSetIterator
	{
		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06000902 RID: 2306 RVA: 0x00020772 File Offset: 0x0001E972
		public IPXResultset Rows { get; }

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06000903 RID: 2307 RVA: 0x0002077A File Offset: 0x0001E97A
		public int RowCount { get; }

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06000904 RID: 2308 RVA: 0x00020784 File Offset: 0x0001E984
		// (set) Token: 0x06000905 RID: 2309 RVA: 0x0002079C File Offset: 0x0001E99C
		public object Row
		{
			get
			{
				return this._row;
			}
			set
			{
				this._row = value;
				int rowNumber = this.RowNumber;
				this.RowNumber = rowNumber + 1;
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06000906 RID: 2310 RVA: 0x000207C2 File Offset: 0x0001E9C2
		// (set) Token: 0x06000907 RID: 2311 RVA: 0x000207CA File Offset: 0x0001E9CA
		public int RowNumber { get; set; } = -1;

		// Token: 0x06000908 RID: 2312 RVA: 0x000207D3 File Offset: 0x0001E9D3
		public ResultSetIterator(TemplateContext context, IPXResultset rows)
		{
			this.Rows = rows;
			this.RowCount = ((rows != null) ? rows.GetRowCount() : 0);
		}

		// Token: 0x04000334 RID: 820
		private object _row;
	}
}
