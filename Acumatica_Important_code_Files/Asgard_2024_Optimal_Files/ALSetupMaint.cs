using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AA.Objects.AL.LabelZoom;
using AA.Objects.AL.PrintNode;
using PX.Data;
using PX.Objects.CS;
using PX.SM;

namespace AA.Objects.AL
{
	// Token: 0x020001C9 RID: 457
	public class ALSetupMaint : PXGraph<ALSetupMaint>
	{
		// Token: 0x060011F2 RID: 4594 RVA: 0x0003BC4D File Offset: 0x00039E4D
		static ALSetupMaint()
		{
			ALSetupMaint.AddFont(".ttf", "font/ttf");
			ALSetupMaint.AddFont(".otf", "font/otf");
		}

		// Token: 0x060011F3 RID: 4595 RVA: 0x00019FF9 File Offset: 0x000181F9
		public void Initialize()
		{
		}

		// Token: 0x060011F4 RID: 4596 RVA: 0x0003BC7C File Offset: 0x00039E7C
		private static void AddFont(string ext, string mimeType)
		{
			bool flag = !ALSetupMaint.FONTS.ContainsKey(ext);
			if (flag)
			{
				ALSetupMaint.FONTS[ext] = mimeType;
			}
		}

		// Token: 0x060011F5 RID: 4597 RVA: 0x0003BCAC File Offset: 0x00039EAC
		[PXButton(IsLockedOnToolbar = true)]
		[PXUIField(DisplayName = "Enable Features")]
		public IEnumerable runReset(PXAdapter adapter)
		{
			FeaturesMaint featuresMaint = HiddenUtils.CreateInstance<FeaturesMaint>();
			featuresMaint.Insert.Press();
			try
			{
				featuresMaint.RequestValidation.Press();
			}
			catch (PXRefreshException ex)
			{
			}
			return adapter.Get();
		}

		// Token: 0x060011F6 RID: 4598 RVA: 0x0003BCFC File Offset: 0x00039EFC
		[PXButton]
		[PXUIField(DisplayName = "Allow Fonts Upload")]
		public IEnumerable allowFontsUpload(PXAdapter adapter)
		{
			ALSetupMaint alsetupMaint = HiddenUtils.CreateInstance<ALSetupMaint>();
			PXLongOperation.StartOperation(this, new PXToggleAsyncDelegate(alsetupMaint.DoAllowFontsUpload));
			return adapter.Get();
		}

		// Token: 0x060011F7 RID: 4599 RVA: 0x0003BD30 File Offset: 0x00039F30
		private void DoAllowFontsUpload()
		{
			UploadAllowedFileTypesMaint uploadAllowedFileTypesMaint = HiddenUtils.CreateInstance<UploadAllowedFileTypesMaint>();
			foreach (KeyValuePair<string, string> fileTypeKvp in ALSetupMaint.FONTS)
			{
				ALSetupMaint.AddIfMissing(uploadAllowedFileTypesMaint, fileTypeKvp);
			}
			uploadAllowedFileTypesMaint.Actions.PressSave();
		}

		// Token: 0x060011F8 RID: 4600 RVA: 0x0003BD94 File Offset: 0x00039F94
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

		// Token: 0x060011F9 RID: 4601 RVA: 0x0003BE64 File Offset: 0x0003A064
		[PXButton(CommitChanges = true)]
		[PXUIField(DisplayName = "Validate")]
		public IEnumerable validateLabelary(PXAdapter adapter)
		{
			ALSetup alsetup = this.Setup.Current;
			bool flag = alsetup != null;
			if (flag)
			{
				this.Actions.PressSave();
				IFormat format = RuleUtils.FORMAT_FACTORY.GetValue(alsetup.DefaultFormatID);
				bool flag2 = format == null;
				if (flag2)
				{
					Formats.TryGetFormat("4x6", out format);
				}
				if (format == null)
				{
					format = Formats.Format.DEFAULT_FORMAT;
				}
				LabelaryUtils.Validate(format);
			}
			return adapter.Get();
		}

		// Token: 0x060011FA RID: 4602 RVA: 0x0003BED8 File Offset: 0x0003A0D8
		[PXButton(CommitChanges = true)]
		[PXUIField(DisplayName = "Validate")]
		public IEnumerable validateLabelZoom(PXAdapter adapter)
		{
			ALSetup alsetup = this.Setup.Current;
			bool flag = alsetup != null;
			if (flag)
			{
				this.Actions.PressSave();
				LabelZoomUtils.Validate();
			}
			return adapter.Get();
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x0003BF18 File Offset: 0x0003A118
		[PXButton(CommitChanges = true)]
		[PXUIField(DisplayName = "Validate")]
		public IEnumerable validatePrintNode(PXAdapter adapter)
		{
			ALSetup alsetup = this.Setup.Current;
			bool flag = alsetup != null;
			if (flag)
			{
				PrintNodeHelper.Validate(null);
				throw new PXException("Successfully Connected to {0}", new object[]
				{
					"Cloud Print"
				});
			}
			return adapter.Get();
		}

		// Token: 0x04000844 RID: 2116
		private static readonly IDictionary<string, string> FONTS = new Dictionary<string, string>();

		// Token: 0x04000845 RID: 2117
		public PXSave<ALSetup> Save;

		// Token: 0x04000846 RID: 2118
		public PXCancel<ALSetup> Cancel;

		// Token: 0x04000847 RID: 2119
		public PXSelect<ALSetup> Setup;

		// Token: 0x04000848 RID: 2120
		public PXAction<ALSetup> RunReset;

		// Token: 0x04000849 RID: 2121
		public PXAction<ALSetup> AllowFontsUpload;

		// Token: 0x0400084A RID: 2122
		public PXAction<ALSetup> ValidateLabelary;

		// Token: 0x0400084B RID: 2123
		public PXAction<ALSetup> ValidateLabelZoom;

		// Token: 0x0400084C RID: 2124
		public PXAction<ALSetup> ValidatePrintNode;
	}
}
