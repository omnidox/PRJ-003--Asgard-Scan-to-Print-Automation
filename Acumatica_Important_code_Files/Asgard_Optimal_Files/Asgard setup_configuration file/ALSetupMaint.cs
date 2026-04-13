using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AA.Objects.Core;
using AA.Objects.Core.PrintNode;
using AA.Objects.Labels.Helpers;
using Asgard.Labels.Abstractions.Helpers;
using Asgard.Labels.Abstractions.Interface;
using Asgard.Labels.Abstractions.Poco;
using Asgard.Labels.Impl.Context;
using Asgard.Labels.Impl.Destination;
using PX.Data;
using PX.Objects.CS;
using PX.SM;

namespace AA.Objects.Labels
{
	// Token: 0x02000140 RID: 320
	public class ALSetupMaint : PXGraph<ALSetupMaint>
	{
		// Token: 0x06000E8B RID: 3723 RVA: 0x0002B574 File Offset: 0x00029774
		static ALSetupMaint()
		{
			ALSetupMaint.AddMediaType(".ttf", "font/ttf");
			ALSetupMaint.AddMediaType(".otf", "font/otf");
			ALSetupMaint.AddMediaType(".lbxml", "application/lbxml");
			ALSetupMaint.AddMediaType(".b64", "application/octet-stream");
		}

