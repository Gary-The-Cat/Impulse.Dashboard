namespace Solvle.Tests;

using Xunit;
using FluentAssertions;
using Solvle.Domain;

public class SolverTests
{
    [Fact]
    public void GetRefinedList_ShouldRefineList_WhenGuessesProvided()
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

        var letterOnePosition = new LetterPosition(0);
        var letterTwoPosition = new LetterPosition(1);
        var letterThreePosition = new LetterPosition(2);
        var letterFourPosition = new LetterPosition(3);
        var letterFivePosition = new LetterPosition(4);


        var guessLetterOne = new GuessLetter(letterOne, Feedback.RightLetterWrongPlace, letterOnePosition);
        var guessLetterTwo = new GuessLetter(letterTwo, Feedback.RightLetterRightPlace, letterTwoPosition);
        var guessLetterThree = new GuessLetter(letterThree, Feedback.WrongLetter, letterThreePosition);
        var guessLetterFour = new GuessLetter(letterFour, Feedback.WrongLetter, letterFourPosition);
        var guessLetterFive = new GuessLetter(letterFive, Feedback.WrongLetter, letterFivePosition);

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

    [Fact]
    public void CheckPotentialWord_ShouldReturnTrue_WhenRightLetterInRightPlaceProvided()
    {
        //Arrange
        //Give a guessLetter and potentialWord

        var letter = new Letter('p');
        var feedback = Feedback.RightLetterRightPlace;
        var position = new LetterPosition(0);

        var guessLetter = new GuessLetter(letter, feedback, position);
        var potentialWord = new string("pilot");

        //Act
        var fakeAssList = new List<string>();

        var solver = new Solver(fakeAssList);

        var checkPotentialWordResult = solver.CheckPotentialWord(guessLetter, potentialWord);

        //Assert

        checkPotentialWordResult.Should().BeTrue();

    }

    [Fact]
    public void CheckPotentialWord_ShouldReturnFalse_WhenRightLetterInWrongPlaceProvided()
    {
        //Arrange
        //Give a guessLetter and potentialWord

        // User made the guess 'sound'
        var letter = new Letter('s');

        // The correct word is 'stripe'
        /// That is the right letter in the right place
        var feedback = Feedback.RightLetterRightPlace;
        var position = new LetterPosition(0);

        var guessLetter = new GuessLetter(letter, feedback, position);

        // The current word in wordles list of potential words is pilot
        // Considering 's' was the right letter at position 0, 'pilot' cannot be the right word
        var potentialWord = new string("pilot");

        //Act
        var fakeAssList = new List<string>();

        var solver = new Solver(fakeAssList);

        var checkPotentialWordResult = solver.CheckPotentialWord(guessLetter, potentialWord);

        //Assert

        checkPotentialWordResult.Should().BeFalse();

    }

    [Fact]
    public void CheckPotentialWord_ShouldReturnTrue_WhenWrongLetterProvidedDoesNotExistInWord()
    {
        //Arrange
        //Give a guessLetter and potentialWord

        var letter = new Letter('q');

        // The correct word is 'stripe'
        /// That is the wrong letter
        var feedback = Feedback.WrongLetter;
        var position = new LetterPosition(0);

        var guessLetter = new GuessLetter(letter, feedback, position);

        // The current word in wordles list of potential words is pilot

        var potentialWord = new string("pilot");

        //Act
        var fakeAssList = new List<string>();

        var solver = new Solver(fakeAssList);

        var checkPotentialWordResult = solver.CheckPotentialWord(guessLetter, potentialWord);

        //Assert

        checkPotentialWordResult.Should().BeTrue();

    }

    [Fact]
    public void CheckPotentialWord_ShouldReturnFalse_WhenWrongLetterProvidedDoesExistInWord()
    {
        //Arrange
        //Give a guessLetter and potentialWord

        var letter = new Letter('o');

        // The correct word is 'stripe'
        /// That is the wrong letter for 'stripe'
        var feedback = Feedback.WrongLetter;
        var position = new LetterPosition(0);

        var guessLetter = new GuessLetter(letter, feedback, position);

        // The current word in wordles list of potential words is pilot
        /// That is the wrong letter for 'stripe' but does exist in 'pilot' therefore it cannot be 'pilot'

        var potentialWord = new string("pilot");

        //Act
        var fakeAssList = new List<string>();

        var solver = new Solver(fakeAssList);

        var checkPotentialWordResult = solver.CheckPotentialWord(guessLetter, potentialWord);

        //Assert

        checkPotentialWordResult.Should().BeFalse();

    }

    [Fact]
    public void CheckPotentialWord_ShouldReturnFalse_RightLetterInWrongPlace()
    {
        //Arrange
        //Give a guessLetter and potentialWord

        var letter = new Letter('p');

        // The correct word is 'stripe'
        /// That is the right letter in the wrong place for 'stripe'
        var feedback = Feedback.RightLetterWrongPlace;
        var position = new LetterPosition(0);

        var guessLetter = new GuessLetter(letter, feedback, position);

        // The current word in wordles list of potential words is pilot
        /// That is the right letter in the wrong place which means it cant start with 'p' therefore it cannot be 'pilot'

        var potentialWord = new string("pilot");

        //Act
        var fakeAssList = new List<string>();

        var solver = new Solver(fakeAssList);

        var checkPotentialWordResult = solver.CheckPotentialWord(guessLetter, potentialWord);

        //Assert

        checkPotentialWordResult.Should().BeFalse();

    }

    [Fact]
    public void CheckPotentialWord_ShouldReturnTrue_RightLetterInWrongPlace()
    {
        //Arrange
        //Give a guessLetter and potentialWord

        var letter = new Letter('e');

        // The correct word is 'stripe'
        /// That is the right letter in the wrong place for 'stripe'
        var feedback = Feedback.RightLetterWrongPlace;
        var position = new LetterPosition(0);

        var guessLetter = new GuessLetter(letter, feedback, position);

        // The current word in wordles list of potential words is weird
        /// That is the right letter in the wrong place 

        var potentialWord = new string("weird");

        //Act
        var fakeAssList = new List<string>();

        var solver = new Solver(fakeAssList);

        var checkPotentialWordResult = solver.CheckPotentialWord(guessLetter, potentialWord);

        //Assert

        checkPotentialWordResult.Should().BeTrue();

    }
}