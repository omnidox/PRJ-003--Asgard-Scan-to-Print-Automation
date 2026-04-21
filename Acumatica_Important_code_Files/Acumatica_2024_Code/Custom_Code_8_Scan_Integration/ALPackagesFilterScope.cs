using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AA.Objects.AL.Integration.PerPackage
{
    internal sealed class ALPackagesFilterState
    {
        public string ShipmentNbr { get; set; }
        public HashSet<int?> PackageLineNbrs { get; set; }
    }

    internal static class ALPackagesFilterScope
    {
        private static readonly AsyncLocal<ALPackagesFilterState> _state = new AsyncLocal<ALPackagesFilterState>();

        public static bool IsActive => _state.Value != null;

        public static string ShipmentNbr => _state.Value?.ShipmentNbr;

        public static bool Matches(string shipmentNbr, int? packageLineNbr)
        {
            ALPackagesFilterState state = _state.Value;
            if (state == null)
                return false;

            if (!string.Equals(state.ShipmentNbr, shipmentNbr, StringComparison.OrdinalIgnoreCase))
                return false;

            if (state.PackageLineNbrs == null || state.PackageLineNbrs.Count == 0)
                return false;

            return state.PackageLineNbrs.Contains(packageLineNbr);
        }

        public static IDisposable Activate(string shipmentNbr, IEnumerable<int?> packageLineNbrs)
        {
            HashSet<int?> lines = new HashSet<int?>(packageLineNbrs ?? Enumerable.Empty<int?>());

            ALPackagesFilterState previous = _state.Value;

            _state.Value = new ALPackagesFilterState
            {
                ShipmentNbr = shipmentNbr,
                PackageLineNbrs = lines
            };

            return new RestoreDisposable(previous);
        }

        private sealed class RestoreDisposable : IDisposable
        {
            private readonly ALPackagesFilterState _previous;
            private bool _disposed;

            public RestoreDisposable(ALPackagesFilterState previous)
            {
                _previous = previous;
            }

            public void Dispose()
            {
                if (_disposed)
                    return;

                _state.Value = _previous;
                _disposed = true;
            }
        }
    }
}