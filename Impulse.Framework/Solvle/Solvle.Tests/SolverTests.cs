namespace Solvle.Tests;

using Xunit;
using FluentAssertions;
using Solvle.Domain;

public class SolverTests
{
    [Fact]
    public void MakeGuess_ShouldRefineList_WhenGuessProvided()
    {
        // Arrange
        // Get our list of potential words.
        var worksString = "works";
        var steamString = "steam";
        var words = new List<string>() { worksString, steamString };

        // Make a solver using our reduced set of potential words
        var solver = new Solver(words);

        // Construct a guess
        var letterOne = new Letter('s');
        var letterTwo = new Letter('o');
        var letterThree = new Letter('u');
        var letterFour = new Letter('n');
        var letterFive = new Letter('d');

        var guessLetterOne = new GuessLetter(letterOne, Feedback.RightLetterWrongPlace);
        var guessLetterTwo = new GuessLetter(letterTwo, Feedback.RightLetterRightPlace);
        var guessLetterThree = new GuessLetter(letterThree, Feedback.WrongLetter);
        var guessLetterFour = new GuessLetter(letterFour, Feedback.WrongLetter);
        var guessLetterFive = new GuessLetter(letterFive, Feedback.WrongLetter);

        var guess = new Guess(
            guessLetterOne,
            guessLetterTwo,
            guessLetterThree,
            guessLetterFour,
            guessLetterFive);
        
        // Act
        solver.MakeGuess(guess);

        // Assert
        var refinedList = solver.GetRefinedList();
        refinedList.Should().HaveCount(1);
        refinedList.Should().Contain(worksString);
        refinedList.Should().NotContain(steamString);
    }
}