// <copyright file="ILogService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Impulse.SharedFramework.Services.Logging;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ILogService : IObservable<LogRecordBase>
{
    Task LogException(string message, Exception exception);

    Task LogInfo(string message);

    Task LogWarning(string message);

    Task LogError(string message);

    IEnumerable<LogRecordBase> GetLogRecordsForCricicality(Criticality criticality);

    Task DeleteRecordsAsync(IEnumerable<Guid> recordIds);
}
