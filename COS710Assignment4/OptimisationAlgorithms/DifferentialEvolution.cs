using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimisationAlgorithms
{
    public class DifferentialEvolution : IOptimiser
    {
        public double ScalingFactor { get; set; }
        public double CrossoverProbability { get; set; }
        List<Agent> Population { get; set; }
        int PopulationSize { get; set; }
        int Dimensions { get; set; }
        int MaxGenerations { get; set; }

        public DifferentialEvolution(int dim, int pop, int gens)
        {
            PopulationSize = pop;
            Dimensions = dim;
            MaxGenerations = gens;
        }

        public void SetUp()
        {
            ScalingFactor = 0.5;
            CrossoverProbability = 0.3;
            Population = new List<Agent>();
        }

        public IEnumerable<PerformanceIndicator> Run(OptimisationFunctionSet Functions)
        {
            foreach(FunctionWrapper function in Functions.Functions)
            {
                SetUp();
                //initialise population
                Random rand = new Random();
                for (int i = 0; i < PopulationSize; i++)
                {
                    Agent agent = new Agent()
                    {
                        Position = new double[Dimensions],
                        Fitness = 0
                    };
                    for (int j = 0; j < Dimensions; j++)
                    {
                        agent.Position[j] = function.LowerBound + rand.NextDouble() * (function.UpperBound - function.LowerBound);
                    }
                    Population.Add(agent);
                }
                int generation = 0;
                while (generation < MaxGenerations)
                {
                    List<Agent> newPopulation = new List<Agent>();
                    foreach(Agent parentAgent in Population)
                    {
                        //fitness
                        parentAgent.Fitness = function.Function(parentAgent.Position);
                        //trial vector (mutation)
                        Agent trialAgent = new Agent()
                        {
                            Position = new double[Dimensions],
                            Fitness = 0
                        };
                        List<Agent> targetAgents = new List<Agent>();
                        rand = new Random();
                        while (targetAgents.Count < 3)
                        {
                            Agent testAgent = Population.ElementAt(rand.Next(PopulationSize));
                            if (!testAgent.Equals(parentAgent) && !targetAgents.Contains(testAgent))
                            {
                                targetAgents.Add(testAgent);
                            }
                        }
                        Agent a, b, c;
                        a = targetAgents.ElementAt(0);
                        b = targetAgents.ElementAt(1);
                        c = targetAgents.ElementAt(2);
                        for (int i = 0; i < Dimensions; i++)
                        {
                            trialAgent.Position[i] = a.Position[i] + ScalingFactor * (b.Position[i] - c.Position[i]);
                            if (!(trialAgent.Position[i] > function.LowerBound && trialAgent.Position[i] < function.UpperBound)){
                                trialAgent.Position[i] = function.LowerBound + rand.NextDouble() * (function.UpperBound - function.LowerBound);
                            }
                        }
                        //crossover
                        rand = new Random();
                        Agent offspringAgent= new Agent()
                        {
                            Position = new double[Dimensions],
                            Fitness = 0
                        };
                        int crossoverDefault = rand.Next(Dimensions);
                        offspringAgent.Position[crossoverDefault] = trialAgent.Position[crossoverDefault];
                        for (int i = 0; i < Dimensions; i++)
                        {
                            if (i != crossoverDefault)
                            {
                                if (rand.NextDouble() < CrossoverProbability)
                                {
                                    offspringAgent.Position[i] = trialAgent.Position[i];
                                }
                                else
                                {
                                    offspringAgent.Position[i] = parentAgent.Position[i];
                                }
                            }
                        }
                        offspringAgent.Fitness = function.Function(offspringAgent.Position);
                        //offspring vs parent test
                        if (offspringAgent.Fitness < parentAgent.Fitness)
                        {
                            newPopulation.Add(offspringAgent);
                        } else
                        {
                            newPopulation.Add(parentAgent);
                        }
                    }
                    Population = newPopulation;
                    generation++;
                    double avg = 0;
                    foreach(Agent agent in Population)
                    {
                        avg += agent.Fitness;
                    }
                    avg /= PopulationSize;
                    Console.WriteLine(avg);
                }
            }
            Console.ReadLine();
            return new List<PerformanceIndicator>();
        }

        public void setPopulationSize(int size)
        {
            this.PopulationSize = size;
        }

        private class Agent
        {
            public double[] Position { get; set; }
            public double Fitness { get; set; }
        }
    }
}
