

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PX.Common;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.BarcodeProcessing;
using PX.Objects.CS;
using PX.Objects.Common.Extensions;

namespace PX.Objects.IN.WMS
{
	public abstract class INScanRegisterBase : WarehouseManagementSystem
		where TSelf : INScanRegisterBase
		where TGraph : INRegisterEntryBase, new()
		where TDocType : IConstant, IBqlOperand, new()
	{
		#region State
		public RegisterScanHeader RegisterHeader => Header.Get() ?? new RegisterScanHeader();
		public ValueSetter.Ext RegisterSetter => HeaderSetter.With();

		#region DocType
		public string DocType => RegisterHeader.DocType;
		#endregion
		#region ReasonCodeID
		public string ReasonCodeID
		{
			get => RegisterHeader.ReasonCodeID;
			set => RegisterSetter.Set(h => h.ReasonCodeID, value);
		}
		#endregion
		#endregion

		#region Selected Entities
		public ReasonCode SelectedReasonCode => ReasonCode.PK.Find(Graph, ReasonCodeID);
		#endregion

		public INRegister Document => DocumentView.Current;
		public PXSelectBase DocumentView => Graph.INRegisterDataMember;
		public PXSelectBase Details => Graph.INTranDataMember;

		public bool NotReleasedAndHasLines => Document?.Released != true && Details.SelectMain().Any();

		#region Configuration
		public abstract bool PromptLocationForEveryLine { get; }
		public abstract bool UseDefaultReasonCode { get; }
		public abstract bool UseDefaultWarehouse { get; }

		public override bool DocumentLoaded => Document != null;
		public override bool DocumentIsEditable => base.DocumentIsEditable && INRegister.PK.Find(Base, Document)?.Released != true;
		#endregion

		#region Scan Setup (Common/User's)
		public PXSetupOptional>> Setup;
		public abstract class UserSetup : PXUserSetupPerMode { }
		#endregion

		#region Event Handlers
		protected override void _(Events.RowSelected e)
		{
			base._(e);

			if (Document == null && !string.IsNullOrEmpty(RefNbr))
			{
				RefNbr = null;
				NoteID = null; 
			}

			Details.Cache.SetAllEditPermissions(Document == null || Document.Released != true);
			Details.Cache.AllowInsert = false;
		}

		protected virtual void _(Events.FieldDefaulting e)
			=> e.NewValue = new TDocType().Value;

		protected virtual void _(Events.FieldUpdated e)
			=> DocumentView.Current = e.NewValue == null ? null : DocumentView.Search(e.NewValue);

		protected virtual void _(Events.RowSelected e)
		{
			bool isMobileAndNotReleased = Graph.IsMobile && (Document == null || Document.Released != true);

			Details.Cache
				.AdjustUI()
				.For(ui => ui.Enabled = false)
				.SameFor()
				.SameFor()
				.SameFor()
				.For(ui => ui.Enabled = isMobileAndNotReleased)
				.SameFor()
				.SameFor()
				.SameFor();
		}

		protected virtual void _(Events.RowUpdated e) => e.Row.IsOverridden = !e.Row.SameAs(Setup.Current);
		protected virtual void _(Events.RowInserted e) => e.Row.IsOverridden = !e.Row.SameAs(Setup.Current);
		#endregion

		#region DAC overrides
		[PXMergeAttributes]
		[PXUnboundDefault(typeof(INRegister.refNbr))]
		[PXSelector(typeof(SearchFor.Where>))]
		protected virtual void _(Events.CacheAttached e) { }

		[PXMergeAttributes]
		[PXUnboundDefault(typeof(
				 INTranType.transfer.When>.
			Else	.When>.
			Else.When>.
			ElseNull))]
		protected virtual void _(Events.CacheAttached e) { }

		[PXMergeAttributes]
		[PXUnboundDefault(typeof(
				 InventoryMultiplicator.decrease	.When>.
			Else	.When>.
			ElseNull))]
		protected virtual void _(Events.CacheAttached e) { }
		#endregion

		#region Overrides
		protected override bool ProcessSingleBarcode(string barcode)
		{
			// just clears the selected document after it got released on the next scan
			if (Header.ProcessingSucceeded == true && INRegister.PK.Find(Graph, Document)?.Released == true)
			{
				RefNbr = null;
				NoteID = null;
			}

			return base.ProcessSingleBarcode(barcode);
		}

		protected override ScanCommand DecorateScanCommand(ScanCommand original)
		{
			var command = base.DecorateScanCommand(original);

			if (command is RemoveCommand remove)
				remove.Intercept.IsEnabled.ByConjoin(basis => basis.NotReleasedAndHasLines);

			if (command is QtySupport.SetQtyCommand setQty)
				setQty.Intercept.IsEnabled.ByConjoin(basis => basis.UseQtyCorrection.Implies(basis.DocumentIsEditable && basis.NotReleasedAndHasLines));

			return command;
		}

		/// Overrides 
		[PXOverride]
		public virtual void Persist(Action base_Persist)
		{
			base_Persist();

			RefNbr = Document?.RefNbr;
			NoteID = Document?.NoteID;

			Details.Cache.Clear();
			Details.Cache.ClearQueryCacheObsolete();
		}
		#endregion

		#region States
		public new sealed class WarehouseState : WarehouseManagementSystem.WarehouseState
		{
			protected override bool UseDefaultWarehouse => Basis.UseDefaultWarehouse;
			protected override int? DefaultSiteID => Basis.Document?.SiteID ?? base.DefaultSiteID;
		}

		public sealed class ReasonCodeState : EntityState
		{
			public const string Value = "RSNC";
			public class value : BqlString.Constant { public value() : base(ReasonCodeState.Value) { } }

			public override string Code => Value;
			protected override string StatePrompt => Msg.Prompt;

			protected override bool IsStateActive() => Basis.UseDefaultReasonCode == false;

			protected override ReasonCode GetByBarcode(string barcode) => ReasonCode.PK.Find(Basis, barcode);
			protected override void ReportMissing(string barcode) => Basis.Reporter.Error(Msg.Missing, barcode);
			protected override Validation Validate(ReasonCode reasonCode) => Basis.IsValid(reasonCode.ReasonCodeID, out string error) ? Validation.Ok : Validation.Fail(error);
			protected override void Apply(ReasonCode reasonCode) => Basis.ReasonCodeID = reasonCode.ReasonCodeID;
			protected override void ReportSuccess(ReasonCode reasonCode) => Basis.Reporter.Info(Msg.Ready, reasonCode.Descr ?? reasonCode.ReasonCodeID);
			protected override void ClearState() => Basis.ReasonCodeID = null;

			#region Messages
			[PXLocalizable]
			public abstract class Msg
			{
				public const string Prompt = "Scan the barcode of the reason code.";
				public const string Ready = "The {0} reason code is selected.";
				public const string Missing = "The {0} reason code is not found.";
				public const string NotSet = "The reason code is not selected.";
			}
			#endregion
		}
		#endregion

		#region Commands
		public abstract class ReleaseCommand : ScanCommand
		{
			public override string Code => "RELEASE";
			public override string ButtonName => "scanRelease";
			public override string DisplayName => Msg.DisplayName;
			protected override bool IsEnabled => Basis.DocumentIsEditable && Basis.NotReleasedAndHasLines;

			protected override bool Process()
			{
				if (Basis.Document != null)
				{
					if (Basis.Document.Released == true)
					{
						Basis.ReportError(Messages.Document_Status_Invalid);
						return true;
					}

					if (Basis.Document.Hold != false)
						Basis.DocumentView.SetValueExt(Basis.Document, false);
					Basis.Save.Press();

					Basis.Reset(fullReset: false);
					Basis.Clear();

					var msg = (DocumentIsReleased, DocumentReleaseFailed);

					Basis
					.AwaitFor(async (basis, doc, ct) =>
					{
						INDocumentRelease.ReleaseDoc(new List() { doc }, false);
						await basis.CurrentMode.Commands.OfType().FirstOrDefault()?.OnAfterRelease(doc, ct);
					})
					.WithDescription(DocumentReleasing, Basis.Document.RefNbr)
					.ActualizeDataBy((basis, doc) => INRegister.PK.Find(basis, doc))
					.OnSuccess(ConfigureOnSuccessAction)
					.OnFail(x => x.Say(msg.DocumentReleaseFailed))
					.BeginAwait(Basis.Document);

					return true;
				}
				return false;
			}

			protected virtual System.Threading.Tasks.Task OnAfterRelease(INRegister doc, CancellationToken cancellationToken) { return Task.CompletedTask;}

			public virtual void ConfigureOnSuccessAction(ScanLongRunAwaiter.ISuccessProcessor onSuccess)
			{
				onSuccess
					.Say(DocumentIsReleased);
			}

			protected abstract string DocumentReleasing { get; }
			protected abstract string DocumentIsReleased { get; }
			protected abstract string DocumentReleaseFailed { get; }

			#region Messages
			[PXLocalizable]
			public abstract class Msg
			{
				public const string DisplayName = "Release";
			}
			#endregion
		}
		#endregion

		#region Redirect
		public new abstract class RedirectFrom : WarehouseManagementSystem.RedirectFrom
			where TForeignBasis : PXGraphExtension, IBarcodeDrivenStateMachine
		{
			public override bool IsPossible => PXAccess.FeatureInstalled();
		}
		#endregion
	}

	public sealed class RegisterScanHeader : PXCacheExtension
	{
		#region DocType
		[PXUnboundDefault(typeof(INRegister.docType))]
		[PXString(1, IsFixed = true)]
		[INDocType.List]
		public string DocType { get; set; }
		public abstract class docType : BqlString.Field { }
		#endregion
		#region ReasonCodeID
		[PXString]
		[PXSelector(typeof(SearchFor))]
		[PXRestrictor(typeof(Where>), Messages.ReasonCodeDoesNotMatch)]
		public string ReasonCodeID { get; set; }
		public abstract class reasonCodeID : BqlString.Field { }
		#endregion
	}
}
