namespace ClassicalCipherSolver.Ciphers
{
    internal interface IScoreable
    {
        float CandidateMatch
        { get; }

        float[] Means
        { get; }

        float[] SampleVariances
        { get; }

        float[,] DistancesFromMeans
        { get; }

        string[] EnglishTexts
        { get; }

        Plaintext DecryptAutomatically(string ciphertext, FitnessChecker fitnessChecker);
    }
}