		// Token: 0x06000E8C RID: 3724 RVA: 0x0002B5CC File Offset: 0x000297CC
		private static void AddMediaType(string ext, string mimeType)
		{
			bool flag = !ALSetupMaint.MEDIA_TYPES.ContainsKey(ext);
			if (flag)
			{
				ALSetupMaint.MEDIA_TYPES[ext] = mimeType;
			}
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x0002B5FC File Offset: 0x000297FC
		[PXButton(IsLockedOnToolbar = true)]
		[PXUIField(DisplayName = "Enable Features")]
		public IEnumerable runReset(PXAdapter adapter)
		{
			FeaturesMaint featuresMaint = HiddenUtils.CreateInstance<FeaturesMaint>();
			featuresMaint.Insert.Press();
			featuresMaint.RequestValidation.Press();
			return adapter.Get();
		}

		// Token: 0x06000E8E RID: 3726 RVA: 0x0002B634 File Offset: 0x00029834
		[PXButton]
		[PXUIField(DisplayName = "Allow Asgard Media Types")]
		public IEnumerable allowNewMediaTypes(PXAdapter adapter)
		{
			ALSetupMaint alsetupMaint = HiddenUtils.CreateInstance<ALSetupMaint>();
			PXLongOperation.StartOperation(this, new PXToggleAsyncDelegate(alsetupMaint.DoAllowNewMediaTypes));
			return adapter.Get();
		}

		// Token: 0x06000E8F RID: 3727 RVA: 0x0002B668 File Offset: 0x00029868
		private void DoAllowNewMediaTypes()
		{
			UploadAllowedFileTypesMaint uploadAllowedFileTypesMaint = HiddenUtils.CreateInstance<UploadAllowedFileTypesMaint>();
			foreach (KeyValuePair<string, string> fileTypeKvp in ALSetupMaint.MEDIA_TYPES)
			{
				ALSetupMaint.AddIfMissing(uploadAllowedFileTypesMaint, fileTypeKvp);
			}
			uploadAllowedFileTypesMaint.Actions.PressSave();
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x0002B6CC File Offset: 0x000298CC
		private static UploadAllowedFileTypes AddIfMissing(UploadAllowedFileTypesMaint graph, KeyValuePair<string, string> fileTypeKvp)
		{
			IEnumerable<UploadAllowedFileTypes> firstTableItems = graph.PrefsDetail.Select(Array.Empty<object>()).FirstTableItems;
			UploadAllowedFileTypes uploadAllowedFileTypes = firstTableItems.FirstOrDefault((UploadAllowedFileTypes ft) => ft.FileExt == fileTypeKvp.Key);
			bool flag = uploadAllowedFileTypes == null;
			UploadAllowedFileTypes result;
			if (flag)
			{
				uploadAllowedFileTypes = new UploadAllowedFileTypes
				{
					FileExt = fileTypeKvp.Key,
					DefApplication = fileTypeKvp.Value,
					Forbidden = new bool?(false)
				};
				UploadAllowedFileTypes uploadAllowedFileTypes2 = graph.PrefsDetail.Insert(uploadAllowedFileTypes);
				result = uploadAllowedFileTypes2;
			}
			else
			{
				uploadAllowedFileTypes.DefApplication = fileTypeKvp.Value;
				uploadAllowedFileTypes.Forbidden = new bool?(false);
				UploadAllowedFileTypes uploadAllowedFileTypes3 = graph.PrefsDetail.Update(uploadAllowedFileTypes);
				result = uploadAllowedFileTypes3;
			}
			return result;
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x0002B79C File Offset: 0x0002999C
		[PXButton(CommitChanges = true)]
		[PXUIField(DisplayName = "Validate")]
		public IEnumerable validateLabelary(PXAdapter adapter)
		{
			ALSetup alsetup = this.Setup.Current;
			bool flag = alsetup != null;
			if (flag)
			{
				this.Actions.PressSave();
				AcuLabelContext acuLabelContext = AcuLabelContext.CreateTestContext(this, alsetup);
				IFormat format = RuleUtils.FORMAT_FACTORY.GetValue(acuLabelContext, alsetup.DefaultFormatID);
				bool flag2 = format == null;
				if (flag2)
				{
					Formats.TryGetValue("4x6", out format);
				}
				if (format == null)
				{
					format = DefaultFormat.DEFAULT_FORMAT;
				}
				acuLabelContext.ModelFormat = format;
				acuLabelContext.FinalOutputFormat = ContentFormat.PNG;
				LabelaryUtils.Validate(acuLabelContext);
			}
			return adapter.Get();
		}

		// Token: 0x06000E92 RID: 3730 RVA: 0x0002B82C File Offset: 0x00029A2C
		[PXButton(CommitChanges = true)]
		[PXUIField(DisplayName = "Validate")]
		[PXUIVisible(typeof(ALHasDevOption<ALDev.Operands.LabelZoom>))]
		public IEnumerable validateLabelZoom(PXAdapter adapter)
		{
			ALSetup alsetup = this.Setup.Current;
			bool flag = alsetup != null;
			if (flag)
			{
				this.Actions.PressSave();
				AcuLabelContext lc = AcuLabelContext.CreateTestContext(this, alsetup);
				LabelZoomUtils.Validate(lc);
			}
			return adapter.Get();
		}

		// Token: 0x06000E93 RID: 3731 RVA: 0x0002B878 File Offset: 0x00029A78
		[PXButton(CommitChanges = true)]
		[PXUIField(DisplayName = "Validate")]
		public IEnumerable validatePrintNode(PXAdapter adapter)
		{
			ALSetup alsetup = this.Setup.Current;
			bool flag = alsetup != null;
			if (flag)
			{
				PrintNodeHelper.Validate(null);
				throw new AAException("Successfully Connected to {0}", new object[]
				{
					"Cloud Print"
				});
			}
			return adapter.Get();
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x0002B8C8 File Offset: 0x00029AC8
		[PXButton(CommitChanges = true)]
		[PXUIField(DisplayName = "Validate")]
		[PXUIVisible(typeof(ALHasDevOption<ALDev.Operands.MongoDb>))]
		public IEnumerable validateMongoDb(PXAdapter adapter)
		{
			ALSetup alsetup = this.Setup.Current;
			bool flag = alsetup != null;
			if (flag)
			{
				this.Actions.PressSave();
				AcuLabelContext lc = AcuLabelContext.CreateTestContext(this, alsetup);
				MongoHelper.Validate(lc);
				throw new AAException("Successfully Connected to {0}", new object[]
				{
					"Mongo Db"
				});
			}
			return adapter.Get();
		}

		// Token: 0x040006AB RID: 1707
		private static readonly IDictionary<string, string> MEDIA_TYPES = new Dictionary<string, string>();

		// Token: 0x040006AC RID: 1708
		public PXSave<ALSetup> Save;

		// Token: 0x040006AD RID: 1709
		public PXCancel<ALSetup> Cancel;

		// Token: 0x040006AE RID: 1710
		public PXSelect<ALSetup> Setup;

		// Token: 0x040006AF RID: 1711
		public PXSelect<ALSetupRenderer> Renderers;

		// Token: 0x040006B0 RID: 1712
		public PXAction<ALSetup> RunReset;

		// Token: 0x040006B1 RID: 1713
		public PXAction<ALSetup> AllowNewMediaTypes;

		// Token: 0x040006B2 RID: 1714
		public PXAction<ALSetup> ValidateLabelary;

		// Token: 0x040006B3 RID: 1715
		public PXAction<ALSetup> ValidateLabelZoom;

		// Token: 0x040006B4 RID: 1716
		public PXAction<ALSetup> ValidatePrintNode;

		// Token: 0x040006B5 RID: 1717
		public PXAction<ALSetup> ValidateMongoDb;
	}
}
