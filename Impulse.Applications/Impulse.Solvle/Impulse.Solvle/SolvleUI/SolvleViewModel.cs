using Impulse.SharedFramework.Services.Layout;
using Ninject;
using Solvle.Domain;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace Impulse.Solvle.SolvleUI;
public class SolvleViewModel : DocumentBase
{
    private Solver solver;

    private Dictionary<SolidColorBrush, Feedback> feedbackColour;

    public SolvleViewModel()
    {
        DisplayName = "Wordle Solver";

        Colours = new List<SolidColorBrush>()
        {
            new SolidColorBrush(Color.FromRgb(0x75, 0xB3, 0x6E)),
            new SolidColorBrush(Color.FromRgb(0xCF, 0xBD, 0x61)),
            new SolidColorBrush(Color.FromRgb(0x83, 0x87, 0x89)),
        };

        feedbackColour = new Dictionary<SolidColorBrush, Feedback>()
        {
            { Colours[0], Feedback.RightLetterRightPlace },
            { Colours[1], Feedback.RightLetterWrongPlace },
            { Colours[2], Feedback.WrongLetter },
        };

        solver = new Solver(WordleRepo.Words);

        GuessLetterOneColour = Colours.Last();
        GuessLetterTwoColour = Colours.Last();
        GuessLetterThreeColour = Colours.Last();
        GuessLetterFourColour = Colours.Last();
        GuessLetterFiveColour = Colours.Last();

        // make a dumb boring list here with all of our test values
        var words = new List<string>() { "water", "string", "which" };

        // Pass them into our observableCollection
        PotentialWords = new ObservableCollection<string>(words);
    }

    public List<SolidColorBrush> Colours { get; set; }
    
    public SolidColorBrush GuessLetterOneColour { get; set; } 
    public SolidColorBrush GuessLetterTwoColour { get; set; }
    public SolidColorBrush GuessLetterThreeColour { get; set; }
    public SolidColorBrush GuessLetterFourColour { get; set; }
    public SolidColorBrush GuessLetterFiveColour { get; set; }

    public ObservableCollection <string> PotentialWords { get; set; }
    public void MakeGuess()
    {
        // Get the letters for each of the guesses
        
        // Create the 'LetterPosition's for each of our letters

        // Get the feedback for each of the guesses:
        var feedback = feedbackColour[GuessLetterOneColour];

        // Make the guess letters
        //var guessLetter = new GuessLetter()

        // Make the guess
        //var guess = new Guess()

        // Perform the guess
        //solver.MakeGuess();

        // Ask for the new list of refined words

        // Update our UI to show the list of refined words.
    }
}
