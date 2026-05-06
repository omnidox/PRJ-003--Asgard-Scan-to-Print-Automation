

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Threading;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Common;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.SM;
using PX.CarrierService;
using PX.Objects.Common;
using PX.Api;
using PdfSharp.Pdf.IO;
using ShipmentActions = PX.Objects.SO.SOShipmentEntryActionsAttribute;

namespace PX.Objects.SO.GraphExtensions.SOShipmentEntryExt
{
	[PXProtectedAccess]
	// Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
	public abstract class LabelsPrinting : PXGraphExtension
	{
		private const string ReturnLabel = "ReturnLabel";

		#region Action Declarations
		public PXAction printLabels;
		[PXButton(CommitChanges = true), PXUIField(DisplayName = ShipmentActions.Messages.PrintLabels, MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
		protected virtual IEnumerable PrintLabels(PXAdapter adapter)
		{
			var list = adapter.Get().ToList();

			if (adapter.MassProcess)
			{
				Base.Save.Press();

				var printArgs = new PrintPackageFilesArgs
				{
					Shipments = list,
					Adapter = adapter,
					Category = PackageFileCategory.CarrierLabel
				};

				var graph = PXGraph.CreateInstance().GetExtension();
				Base.LongOperationManager.StartAsyncOperation(ct => graph.PrintPackageFiles(printArgs, ct));
			}
			else
			{
				PrintCarrierLabels();
			}

			return list;
		}

		public PXAction printCommercialInvoices;
		[PXButton(CommitChanges = true)]
		[PXUIField(DisplayName = ShipmentActions.Messages.PrintCommercialInvoices, MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select, FieldClass = nameof(FeaturesSet.CarrierIntegration))]
		protected virtual IEnumerable PrintCommercialInvoices(PXAdapter adapter)
		{
			var list = adapter.Get().ToList();

			if (adapter.MassProcess)
			{
				Base.Save.Press();

				var printArgs = new PrintPackageFilesArgs
				{
					Shipments = list,
					Adapter = adapter,
					Category = PackageFileCategory.CommercialInvoice
				};

				var graph = PXGraph.CreateInstance().GetExtension(); ;
				Base.LongOperationManager.StartAsyncOperation(ct => graph.PrintPackageFiles(printArgs, ct));
			}
			else
			{
				Base.LongOperationManager.StartAsyncOperation(ct => PrintCommercInvoices(ct));
			}

			return list;
		}

		public PXAction getReturnLabelsAction;
		[PXButton(CommitChanges = true), PXUIField(DisplayName = ShipmentActions.Messages.GetReturnLabels, MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
		protected virtual IEnumerable GetReturnLabelsAction(PXAdapter adapter)
		{
			var list = adapter.Get().ToList();

			Base.Save.Press();

			bool massProcess = adapter.MassProcess;

			PXLongOperation.StartOperation(Base, delegate ()
			{
				LabelsPrinting docgraph = PXGraph.CreateInstance().GetExtension();

				foreach (SOShipment order in list)
				{
					try
					{
						if (massProcess)
						{
							PXProcessing.SetCurrentItem(order);
							if (order.UnlimitedPackages == true)
							{
								PXProcessing.SetError(new PXException(Messages.GetLabelsDisabledDueToUnlimitedPackages));
								continue;
							}
						}
						docgraph.GetReturnLabels(order);
					}
					catch (Exception ex)
					{
						if (!massProcess)
							throw;

						PXProcessing.SetError(ex);
					}
				}
			});

			return list;
		}
		#endregion

		#region Print Labels and Commercial Invoices

		public virtual void PrintCarrierLabels()
		{
			if (Base.Document.Current == null)
				return;

			if (Base.Document.Current.LabelsPrinted == true)
			{
				WebDialogResult result = Base.Document.View.Ask(Base.Document.Current, GL.Messages.Confirmation, Messages.ReprintLabels, MessageButtons.YesNo, MessageIcon.Question);
				if (result != WebDialogResult.Yes)
				{
					return;
				}
				else
				{
					PXTrace.WriteInformation("User Forced Labels Reprint for Shipment {0}", Base.Document.Current.ShipmentNbr);
				}
			}

			Base.LongOperationManager.StartAsyncOperation(ct => PrintCarrierLabels(ct));
		}

		public virtual async System.Threading.Tasks.Task PrintCarrierLabels(CancellationToken cancellationToken)
		{
			if (Base.Document.Current == null)
				return;

			var args = new PrintPackageFilesArgs
			{
				Shipments = new List { Base.Document.Current },
				Adapter = CreateDummyAdapter(),
				Category = PackageFileCategory.CarrierLabel
			};

			await PrintPackageFiles(args, cancellationToken);
		}

		public virtual async System.Threading.Tasks.Task PrintCommercInvoices(CancellationToken cancellationToken)
		{
			if (Base.Document.Current == null)
				return;

			var args = new PrintPackageFilesArgs
			{
				Shipments = new List { Base.Document.Current },
				Adapter = CreateDummyAdapter(),
				Category = PackageFileCategory.CommercialInvoice
			};

			await PrintPackageFiles(args, cancellationToken);
		}

		protected virtual void MarkPackageFilesPrinted(SOShipment shipment, PackageFileCategory category)
		{
			if (category.HasFlag(PackageFileCategory.CarrierLabel))
				shipment.LabelsPrinted = true;
			if (category.HasFlag(PackageFileCategory.CommercialInvoice))
				shipment.CommercialInvoicesPrinted = true;
		}

		protected virtual string GetPrintFormID(PackageFileCategory category)
		{
			if (category.HasFlag(PackageFileCategory.CarrierLabel))
				return SOReports.PrintLabels;
			if (category.HasFlag(PackageFileCategory.CommercialInvoice))
				return SOReports.PrintCommercialInvoices;
			return null;
		}

		public virtual void LoadPackageFiles(PrintPackageFilesArgs printArg)
		{
			Guid? userDefinedPrinterID = SMPrintJobMaint.GetPrintSettings(printArg.Adapter)?.PrinterID;

			PXReportRequiredException reportRedirect = null;
			var printerToReportsMap = new Dictionary();

			var searchPrinter = Func.Memorize(
				(string shipVia) => SearchPrinter(SONotificationSource.Customer, printArg.PrintFormID, Base.Accessinfo.BranchID, shipVia));

			var notificationUtility = new NotificationUtility(Base);
			var searchReport = Func.Memorize(
				(int? customerID) => notificationUtility.SearchCustomerReport(printArg.PrintFormID, customerID, Base.Accessinfo.BranchID));

			{
				UploadFileMaintenance upload = PXGraph.CreateInstance();

				foreach (SOShipment shiporder in printArg.Shipments)
				{
					Guid printerID = userDefinedPrinterID ?? searchPrinter(shiporder.ShipVia ?? string.Empty) ?? Guid.Empty;
					var reports = printerToReportsMap.Ensure(printerID, () => new ShipmentRelatedReports());

					PXResultset packagesResultset = PXSelect>>>.Select(Base, shiporder.ShipmentNbr);
					if (packagesResultset.Count > 0)
					{
						var firstFile = GetFirstFile(packagesResultset, printArg.Category);
						if (firstFile != null)
						{
							if (IsThermalPrinter(firstFile))
							{
								reports.LabelFiles.AddRange(LoadFiles(packagesResultset, printArg.Category));
							}
							else
							{
								reports.LaserLabels.Add(shiporder.ShipmentNbr);

								string actualReportID = searchReport(shiporder.CustomerID);
								var parameters = new Dictionary
								{
									[$"{nameof(SOShipment)}.{nameof(SOShipment.ShipmentNbr)}"] = shiporder.ShipmentNbr
								};
								reports.ReportRedirect = PXReportRequiredException.CombineReport(reports.ReportRedirect, actualReportID, parameters);
								reportRedirect = PXReportRequiredException.CombineReport(reportRedirect, actualReportID, parameters);
								reportRedirect.Mode = PXBaseRedirectException.WindowMode.New;
							}
						}
						else
						{
							PXTrace.WriteWarning("No Label files to print for Shipment {0}", shiporder.ShipmentNbr);
						}
					}
				}

				string screenID = Base.Accessinfo.ScreenID?.Replace(".", "") ?? "";

				if (!string.IsNullOrEmpty(screenID))
					upload.FieldDefaulting.AddHandler(args => args.NewValue = screenID);

				foreach (var pair in printerToReportsMap)
				{
					ShipmentRelatedReports reports = pair.Value;

					if (reports.LabelFiles.Count > 1 && CanMerge(reports.LabelFiles))
					{
						FileInfo mergedFile = MergeFiles(reports.LabelFiles);
						reports.LabelFiles.Clear();

						if (upload.SaveFile(mergedFile))
							reports.LabelFiles.Add(mergedFile);
						else
							throw new PXException(Messages.FailedToSaveMergedFile);
					}
				}
			}

			printArg.PrinterToReportsMap = printerToReportsMap;
			printArg.RedirectToReport = reportRedirect;
		}

		public virtual async System.Threading.Tasks.Task PrintPackageFiles(PrintPackageFilesArgs printArg, CancellationToken cancellationToken)
		{
			if (printArg.Category == PackageFileCategory.None)
				return;

			printArg.PrintFormID = printArg.PrintFormID ?? GetPrintFormID(printArg.Category);

			using (PXTransactionScope ts = new PXTransactionScope())
			{
				LoadPackageFiles(printArg);

				foreach (SOShipment shiporder in printArg.Shipments)
				{
					MarkPackageFilesPrinted(shiporder, printArg.Category);
					Base.Document.Update(shiporder);
				}

				Base.Save.Press();
				ts.Complete();
			}

			if (printArg.Adapter?.MassProcess == true)
			{
				for (var i = 0; i < printArg.Shipments.Count; i++)
					PXProcessing.SetInfo(i, ActionsMessages.RecordProcessed);
			}

			if (PXAccess.FeatureInstalled())
				await CreatePrintJobs(printArg, cancellationToken);

			try
			{
				RedirectToFiles(printArg);
			}
			finally
			{
				printArg.PrinterToReportsMap = null;
				printArg.RedirectToReport = null;
			}
		}

		public virtual async System.Threading.Tasks.Task CreatePrintJobs(PrintPackageFilesArgs printArg, CancellationToken cancellationToken)
		{
			if (printArg.PrinterToReportsMap == null)
				return;

			var description = string.Empty;
			if (printArg.Category.HasFlag(PackageFileCategory.CarrierLabel))
				description = PXMessages.Localize(ShipmentActions.Messages.PrintLabels);
			if (printArg.Category.HasFlag(PackageFileCategory.CommercialInvoice))
				description = PXMessages.Localize(ShipmentActions.Messages.PrintCommercialInvoices);

			foreach (var (printerID, reports) in printArg.PrinterToReportsMap)
			{
				if (printerID == Guid.Empty)
					continue;

				foreach (FileInfo file in reports.LabelFiles)
				{
					await SMPrintJobMaint.CreatePrintJobForRawFile(
					printArg.Adapter,
					delegate { return printerID; }, // already found
					SONotificationSource.Customer,
					printArg.PrintFormID,
					Base.Accessinfo.BranchID,
					new Dictionary { { "FILEID", file.UID.ToString() } },
					description, cancellationToken);
				}

				if (reports.LaserLabels.Count > 0 && reports.ReportRedirect != null)
				{
					await SMPrintJobMaint.CreatePrintJobGroup(
					printArg.Adapter,
					delegate { return printerID; }, // already found
					SONotificationSource.Customer,
					printArg.PrintFormID,
					Base.Accessinfo.BranchID,
					reports.ReportRedirect,
					description, cancellationToken);
				}
			}
		}

		public virtual void RedirectToFiles(PrintPackageFilesArgs printArg)
		{
			if (printArg.PrinterToReportsMap == null)
				return;

			PXReportRequiredException reportRedirect = printArg.RedirectToReport;
			PXRedirectToFileException targetFileRedirect = null;

			if (printArg.PrinterToReportsMap.Count == 1 && printArg.PrinterToReportsMap.First().Value.LabelFiles.Count == 1)
				targetFileRedirect = new PXRedirectToFileException(printArg.PrinterToReportsMap.First().Value.LabelFiles.First().UID, forceDownload: true);

			if (targetFileRedirect != null && reportRedirect != null)
				return; // can't redirect to both a file and a report

			if (targetFileRedirect != null)
				throw targetFileRedirect;

			if (reportRedirect != null)
				throw reportRedirect;
		}

		protected virtual Guid? SearchPrinter(string source, string reportID, int? branchID, string shipVia)
		{
			NotificationSetupUserOverride userSetup =
				SelectFrom
				.InnerJoin.On
				.Where
					.And>
					.And>
					.And>
					.And>
					.And>
					.And.Or>>
				.OrderBy
				.View.Select(Base, source, reportID, shipVia, branchID);
			if (userSetup?.DefaultPrinterID != null)
				return userSetup.DefaultPrinterID;

			if (source != null && reportID != null)
			{
				NotificationSetup setup =
					SelectFrom
					.Where
						.And>
						.And>
						.And>
						.And.Or>>
					.OrderBy
					.View.SelectWindowed(Base, 0, 1, source, reportID, shipVia, branchID);
				if (setup?.DefaultPrinterID != null)
					return setup.DefaultPrinterID;
			}

			return new NotificationUtility(Base).SearchPrinter(source, reportID, branchID);
		}

		protected virtual bool IsThermalPrinter(FileInfo fileInfo)
		{
			if (System.IO.Path.HasExtension(fileInfo.Name))
			{
				string extension = System.IO.Path.GetExtension(fileInfo.Name).Substring(1).ToLower();
				if (extension.Length > 2)
				{
					string ext = extension.Substring(0, 3);
					if (ext == "zpl" || ext == "epl" || ext == "dpl" || ext == "spl" || extension == "starpl" || ext == "pdf")
						return true;
					else
						return false;
				}
				else
					return false;
			}
			else
				return false;
		}

		protected virtual PackageFileCategory GetFileCategory(FileInfo fileInfo)
		{
			var packageNoteAttribute = Base.Packages.Cache.GetAttributesReadonly(null).OfType().FirstOrDefault();
			return packageNoteAttribute?.GetFileCategory(fileInfo.Name) ?? PackageFileCategory.None;
		}

		protected virtual FileInfo GetFirstFile(PXResultset packages, PackageFileCategory category)
		{
			UploadFileMaintenance upload = PXGraph.CreateInstance();

			foreach (SOPackageDetailEx package in packages)
			{
				foreach (Guid fileID in PXNoteAttribute.GetFileNotes(Base.Packages.Cache, package))
				{
					var fileInfo = upload.GetFileWithNoData(fileID);//no need to load data

					if (category.HasFlag(GetFileCategory(fileInfo)))
						return fileInfo;
				}
			}
			return null;
		}

		protected virtual IList LoadFiles(PXResultset packages, PackageFileCategory category)
		{
			var files = new List();
			UploadFileMaintenance upload = PXGraph.CreateInstance();

			foreach (SOPackageDetailEx package in packages)
			{
				FileInfo firstFile = null;

				foreach (Guid fileID in PXNoteAttribute.GetFileNotes(Base.Packages.Cache, package))
				{
					var fileInfo = upload.GetFileWithNoData(fileID);

					if (category.HasFlag(GetFileCategory(fileInfo)))
					{
						if (firstFile == null)
						{
							upload.Filter.Current.FileRevisionID = null;
							firstFile = upload.GetFile(fileID);
							files.Add(firstFile);
						}
						else
						{
							PXTrace.WriteWarning("There are more then one file attached to the package. But only first will be processed for Shipment {0}/{1}",
								package.ShipmentNbr, package.LineNbr);
						}
					}
				}
			}

			return files;
		}

		protected virtual FileInfo GetReturnLabelFileForPackage(SOPackageDetail result, UploadFileMaintenance uploadFileMaintenance)
		{
			FileInfo returnFileInfo = null;
			if (result != null)
			{
				Guid[] ids = PXNoteAttribute.GetFileNotes(Base.Packages.Cache, result);
				if (ids.Length > 0)
				{
					for (int i = 0; i < ids.Length; i++)
					{
						FileInfo fileInfo = uploadFileMaintenance.GetFile(ids[i]);

						if (fileInfo.Name.StartsWith(ReturnLabel))
						{
							returnFileInfo = fileInfo;
							break;
						}
					}
				}
			}
			return returnFileInfo;
		}

		private readonly string[] mergeExtensions = { "zpl", "epl", "dpl", "spl", "starpl", "pdf" };

		protected virtual bool CanMerge(IList files)
		{
			string previousExt = null;
			foreach (var file in files)
			{
				string fileExt = System.IO.Path.GetExtension(file.Name).ToLower();
				if (fileExt.StartsWith("."))
					fileExt = fileExt.Substring(1);
				if (string.IsNullOrEmpty(fileExt))
					return false;

				previousExt = previousExt ?? fileExt;
				if (previousExt != fileExt
				  || (!mergeExtensions.Contains(fileExt)
					&& (fileExt.Length <= 2 || !mergeExtensions.Contains(fileExt.Substring(0, 3)))))
					return false;
			}
			return true;
		}

		protected virtual FileInfo MergeFiles(IList files)
		{
			FileInfo result = null;
			string extension = null;
			PdfSharp.Pdf.PdfDocument mergedPDFLabels = null;

			try
			{
				using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
				{
					foreach (FileInfo file in files)
					{
						string ext = System.IO.Path.GetExtension(file.Name);

						if (extension == null)
						{
							extension = ext;
						}
						else
						{
							if (ext.ToLowerInvariant() != extension.ToLowerInvariant())
								throw new PXException(Messages.CannotMergeFiles);
						}

						if (ext.ToLowerInvariant() == ".pdf")
						{
							mergedPDFLabels = mergedPDFLabels ?? new PdfSharp.Pdf.PdfDocument();
							using (System.IO.MemoryStream mem = new System.IO.MemoryStream(file.BinData))
							{
								using (var pdfPages = PdfReader.Open(mem, PdfDocumentOpenMode.Import))
								{
									foreach (PdfSharp.Pdf.PdfPage page in pdfPages.Pages)
									{
										mergedPDFLabels.AddPage(page);
									}
								}
							}
							mergedPDFLabels.Save(stream, false);
						}
						else
						{
							stream.Write(file.BinData, 0, file.BinData.Length);
						}
					}
					mergedPDFLabels?.Close();
					string fileName = Guid.NewGuid().ToString() + extension;
					result = new FileInfo(fileName, null, stream.ToArray());
				}
				return result;
			}
			finally
			{
				mergedPDFLabels?.Dispose();
			}
		}

		#endregion

		public virtual void GetReturnLabels(SOShipment shiporder)
		{
			if (shiporder.UnlimitedPackages == true)
				throw new PXException(Messages.GetLabelsDisabledDueToUnlimitedPackages);

			var shipVia = Carrier.PK.Find(Base, shiporder.ShipVia);

			if (shipVia?.IsActive == false)
			{
				throw new PXException(Messages.ShipViaNotActive);
			}

			if (IsWithLabels(shiporder.ShipVia))
			{
				ICarrierService cs = CarrierMaint.CreateCarrierService(Base, shiporder.ShipVia);
				CarrierRequest cr = Base.CarrierRatesExt.BuildRequest(shiporder);

				var results = cs.GetType().FullName.IsIn("PX.FedExRestCarrier.FedExRestCarrier", "PX.UpsRestCarrier.UpsRestCarrier")
					? cr.Packages
						.ToArray()
						.Select(package =>
						{
							cr.Packages = new List() { package };
							return cs.Return(cr);
						})
						.ToArray()
					: new[] { cs.Return(cr) };

				StringBuilder warningSB = new StringBuilder();
				StringBuilder errorSB = new StringBuilder();

				foreach (var result in results.WhereNotNull())
				{
					if (result.IsSuccess)
					{
						var packagesRefNbrs = result.Result.Data.Select(t => t.RefNbr).ToHashSet();
						UploadFileMaintenance upload = PXGraph.CreateInstance();

						foreach (SOPackageDetail pd in PXSelect>>>.Select(Base, shiporder.ShipmentNbr))
						{
							if (packagesRefNbrs.Contains(pd.LineNbr.Value))
							{
								FileInfo returnLabelFile = GetReturnLabelFileForPackage(pd, upload);

								if (returnLabelFile != null)
								{
									foreach (NoteDoc nd in PXSelect>,
											And>>>>.Select(Base, pd.NoteID, returnLabelFile.UID))
									{
										UploadFileMaintenance.DeleteFile(nd.FileID);
									}
								}
							}
						}

						using (PXTransactionScope ts = new PXTransactionScope())
						{
							PXTransactionScope.SetSuppressWorkflow(true);

							foreach (PackageData pd in result.Result.Data)
							{
								SOPackageDetailEx sdp =
									PXSelect>,
												And>>>>
										.Select(Base, shiporder.ShipmentNbr, pd.RefNbr);
								if (sdp != null)
								{
									string fileName = string.Format("{2} #{0}.{1}", pd.TrackingNumber, pd.Format, ReturnLabel);
									FileInfo file = new FileInfo(fileName, null, pd.Image);
									upload.SaveFile(file);

									sdp.ReturnTrackNumber = pd.TrackingNumber;
									PXNoteAttribute.SetFileNotes(Base.Packages.Cache, sdp, file.UID.Value);
									Base.Packages.Update(sdp);

									Carrier carrier = Carrier.PK.Find(Base, shiporder.ShipVia);
									CarrierPlugin plugin = CarrierPlugin.PK.Find(Base, carrier.CarrierPluginID);

									var pluginMethod = PXSelectorAttribute.Select(Base.carrier.Cache, carrier) as PX.Objects.CS.CarrierMethodSelectorAttribute.CarrierPluginMethod;
									string serviceMethod = $"{carrier.PluginMethod} - {pluginMethod?.Description}";
									if (serviceMethod.Length > CarrierLabelHistory.serviceMethod.Length)
									{
										serviceMethod = serviceMethod.Substring(0, CarrierLabelHistory.serviceMethod.Length);
									}
									decimal rateAmount = ConvertAmtToBaseCury(result.Result.Cost.Currency, Base.arsetup.Current.DefaultRateTypeID, shiporder.ShipDate.Value, pd.RateAmount);

									Base.LabelHistory.Insert(new CarrierLabelHistory()
									{
										ShipmentNbr = shiporder.ShipmentNbr,
										LineNbr = pd.RefNbr,
										PluginTypeName = plugin?.PluginTypeName,
										ServiceMethod = serviceMethod,
										RateAmount = rateAmount
									});
								}
							}

							Base.Actions.PressSave();
							ts.Complete();
						}

						foreach (Message message in result.Messages)
							warningSB.AppendFormat("{0}:{1} ", message.Code, message.Description);
					}
					else
					{
						foreach (Message message in result.Messages)
							errorSB.AppendFormat("{0}:{1} ", message.Code, message.Description);
					}
				}

				if (errorSB.Length > 0 && warningSB.Length > 0)
				{
					string msg = string.Format("Errors: {0}" + Environment.NewLine + "Warnings: {1}", errorSB.ToString(), warningSB.ToString());
					Base.Document.Cache.RaiseExceptionHandling(
						shiporder,
						shiporder.CuryFreightCost,
						new PXSetPropertyException(shiporder, Messages.CarrierServiceError, PXErrorLevel.Error, msg));

					throw new PXException(Messages.CarrierServiceError, msg);
				}
				else if (errorSB.Length > 0)
				{
					Base.Document.Cache.RaiseExceptionHandling(
						shiporder,
						shiporder.CuryFreightCost,
						new PXSetPropertyException(shiporder, Messages.CarrierServiceError, PXErrorLevel.Error, errorSB.ToString()));

					throw new PXException(Messages.CarrierServiceError, errorSB.ToString());
				}
				else if (warningSB.Length > 0)
				{
					Base.Document.Cache.RaiseExceptionHandling(
						shiporder,
						shiporder.CuryFreightCost,
						new PXSetPropertyException(shiporder, warningSB.ToString(), PXErrorLevel.Warning));
				}
			}
		}

		#region Protected access
		/// Uses 
		[PXProtectedAccess]
		protected abstract PXAdapter CreateDummyAdapter();

		/// Uses 
		[PXProtectedAccess]
		protected abstract bool IsWithLabels(string shipVia);

		/// Uses 
		[PXProtectedAccess]
		protected abstract decimal ConvertAmtToBaseCury(string from, string rateType, DateTime effectiveDate, decimal amount);
		#endregion
	}
}
