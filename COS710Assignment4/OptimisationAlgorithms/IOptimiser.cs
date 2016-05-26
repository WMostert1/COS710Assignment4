using System.Collections.Generic;

namespace OptimisationAlgorithms
{
    public interface IOptimiser
    {
        void setPopulationSize(int size);

        IEnumerable<PerformanceIndicator> Run(OptimisationFunctionSet Functions);


    }
}
