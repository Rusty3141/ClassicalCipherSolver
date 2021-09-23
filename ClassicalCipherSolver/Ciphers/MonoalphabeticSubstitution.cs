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
            string maximum = string.Empty;
            float maximumScore = float.NegativeInfinity;

            return new Plaintext(maximum);
        }

        public override string Decrypt(string ciphertext, string key)
        {
            return new string(ciphertext.ToUpper().Select(x => char.IsLetter(x) ? (char)(key.IndexOf(x) + 65) : x).ToArray());
        }
    }
}