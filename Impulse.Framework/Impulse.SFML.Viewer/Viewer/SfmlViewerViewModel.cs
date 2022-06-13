using Impulse.SharedFramework.Services.Layout;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;

namespace Impulse.SFML.Viewer.Viewer;

public class SfmlViewerViewModel : DocumentBase
{
    private Clock timer;

    private static Color ClearColor = new Color(0xe1, 0xe1, 0xe1);

    public SfmlViewerViewModel()
    {
        timer = new Clock();
    }

    public RenderWindow RenderWindow { get; private set; }

    public event EventHandler<float> UpdateEvent;

    public event EventHandler<RenderWindow> RenderEvent;

    public event EventHandler<MouseButtonEventArgs> MouseClickEvent;

    public virtual void OnRender()
    {
        var elapsed = timer.ElapsedTime.AsMilliseconds() / 1000f;
        timer.Restart();

        this.RenderWindow.Clear(new Color(0xe1, 0x51, 0x51));

        this.RenderWindow.DispatchEvents();

        UpdateEvent?.Invoke(this, elapsed);
        RenderEvent?.Invoke(this, this.RenderWindow);

        this.RenderWindow.Display();
    }

    public virtual void Initialize(IntPtr renderTargetPtr)
    {
        this.RenderWindow = new RenderWindow(renderTargetPtr);
        this.RenderWindow.MouseButtonPressed += MouseClickEvent;
    }
}