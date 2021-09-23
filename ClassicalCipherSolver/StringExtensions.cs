using System;
using System.Linq;

namespace ClassicalCipherSolver
{
    internal static class StringExtensions
    {
        public static string TrimForAnalysis(this string s)
        {
            return new string(s.Where(c => Char.IsLetter(c)).ToArray());
        }

        public static string Shuffle(this string s)
        {
            Random random = new();

            return new string(s.ToCharArray().OrderBy(s => random.Next()).ToArray());
        }
    }
}