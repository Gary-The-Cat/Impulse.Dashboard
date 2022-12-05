using System;
using System.Threading.Tasks;
using Impulse.ErrorReporting;
using Impulse.Shared.ExtensionMethods;
using Impulse.Shared.Interfaces;
using Impulse.SharedFramework.Services;

namespace Impulse.Framework.Dashboard.Services;
public class LogService : ILogService
{
    private WeakReference<IDateTimeProvider> dateTimeProviderReference;

    public LogService(IDateTimeProvider dateTimeProvider)
    {
        this.dateTimeProviderReference = new WeakReference<IDateTimeProvider>(dateTimeProvider);
    }

    private IDateTimeProvider DateTimeProvider => this.dateTimeProviderReference.Value();

    public Task LogException(string message, Exception exception) =>
        LogError(LogRecord.CreateException(DateTimeProvider.Now, message, exception));

    public Task LogInfo(string message) =>
         LogError(LogRecord.CreateInfo(DateTimeProvider.Now, message));

    public Task LogWarning(string message) =>
        LogError(LogRecord.CreateWarning(DateTimeProvider.Now, message));

    private Task LogError(LogRecord error)
    {
        return Task.CompletedTask;
    }
}
