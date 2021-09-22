namespace ClassicalCipherSolver.Ciphers
{
    internal sealed class ConstantTestingCipher : Cipher<int>
    {
        public ConstantTestingCipher()
        {
            SetStats(7);
        }

        public override string Encrypt(string plaintext, int key)
        {
            return plaintext;
        }

        public override Plaintext DecryptAutomatically(string ciphertext, FitnessChecker fitnessChecker)
        {
            return new Plaintext("HHHHHHHHHHHHHHHHHHH");
        }

        public override string Decrypt(string ciphertext, int key)
        {
            return ciphertext;
        }
    }
}