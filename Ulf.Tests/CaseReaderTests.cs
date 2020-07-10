using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ulf.Excel;
using Ulf.Util;
using Xunit;
namespace Ulf.Tests
{
    public class CaseReaderTests
    {
        [Fact]
        public void ReadCases_ReadsExcelFile_ReturnsCorrectCases()
        {
            CaseReader cr = new CaseReader("T:\\az\\as\\IAG\\IAG1003_NBS_W-U_22-23-24-25a1_SV\\PFA_23_Albhochflaeche\\Ing\\VE230-XY\\Dyn-Stab\\Plaene_Dyn-VB\\20200127_Dyn-GB_Fallunterscheidung.xlsx");

            List<CaseAssignment> ca =  cr.CaseAssignments;

            Assert.Equal(1, 2);
        }
    }
}
