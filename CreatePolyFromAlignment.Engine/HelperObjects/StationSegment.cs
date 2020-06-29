using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatePolyFromAlignment.Engine.HelperObjects
{
    public enum SegmentType
    {
        straight,
        nonstraight
    }

    public class StationSegment
    {
        public double StartStation { get; }
        public double EndStation { get; }
        public SegmentType Type { get; }

        public double Length { get { return EndStation - StartStation; } }
        public bool IsInside(double station) =>
            (StartStation <= station && EndStation > station);

        public StationSegment(double start, double end, SegmentType type)
        {
            if (start >= end) {
                throw new ArgumentException("End station must be bigger than start station");
            }
            (StartStation, EndStation, Type) = (start, end, type);
        }
    }
}
