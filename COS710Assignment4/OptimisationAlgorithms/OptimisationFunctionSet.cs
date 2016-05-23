using System.Collections.Generic;

namespace OptimisationAlgorithms
{
    public class OptimisationFunctionSet
    {
        public List<FunctionWrapper> Functions { get; set; }

        public OptimisationFunctionSet()
        {
            Functions = new List<FunctionWrapper>();

            Functions.Add(new FunctionWrapper()
            {
                Function = new OptimisationFunctions.FitnessFunction(OptimisationFunctions.Ackley),
                FunctionName = "Ackley's Function",
                LowerBound = -5,
                UpperBound = 5
            });
            //Add the other functions
        }
    }
}
