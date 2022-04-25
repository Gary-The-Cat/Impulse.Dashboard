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
        //get a copy of the potential words
        var output = new List<string>(wordleWords);

        //loop over all guesses
        ////foreach (var guess in guessList)
        ////{
        ////    //for each guess, compare to potential words
            

        ////    if (CheckPotentialWord(guess.LetterOne, potentialWor))
        ////    {

        ////    }
            
        ////    if (CheckPotentialWord(guess.LetterTwo, ))
        ////    {

        ////    }
            
        ////    if (CheckPotentialWord(guess.LetterThree, ))
        ////    {

        ////    }
            
        ////    if (CheckPotentialWord(guess.LetterFour, ))
        ////    {

        ////    }
            
        ////    if (CheckPotentialWord(guess.LetterFive, ))
        ////    {

        ////    }
        ////}

        //if could be it, leave in the potential words

        //else remove from potential words

        return null;
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