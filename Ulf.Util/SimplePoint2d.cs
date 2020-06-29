using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ulf.Util
{
    public class SimplePoint2d : IEquatable<SimplePoint2d>
    {
        public readonly double X;
        public readonly double Y;

        public SimplePoint2d(double x, double y) => (X, Y) = (x, y);

        public override string ToString()
        {
            return ($"X: {X} - Y: {Y}");
        }

        public bool Equals(SimplePoint2d other)
        {
            if (other == null && this == null) {
                return true;
            } else if (other == null || this == null) {
                return false;
            } else {
                // To points are presumed if the distance between them is less than tolerance.
                double tolerance = 0.0001;
                double deltaX = other.X - this.X;
                double deltaY = other.Y - this.Y;
                double distance = Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2));
                if (distance < tolerance) {
                    return true;
                } else {
                    return false;
                }
            }
        }
    }
}
