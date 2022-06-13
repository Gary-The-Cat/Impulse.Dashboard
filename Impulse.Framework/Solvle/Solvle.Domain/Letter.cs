using CSharpFunctionalExtensions;

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

    public static Result<Letter> Create(char character)
    {
        if (!char.IsLetter(character))
        {
            // It was not a letter
            return Result.Failure<Letter>("The character '" + character + "' is not a letter.");
        }

        return new Letter(character);
    }
}
