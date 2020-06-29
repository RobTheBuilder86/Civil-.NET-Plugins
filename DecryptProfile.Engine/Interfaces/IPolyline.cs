using DeconstructSurfaceSampleView.Engine.HelperObjects;
using System.Collections.Generic;


namespace DeconstructSurfaceSampleView.Engine.Interfaces
{
    public interface IPolyline
    {
        List<SimplePoint2d> GetPoints();
    }
}
