using System.Collections.ObjectModel;
using Impulse.ErrorReporting;
using Impulse.Shared.ReactiveUI;

namespace Impulse.Framework.Dashboard.Configuration.Screens;

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
