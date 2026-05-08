using System.Collections;
using System.Collections.Generic;
using PX.Data;
using PX.Objects.SO;

namespace AA.Objects.AL.Integration.PerPackage
{
    /// <summary>
    /// Generic Acumatica package view filter extension that intercepts package-related views.
    /// 
    /// SUPPORTED VIEWS:
    /// - "Packages"         - Native Acumatica package grid (SOPackageDetail/SOPackageDetailEx)
    /// - "ALPackages"       - Asgard label view (basic package structure)
    /// - "ALiStarPackages"  - Asgard label view (joined with iStar custom fields)
    /// 
    /// CRITICAL: This extension filters views only while ALPackagesFilterScope is ACTIVE.
    /// When scope is inactive, original view behavior is fully preserved.
    /// 
    /// FILTERING LOGIC:
    /// - Captures original view in Initialize()
    /// - Replaces with filtered delegate that checks ALPackagesFilterScope.IsActive
    /// - If active: yields only rows matching selected package (ShipmentNbr + LineNbr)
    /// - If inactive: yields all rows unchanged
    /// - Preserves row shape (PXResult, joined rows, original structure)
    /// 
    /// ROW SHAPE PRESERVATION:
    /// For joined views like ALiStarPackages, the original row is a PXResult with multiple tables.
    /// This extension unwraps only the SOPackageDetail for filtering logic, but yields the
    /// entire original row object to maintain joined structure for Asgard.
    /// </summary>
    public class SOShipmentEntry_AsgardViewFilterExt : PXGraphExtension<SOShipmentEntry>
    {
        // Store original views for reference
        private PXView _originalPackagesView;
        private PXView _originalALPackagesView;
        private PXView _originalALiStarPackagesView;

        // List of views to filter
        private static readonly string[] ViewsToFilter = new[] { "Packages", "ALPackages", "ALiStarPackages" };

        public static bool IsActive()
        {
            return true;
        }

        public override void Initialize()
        {
            base.Initialize();

            PXTrace.WriteInformation("[VIEW-FILTER-INIT] ========== SOShipmentEntry_AsgardViewFilterExt.Initialize() ==========");
            PXTrace.WriteInformation("[VIEW-FILTER-INIT] Base.Views count: {0}", Base.Views.Count);

            // Filter each supported view
            foreach (string viewName in ViewsToFilter)
            {
                FilterViewIfExists(viewName);
            }

            PXTrace.WriteInformation("[VIEW-FILTER-INIT] ========== Initialization complete ==========");
        }

        /// <summary>
        /// Generic method to wrap any supported view with a filter delegate.
        /// If view does not exist, logs and skips safely.
        /// </summary>
        private void FilterViewIfExists(string viewName)
        {
            if (!Base.Views.ContainsKey(viewName))
            {
                PXTrace.WriteInformation("[VIEW-FILTER-INIT] View '{0}' does not exist in Base.Views - skipping", viewName);
                return;
            }

            PXView originalView = Base.Views[viewName];
            if (originalView == null)
            {
                PXTrace.WriteInformation("[VIEW-FILTER-INIT] View '{0}' exists but is NULL - skipping", viewName);
                return;
            }

            PXTrace.WriteInformation("[VIEW-FILTER-INIT] Found view '{0}' - wrapping with filter delegate", viewName);
            PXTrace.WriteInformation("[VIEW-FILTER-INIT] View '{0}' BqlSelect type: {1}", 
                viewName, originalView.BqlSelect?.GetType().FullName ?? "null");

            // Store reference based on view name for debugging
            switch (viewName)
            {
                case "Packages":
                    _originalPackagesView = originalView;
                    break;
                case "ALPackages":
                    _originalALPackagesView = originalView;
                    break;
                case "ALiStarPackages":
                    _originalALiStarPackagesView = originalView;
                    break;
            }

            // Replace view with filtered delegate
            // Use closure to capture viewName and originalView
            Base.Views[viewName] = new PXView(
                Base,
                true,
                originalView.BqlSelect,
                new PXSelectDelegate(() => FilteredPackageView(viewName, originalView)));

            PXTrace.WriteInformation("[VIEW-FILTER-INIT] ✅ Base.Views['{0}'] successfully wrapped with filtered delegate", viewName);
        }

