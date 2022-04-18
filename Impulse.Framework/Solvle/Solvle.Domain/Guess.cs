namespace Solvle.Domain;

public class Guess
{
    // contains 5 characters with feedback
    public GuessLetter LetterOne;
    public GuessLetter LetterTwo;
    public GuessLetter LetterThree;
    public GuessLetter LetterFour;
    public GuessLetter LetterFive;

    public Guess(GuessLetter letterOne, GuessLetter letterTwo, GuessLetter letterThree, GuessLetter letterFour, GuessLetter letterFive)
    {
        LetterOne = letterOne;
        LetterTwo = letterTwo;
        LetterThree = letterThree;
        LetterFour = letterFour;
        LetterFive = letterFive;
    }
}
