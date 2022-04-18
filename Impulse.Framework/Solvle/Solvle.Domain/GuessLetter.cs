namespace Solvle.Domain;

public class GuessLetter
{
    public Letter Letter;
    public Feedback Feedback;

    public GuessLetter(Letter letter, Feedback feedback)
    {
        Letter = letter;
        Feedback = feedback;
    }
}
