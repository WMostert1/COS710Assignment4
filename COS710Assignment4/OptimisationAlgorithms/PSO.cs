using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimisationAlgorithms
{
    public class PSO : IOptimiser
    {
        int PopulationSize;
        int Dimensions;
        int MaxIterations;
        double W;
        double C1;
        double C2;
        Particle[] Population;

        public PSO(int dim, int pop, int gens)
        {
            PopulationSize = pop;
            Dimensions = dim;
            MaxIterations = gens;
        }

        public void SetUp()
        {
            W = 0.729844;
            C1 = C2 = 1.49618;
        }

        public IEnumerable<PerformanceIndicator> Run(OptimisationFunctionSet Functions)
        {
            Random r1 = new Random(System.DateTime.Now.Millisecond);
            Random r2 = new Random(System.DateTime.Now.Millisecond);
            SetUp();
            foreach (FunctionWrapper function in Functions.Functions)
            {
                //initialise swarm
                Population = new Particle[PopulationSize];
                double[] GlobalBest = new double[Dimensions];
                for (int i = 0; i < Dimensions; i++)
                {
                    GlobalBest[i] = function.UpperBound;
                }
                Random rand = new Random();
                for (int i = 0; i < PopulationSize; i++)
                {
                    Population[i] = new Particle()
                    {
                        Position = new double[Dimensions],
                        BestPosition = new double[Dimensions],
                        Velocity = new double[Dimensions],
                        Fitness = 0
                    };
                    //define Position
                    for (int j = 0; j < Dimensions; j++)
                    {
                        Population[i].Velocity[j] = 0;
                        Population[i].Position[j] = function.LowerBound + rand.NextDouble() * (function.UpperBound - function.LowerBound);
                        Population[i].BestPosition[j] = Population[i].Position[j];
                    }
                }
                //execute pso algorithm
                int iteration = 0;
                while (iteration <= MaxIterations)
                {
                    for (int i = 0; i < PopulationSize; i++)
                    {
                        //check BestPosition Position
                        if (function.Function(Population[i].Position) < function.Function(Population[i].BestPosition))
                        {
                            Array.Copy(Population[i].Position, Population[i].BestPosition, Dimensions);
                        }
                        //check gBest Position
                        if (function.Function(Population[i].BestPosition) < function.Function(GlobalBest))
                        {
                            Array.Copy(Population[i].BestPosition, GlobalBest, GlobalBest.Length);
                        }
                    }
                    //Velocity and Position updates
                    
                    for (int i = 0; i < PopulationSize; i++)
                    {
                        //Velocity
                        for (int j = 0; j < Dimensions; j++)
                        {
                            Population[i].Velocity[j] = W * Population[i].Velocity[j] + C1 * r1.NextDouble() * (Population[i].BestPosition[j] - Population[i].Position[j])
                                + C2 * r2.NextDouble() * (GlobalBest[j] - Population[i].Position[j]);
                        }
                        //Position
                        for (int j = 0; j < Dimensions; j++)
                        {
                            Population[i].Position[j] = Population[i].Position[j] + Population[i].Velocity[j];
                            if (Population[i].Position[j] < function.LowerBound)
                            {
                                Population[i].Position[j] = function.LowerBound;
                            }
                            else if (Population[i].Position[j] > function.UpperBound)
                            {
                                Population[i].Position[j] = function.UpperBound;
                            }
                        }
                    }
                    Console.WriteLine(iteration+". "+function.Function(GlobalBest));
                    /*!!!!!!!!!For diversity measure
                    if (iteration % 10 == 0)
                    {
                        //work out average of average distance
                        //every particle
                        double outerSum = 0;
                        for (int i = 0; i < PopulationSize; i++)
                        {
                            double innerSum = 0;
                            for (int j = 0; j < PopulationSize; j++)
                            {
                                double ksum = 0;
                                for (int k = 0; k < Dimensions; k++)
                                {
                                    ksum += Math.Pow(Population[i].Position[k] - Population[j].Position[k], 2);
                                }
                                innerSum += Math.Sqrt(ksum);
                            }
                            outerSum += 1.0 / PopulationSize * innerSum;
                        }
                        outerSum = 1.0 / PopulationSize * outerSum;
                    }!!!!!!!!!!!!*/
                    /*!!!!!!!!!!!!!!for efficiency
                    if (Math.Abs(function.optimumGoal - calculateFitness(gBest)) < 0.005 && !solFound)
                    {
                        solFound = true;
                        solutionIteration = iteration;
                    }!!!!!!!!!!!!!!!!!!*/
                    iteration++;
                }
            }
            //accuracy
            //double acc = Math.Abs(function.Function(gBest) - function.optimumGoal);
            Console.ReadLine();
            return new List<PerformanceIndicator>();
        }
            
        }

        class Particle
        {
            public double[] Velocity { get; set; }
            public double[] Position { get; set; }
            public double[] BestPosition { get; set; }
            public double Fitness { get; set; }
        }
    }
