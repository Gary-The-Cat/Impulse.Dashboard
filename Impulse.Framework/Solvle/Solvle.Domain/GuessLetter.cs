namespace Solvle.Domain;

public class GuessLetter
{
    public Letter Letter;
    public Feedback Feedback;
    public LetterPosition Position;

    public GuessLetter(Letter letter, Feedback feedback, LetterPosition position)
    {
        Letter = letter;
        Feedback = feedback;
        Position = position;
    }
}
