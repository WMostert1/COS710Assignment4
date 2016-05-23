using OptimisationAlgorithms;

namespace COS710Assignment4
{
    class Program
    {
        static void Main(string[] args)
        {
            OptimisationFunctionSet functions = new OptimisationFunctionSet();
            PSO algorithm = new PSO(2, 20, 2000);
            algorithm.Run(functions);
        }
    }
}
