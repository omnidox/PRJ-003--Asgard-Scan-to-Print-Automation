using System.Collections;
using System.Collections.Generic;
using PX.Data;
using PX.Objects.SO;

namespace AA.Objects.AL.Integration.PerPackage
{
    public class SOShipmentEntry_ALPackagesFilterExt : PXGraphExtension<SOShipmentEntry>
    {
        private PXView _originalALPackagesView;

        public static bool IsActive()
        {
            return true;
        }

        public override void Initialize()
        {
            base.Initialize();

            if (!Base.Views.ContainsKey("ALPackages"))
                return;

            _originalALPackagesView = Base.Views["ALPackages"];

            if (_originalALPackagesView == null)
                return;

            Base.Views["ALPackages"] = new PXView(
                Base,
                true,
                _originalALPackagesView.BqlSelect,
                new PXSelectDelegate(FilteredALPackages));
        }

        protected virtual IEnumerable FilteredALPackages()
        {
            if (_originalALPackagesView == null)
                yield break;

            object[] currents = new object[] { Base.Document.Current };

            IEnumerable rawRows = _originalALPackagesView.SelectMultiBound(currents);

            // ✅ ADD TRACE: Log whether filter is active and how many packages we're filtering
            if (!ALPackagesFilterScope.IsActive)
            {
                PXTrace.WriteInformation("[FILTER] ALPackagesFilterScope is NOT active - returning all packages");
                foreach (object row in rawRows)
                    yield return row;

                yield break;
            }

            PXTrace.WriteInformation("[FILTER] ALPackagesFilterScope IS active for shipment {0}", ALPackagesFilterScope.ShipmentNbr);

            string currentShipmentNbr = Base.Document.Current?.ShipmentNbr;
            int filteredCount = 0;
            int totalCount = 0;

            foreach (object row in rawRows)
            {
                SOPackageDetail package = PXResult.Unwrap<SOPackageDetail>(row);
                if (package == null)
                    continue;

                totalCount++;

                if (!ALPackagesFilterScope.Matches(currentShipmentNbr, package.LineNbr))
                {
                    PXTrace.WriteInformation("[FILTER] Package {0} does NOT match filter", package.LineNbr);
                    continue;
                }

                filteredCount++;
                PXTrace.WriteInformation("[FILTER] Package {0} MATCHES filter - yielding", package.LineNbr);
                yield return row;
            }

            PXTrace.WriteInformation("[FILTER] Filtered {0} out of {1} packages", filteredCount, totalCount);
        }
    }
}