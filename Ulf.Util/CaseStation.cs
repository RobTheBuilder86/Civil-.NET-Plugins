using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ulf.Util
{
    public class CaseStation
    {
        public readonly CalculationCase CalcCase;
        public readonly double Station;

        public CaseStation(CalculationCase calcCase, double station) =>
            (CalcCase, Station) = (calcCase, station);

        public double X {
            get { return StationToXConverter.ToX(Station); }
        }
        public double Y {
            get { return CaseToYConverter.ToY(CalcCase); }
        }
    }
}
