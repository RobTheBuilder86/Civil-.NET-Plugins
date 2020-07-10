namespace Ulf.Util
{
    public static class CaseToYConverter
    {
        private static readonly double YDatabandInsert = 5350000;
        private static readonly double DeltaYCase1 = -316;
        private static readonly double DeltaYCase2 = -326;
        private static readonly double DeltaYCase3a = -336;
        private static readonly double DeltaYCase4a = -346;
        private static readonly double DeltaYCase3b = -359;
        private static readonly double DeltaYCase4b = -369;

        public static double ToY(CalculationCase CalcCase)
        {
            switch (CalcCase) {
                case CalculationCase.Case1:
                    return YDatabandInsert + DeltaYCase1;
                case CalculationCase.Case2:
                    return YDatabandInsert + DeltaYCase2;
                case CalculationCase.Case3a:
                    return YDatabandInsert + DeltaYCase3a;
                case CalculationCase.Case3b:
                    return YDatabandInsert + DeltaYCase3b;
                case CalculationCase.Case4a:
                    return YDatabandInsert + DeltaYCase4a;
                case CalculationCase.Case4b:
                    return YDatabandInsert + DeltaYCase4b;
                default:
                    return 0;
            }
        }
    }
}