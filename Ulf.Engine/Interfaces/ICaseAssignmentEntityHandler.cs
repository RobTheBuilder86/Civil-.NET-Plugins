using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ulf.Util;

namespace Ulf.Engine.Interfaces
{
    public interface ICaseAssignmentEntityHandler
    {
        void Reset();
        void StartCaseLine(CaseStation cs);
        void AddTransition(CaseStation csTo);
        void EndCaseLine(CaseStation csTo);
    }
}
