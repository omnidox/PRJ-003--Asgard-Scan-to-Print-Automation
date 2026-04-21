using System;
using PX.Data;

namespace AA.Objects.AL.Integration.PerPackage
{
    /// <summary>
    /// Extension that adds scan-print feature toggle to ALSetup
    /// Uses unbound fields (not persisted to database)
    /// </summary>
    public class ALSetup_ScanPrintExt : PXCacheExtension<ALSetup>
    {
        public static bool IsActive() => true;

        #region PrintOnScanConfirm
        [PXBool]
        [PXDefault(false)]
        [PXUIField(DisplayName = "Print Label on Scan Confirm")]
        public bool? PrintOnScanConfirm { get; set; }

        public abstract class printOnScanConfirm : PX.Data.BQL.BqlBool.Field<printOnScanConfirm> { }
        #endregion

        #region PrintOnScanConfirmModelID
        [PXGuid]
        [PXUIField(DisplayName = "Scan Confirm Label Model")]
        public Guid? PrintOnScanConfirmModelID { get; set; }

        public abstract class printOnScanConfirmModelID : PX.Data.BQL.BqlGuid.Field<printOnScanConfirmModelID> { }
        #endregion
    }
}