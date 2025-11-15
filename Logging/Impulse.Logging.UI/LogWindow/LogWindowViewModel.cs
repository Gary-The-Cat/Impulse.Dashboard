namespace Impulse.Logging.UI.LogWindow;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using Caliburn.Micro;
using Impulse.Logging.Contracts;
using Impulse.Shared.Enums;
using Impulse.SharedFramework.Services.Layout;

public sealed class LogWindowViewModel : ToolWindowBase
{
    private readonly ILogService logService;
    private readonly ObservableCollection<LogRecordBase> records = new();
    private IDisposable? subscription;
    private LogSeverityFilterOption selectedSeverity;

    public LogWindowViewModel(ILogService logService)
    {
        this.logService = logService;

        Placement = ToolWindowPlacement.Bottom;
        LogRecords = CollectionViewSource.GetDefaultView(records);
        LogRecords.Filter = FilterRecord;
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

    public ICollectionView LogRecords { get; }
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
        Execute.OnUIThread(() => records.Add(record));
    }

    public void ClearLogs()
    {
        Execute.OnUIThread(records.Clear);
    }

    public void RemoveRecords(IEnumerable<LogRecordBase> records)
    {
        if (records == null)
        {
            return;
        }

        Execute.OnUIThread(() =>
        {
            var targets = records.Where(record => record != null).ToList();
            foreach (var record in targets)
            {
                this.records.Remove(record);
            }
        });
    }

    public async Task DeleteRecordsAsync(IEnumerable<LogRecordBase> records)
    {
        if (records == null)
        {
            return;
        }

        var targets = records.Where(record => record != null).ToList();
        if (targets.Count == 0)
        {
            return;
        }

        await logService.DeleteRecordsAsync(targets.Select(record => record.Id)).ConfigureAwait(false);
        RemoveRecords(targets);
    }

    public IReadOnlyList<LogRecordBase> GetVisibleRecords() =>
        LogRecords.Cast<LogRecordBase>().ToList();

    private bool FilterRecord(object value) =>
        value is LogRecordBase record && (selectedSeverity?.Predicate(record) ?? true);

    private void ApplyFilter() => Execute.OnUIThread(LogRecords.Refresh);

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

public sealed record LogSeverityFilterOption(string DisplayName, Func<LogRecordBase, bool> Predicate);
