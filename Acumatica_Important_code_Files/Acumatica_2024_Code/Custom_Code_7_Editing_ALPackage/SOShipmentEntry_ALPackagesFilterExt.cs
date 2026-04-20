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

            if (!ALPackagesFilterScope.IsActive)
            {
                foreach (object row in rawRows)
                    yield return row;

                yield break;
            }

            string currentShipmentNbr = Base.Document.Current?.ShipmentNbr;

            foreach (object row in rawRows)
            {
                SOPackageDetail package = PXResult.Unwrap<SOPackageDetail>(row);
                if (package == null)
                    continue;

                if (!ALPackagesFilterScope.Matches(currentShipmentNbr, package.LineNbr))
                    continue;

                yield return row;
            }
        }
    }
}