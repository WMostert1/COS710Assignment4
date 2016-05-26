using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimisationAlgorithms
{
    public class Genome
    {
        public double[] m_genes;

        public double[] strat_params;

        static Random m_random = new Random();
        static Normal normalDist = new Normal(0, 1);
        public double Fitness { get; set; }

        public static double MutationRate { get; set; }

        public double MinX { get; set; }
        public double MaxX { get; set; }
        public int Length { get; set; }

        public Genome(Genome other)
        {
            Length = other.Length;
            MaxX = other.MaxX;
            MinX = other.MinX;
            strat_params = new double[Length];
            m_genes = new double[Length];
            for(int i = 0; i < Length; i++)
            {
                strat_params[i] = other.strat_params[i];
                m_genes[i] = other.m_genes[i];
            }
        }

        public Genome(int length, double minX, double maxX)
        {
            MaxX = maxX;
            MinX = minX;
            Length = length;
            m_genes = new double[length];
            strat_params = new double[length];
            CreateGenes();
           
        }
        public Genome(int length, bool createGenes, double minX, double maxX)
        {
            MaxX = maxX;
            MinX = minX;
            Length = length;
            m_genes = new double[length];
            strat_params = new double[length];
            if (createGenes)
                CreateGenes();
          
        }

        public Genome(ref double[] genes)
        {
            Length = genes.GetLength(0);
            m_genes = new double[Length];
            for (int i = 0; i < Length; i++)
                m_genes[i] = genes[i];
        }


        private void CreateGenes()
        {
            // DateTime d = DateTime.UtcNow;
            for (int i = 0; i < Length; i++)
                m_genes[i] = MinX + m_random.NextDouble() * (MaxX - MinX);
        }

        public void Crossover(ref Genome genome2, out Genome child1)
        {
            //Use Arithmetic Crossover
            child1 = new Genome(Length, false, MinX, MaxX);
            double[] newGenes = new double[Length];

            for (int i = 0; i < Length; i++)
            {
                newGenes[i] = (m_genes[i] + genome2.m_genes[i]) / 2.0;
            }

            child1.m_genes = newGenes;
        }

        //public void Crossover(ref Genome genome2, out Genome child1, out Genome child2)
        //{
        //          //int pos = (int)(m_random.NextDouble() * (double)Length);
        //          //child1 = new Genome(Length, false);
        //          //child2 = new Genome(Length, false);
        //          //for(int i = 0 ; i < Length ; i++)
        //          //{
        //          //	if (i < pos)
        //          //	{
        //          //		child1.m_genes[i] = m_genes[i];
        //          //		child2.m_genes[i] = genome2.m_genes[i];
        //          //	}
        //          //	else
        //          //	{
        //          //		child1.m_genes[i] = genome2.m_genes[i];
        //          //		child2.m_genes[i] = m_genes[i];
        //          //	}
        //          //}
        //      }


        public void Mutate(Genome bestSolution)
        {
            //Wong and Yuryevich [916] proposed a uniform mutation operator where

            for (int pos = 0; pos < Length; pos++)
            {

                if (m_random.NextDouble() < MutationRate)
                    m_genes[pos] = m_random.NextDouble() * (bestSolution.m_genes[pos] - m_genes[pos]);
                //	m_genes[pos] = (m_genes[pos] + m_random.NextDouble())/2.0;
            }
        }

        public Genome MutateEP()
        {
            Genome clone = new Genome(this);
            double Ni = normalDist.Sample();
            
            for (int pos = 0; pos < Length; pos++)
            {   
                double Nij = normalDist.Sample();
                double rDash = 1.0 / Math.Sqrt(2.0 * Math.Sqrt(Length));
                double r = 1.0 / Math.Sqrt(2.0 * Length);
                double logNorm = clone.strat_params[pos] * Math.Pow(Math.E, r * Ni + rDash * Nij);
                clone.strat_params[pos] = clone.strat_params[pos] * logNorm;
                clone.m_genes[pos] = clone.m_genes[pos] + clone.strat_params[pos] * normalDist.Sample();
            }

            return clone;
        }


        public double[] Genes()
        {
            return m_genes;
        }

        public void Output()
        {
            for (int i = 0; i < Length; i++)
            {
                System.Console.WriteLine("{0:F4}", m_genes[i]);
            }
            System.Console.Write("\n");
        }

        public void GetValues(ref double[] values)
        {
            for (int i = 0; i < Length; i++)
                values[i] = m_genes[i];
        }
    }
}
