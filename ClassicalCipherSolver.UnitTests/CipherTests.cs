using NUnit.Framework;
using ClassicalCipherSolver.Ciphers;

namespace ClassicalCipherSolver.UnitTests
{
    public class CipherTests
    {
        [Test]
        public void KeywordEncryption()
        {
            MonoalphabeticSubstitution test = new MonoalphabeticSubstitution();
            string exampleKey = "CAISTORUVWXYZBDEFGHJKLMNPQ";

            Ciphertext result = new(test.Encrypt(Properties.Resources.ExamplePlaintext, exampleKey));

            Assert.AreEqual(Properties.Resources.ExampleKeywordCiphertext, result.Text, "Encryption was not correct.");
        }

        [Test]
        public void KeywordDecryption()
        {
            MonoalphabeticSubstitution test = new MonoalphabeticSubstitution();
            string exampleKey = "CAISTORUVWXYZBDEFGHJKLMNPQ";

            Plaintext result = new(test.Decrypt(Properties.Resources.ExampleKeywordCiphertext, exampleKey));

            Assert.AreEqual(Properties.Resources.ExamplePlaintext, result.Text, "Decryption was not correct.");
        }
    }
}