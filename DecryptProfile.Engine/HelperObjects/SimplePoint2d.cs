namespace DeconstructSurfaceSampleView.Engine.HelperObjects
{
    public struct SimplePoint2d
    {
        public readonly double X;
        public readonly double Y;

        public SimplePoint2d(double x, double y) =>
            (this.X, this.Y) = (x, y);
    }
}
