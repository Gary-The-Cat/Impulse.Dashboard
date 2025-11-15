using System.Collections.ObjectModel;
using Impulse.Logging.Contracts;
using Impulse.Shared.ReactiveUI;

namespace Impulse.Logging.UI.Configuration.Screens;

public class LogSettingsViewModel : ReactiveScreen
{
    public LogSettingsViewModel()
    {
        LogCriticalities = new ObservableCollection<Criticality>()
        {
            Criticality.Info,
            Criticality.Warning,
            Criticality.Error,
        };
    }

    public ObservableCollection<Criticality> LogCriticalities { get; set; }

    public Criticality SelectedLogCriticality { get; set; }
}
