using System;
using System.Collections;
using System.Collections.Generic;

namespace OptimisationAlgorithms
{
    public class GGA : IOptimiser
    {

            protected static double FITNESS_MARGIN = 0.005; 
            protected static int NUMBER_OF_RUNS = 30;
            public int PopulationSize { get; set; }
            public int Generations { get; set; }
            public int Dimensions { get; set; }
            public double CrossoverRate { get; set; }
            public double MutationRate { get; set; }
            public int TournamentSize { get; set; }
            public bool Elitism { get; set; }

            protected static Random m_random = new Random();
            protected bool useStaticProbabilities;

            protected ArrayList m_thisGeneration;
            protected ArrayList m_nextGeneration;
    

            public GGA(double crossoverRate, double mutationRate, int populationSize, int generationSize, int dimensionSize, int tournamentSize)
            {
             
                Elitism = false;
                MutationRate = mutationRate;
                CrossoverRate = crossoverRate;
                PopulationSize = populationSize;
                Generations = generationSize;
                Dimensions = dimensionSize;
                TournamentSize = tournamentSize;
                useStaticProbabilities = true;           
            }

            public GGA( double crossoverRate, int populationSize, int generationSize, int dimensionSize, int tournamentSize)
            {
              
                Elitism = false;
                CrossoverRate = crossoverRate;
                PopulationSize = populationSize;
                Generations = generationSize;
                Dimensions = dimensionSize;
                TournamentSize = tournamentSize;
                useStaticProbabilities = false;
            }

            public void InitialValues()
            {
               
            }

            public IEnumerable<PerformanceIndicator> Run(OptimisationFunctionSet Functions)
            {
            List<PerformanceIndicator> performances = new List<PerformanceIndicator>();
                foreach(var func in Functions.Functions) {

                double accuracy = double.NaN;
                int averageEpochsToConverge = 0;
                double averageAccuracy = 0.0;

                for (int runNum = 0; runNum < NUMBER_OF_RUNS; runNum++)
                {
                    bool converged = false;

                    m_thisGeneration = new ArrayList(Generations);
                    m_nextGeneration = new ArrayList(Generations);

                    CreateGenomes(func);
                    RankPopulation(func.Function);

                    if (useStaticProbabilities)
                    {
                        Genome.MutationRate = MutationRate;

                    }

                    for (int generationNum = 0; generationNum < Generations; generationNum++)
                    {
                        
                        if (!converged && Math.Abs(((Genome)m_thisGeneration[0]).Fitness - func.KnownGoal) < FITNESS_MARGIN)
                        {
                            converged = true;
                            averageEpochsToConverge += generationNum;
                        }



                        if (!useStaticProbabilities)
                            Genome.MutationRate = (1.0 / 240.0) + (0.11375 / Math.Pow(2.0, generationNum));

                        CreateNextGeneration();
                        RankPopulation(func.Function);
                    }

                    if (!converged)
                    {
                        averageEpochsToConverge += Generations;
                    }

                    if (double.IsNaN(accuracy))
                        accuracy = ((Genome)m_thisGeneration[0]).Fitness;


                    averageAccuracy += ((Genome)m_thisGeneration[0]).Fitness;

                    if (((Genome)m_thisGeneration[0]).Fitness < accuracy)
                        accuracy = ((Genome)m_thisGeneration[0]).Fitness;

                    if (runNum % (NUMBER_OF_RUNS / 10) == 0)
                        Console.Write("..." + (((double)(runNum) / NUMBER_OF_RUNS * 100.0) + 10.0) + "%");
                }
                
                averageEpochsToConverge /= NUMBER_OF_RUNS;
                averageAccuracy /= NUMBER_OF_RUNS;
                

                performances.Add(new PerformanceIndicator()
                {
                    Accuracy = accuracy,
                    Function = func,
                    AlgorithmName = "GGA",
                    PopulationNumber = PopulationSize,
                    Iterations = averageEpochsToConverge
                });
           
            }

            return performances;
            }

            protected Genome TournamentSelection()
            {
                ArrayList tournament = new ArrayList();
                for (int i = 0; i < TournamentSize; i++)
                {
                    int id = (int)(m_random.NextDouble() * PopulationSize);
                    tournament.Add((Genome)m_thisGeneration[id]);
                }

                tournament.Sort(new GenomeComparer());
                return (Genome)tournament[0];
            }

        protected void RankPopulation(OptimisationFunctions.FitnessFunction Function)
        {

            for (int i = 0; i < PopulationSize; i++)
            {
                Genome g = ((Genome)m_thisGeneration[i]);
                g.Fitness = Function(g.Genes());
            }


            m_thisGeneration.Sort(new GenomeComparer());

        }
            protected void CreateGenomes(FunctionWrapper Function)
            {
                for (int i = 0; i < PopulationSize; i++)
                {
                    Genome g = new Genome(Dimensions, Function.LowerBound, Function.UpperBound);
                    m_thisGeneration.Add(g);
                }
            }

            virtual protected void CreateNextGeneration()
            {
                m_nextGeneration.Clear();
                Genome g = null;
                if (Elitism)
                    g = (Genome)m_thisGeneration[0];

                for (int i = 0; i < PopulationSize; i++)
                {
                    Genome parent1, parent2, child1;
                    parent1 = TournamentSelection();
                    parent2 = TournamentSelection();

                    if (m_random.NextDouble() < CrossoverRate)
                    {
                        parent1.Crossover(ref parent2, out child1);
                    }
                    else
                    {
                        child1 = parent1;

                    }
                    child1.Mutate((Genome)m_thisGeneration[0]);


                    m_nextGeneration.Add(child1);
                }
                if (Elitism && g != null)
                    m_nextGeneration[0] = g;

                m_thisGeneration.Clear();
                for (int i = 0; i < PopulationSize; i++)
                    m_thisGeneration.Add(m_nextGeneration[i]);
            }

            public void GetBest(out double[] values, out double fitness)
            {
                Genome g = ((Genome)m_thisGeneration[0]);
                values = new double[g.Length];
                g.GetValues(ref values);
                fitness = g.Fitness;
            }

            public void GetWorst(out double[] values, out double fitness)
            {
                GetNthGenome(PopulationSize - 1, out values, out fitness);
            }

            public void GetNthGenome(int n, out double[] values, out double fitness)
            {
                if (n < 0 || n > PopulationSize - 1)
                    throw new ArgumentOutOfRangeException("n too large, or too small");
                Genome g = ((Genome)m_thisGeneration[n]);
                values = new double[g.Length];
                g.GetValues(ref values);
                fitness = g.Fitness;
            }

        public void setPopulationSize(int size)
        {
            PopulationSize = size;
        }
    }
    
}
