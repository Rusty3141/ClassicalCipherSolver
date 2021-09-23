using System;
using System.Linq;

namespace ClassicalCipherSolver.Ciphers
{
    public sealed class MonoalphabeticSubstitution : Cipher<string>
    {
        public MonoalphabeticSubstitution()
        {
            SetStats("ZYXWVUTSRQPONMLKJIHGFEDCBA");
        }

        public override string Encrypt(string plaintext, string key)
        {
            return new string(plaintext.ToUpper().Select(x => char.IsLetter(x) ? key[Modulo(x - 65, 26)] : x).ToArray());
        }

        public override Plaintext DecryptAutomatically(string ciphertext, FitnessChecker fitnessChecker)
        {
            float maximumScore = float.NegativeInfinity;
            string maximumKey = "ABCDEFGHIJKLMNOPQRSTUVWYXZ";

            float parentScore = maximumScore;
            string parentKey = new string(maximumKey);

            for (int i = 1; i <= 5; ++i)
            {
                parentKey = parentKey.Shuffle();
                string result = Decrypt(ciphertext, parentKey);
                float score = fitnessChecker.Evaluate(result.TrimForAnalysis());

                int iterationsSinceImprovement = 0;
                Random random = new();
                while (iterationsSinceImprovement < 1000)
                {
                    int aSwap = random.Next(26);
                    int bSwap = random.Next(26);

                    char[] childKey = parentKey.ToCharArray();
                    char temp = childKey[aSwap];
                    childKey[aSwap] = childKey[bSwap];
                    childKey[bSwap] = temp;

                    string childResult = Decrypt(ciphertext, new string(childKey));
                    float childScore = fitnessChecker.Evaluate(childResult.TrimForAnalysis());

                    if (childScore > parentScore)
                    {
                        parentScore = childScore;
                        parentKey = new string(childKey);
                        iterationsSinceImprovement = 0;
                    }

                    ++iterationsSinceImprovement;
                }

                if (parentScore > maximumScore)
                {
                    maximumScore = parentScore;
                    maximumKey = new string(parentKey);
                }
            }

            return new Plaintext(Decrypt(ciphertext, maximumKey));
        }

        public override string Decrypt(string ciphertext, string key)
        {
            return new string(ciphertext.ToUpper().Select(x => char.IsLetter(x) ? (char)(key.IndexOf(x) + 65) : x).ToArray());
        }
    }
}