namespace Solvle.Tests;

using Xunit;
using FluentAssertions;
using Solvle.Domain;

public class LetterTests
{
    [Fact]
    public void Constructor_ShouldCreateSuccessfully_WhenLetterProvided()
    {
        // Arrange
        var letterCharacter = 'a';

        // Act
        var letter = new Letter(letterCharacter);

        // Assert
        letter.LetterChar.Should().Be(letterCharacter);
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenNotLetterProvided()
    {
        // Arrange
        var nonLetterCharacter = '9';

        // Act
        var createLetter = () => new Letter(nonLetterCharacter);

        // Assert
        var errorMessage = "The character '" + nonLetterCharacter + "' is not a letter.";
        createLetter.Should().Throw<ArgumentException>().WithMessage(errorMessage); 
    }
}
