using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimisationAlgorithms
{
    public class HillClimber
    {
        public int NeighbourhoodSize { get; set; }

        public int GuessedPopSize { get; set; }

        public int Dimension { get; set; }

        public OptimisationFunctionSet Functions { get; set; }

        public IOptimiser Optimiser { get; set; }

        public HillClimber(int NeighbourHoodSize, int gPopSize, int Dim, OptimisationFunctionSet func, IOptimiser optimiser)
        {
            NeighbourhoodSize = NeighbourHoodSize;
            GuessedPopSize = gPopSize;
            Dimension = Dim;
            Functions = func;
            Optimiser = optimiser;
        }

        public int StochasticHillClimbing()
        {
            Dictionary<FunctionWrapper, int> populationsToUse = new Dictionary<FunctionWrapper, int>();
 
            foreach (var functionWrapper in Functions.Functions)
            {
                Dictionary<int, double> neighbourhood = new Dictionary<int, double>();

                for (int i = GuessedPopSize - NeighbourhoodSize / 2 > 0 ? GuessedPopSize - NeighbourhoodSize / 2 : 1; i <= GuessedPopSize + NeighbourhoodSize / 2.0; i++)
                {
                    neighbourhood.Add(i, double.NaN);
                }


                KeyValuePair<int, double> best = new KeyValuePair<int, double>(-1,0.0);
                bool solutionFound = false;
                while (!solutionFound)
                {
                    List<KeyValuePair<int, double>> betterNeighbours = new List<KeyValuePair<int, double>>();
                    foreach (int size in neighbourhood.Keys)
                    {
                    
                    
                        Optimiser.setPopulationSize(size);
                        List<FunctionWrapper> funcList = new List<FunctionWrapper>();
                        funcList.Add(functionWrapper);

                        var res = Optimiser.Run(new OptimisationFunctionSet() { Functions = funcList });
                        PerformanceIndicator indic = res.ElementAt(0);
                        if (best.Key == -1)
                            best = new KeyValuePair<int, double>(size, indic.Accuracy);

                        if (functionWrapper.KnownGoal - indic.Accuracy < functionWrapper.KnownGoal - best.Value)
                        {
                            betterNeighbours.Add(new KeyValuePair<int, double>(size, indic.Accuracy));
                        }                      
                    }
                    if (betterNeighbours.Count == 0)
                        solutionFound = true;
                    else
                    {
                        var chosen = new KeyValuePair<int, double>();
                        Random r = new Random(DateTime.Now.Millisecond);
                        chosen = betterNeighbours.ElementAt(r.Next(betterNeighbours.Count));
                        best = chosen;
                    }
                }

                populationsToUse.Add(functionWrapper, best.Key);

            }
            int popSizeToUse = 0;
            foreach(var fPop in populationsToUse.Values)
                popSizeToUse += fPop;

            return (int)(popSizeToUse / (double)populationsToUse.Count);
            
        }

    }
}
