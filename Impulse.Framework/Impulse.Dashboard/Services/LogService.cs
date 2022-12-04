using Caliburn.Micro;
using Impulse.Dashboard.Shell;
using Impulse.ErrorReporting;
using Impulse.Shared.ExtensionMethods;
using Impulse.Shared.Interfaces;
using Impulse.SharedFramework.Plugin;
using Impulse.SharedFramework.Services;
using Impulse.SharedFramework.Shell;
using System;
using System.Threading.Tasks;

namespace Impulse.Framework.Dashboard.Services;
public class LogService : ILogService
{
    private WeakReference<IDateTimeProvider> dateTimeProviderReference;

    public LogService(IDateTimeProvider dateTimeProvider)
    {
        this.dateTimeProviderReference = new WeakReference<IDateTimeProvider>(dateTimeProvider);
    }

    private IDateTimeProvider DateTimeProvider => this.dateTimeProviderReference.Value();

    public Task LogException(string message, Exception exception)
    {
        var error = new Error()
        {
            Timestamp = dateTimeProvider.Now,
            Criticality = Criticality.Error,
            Message = message,
            Exception = exception,
        };

        return LogError(error);
    }

    public Task LogInfo(string message)
    {
        var error = new Error()
        {
            Timestamp = dateTimeProvider.Now,
            Criticality = Criticality.Info,
            Message = message,
        };

        return LogError(error);
    }

    public Task LogWarning(string message)
    {
        var error = new Error()
        {
            Timestamp = dateTimeProvider.Now,
            Criticality = Criticality.Info,
            Message = message,
        };

        return LogError(error);
    }

    private Task LogError(Error error)
    {
        return Execute.OnUIThreadAsync(() =>
        {
            // Do something
            return Task.CompletedTask;
        });
    }
}
