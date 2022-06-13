namespace Solvle.Tests;

using Xunit;
using FluentAssertions;
using Solvle.Domain;

public class SolverTests
{
    [Fact]
    public void LukesFirstSolvleUse()
    {
        // Make a solver using our reduced set of potential words
        var solver = new Solver(Wordle.Words);

        var guess = MakeGuess(
            ('w', Feedback.WrongLetter),
            ('a', Feedback.RightLetterRightPlace),
            ('t', Feedback.WrongLetter),
            ('e', Feedback.WrongLetter),
            ('r', Feedback.RightLetterWrongPlace));

        var guess2 = MakeGuess(
            ('l', Feedback.RightLetterRightPlace),
            ('o', Feedback.WrongLetter),
            ('u', Feedback.WrongLetter),
            ('s', Feedback.WrongLetter),
            ('y', Feedback.WrongLetter));

        // Act
        solver.MakeGuess(guess);
        solver.MakeGuess(guess2);

        // Assert
        var refinedList = solver.GetRefinedList();
    }

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

        var guess = MakeGuess(
            ('s', Feedback.RightLetterRightPlace),
            ('o', Feedback.RightLetterRightPlace),
            ('u', Feedback.RightLetterRightPlace),
            ('n', Feedback.RightLetterRightPlace),
            ('d', Feedback.RightLetterRightPlace));
        
        // Act
        solver.MakeGuess(guess);

        // Assert
        var refinedList = solver.GetRefinedList();
        refinedList.Should().HaveCount(1);
        refinedList.Should().Contain(worksString);
        refinedList.Should().NotContain(steamString);
    }

    private Guess MakeGuess(
        (char, Feedback) p1,
        (char, Feedback) p2,
        (char, Feedback) p3,
        (char, Feedback) p4,
        (char, Feedback) p5)
    {
        var letterOnePosition = new LetterPosition(0);
        var letterTwoPosition = new LetterPosition(1);
        var letterThreePosition = new LetterPosition(2);
        var letterFourPosition = new LetterPosition(3);
        var letterFivePosition = new LetterPosition(4);


        var guessLetterOne = new GuessLetter(new Letter(p1.Item1), p1.Item2, letterOnePosition);
        var guessLetterTwo = new GuessLetter(new Letter(p2.Item1), p2.Item2, letterTwoPosition);
        var guessLetterThree = new GuessLetter(new Letter(p3.Item1), p3.Item2, letterThreePosition);
        var guessLetterFour = new GuessLetter(new Letter(p4.Item1), p4.Item2, letterFourPosition);
        var guessLetterFive = new GuessLetter(new Letter(p5.Item1), p5.Item2, letterFivePosition);

        var guess = new Guess(
            guessLetterOne,
            guessLetterTwo,
            guessLetterThree,
            guessLetterFour,
            guessLetterFive);

        return guess;
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