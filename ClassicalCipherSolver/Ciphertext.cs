using System.Collections.Generic;
using System.Linq;

namespace ClassicalCipherSolver
{
    public class Ciphertext
    {
        public string Text { get; protected set; }
        public float IoC { get; protected set; }

        public Ciphertext(string ciphertext)
        {
            Text = ciphertext.ToUpper();

            IoC = IndexOfCoincidence(Text);
        }

        protected static float IndexOfCoincidence(string ciphertext)
        {
            if (ciphertext.Length < 2) return 0;

            ciphertext = ciphertext.TrimForAnalysis();
            long ic = 0;

            foreach (char c in new HashSet<char>(ciphertext))
            {
                long n_i = ciphertext.Where(x => x == c).Count();

                ic += n_i * (n_i - 1);
            }

            return (float)(ic / ((decimal)ciphertext.Length * (ciphertext.Length - 1)));
        }
    }
}