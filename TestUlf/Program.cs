using Excel_Ulf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Ulf
{
    class Program
    {
        static void Main(string[] args)
        {
            try {
                CaseReader caseReader = new CaseReader(
                    "D:\\az\\as\\IAG\\IAG1003_NBS_W-U_22-23-24-25a1_SV\\" +
                    "PFA_23_Albhochflaeche\\Ing\\VE230-XY\\Dyn-Stab\\Plaene_Dyn-VB\\" +
                    "20200127_Dyn-GB_Fallunterscheidung.xlsx");
                foreach (CaseAssignment caseAssign in caseReader.CaseAssignments) {
                    Console.WriteLine(caseAssign);
                }
            } catch (System.Exception ex) {
                Console.WriteLine("Fehler!");
                Console.WriteLine(ex.Message);
            }
            Console.Write("Press Enter to continue...");
            Console.ReadLine();
        }
    }
}
