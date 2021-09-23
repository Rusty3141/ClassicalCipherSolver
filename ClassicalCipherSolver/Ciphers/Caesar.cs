using System.Linq;

namespace ClassicalCipherSolver.Ciphers
{
    sealed class Caesar : Cipher<int>
    {
        public Caesar()
        {
            SetStats(7);
        }

        public override string Encrypt(string plaintext, int key)
        {
            return new string(plaintext.ToUpper().Select(x => char.IsLetter(x) ? (char)(Modulo(x - 65 + key, 26) + 65) : x).ToArray());
        }

        public override Plaintext DecryptAutomatically(string ciphertext, FitnessChecker fitnessChecker)
        {
            string maximum = string.Empty;
            float maximumScore = float.NegativeInfinity;

            for (int i = 0; i < 26; ++i)
            {
                string result = Decrypt(ciphertext, i);
                float score = fitnessChecker.Evaluate(result.TrimForAnalysis());

                if (score > maximumScore)
                {
                    maximum = result;
                    maximumScore = score;
                }
            }

            return new Plaintext(maximum);
        }

        public override string Decrypt(string ciphertext, int key)
        {
            return Encrypt(ciphertext, -key);
        }
    }
}