namespace ClassicalCipherSolver.Ciphers
{
    internal abstract class Cipher<T>
    {
        public virtual float CandidateMatch
        {
            get;
            protected set;
        }

        public abstract string Encrypt(T key);

        public abstract Plaintext DecryptAutomatically(FitnessChecker fitnessChecker);

        public abstract string Decrypt(T key);

        protected string text;

        protected static int Modulo(int a, int b)
        {
            int result = a % b;
            if (result < 0) result += b;

            return result;
        }
    }
}