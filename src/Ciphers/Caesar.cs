using System;
using System.Linq;

namespace ClassicalCipherSolver.Ciphers
{
    internal class Caesar : Cipher<int>
    {
        public override float CandidateMatch
        {
            get;
            protected set;
        }

        public Caesar(Ciphertext _text)
        {
            text = _text.Text;
        }

        public override string Encrypt(int key)
        {
            return new string(text.Select(x => Char.IsLetter(x) ? (char)(Modulo(x - 65 + key, 26) + 65) : x).ToArray());
        }

        public override Plaintext DecryptAutomatically(FitnessChecker fitnessChecker)
        {
            string maximum = string.Empty;
            float maximumScore = float.NegativeInfinity;

            for (int i = 0; i < 26; ++i)
            {
                string result = Decrypt(i);
                float score = fitnessChecker.Evaluate(result.TrimForAnalysis());

                if (score > maximumScore)
                {
                    maximum = result;
                    maximumScore = score;
                }
            }

            return new Plaintext(maximum);
        }

        public override string Decrypt(int key)
        {
            return new string(text.Select(x => Char.IsLetter(x) ? (char)(Modulo(x - 65 - key, 26) + 65) : x).ToArray());
        }
    }
}