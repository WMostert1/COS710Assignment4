using System;

namespace OptimisationAlgorithms
{
    public class OptimisationFunctions
    {
        public delegate double FitnessFunction(double[] values);

        public static double GoldsteinPrinceFunction(double[] X)
        {
            return (1 + Math.Pow((X[0] + X[1] + 1), 2) * (19 - 14 * X[0] + 3 * X[0] * X[0] - 14 * X[1] + 6 * X[0] * X[1] + 3 * X[1] * X[1])) * (30 + Math.Pow((2 * X[0] - 3 * X[1]), 2) * (18 - 32 * X[0] + 12 * X[0] * X[0] + 48 * X[1] - 36 * X[0] * X[1] + 27 * X[1] * X[1]));
        }

        public static double EggHolderFunction(double[] X)
        {
            return -(X[1] + 47) * Math.Sin(Math.Sqrt(Math.Abs(X[0] / 2.0 + (X[1] + 47)))) - X[0] * Math.Sin(Math.Sqrt(Math.Abs(X[0] - (X[1] + 47))));
        }

        public static double Ackley(double[] X)
        {
            return -20.0 * Math.Exp(-0.2 * Math.Sqrt(0.5 * (X[0] * X[0] + X[1] * X[1]))) - Math.Exp(0.5 * (Math.Cos(2.0 * Math.PI * X[0]) + Math.Cos(2.0 * Math.PI * X[1]))) + Math.E + 20.0;
        }

        public static double SphereFunction(double[] X)
        {
            double sum = 0.0;
            foreach (double d in X)
                sum += d * d;
            return sum;
        }

        public static double SchafferFunction(double[] X)
        {
            return 0.5 + (Math.Pow(Math.Sin(Math.Abs(X[0] * X[0] - X[1] * X[1])), 2.0) - 0.5) / Math.Pow(1.0 + 0.001 * (X[0] * X[0] + X[1] * X[1]), 2.0);
        }

        public static double LevisFunction(double[] X)
        {
            return Math.Pow(Math.Sin(3.0 * Math.PI * X[0]), 2.0) + Math.Pow(X[0] - 1.0, 2.0) * (1.0 + Math.Pow(Math.Sin(3.0 * Math.PI * X[1]), 2.0)) + Math.Pow(X[1] - 1.0, 2.0) * (1.0 + Math.Pow(Math.Sin(2.0 * Math.PI * X[1]), 2.0));
        }
    }
}
