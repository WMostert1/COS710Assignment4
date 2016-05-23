using OptimisationAlgorithms;

namespace COS710Assignment4
{
    class Program
    {
        static void Main(string[] args)
        {
            OptimisationFunctionSet functions = new OptimisationFunctionSet();
            DifferentialEvolution algorithm = new DifferentialEvolution(6, 20, 2000);
            algorithm.Run(functions);
        }
    }
}
