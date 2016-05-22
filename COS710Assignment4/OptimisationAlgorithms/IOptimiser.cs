using System.Collections.Generic;

namespace OptimisationAlgorithms
{
    public interface IOptimiser
    {
        void SetUp(); //Set's the parameter's for the specific algorithm

        IEnumerable<PerformanceIndicator> Run(OptimisationFunctionSet Functions);
    }
}
