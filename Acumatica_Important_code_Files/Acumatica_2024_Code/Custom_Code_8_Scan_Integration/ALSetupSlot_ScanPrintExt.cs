using System;
using PX.Data;

namespace AA.Objects.AL.Integration.PerPackage
{
    /// <summary>
    /// Helper to expose PrintOnScanConfirm from ALSetup cache extension
    /// </summary>
    public static class ALSetupSlot_ScanPrintExt
    {
        public static bool PrintOnScanConfirm
        {
            get
            {
                try
                {
                    ALSetup setup = PXSelect<ALSetup>.Select(null);
                    if (setup == null)
                        return false;

                    var ext = setup.GetExtension<ALSetup_ScanPrintExt>();
                    if (ext == null)
                        return false;

                    return ext.PrintOnScanConfirm == true;
                }
                catch (Exception ex)
                {
                    PXTrace.WriteError("Error reading PrintOnScanConfirm: {0}", ex.Message);
                    return false;
                }
            }
        }

        public static Guid? PrintOnScanConfirmModelID
        {
            get
            {
                try
                {
                    ALSetup setup = PXSelect<ALSetup>.Select(null);
                    if (setup == null)
                        return null;

                    var ext = setup.GetExtension<ALSetup_ScanPrintExt>();
                    if (ext == null)
                        return null;

                    return ext.PrintOnScanConfirmModelID;
                }
                catch (Exception ex)
                {
                    PXTrace.WriteError("Error reading PrintOnScanConfirmModelID: {0}", ex.Message);
                    return null;
                }
            }
        }
    }
}