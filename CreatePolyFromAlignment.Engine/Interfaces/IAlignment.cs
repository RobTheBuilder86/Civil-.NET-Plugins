using CreatePolyFromAlignment.Engine.HelperObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatePolyFromAlignment.Engine.Interfaces
{
    public interface IAlignment
    {
        List<StationSegment> GetSegments();
        (double x, double y)GetCoordsAt(double Station);
    }
}
