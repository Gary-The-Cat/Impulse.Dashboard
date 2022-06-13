using System.Collections;

namespace Solvle.Domain;

public class Guess : IEnumerable<GuessLetter>
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

    public IEnumerator<GuessLetter> GetEnumerator()
    {
        yield return LetterOne;
        yield return LetterTwo;
        yield return LetterThree;
        yield return LetterFour;
        yield return LetterFive;
    }

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}
