using DeconstructSurfaceSampleView.Engine.HelperObjects;

namespace DeconstructSurfaceSampleView.Engine.Interfaces
{
    public interface IAlignment
    {
        SimplePoint2d PointLocation(double officialStation, double offset);
    }
}
