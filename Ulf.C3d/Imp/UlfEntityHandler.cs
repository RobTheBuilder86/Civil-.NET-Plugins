using System;
using Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ulf.Engine.Interfaces;

namespace Ulf.C3D.Imp
{
    class UlfEntityHandler : IUlfEntityHandler
    {
        public void AddUlfEntity(double station, double ulf)
        {
        }

        public void Reset()
        {
            Active.DeleteAllEntitiesOnLayer("IAG_BWB_Min-Ulf");
        }
    }
}
