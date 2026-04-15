using System;
using System.Diagnostics;
using Asgard.Labels.Abstractions.Interface;
using Newtonsoft.Json;
using PX.Data;

namespace AA.Objects.Core
{
	// Token: 0x02000031 RID: 49
	[DebuggerDisplay("Printer: ID={ID}, Description={Description}")]
	public class Printer : AcuRenderableConfig, IAcuPrinter, IPrinter, IRenderableConfig, ICloudPrinter, IPrintNodePrinter, IPrintNodeObject, IEpsonPrinter, ILabelPrinter
	{
		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x00008869 File Offset: 0x00006A69
		public string PrinterType { get; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060001D8 RID: 472 RVA: 0x00008871 File Offset: 0x00006A71
		public bool? IsRendering { get; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x00008879 File Offset: 0x00006A79
		public string Drive { get; }

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060001DA RID: 474 RVA: 0x00008881 File Offset: 0x00006A81
		public bool? SupportsLongFiles { get; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001DB RID: 475 RVA: 0x00008889 File Offset: 0x00006A89
		public Guid? PrintStationID { get; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001DC RID: 476 RVA: 0x00008891 File Offset: 0x00006A91
		public Guid? FormatID { get; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060001DD RID: 477 RVA: 0x00008899 File Offset: 0x00006A99
		public Guid? MarginID { get; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060001DE RID: 478 RVA: 0x000088A1 File Offset: 0x00006AA1
		public int? Encoding { get; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060001DF RID: 479 RVA: 0x000088A9 File Offset: 0x00006AA9
		public bool? PushFonts { get; }

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060001E0 RID: 480 RVA: 0x000088B1 File Offset: 0x00006AB1
		public bool? IsEpson { get; }

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060001E1 RID: 481 RVA: 0x000088B9 File Offset: 0x00006AB9
		public string MediaType { get; }

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060001E2 RID: 482 RVA: 0x000088C1 File Offset: 0x00006AC1
		public string MediaForm { get; }

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x000088C9 File Offset: 0x00006AC9
		public string MediaSource { get; }

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x000088D1 File Offset: 0x00006AD1
		public string MediaShape { get; }

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x000088D9 File Offset: 0x00006AD9
		public string EdgeDetection { get; }

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x000088E1 File Offset: 0x00006AE1
		public string PrintMode { get; }

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x000088E9 File Offset: 0x00006AE9
		public int? ContentType { get; }

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x000088F1 File Offset: 0x00006AF1
		public Guid? AcuPrinterID { get; }

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x000088F9 File Offset: 0x00006AF9
		public int? PrintNodePrinterID { get; }

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060001EA RID: 490 RVA: 0x00008901 File Offset: 0x00006B01
		public int? PrintNodeComputerID { get; }

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060001EB RID: 491 RVA: 0x00008909 File Offset: 0x00006B09
		// (set) Token: 0x060001EC RID: 492 RVA: 0x00008911 File Offset: 0x00006B11
		[JsonIgnore]
		public string PrintNodeAPIKey { get; set; }

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060001ED RID: 493 RVA: 0x0000891A File Offset: 0x00006B1A
		public string FieldName { get; }

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060001EE RID: 494 RVA: 0x00008922 File Offset: 0x00006B22
		public string PrinterState
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060001EF RID: 495 RVA: 0x00008922 File Offset: 0x00006B22
		public string Capabilities
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00008928 File Offset: 0x00006B28
		public Printer(PXDataRecord record) : base(record)
		{
			int index = this.index;
			this.index = index + 1;
			this.PrinterType = record.GetString(index);
			index = this.index;
			this.index = index + 1;
			this.IsRendering = record.GetBoolean(index);
			index = this.index;
			this.index = index + 1;
			this.Drive = record.GetString(index);
			index = this.index;
			this.index = index + 1;
			this.SupportsLongFiles = record.GetBoolean(index);
			index = this.index;
			this.index = index + 1;
			this.PrintStationID = record.GetGuid(index);
			index = this.index;
			this.index = index + 1;
			this.FormatID = record.GetGuid(index);
			index = this.index;
			this.index = index + 1;
			this.MarginID = record.GetGuid(index);
			index = this.index;
			this.index = index + 1;
			this.Encoding = record.GetInt32(index);
			index = this.index;
			this.index = index + 1;
			this.PushFonts = record.GetBoolean(index);
			index = this.index;
			this.index = index + 1;
			this.IsEpson = record.GetBoolean(index);
			index = this.index;
			this.index = index + 1;
			this.MediaType = record.GetString(index);
			index = this.index;
			this.index = index + 1;
			this.MediaForm = record.GetString(index);
			index = this.index;
			this.index = index + 1;
			this.MediaSource = record.GetString(index);
			index = this.index;
			this.index = index + 1;
			this.MediaShape = record.GetString(index);
			index = this.index;
			this.index = index + 1;
			this.EdgeDetection = record.GetString(index);
			index = this.index;
			this.index = index + 1;
			this.PrintMode = record.GetString(index);
			index = this.index;
			this.index = index + 1;
			this.ContentType = record.GetInt32(index);
			index = this.index;
			this.index = index + 1;
			this.AcuPrinterID = record.GetGuid(index);
			index = this.index;
			this.index = index + 1;
			this.PrintNodePrinterID = record.GetInt32(index);
			index = this.index;
			this.index = index + 1;
			this.PrintNodeComputerID = record.GetInt32(index);
			index = this.index;
			this.index = index + 1;
			this.PrintNodeAPIKey = AsgardCoreUtils.Decrypt(record.GetString(index));
			index = this.index;
			this.index = index + 1;
			this.FieldName = record.GetString(index);
		}
	}
}
