using ClassicalCipherSolver.Ciphers;
using System;
using System.IO;
using System.Reflection;

namespace ClassicalCipherSolver
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            FitnessChecker fitnessChecker = new(4, Properties.Resources.Quadgrams);

            Console.WriteLine("Ciphertext (CTRL+Z, Return to stop input): ");

            string input = string.Empty;

            string line;
            while ((line = Console.ReadLine()) != null)
            {
                if (input != string.Empty) input += Environment.NewLine;
                input += line;
            }

            Ciphertext ciphertext = new(input);

            Console.Clear();
            Console.WriteLine("Provided ciphertext: ");
            Console.WriteLine(ciphertext.Text);

            foreach (PropertyInfo ciphertextStat in ciphertext.GetType().GetProperties())
            {
                if (ciphertextStat.PropertyType != typeof(string))
                {
                    Console.WriteLine($"{ciphertextStat.Name}: {ciphertextStat.GetValue(ciphertext)}");
                }
            }

            Caesar cs = new(ciphertext);
            Console.WriteLine(cs.DecryptAutomatically(fitnessChecker));
        }
    }
}