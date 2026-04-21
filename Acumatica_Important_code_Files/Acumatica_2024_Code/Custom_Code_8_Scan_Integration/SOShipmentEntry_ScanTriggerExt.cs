using System;
using PX.Data;
using PX.Objects.SO;

namespace AA.Objects.AL.Integration.PerPackage
{
    /// <summary>
    /// Extension that triggers the print action when a package is confirmed via WMS scan
    /// Does NOT press the button - calls the action method directly
    /// </summary>
    public class SOShipmentEntry_ScanTriggerExt : PXGraphExtension<SOShipmentEntry>
    {
        // Always return true - IsActive is called before graph exists
        // Feature check happens at runtime in RowPersisted
        public static bool IsActive() => true;

        /// <summary>
        /// RowPersisted fires AFTER package confirmation is saved
        /// Calls PrintForPackage() action method directly when package is confirmed
        /// </summary>
        protected virtual void _(Events.RowPersisted<SOPackageDetail> e)
        {
            SOPackageDetail package = e.Row;
            SOShipment shipment = Base.Document.Current;

            // Safety checks
            if (shipment == null || package == null)
                return;

            // Only trigger if package was just confirmed
            if (package.Confirmed != true)
                return;

            // Check if feature is enabled (at runtime when graph exists)
            if (!IsFeatureEnabled())
                return;

            try
            {
                PXTrace.WriteInformation(
                    "Scan confirm detected for package {0} in shipment {1}. Triggering print action.",
                    package.LineNbr,
                    shipment.ShipmentNbr);

                // Get the button extension (where PrintForPackage lives)
                var asgardExt = Base.FindImplementation<SOShipmentEntry_AsgardExt>();

                if (asgardExt == null)
                {
                    throw new PXException(
                        "SOShipmentEntry_AsgardExt not found. Print action cannot be triggered.");
                }

                // Call the action method directly (NOT the button)
                PXAdapter adapter = new PXAdapter(Base.Document);
                adapter.Searches = new string[] { };
                adapter.Parameters = new object[] { };
                asgardExt.PrintForPackage(adapter);
            }
            catch (Exception ex)
            {
                PXTrace.WriteError(
                    "Error triggering print action on scan confirm for package {0}: {1}",
                    package.LineNbr,
                    ex.Message);
                // Don't throw - allow user to continue packing
            }
        }

        /// <summary>
        /// Check if the scan-to-print feature is enabled
        /// Called at runtime when the graph is available
        /// </summary>
        private bool IsFeatureEnabled()
        {
            try
            {
                ALSetup setup = PXSelect<ALSetup>.Select(Base);
                if (setup == null)
                    return false;

                var ext = setup.GetExtension<ALSetup_ScanPrintExt>();
                return ext?.PrintOnScanConfirm == true;
            }
            catch (Exception ex)
            {
                PXTrace.WriteError("Error checking PrintOnScanConfirm: {0}", ex.Message);
                return false;
            }
        }
    }
}