using System;
using System.Linq;
using System.Reflection;

namespace ClassicalCipherSolver.Ciphers
{
    public abstract class Cipher<T> : IScoreable
    {
        public float[] Means
        { get; protected set; }

        public float[] SampleVariances
        { get; protected set; }

        public float[,] DistancesFromMeans
        { get; protected set; }

        public string[] EnglishTexts
        { get; protected set; } = { Properties.Resources.Gutenberg0, Properties.Resources.Gutenberg1, Properties.Resources.Gutenberg2, Properties.Resources.Gutenberg3, Properties.Resources.Gutenberg4 };

        public float CandidateStandardDeviationsFromSampleMean
        { get; set; }

        public abstract string Encrypt(string plaintext, T key);

        public abstract Plaintext DecryptAutomatically(string ciphertext, FitnessChecker fitnessChecker);

        public abstract string Decrypt(string ciphertext, T key);

        protected static int Modulo(int a, int b)
        {
            int result = a % b;
            if (result < 0) result += b;

            return result;
        }

        protected virtual void SetStats(T key)
        {
            Random random = new();

            int numberOfStatsTracked = new Ciphertext("").GetType().GetProperties().Where(x => x.PropertyType == typeof(float)).ToArray().Length;
            Means = new float[numberOfStatsTracked];
            SampleVariances = new float[numberOfStatsTracked];
            DistancesFromMeans = new float[numberOfStatsTracked, EnglishTexts.Length];

            Ciphertext[] results = new Ciphertext[EnglishTexts.Length];

            for (int i = 0; i < EnglishTexts.Length; ++i)
            {
                results[i] = new Ciphertext(Encrypt(EnglishTexts[i].ToUpper(), key));

                PropertyInfo[] properties = results[i].GetType().GetProperties().Where(x => x.PropertyType == typeof(float)).ToArray();
                for (int j = 0; j < properties.Length; ++j)
                {
                    Means[j] += Convert.ToSingle(properties[j].GetValue(results[i]));
                }
            }
            for (int i = 0; i < Means.Length; ++i)
            {
                Means[i] /= EnglishTexts.Length;
            }

            for (int i = 0; i < EnglishTexts.Length; ++i)
            {
                PropertyInfo[] properties = results[i].GetType().GetProperties().Where(x => x.PropertyType == typeof(float)).ToArray();
                for (int j = 0; j < properties.Length; ++j)
                {
                    float observation = Convert.ToSingle(properties[j].GetValue(results[i]));
                    DistancesFromMeans[j, i] = observation - Means[j];
                    SampleVariances[j] += (float)Math.Pow(observation - Means[j], 2);
                }
            }
            for (int i = 0; i < SampleVariances.Length; ++i)
            {
                SampleVariances[i] /= (EnglishTexts.Length - 1);
            }
        }
    }
}