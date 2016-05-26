using MathNet.Numerics.Distributions;
using System;
using System.Collections;
using System.Collections.Generic;

namespace OptimisationAlgorithms
{
    public class EvolutionaryProgram : IOptimiser
    {
        protected static double FITNESS_MARGIN = 0.000001;
        protected static int NUMBER_OF_RUNS = 30;
        public int PopulationSize { get; set; }
        public int Generations { get; set; }
        public int Dimensions { get; set; }
 

        protected static Random m_random = new Random();
        protected bool useStaticProbabilities;

        protected ArrayList m_thisGeneration;
        protected ArrayList m_nextGeneration;



        public EvolutionaryProgram( int populationSize, int generationSize, int dimensionSize)
        {

       
            PopulationSize = populationSize;
            Generations = generationSize;
            Dimensions = dimensionSize;
        
       
        }

        public void InitialValues()
        {

        }

        public IEnumerable<PerformanceIndicator> Run(OptimisationFunctionSet Functions)
        {
            List<PerformanceIndicator> performances = new List<PerformanceIndicator>();
            foreach (var func in Functions.Functions)
            {

                double accuracy = double.NaN;
                int averageEpochsToConverge = 0;
                double averageAccuracy = 0.0;

                for (int runNum = 0; runNum < NUMBER_OF_RUNS; runNum++)
                {
                    bool converged = false;

                    m_thisGeneration = new ArrayList(Generations);
                    

                    CreateGenomes(func);
                    

                  

                    for (int generationNum = 0; generationNum < Generations; generationNum++)
                    {
                        m_nextGeneration = new ArrayList(Generations);

                        if (!converged && Math.Abs(((Genome)m_thisGeneration[0]).Fitness - func.KnownGoal) < FITNESS_MARGIN)
                        {
                            converged = true;
                            averageEpochsToConverge += generationNum;
                            break;
                        }

                        RankPopulation(ref m_thisGeneration, func.Function);
                        CreateNextGeneration(ref m_thisGeneration, ref m_nextGeneration);
                        RankPopulation(ref m_nextGeneration, func.Function);
                        ApplyElitism(ref m_thisGeneration, ref m_nextGeneration);
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

                    //if (runNum % (NUMBER_OF_RUNS / 10) == 0)
                    //    Console.Write("..." + (((double)(runNum) / NUMBER_OF_RUNS * 100.0) + 10.0) + "%");
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

        public void ApplyElitism(ref ArrayList g1, ref ArrayList g2)
        {
            g1.AddRange(g2);
            g1.Sort(new GenomeComparer());
            g1.RemoveRange(g2.Count, g2.Count);
        }


        protected void RankPopulation(ref ArrayList generation, OptimisationFunctions.FitnessFunction Function)
        {

            for (int i = 0; i < PopulationSize; i++)
            {
                Genome g = ((Genome)generation[i]);
                g.Fitness = Function(g.Genes());
            }


            generation.Sort(new GenomeComparer());

        }
        protected void CreateGenomes(FunctionWrapper Function)
        {
            for (int i = 0; i < PopulationSize; i++)
            {
                Genome g = new Genome(Dimensions, Function.LowerBound, Function.UpperBound);
                m_thisGeneration.Add(g);
            }
        }

     

        virtual protected void CreateNextGeneration(ref ArrayList currentGeneration, ref ArrayList newGeneration)
        {
            newGeneration.Clear();
            for (int i = 0; i < PopulationSize; i++)
            {
                Genome parent = (Genome)currentGeneration[i];
                Genome child = parent.MutateEP();
                newGeneration.Add(child);
            }
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
