using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solvle.Domain
{
    public class Solver
    {
        private List<string> potentialWords;

        private List<Guess> guessList;

        public Solver(List<string> wordleWords)
        {
            potentialWords = wordleWords;
            guessList = new List<Guess>();
        }

        public void MakeGuess(Guess guess)
        {
            guessList.Add(guess);
        }

        public List<string> GetRefinedList()
        {
            //get a copy of the potential words

            //loop over all guesses

            //for each guess, compare to potential words
            foreach (var guess in guessList)
            {
                if (CheckPotentialWord(guess.LetterOne, "works"))
                {

                }
            }

            //if could be it, leave in the potential words

            //else remove from potential words

            return null;
        }

        private bool CheckPotentialWord (GuessLetter guessLetter, string potentialWord)
        {
            return false;
        }
    }
}
