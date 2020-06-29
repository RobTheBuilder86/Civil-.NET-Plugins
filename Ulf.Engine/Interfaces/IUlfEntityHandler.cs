using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ulf.Engine.Interfaces
{
    public interface IUlfEntityHandler
    {
        void Reset();
        void AddUlfEntity(double station, double ulf);
    }
}
