using ClassicalCipherSolver.Ciphers;
using System;
using System.Linq;
using System.Reflection;

namespace ClassicalCipherSolver
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            FitnessChecker fitnessChecker = new(4, Properties.Resources.Quadgrams);

            Console.WriteLine("Ciphertext (CTRL+Z, Return to stop input): ");

            string input = string.Empty;

            string line;
            while ((line = Console.ReadLine()) != null)
            {
                if (input != string.Empty) input += Environment.NewLine;
                input += line;
            }

            Ciphertext ciphertext = new(input);

            Console.Clear();

            PropertyInfo[] properties = ciphertext.GetType().GetProperties().Where(x => x.PropertyType == typeof(float)).ToArray();
            float[] stats = new float[properties.Length];
            for (int i = 0; i < properties.Length; ++i)
            {
                stats[i] = (float)properties[i].GetValue(ciphertext);
                Console.WriteLine($"{properties[i].Name}: {stats[i]}");
            }
            Console.WriteLine();

            Type[] ciphers = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsClass && !x.IsAbstract && !x.IsInterface && typeof(IScoreable).IsAssignableFrom(x)).ToArray();

            foreach (Type cipherType in ciphers)
            {
                IScoreable cipher = (IScoreable)Activator.CreateInstance(cipherType);

                float differenceFromMean = 0f;
                float sampleVariance = 0f;

                for (int i = 0; i < stats.Length; ++i)
                {
                    differenceFromMean += stats[i] - cipher.Means[i];
                    sampleVariance += cipher.SampleVariances[i];
                }

                float standardDeviationsFromTheMean = Math.Abs(differenceFromMean / (float)Math.Sqrt(sampleVariance));

                Console.WriteLine($"{cipherType.Name} - {standardDeviationsFromTheMean}");

                Console.WriteLine(cipher.DecryptAutomatically(ciphertext.Text, fitnessChecker).Text);
            }
        }
    }
}