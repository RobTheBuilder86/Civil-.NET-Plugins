using CreatePolyFromAlignment.Engine.HelperObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatePolyFromAlignment.Engine.Interfaces
{
    public interface IProfile
    {
        List<StationSegment> GetSegments();
        double GetElevationAt(double station);
    }
}