        /// <summary>
        /// Generic filtered view delegate for package-related views.
        /// 
        /// Behavior:
        /// - If ALPackagesFilterScope is NOT active: yields ALL rows unchanged (normal behavior)
        /// - If ALPackagesFilterScope IS active: yields ONLY rows matching selected package
        /// 
        /// Row Shape:
        /// - Unwraps SOPackageDetail for filtering logic only
        /// - Yields entire original row object (preserves PXResult and joined structure)
        /// </summary>
        protected virtual IEnumerable FilteredPackageView(string viewName, PXView originalView)
        {
            if (originalView == null)
            {
                PXTrace.WriteInformation("[FILTER-{0}] originalView is NULL - yielding nothing", viewName);
                yield break;
            }

            // ✅ Query the original view with current Document context
            object[] currents = new object[] { Base.Document.Current };
            IEnumerable rawRows = originalView.SelectMultiBound(currents);

            // ✅ If filter scope is NOT active, pass through all rows unchanged
            if (!ALPackagesFilterScope.IsActive)
            {
                PXTrace.WriteInformation("[FILTER-{0}] ALPackagesFilterScope is INACTIVE - returning all rows", viewName);
                
                int passthrough = 0;
                foreach (object row in rawRows)
                {
                    passthrough++;
                    yield return row;
                }
                
                PXTrace.WriteInformation("[FILTER-{0}] Passed through {1} rows (scope inactive)", viewName, passthrough);
                yield break;
            }

            // ✅ Filter scope IS active - filter rows by selected package
            string currentShipmentNbr = Base.Document.Current?.ShipmentNbr;
            PXTrace.WriteInformation("[FILTER-{0}] ALPackagesFilterScope IS ACTIVE for shipment '{1}'", 
                viewName, currentShipmentNbr ?? "null");

            int totalRows = 0;
            int filteredRows = 0;
            int rejectedRows = 0;

            foreach (object row in rawRows)
            {
                totalRows++;

                // ✅ Unwrap SOPackageDetail from the row
                // Works for both simple rows (Packages, ALPackages) and joined rows (ALiStarPackages)
                SOPackageDetail packageDetail = PXResult.Unwrap<SOPackageDetail>(row);
                
                if (packageDetail == null)
                {
                    PXTrace.WriteInformation("[FILTER-{0}] Row {1}: Could not unwrap SOPackageDetail from row type {2}", 
                        viewName, totalRows, row.GetType().Name);
                    rejectedRows++;
                    continue;
                }

                // ✅ Check if this package matches the active filter scope
                if (!ALPackagesFilterScope.Matches(currentShipmentNbr, packageDetail.LineNbr))
                {
                    PXTrace.WriteInformation("[FILTER-{0}] Row {1}: Package ShipmentNbr='{2}', LineNbr={3} - NO MATCH", 
                        viewName, totalRows, packageDetail.ShipmentNbr ?? "null", packageDetail.LineNbr);
                    rejectedRows++;
                    continue;
                }

                // ✅ Row matches filter - yield entire original row (preserves row shape)
                filteredRows++;
                PXTrace.WriteInformation("[FILTER-{0}] Row {1}: Package ShipmentNbr='{2}', LineNbr={3} - ✅ MATCH (row type: {4})", 
                    viewName, totalRows, packageDetail.ShipmentNbr ?? "null", packageDetail.LineNbr, row.GetType().Name);
                
                yield return row;
            }

            PXTrace.WriteInformation("[FILTER-{0}] Summary: Total={1}, Accepted={2}, Rejected={3}", 
                viewName, totalRows, filteredRows, rejectedRows);
        }
    }
}