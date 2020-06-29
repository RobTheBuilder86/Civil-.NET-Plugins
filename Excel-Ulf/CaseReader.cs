using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excel_Ulf
{
    public class CaseReader
    {
        private const int rowStartData = 5;
        private const int colStationFrom = 2;
        private const int colStationTo = 3;
        private const int colCase = 4;

        private readonly IXLWorkbook _workbook;
        private readonly IXLWorksheet _worksheet;
        private IXLRow _currentDataRow;

        private List<CaseAssignment> _cases;

        public CaseReader(string excelFilePath)
        {
            _workbook = new XLWorkbook(excelFilePath);
            _worksheet = _workbook.Worksheet("Fallzuordnung");
            _currentDataRow = _worksheet.Row(rowStartData);
        }

        public List<CaseAssignment> CaseAssignments
        {
            get {
                if (_cases == null) {
                    ReadCases();
                }
                return _cases;
            }
        }

        private void ReadCases()
        {
            _cases = new List<CaseAssignment>();
            while (!_currentDataRow.IsEmpty()) {
                try {
                    CaseAssignment caseAssign = ReadNextCase();
                    _cases.Add(caseAssign);
                } catch (System.Exception ex){
                    Console.WriteLine(
                        $"Error while reading row {_currentDataRow.RowNumber()}:" + 
                        ex.Message);
                }
                _currentDataRow = _currentDataRow.RowBelow();
            }
        }

        private CaseAssignment ReadNextCase()
        {
            double startStation = _currentDataRow.Cell(colStationFrom).GetDouble();
            CalculationCase calcCase = GetCaseFromRow(_currentDataRow);
            IXLRow rowBelow = _currentDataRow.RowBelow();
            while(calcCase == GetCaseFromRow(rowBelow)) {
                _currentDataRow = _currentDataRow.RowBelow();
                rowBelow = _currentDataRow.RowBelow();
            }
            double endStation = _currentDataRow.Cell(colStationTo).GetDouble();
            return new CaseAssignment(startStation, endStation, calcCase);
        }

        private CalculationCase GetCaseFromRow(IXLRow row)
        {
            string caseString = row.Cell(colCase).GetString();
            switch (caseString) {
                case "1":
                    return CalculationCase.Case1;
                case "2":
                    return CalculationCase.Case2;
                case "3a":
                    return CalculationCase.Case3a;
                case "3b":
                    return CalculationCase.Case3b;
                case "4a":
                    return CalculationCase.Case4a;
                case "4b":
                    return CalculationCase.Case4b;
                default:
                    return CalculationCase.CaseNone;
            }
        }

    }
}
