using System;
using System.Linq;

namespace ClassicalCipherSolver
{
    internal static class InputExtensions
    {
        public static string TrimForAnalysis(this string s)
        {
            return new string(s.Where(c => Char.IsLetter(c)).ToArray());
        }
    }
}