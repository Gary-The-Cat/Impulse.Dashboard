using System;

namespace Impulse.TestGenerators
{
    public static class NameGenerator
    {
        public static string Create(int len)
        {
            Random r = new Random();
            string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "t", "v", "w", "x" };
            string[] vowels = { "a", "e", "i", "o", "u", "y" };
            string Name = "";
            Name += consonants[r.Next(consonants.Length)].ToUpper();
            Name += vowels[r.Next(vowels.Length)];

            //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
            int b = 2;
            while (b < len)
            {
                Name += consonants[r.Next(consonants.Length)];
                Name += vowels[r.Next(vowels.Length)];
                b += 2;
            }

            return Name;


        }
    }
}
