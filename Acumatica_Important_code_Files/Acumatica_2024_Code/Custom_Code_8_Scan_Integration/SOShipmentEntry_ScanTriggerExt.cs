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

                PXTrace.WriteInformation("[PASS] Package {0} confirmed in shipment {1}",
                    package.LineNbr,
                    shipment.ShipmentNbr);

                // ✅ DEFER to PXLongOperation - do NOT call PrintForPackage directly!
                string shipmentNbr = shipment.ShipmentNbr;
                int packageLineNbr = (int)package.LineNbr;

                PXLongOperation.StartOperation(Base, delegate()
                {
                    PXTrace.WriteInformation("[LONGOP] Started for package {0}", packageLineNbr);

                    // Create fresh graph inside long operation
                    SOShipmentEntry graph = PXGraph.CreateInstance<SOShipmentEntry>();
                    SOShipment reloadedShipment = SOShipment.PK.Find(graph, shipmentNbr);

                    if (reloadedShipment == null)
                    {
                        PXTrace.WriteError("[ERROR] Could not reload shipment {0}", shipmentNbr);
                        return;
                    }

                    graph.Document.Current = reloadedShipment;

                    // Get Asgard extension from fresh graph
                    var asgardExt = graph.FindImplementation<SOShipmentEntry_AsgardExt>();

                    if (asgardExt == null)
                    {
                        PXTrace.WriteError("[ERROR] SOShipmentEntry_AsgardExt not found");
                        return;
                    }

                    PXTrace.WriteInformation("[CALLING] PrintForPackage() in long operation");

                    // Call PrintForPackage on the fresh graph
                    PXAdapter adapter = new PXAdapter(graph.Document);
                    adapter.Searches = new string[] { };
                    adapter.Parameters = new object[] { };

                    asgardExt.PrintForPackage(adapter);

                    PXTrace.WriteInformation("[SUCCESS] PrintForPackage() completed");
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