namespace Impulse.Framework.Dashboard.Services.Logging.LogWindow;

using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using Impulse.Shared.Enums;
using Impulse.SharedFramework.Services.Layout;
using Impulse.SharedFramework.Services.Logging;

internal class LogWindowViewModel : ToolWindowBase
{
    private readonly ILogService logService;
    private IDisposable? subscription;

    public LogWindowViewModel(ILogService logService)
    {
        this.logService = logService;

        Placement = ToolWindowPlacement.Bottom;
        LogRecords = new ObservableCollection<LogRecordBase>();

        subscription = logService.Subscribe(new LogRecordObserver(this));
    }

    public ObservableCollection<LogRecordBase> LogRecords { get; }

    public override string DisplayName => "Log Viewer";

    protected override async Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
    {
        if (close)
        {
            subscription?.Dispose();
            subscription = null;
        }

        await base.OnDeactivateAsync(close, cancellationToken);
    }

    private void OnRecordReceived(LogRecordBase record)
    {
        Execute.OnUIThread(() =>
        {
            LogRecords.Add(record);
        });
    }

    private sealed class LogRecordObserver : IObserver<LogRecordBase>
    {
        private readonly LogWindowViewModel owner;

        public LogRecordObserver(LogWindowViewModel owner)
        {
            this.owner = owner;
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(LogRecordBase value)
        {
            owner.OnRecordReceived(value);
        }
    }
}
