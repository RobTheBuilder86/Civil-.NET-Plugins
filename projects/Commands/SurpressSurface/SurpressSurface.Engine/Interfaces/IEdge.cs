using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurpressSurface.Engine {
    public interface IEdge {
        MinimalPoint Vertex1MP();
        MinimalPoint Vertex2MP();

        bool IsValid();
    }
}
