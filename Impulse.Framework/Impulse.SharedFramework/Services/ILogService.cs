// <copyright file="ILogService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;

namespace Impulse.SharedFramework.Services;

public interface ILogService
{
    Task LogException(string message, Exception exception);

    Task LogInfo(string message);

    Task LogWarning(string message);
}
