namespace Solvle.Domain;

public class Solver
{
    private List<string> wordleWords;

    private List<Guess> guessList;

    public Solver(List<string> wordleWords)
    {
        this.wordleWords = wordleWords;
        guessList = new List<Guess>();
    }

    public void MakeGuess(Guess guess)
    {
        guessList.Add(guess);
    }

    public List<string> GetRefinedList()
    {
        var output = new List<string>();
        // Iterate through all potential words out of Wordlewords list
        foreach (var word in wordleWords)
        {
            // for each guess in guesslist 
            bool isValidWord = true;
            foreach (var guess in guessList)
            {
                // Checking if a guess means that this potential word cant be the solution for todays wordle
                foreach (var guessLetter in guess)
                {
                    isValidWord &= CheckPotentialWord(guessLetter, word);
                }
            }

            if (isValidWord == true)
            {
                output.Add(word);
            }
        }


        return output;
    }

    public bool CheckPotentialWord (GuessLetter guessLetter, string potentialWord)
    {
        //check if the guess letter is in the potential word
        //if it is, return true
        var index = guessLetter.Position.ValidLetterInt;
        var character = potentialWord[index];
        
        if (guessLetter.Feedback == Feedback.RightLetterRightPlace)
        {
            if (character == guessLetter.Letter.LetterChar)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        if (guessLetter.Feedback == Feedback.WrongLetter)
        {
            if (potentialWord.Contains(guessLetter.Letter.LetterChar))
            {
                return false;
            }

            if (!potentialWord.Contains(guessLetter.Letter.LetterChar))
            {
                return true;
            }
        }

        if (guessLetter.Feedback == Feedback.RightLetterWrongPlace)
        {
            if (character == guessLetter.Letter.LetterChar)
            {
                return false;
            }

            if (potentialWord.Contains(guessLetter.Letter.LetterChar))
            {
                return true;
            }

            return false;
        }

        throw new Exception("Unexpected case.");
    }
}