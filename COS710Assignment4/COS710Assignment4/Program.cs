using OptimisationAlgorithms;
using System;
using System.Collections.Generic;

namespace COS710Assignment4
{
    class Program
    {
        const int START_DIM = 1;
        const int END_DIM = 10;
        static void Main(string[] args)
        {
            OptimisationFunctionSet functions = new OptimisationFunctionSet();
            //PSO algorithm = new PSO(2, 20, 2000);
            //algorithm.SetUp();
            //algorithm.Run(functions);



            var ggaDimensionPerformances = new Dictionary<int, IEnumerable<PerformanceIndicator>>();
            //for (int i = START_DIM; i <= END_DIM; i++)
            //{
            //    GGA gga = new GGA(0.85, 100, 600, 2, 20);
            //    int optimumPopSizes = new HillClimber(5,20,2,functions, gga).StochasticHillClimbing();
            //    gga.setPopulationSize(optimumPopSizes);
            //    ggaDimensionPerformances.Add(i, gga.Run(functions));

            //}

            IOptimiser Ep = new EvolutionaryProgram(30, 2000, 2);
            var res = Ep.Run(functions);
            
          
             
            Console.ReadLine();
        }
    }
}
