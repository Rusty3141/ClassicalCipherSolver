namespace ClassicalCipherSolver
{
    public class Plaintext : Ciphertext
    {
        public Plaintext(string plaintext) : base(plaintext)
        {
            Text = plaintext.ToLower();
        }
    }
}