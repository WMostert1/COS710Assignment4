using System.Collections.Generic;

namespace OptimisationAlgorithms
{
    public class OptimisationFunctionSet
    {
        public OptimisationFunctionSet(FunctionWrapper [] functions)
        {
            foreach(var f in functions)
             Functions.Add(f);
        }

        public List<FunctionWrapper> Functions { get; set; }

        public OptimisationFunctionSet()
        {
            Functions = new List<FunctionWrapper>();

            Functions.Add(new FunctionWrapper()
            {
                Function = new OptimisationFunctions.FitnessFunction(OptimisationFunctions.Ackley),
                FunctionName = "Ackley's Function",
                LowerBound = -5.0,
                UpperBound = 5.0,
                KnownGoal = 0.0
            });

            Functions.Add(new FunctionWrapper()
            {
                Function = new OptimisationFunctions.FitnessFunction(OptimisationFunctions.EggHolderFunction),
                FunctionName = "Egg holder",
                LowerBound = -512.0,
                UpperBound = 512.0,
                KnownGoal = -959.6407
            });
            //Add the other functions
        }
    }
}
