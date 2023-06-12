// <copyright file="ILogService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Impulse.SharedFramework.Services.Logging;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ILogService
{
    Task LogException(string message, Exception exception);

    Task LogInfo(string message);

    Task LogWarning(string message);

    public IEnumerable<LogRecord> GetLogRecordsForCricicality(Criticality criticality);
}
