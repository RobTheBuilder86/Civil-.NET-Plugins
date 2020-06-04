using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurpressSurface.Engine {
    public interface IVertex {

        Double GetX();
        Double GetY();
        Double GetZ();

        void SetZ(double newZ);

        bool IsValid();

    }
}
