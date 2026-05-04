using System;
using System.Collections;
using PX.Data;
using PX.Objects.SO;

namespace PX.Objects.SO
{
    /// <summary>
    /// ========================================================================
    /// SOShipmentEntry Extension - Carrier Package Filter View Replacement
    /// ========================================================================
    /// 
    /// Purpose:
    /// When CarrierPackageFilterScope is active, replaces SOShipmentEntry.Packages
    /// view with a filtered version that returns ONLY the selected package.
    /// 
    /// This ensures CarrierRates.GetPackages() (called by BuildRequest) only
    /// sees the selected package, not all packages on the shipment.
    /// 
    /// Result: Only the selected package is validated for confirmation status,
    /// allowing per-package carrier label generation even if other packages
    /// are not yet confirmed.
    /// 
    /// Hook Point:
    /// Intercepts Packages view delegate when CarrierPackageFilterScope is active.
    /// 
    /// Design Pattern:
    /// Similar to SOShipmentEntry_ALPackagesFilterExt, but for native Packages view.
    /// </summary>
    public class SOShipmentEntry_CarrierPackageFilterExt : PXGraphExtension<SOShipmentEntry>
    {
        public static bool IsActive() => true;

        /// <summary>
        /// Capture the original Packages view on initialization.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            // Replace Packages view if filter scope becomes active
            if (CarrierPackageFilterScope.IsActive)
            {
                PXTrace.WriteInformation(
                    "[CARRIER-PKG-FILTER-EXT] Initialize called with active filter scope");

                ReplacePackagesViewIfNeeded();
            }
        }

        /// <summary>
        /// Replace the Packages view delegate with a filtered version if scope is active.
        /// </summary>
        private void ReplacePackagesViewIfNeeded()
        {
            if (!CarrierPackageFilterScope.IsActive)
            {
                PXTrace.WriteInformation(
                    "[CARRIER-PKG-FILTER-EXT] ReplacePackagesViewIfNeeded: No active filter scope, skipping replacement");
                return;
            }

            if (!Base.Views.ContainsKey("Packages"))
            {
                PXTrace.WriteWarning(
                    "[CARRIER-PKG-FILTER-EXT] Packages view not found in Base.Views");
                return;
            }

            PXView originalView = Base.Views["Packages"];

            if (originalView == null)
            {
                PXTrace.WriteWarning(
                    "[CARRIER-PKG-FILTER-EXT] Original Packages view is null");
                return;
            }

            PXTrace.WriteInformation(
                "[CARRIER-PKG-FILTER-EXT] ✅ Replacing Packages view with filtered delegate");

            // Replace view with filtered delegate
            Base.Views["Packages"] = new PXView(
                Base,
                false,
                new BqlSelect(Base),
                delegate (PXView view)
                {
                    return FilteredPackagesDelegate(originalView);
                });

            PXTrace.WriteInformation(
                "[CARRIER-PKG-FILTER-EXT] ✅ Packages view successfully replaced");
        }

        /// <summary>
        /// Filtered view delegate that yields only the selected package.
        /// </summary>
        protected virtual IEnumerable FilteredPackagesDelegate(PXView originalView)
        {
            PXTrace.WriteInformation(
                "[CARRIER-PKG-FILTER-EXT.Delegate] FilteredPackagesDelegate called");

            if (!CarrierPackageFilterScope.IsActive)
            {
                PXTrace.WriteInformation(
                    "[CARRIER-PKG-FILTER-EXT.Delegate] Filter scope no longer active, returning all packages");
                
                // If scope became inactive, yield all rows from original view
                foreach (object row in originalView.SelectMultiBound(new object[] { Base.Document.Current }))
                {
                    yield return row;
                }
                yield break;
            }

            string filterShipmentNbr = CarrierPackageFilterScope.ShipmentNbr;
            int? filterPackageLineNbr = CarrierPackageFilterScope.SelectedPackageLineNbr;

            PXTrace.WriteInformation(
                "[CARRIER-PKG-FILTER-EXT.Delegate] Filter active: shipment={0}, selectedLineNbr={1}",
                filterShipmentNbr, filterPackageLineNbr);

            SOShipment currentShipment = Base.Document.Current;
            if (currentShipment == null)
            {
                PXTrace.WriteWarning(
                    "[CARRIER-PKG-FILTER-EXT.Delegate] Current shipment is null, returning all packages");
                foreach (object row in originalView.SelectMultiBound(new object[] { Base.Document.Current }))
                {
                    yield return row;
                }
                yield break;
            }

            string currentShipmentNbr = currentShipment.ShipmentNbr;

            if (!string.Equals(currentShipmentNbr, filterShipmentNbr, StringComparison.OrdinalIgnoreCase))
            {
                PXTrace.WriteWarning(
                    "[CARRIER-PKG-FILTER-EXT.Delegate] Shipment mismatch: current={0}, filter={1}, returning all packages",
                    currentShipmentNbr, filterShipmentNbr);
                foreach (object row in originalView.SelectMultiBound(new object[] { Base.Document.Current }))
                {
                    yield return row;
                }
                yield break;
            }

            // Yield only rows matching the selected package LineNbr
            int yieldCount = 0;
            foreach (object rowObj in originalView.SelectMultiBound(new object[] { currentShipment }))
            {
                SOPackageDetailEx package = rowObj as SOPackageDetailEx;
                if (package == null)
                {
                    PXTrace.WriteWarning(
                        "[CARRIER-PKG-FILTER-EXT.Delegate] Row is not SOPackageDetailEx, yielding anyway");
                    yield return rowObj;
                    yieldCount++;
                    continue;
                }

                if (CarrierPackageFilterScope.Matches(currentShipmentNbr, package.LineNbr))
                {
                    PXTrace.WriteInformation(
                        "[CARRIER-PKG-FILTER-EXT.Delegate] ✅ Package LineNbr={0} MATCHES filter, yielding",
                        package.LineNbr);

                    yield return rowObj;
                    yieldCount++;
                }
                else
                {
                    PXTrace.WriteInformation(
                        "[CARRIER-PKG-FILTER-EXT.Delegate] Package LineNbr={0} does NOT match filter, skipping",
                        package.LineNbr);
                }
            }

            PXTrace.WriteInformation(
                "[CARRIER-PKG-FILTER-EXT.Delegate] ✅ Filtered delegate complete. Yielded {0} package(s)",
                yieldCount);
        }
    }
}
