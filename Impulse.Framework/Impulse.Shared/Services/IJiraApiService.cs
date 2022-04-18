// <copyright file="IJiraApiService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using CSharpFunctionalExtensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Impulse.Shared.Services;

public interface IJiraApiService
{
    Task<Result<List<string>>> GetAllReadyForDemoJiraIssuesForEmployee(
        string jiraEndpoint,
        string userName,
        string password,
        string employee);
}
