using System.Collections.Generic;
using System.Linq;

namespace ClassicalCipherSolver
{
    internal class Ciphertext
    {
        public string Text { get; private set; }
        public float IoC { get; private set; }

        public Ciphertext(string ciphertext)
        {
            Text = ciphertext.ToUpper();

            IoC = IndexOfCoincidence(Text);
        }

        private static float IndexOfCoincidence(string ciphertext)
        {
            ciphertext = ciphertext.TrimForAnalysis();
            float ic = 0f;

            foreach (char c in new HashSet<char>(ciphertext))
            {
                int n_i = ciphertext.Where(x => x == c).Count();

                ic += n_i * (n_i - 1);
            }

            return ic / (ciphertext.Length * (ciphertext.Length - 1));
        }
    }
}