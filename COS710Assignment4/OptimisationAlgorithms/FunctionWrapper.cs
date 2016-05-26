namespace OptimisationAlgorithms
{
    public class FunctionWrapper    //Assumes minimisation
    {
        public OptimisationFunctions.FitnessFunction Function { get; set; }

        public string FunctionName { get; set; }

        public double LowerBound { get; set; }

        public double UpperBound { get; set; }

        public double KnownGoal { get; set; }
    }
}
