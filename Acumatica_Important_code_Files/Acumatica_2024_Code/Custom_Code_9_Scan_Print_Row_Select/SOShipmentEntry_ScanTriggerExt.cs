using System;
using PX.Data;
using PX.Objects.SO;

namespace AA.Objects.AL.Integration.PerPackage
{
    public class SOShipmentEntry_ScanTriggerExt : PXGraphExtension<SOShipmentEntry>
    {
        public static bool IsActive() => true;

        protected virtual void _(Events.RowPersisted<SOPackageDetail> e)
        {
            try
            {
                PXTrace.WriteInformation("=== SCAN TRIGGER: RowPersisted<SOPackageDetail> event fired ===");

                SOPackageDetail package = e.Row;
                SOShipment shipment = Base.Document.Current;

                if (shipment == null || package == null)
                {
                    PXTrace.WriteInformation("[SKIP] Shipment or package is null");
                    return;
                }

                if (package.Confirmed != true)
                {
                    PXTrace.WriteInformation("[SKIP] Package not confirmed");
                    return;
                }

                // ✅ NEW: Check if THIS package is the CURRENTLY SELECTED row
                SOPackageDetail currentSelectedPackage = Base.Packages.Current;
                
                if (currentSelectedPackage == null)
                {
                    PXTrace.WriteInformation("[SKIP] No package is currently selected in the grid");
                    return;
                }

                // ✅ Only print if the confirmed package is the same as the selected row
                if (currentSelectedPackage.LineNbr != package.LineNbr)
                {
                    PXTrace.WriteInformation(
                        "[SKIP] Confirmed package {0} is not the selected row (selected: {1})",
                        package.LineNbr,
                        currentSelectedPackage.LineNbr);
                    return;
                }

                PXTrace.WriteInformation(
                    "[PASS] Package {0} confirmed and is the selected row in shipment {1}",
                    package.LineNbr,
                    shipment.ShipmentNbr);

                string shipmentNbr = shipment.ShipmentNbr;
                int packageLineNbr = (int)package.LineNbr;

                // ✅ Use PXLongOperation to defer print action outside of RowPersisted context
                PXLongOperation.StartOperation(Base, delegate()
                {
                    PXTrace.WriteInformation("[LONGOP] Started for package {0}", packageLineNbr);

                    SOShipmentEntry graph = PXGraph.CreateInstance<SOShipmentEntry>();
                    SOShipment reloadedShipment = SOShipment.PK.Find(graph, shipmentNbr);

                    if (reloadedShipment == null)
                    {
                        PXTrace.WriteError("[ERROR] Could not reload shipment {0}", shipmentNbr);
                        return;
                    }

                    graph.Document.Current = reloadedShipment;

                    var asgardExt = graph.FindImplementation<SOShipmentEntry_AsgardExt>();

                    if (asgardExt == null)
                    {
                        PXTrace.WriteError("[ERROR] SOShipmentEntry_AsgardExt not found");
                        return;
                    }

                    PXTrace.WriteInformation("[CALLING] PrintForPackage() with package line {0}", packageLineNbr);

                    // ✅ NEW: Pass the selected package line number explicitly
                    PXAdapter adapter = new PXAdapter(graph.Document);
                    adapter.Searches = new string[] { };
                    adapter.Parameters = new object[] { };

                    asgardExt.PrintForPackage(adapter, packageLineNbr);

                    PXTrace.WriteInformation("[SUCCESS] PrintForPackage() completed for package {0}", packageLineNbr);
                });
            }
            catch (Exception ex)
            {
                PXTrace.WriteError("[FATAL] Exception in RowPersisted: {0}", ex.Message);
                PXTrace.WriteError("[STACK] {0}", ex.StackTrace);
            }
        }
    }
}