using System.Collections.Generic;
using System.Linq;

namespace ClassicalCipherSolver
{
    internal class Plaintext : Ciphertext
    {
        public Plaintext(string plaintext) : base(plaintext)
        {
            Text = plaintext.ToLower();
        }
    }
}