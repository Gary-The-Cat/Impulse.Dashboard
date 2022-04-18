namespace Impulse.Shared.ReactiveUI;

public class ReactiveMenuItem : ReactiveViewModelBase
{
    public ReactiveMenuItem(int id, string name, bool isEnabled)
    {
        this.Id = id;
        this.Name = name;
        this.IsEnabled = isEnabled;
    }

    public string Name { get; set; }

    public int Id { get; set; }

    public bool IsEnabled { get; set; }
}
