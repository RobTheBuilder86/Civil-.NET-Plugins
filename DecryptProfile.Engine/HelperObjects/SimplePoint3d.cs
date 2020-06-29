namespace DeconstructSurfaceSampleView.Engine.HelperObjects
{
    public struct SimplePoint3d
    {
        public readonly double X;
        public readonly double Y;
        public readonly double Z;

        public SimplePoint3d(double x, double y, double z) =>
            (this.X, this.Y, this.Z) = (x, y, z);

        public SimplePoint3d(SimplePoint2d point2d, double z) =>
            (this.X, this.Y, this.Z) = (point2d.X, point2d.Y, z);
    }
}
