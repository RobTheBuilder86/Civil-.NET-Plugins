using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurpressSurface.Engine {
    public interface ISurface {
        IEnumerable<IVertex> GetVertexCollection();

        double FindElevationAtXY(double x, double y);

        void AddVertex(double x, double y, double z);

        MinimalPoint GetIntersectionMidpoint(IEdge edge);


        IEnumerable<IEdge> GetEdges();
    }
}
