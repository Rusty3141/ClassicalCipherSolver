namespace ClassicalCipherSolver.Ciphers
{
    internal interface IScoreable
    {
        float[] Means
        { get; }

        float[] SampleVariances
        { get; }

        float[,] DistancesFromMeans
        { get; }

        string[] EnglishTexts
        { get; }

        float CandidateStandardDeviationsFromSampleMean
        { get; set; }

        Plaintext DecryptAutomatically(string ciphertext, FitnessChecker fitnessChecker);
    }
}