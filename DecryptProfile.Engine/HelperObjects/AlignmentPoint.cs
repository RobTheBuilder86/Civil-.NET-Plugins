namespace DeconstructSurfaceSampleView.Engine.HelperObjects
{
    public struct AlignmentPoint
    {
        public readonly double Station;
        public readonly double Offset;
        public readonly double Elevation;

        public AlignmentPoint(double station, double offset, double elevation) =>
            (this.Station, this.Offset, this. Elevation) = (station, offset, elevation);
    }
}
