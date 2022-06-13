using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Impulse.SharedFramework.Services.Layout;
using ReactiveUI;

namespace Impulse.Framework.Dashboard.Debug.DemoScreens.WordChecker;

public class WordCheckerViewModel : DocumentBase
{
    private List<string> fiveLetterWords;

    public WordCheckerViewModel()
    {
        DisplayName = "Word Checker";

        fiveLetterWords = new List<string>();
        WordSuggestions = new ObservableCollection<string>();

        foreach (var word in WordDictionary.Words)
        {
            fiveLetterWords.Add(word);
        }

        UpdateCommand = ReactiveCommand.CreateFromTask(this.Update);
    }

    public string One { get; set; } = string.Empty;

    public string Two { get; set; } = string.Empty;

    public string Three { get; set; } = string.Empty;

    public string Four { get; set; } = string.Empty;

    public string Five { get; set; } = string.Empty;

    public string NotOne { get; set; } = string.Empty;

    public string NotTwo { get; set; } = string.Empty;

    public string NotThree { get; set; } = string.Empty;

    public string NotFour { get; set; } = string.Empty;

    public string NotFive { get; set; } = string.Empty;

    public string DoesntContain { get; set; } = string.Empty;

    public ICommand UpdateCommand { get; set; }

    public ObservableCollection<string> WordSuggestions { get; set; }

    private async Task Update()
    {
        WordSuggestions.Clear();

        var tempLookup = fiveLetterWords.ToList();

        foreach (var character in DoesntContain)
        {
            tempLookup = tempLookup.Where(w => !w.Contains(character)).ToList();
        }

        if (!string.IsNullOrWhiteSpace(One))
        {
            tempLookup = tempLookup.Where(w => w[0] == One[0]).ToList();
        }

        if (!string.IsNullOrWhiteSpace(Two))
        {
            tempLookup = tempLookup.Where(w => w[1] == Two[0]).ToList();
        }

        if (!string.IsNullOrWhiteSpace(Three))
        {
            tempLookup = tempLookup.Where(w => w[2] == Three[0]).ToList();
        }

        if (!string.IsNullOrWhiteSpace(Four))
        {
            tempLookup = tempLookup.Where(w => w[3] == Four[0]).ToList();
        }

        if (!string.IsNullOrWhiteSpace(Five))
        {
            tempLookup = tempLookup.Where(w => w[4] == Five[0]).ToList();
        }

        foreach (var character in NotOne)
        {
            tempLookup = tempLookup.Where(w => w[0] != character && w.Contains(character)).ToList();
        }

        foreach (var character in NotTwo)
        {
            tempLookup = tempLookup.Where(w => w[1] != character && w.Contains(character)).ToList();
        }

        foreach (var character in NotThree)
        {
            tempLookup = tempLookup.Where(w => w[2] != character && w.Contains(character)).ToList();
        }

        foreach (var character in NotFour)
        {
            tempLookup = tempLookup.Where(w => w[3] != character && w.Contains(character)).ToList();
        }

        foreach (var character in NotFive)
        {
            tempLookup = tempLookup.Where(w => w[4] != character && w.Contains(character)).ToList();
        }

        foreach (var word in tempLookup)
        {
            WordSuggestions.Add(word);
        }
    }
}