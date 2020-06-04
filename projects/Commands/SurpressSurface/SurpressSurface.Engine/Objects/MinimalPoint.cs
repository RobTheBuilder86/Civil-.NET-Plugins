using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SurpressSurface.Engine {
    public class MinimalPoint {
        public double X { get; }
        public double Y { get; }
        public double Z { get; }

        public MinimalPoint(double x, double y, double z) => (X, Y, Z) = (x, y, z);
    }
}
