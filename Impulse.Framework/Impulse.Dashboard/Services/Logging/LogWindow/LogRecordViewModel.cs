namespace Impulse.Framework.Dashboard.Services.Logging.LogWindow; 

using Impulse.Shared.ReactiveUI;
using Impulse.SharedFramework.Services.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LogRecordViewModel : ReactiveViewModelBase
{
    public DateTime Timestamp { get; init; }

    public Criticality Criticality { get; init; }

    public string Message { get; init; }

    public string? StackTrace { get; init; }
}
