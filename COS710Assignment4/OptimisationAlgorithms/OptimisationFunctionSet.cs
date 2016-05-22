using System.Collections.Generic;

namespace OptimisationAlgorithms
{
    public class OptimisationFunctionSet
    {
        public List<FunctionWrapper> Functions { get; set; }

        public OptimisationFunctionSet()
        {
            Functions.Add(new FunctionWrapper()
            {
                Function = new OptimisationFunctions.FitnessFunction(OptimisationFunctions.Ackley),
                FunctionName = "Ackley's Function"
            });
            //Add the other functions
        }
    }
}
