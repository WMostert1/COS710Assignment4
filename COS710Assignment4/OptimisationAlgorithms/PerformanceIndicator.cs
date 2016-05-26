namespace OptimisationAlgorithms
{
    public class PerformanceIndicator
    {
        public double Accuracy { get; set; }

        public int PopulationNumber { get; set; }

        public string AlgorithmName { get; set; }

        public FunctionWrapper Function { get; set; }

        public int Iterations { get; set; }

        public int DimensionNumber { get; set; }
    }
}
