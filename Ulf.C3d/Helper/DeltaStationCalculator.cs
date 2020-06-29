using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ulf.Util;

namespace Ulf.C3D.Helper
{
    internal static class DeltaStationCalculator
    {
        internal static double steepness = 2;

        internal static double StationDelta (CaseStation from, CaseStation to)
        {
            double deltaY =
                Math.Abs(CaseToYConverter.ToY(from.CalcCase) - 
                         CaseToYConverter.ToY(to.CalcCase));
            double deltaStation = deltaY / steepness;
            return deltaStation;
        }
    }
}
