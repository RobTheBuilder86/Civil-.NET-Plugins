namespace Ulf.Util
{
    public enum CalculationCase
    {
        Case1,
        Case2,
        Case3a,
        Case3b,
        Case4a,
        Case4b,
        CaseNone
    }

    public class CaseAssignment
    {
        public CalculationCase CalcCase { get; }
        public double FromStation { get; }
        public double ToStation { get; }

        public CaseAssignment(double fromStation, double toStation, CalculationCase calcCase) =>
            (FromStation, ToStation, CalcCase) = (fromStation, toStation, calcCase);

        public override string ToString()
        {
            return $"From {FromStation} to {ToStation}: {CalcCase.ToString("g")}.";
        }
    }

}