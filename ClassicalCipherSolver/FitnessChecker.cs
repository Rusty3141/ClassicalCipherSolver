using System;
using System.Collections.Generic;
using System.IO;

namespace ClassicalCipherSolver
{
    public class FitnessChecker
    {
        private readonly int _ngramWidth;
        private readonly float _baseScore;

        private readonly Dictionary<string, float> _statistics = new();

        public FitnessChecker(int ngramWidth, string ngramStatistics, char separator = ' ')
        {
            _ngramWidth = ngramWidth;

            foreach (string pair in ngramStatistics.Split('\n'))
            {
                string[] splitPair = pair.Split(separator);
                string ngram = splitPair[0];

                if (ngram.Length != ngramWidth)
                {
                    throw new InvalidDataException("Mismatch between ngram data and selected ngram width. Ensure that the ngram width matches the width of ngrams in the data file.");
                }

                int frequency = int.Parse(splitPair[1]);

                _statistics[ngram] = frequency;
            }

            long totalFrequency = 0;
            foreach (int entry in _statistics.Values)
            {
                totalFrequency += entry;
            }

            foreach (string ngram in _statistics.Keys)
            {
                _statistics[ngram] = (float)Math.Log10(_statistics[ngram] / totalFrequency);
            }

            _baseScore = (float)Math.Log10(0.01 / totalFrequency);
        }

        public float Evaluate(string candidate)
        {
            float epsilon = 0;

            for (int i = 0; i <= candidate.Length - _ngramWidth; i++)
            {
                string candidateNgram = candidate.Substring(i, _ngramWidth);

                if (_statistics.ContainsKey(candidateNgram))
                {
                    epsilon += _statistics[candidateNgram];
                }
                else
                {
                    epsilon += _baseScore;
                }
            }

            return epsilon / candidate.Length;
        }
    }
}