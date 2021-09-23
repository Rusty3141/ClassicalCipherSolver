using ClassicalCipherSolver.Ciphers;
using System;
using System.Collections.Generic;
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

            Type[] cipherTypes = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsClass && !x.IsAbstract && !x.IsInterface && typeof(IScoreable).IsAssignableFrom(x)).ToArray();

            IScoreable[] ciphers = new IScoreable[cipherTypes.Length];

            for (int i = 0; i < cipherTypes.Length; ++i)
            {
                IScoreable cipher = (IScoreable)Activator.CreateInstance(cipherTypes[i]);
                ciphers[i] = cipher;

                float differenceFromMean = 0f;
                float sampleVariance = 0f;

                for (int j = 0; j < stats.Length; ++j)
                {
                    differenceFromMean += stats[j] - cipher.Means[j];
                    sampleVariance += cipher.SampleVariances[j];
                }

                int[] statIndices = Enumerable.Range(0, stats.Length).ToArray();

                foreach (IEnumerable<int> pair in GetKCombinations<int>(statIndices, 2))
                {
                    for (int observation = 0; observation < cipher.EnglishTexts.Length; ++observation)
                    {
                        float covarianceAddition = 1f;

                        foreach (int choice in pair)
                        {
                            covarianceAddition *= cipher.DistancesFromMeans[choice, observation];
                        }

                        sampleVariance += covarianceAddition / (cipher.EnglishTexts.Length - 1);
                    }
                }

                cipher.CandidateStandardDeviationsFromSampleMean = Math.Abs(differenceFromMean / (float)Math.Sqrt(sampleVariance));

                static IEnumerable<IEnumerable<T>> GetKCombinations<T>(IEnumerable<T> list, int length) where T : IComparable
                {
                    if (length == 1) return list.Select(t => new T[] { t });
                    return GetKCombinations(list, length - 1).SelectMany(t => list.Where(o => o.CompareTo(t.Last()) > 0), (t1, t2) => t1.Concat(new T[] { t2 }));
                }
            }

            Array.Sort(ciphers, delegate (IScoreable a, IScoreable b) { return a.CandidateStandardDeviationsFromSampleMean.CompareTo(b.CandidateStandardDeviationsFromSampleMean); });

            for (int i = 0; i < ciphers.Length; ++i)
            {
                Console.WriteLine();
                Console.WriteLine($"{ciphers[i].GetType().Name} - {ciphers[i].CandidateStandardDeviationsFromSampleMean} standard deviations from the mean ({Math.Round(100 - 100 * ZScoreToConfidence(ciphers[i].CandidateStandardDeviationsFromSampleMean), 1)}% confidence in cipher).");

                Console.WriteLine(ciphers[i].DecryptAutomatically(ciphertext.Text, fitnessChecker).Text);
            }

            static float ZScoreToConfidence(float zScore)
            {
                static float Phi(float z)
                {
                    // Via. https://www.johndcook.com/blog/csharp_phi/

                    double a1 = 0.254829592;
                    double a2 = -0.284496736;
                    double a3 = 1.421413741;
                    double a4 = -1.453152027;
                    double a5 = 1.061405429;
                    double p = 0.3275911;

                    int sign = z < 0 ? -1 : 1;
                    z = (float)Math.Abs(z) / (float)Math.Sqrt(2.0);

                    double t = 1.0 / (1.0 + p * z);
                    double y = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.Exp(-z * z);

                    return 0.5f * (float)(1.0 + sign * y);
                }

                return (2 * Phi(zScore) - 1);
            }
        }
    }
}