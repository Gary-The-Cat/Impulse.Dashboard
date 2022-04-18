namespace Solvle.Domain;

public class Letter
{
    public char LetterChar;

    public Letter(char character)
    {
        if (!char.IsLetter(character))
        {
            // It was not a letter
            throw new ArgumentException("The character '" + character + "' is not a letter.");
        }
        
        LetterChar = character;
    }
}
