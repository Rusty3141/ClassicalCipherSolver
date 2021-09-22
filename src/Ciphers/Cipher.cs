using System;
using System.Linq;
using System.Reflection;

namespace ClassicalCipherSolver.Ciphers
{
    internal abstract class Cipher<T> : IScoreable
    {
        public float CandidateMatch
        { get; protected set; }

        public float[] Means
        { get; protected set; }

        public float[] SampleVariances
        { get; protected set; }

        public abstract string Encrypt(string plaintext, T key);

        public abstract Plaintext DecryptAutomatically(string ciphertext, FitnessChecker fitnessChecker);

        public abstract string Decrypt(string ciphertext, T key);

        protected string[] englishTexts = { Properties.Resources.Gutenberg0, Properties.Resources.Gutenberg1, Properties.Resources.Gutenberg2, Properties.Resources.Gutenberg3, Properties.Resources.Gutenberg4 };

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

            foreach (string text in englishTexts)
            {
                Ciphertext result = new(Encrypt(text, key));

                PropertyInfo[] properties = result.GetType().GetProperties().Where(x => x.PropertyType == typeof(float)).ToArray();
                for (int i = 0; i < properties.Length; ++i)
                {
                    Means[i] += Convert.ToSingle(properties[i].GetValue(result));
                }
            }
            for (int i = 0; i < Means.Length; ++i)
            {
                Means[i] /= englishTexts.Length;
            }

            foreach (string text in englishTexts)
            {
                Ciphertext result = new(Encrypt(text, key));

                PropertyInfo[] properties = result.GetType().GetProperties().Where(x => x.PropertyType == typeof(float)).ToArray();
                for (int i = 0; i < properties.Length; ++i)
                {
                    SampleVariances[i] += (float)Math.Pow(Convert.ToSingle(properties[i].GetValue(result)) - Means[i], 2);
                }
            }
            for (int i = 0; i < SampleVariances.Length; ++i)
            {
                SampleVariances[i] /= (englishTexts.Length - 1);
            }
        }
    }
}