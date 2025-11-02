namespace Impulse.Framework.Dashboard.Services.Logging.LogWindow;

using System;
using System.Collections.Generic;
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
    private readonly List<LogRecordBase> allRecords = new();
    private IDisposable? subscription;
    private LogSeverityFilterOption selectedSeverity;

    public LogWindowViewModel(ILogService logService)
    {
        this.logService = logService;

        Placement = ToolWindowPlacement.Bottom;
        LogRecords = new ObservableCollection<LogRecordBase>();
        SeverityOptions = new BindableCollection<LogSeverityFilterOption>
        {
            new("All", _ => true),
            new("Info", record => record is InfoLogRecord),
            new("Warning", record => record is WarningLogRecord),
            new("Error", record => record is ErrorLogRecord && record is not ExceptionLogRecord),
            new("Exception", record => record is ExceptionLogRecord),
        };

        selectedSeverity = SeverityOptions[0];
        NotifyOfPropertyChange(() => SelectedSeverity);

        subscription = logService.Subscribe(new LogRecordObserver(this));
    }

    public ObservableCollection<LogRecordBase> LogRecords { get; }
    public BindableCollection<LogSeverityFilterOption> SeverityOptions { get; }

    public LogSeverityFilterOption SelectedSeverity
    {
        get => selectedSeverity;
        set
        {
            if (value == null || Equals(selectedSeverity, value))
            {
                return;
            }

            selectedSeverity = value;
            NotifyOfPropertyChange(() => SelectedSeverity);
            ApplyFilter();
        }
    }

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
            allRecords.Add(record);

            if (MatchesSelectedFilter(record))
            {
                LogRecords.Add(record);
            }
        });
    }

    public void ClearLogs()
    {
        Execute.OnUIThread(() =>
        {
            allRecords.Clear();
            LogRecords.Clear();
        });
    }

    public IReadOnlyList<LogRecordBase> GetVisibleRecords() => LogRecords;

    private bool MatchesSelectedFilter(LogRecordBase record) =>
        selectedSeverity?.Predicate(record) ?? true;

    private void ApplyFilter()
    {
        Execute.OnUIThread(() =>
        {
            LogRecords.Clear();

            foreach (var record in allRecords)
            {
                if (MatchesSelectedFilter(record))
                {
                    LogRecords.Add(record);
                }
            }
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

internal sealed record LogSeverityFilterOption(string DisplayName, Func<LogRecordBase, bool> Predicate);
