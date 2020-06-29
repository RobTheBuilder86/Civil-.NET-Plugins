using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatePolyFromAlignment.Engine.Interfaces
{
    public interface IPolyline
    {
        void AddVertexAt(double x, double y, double z);
    }
}
