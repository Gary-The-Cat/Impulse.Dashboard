// <copyright file="IUnitOfWork.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Impulse.Shared.Interfaces;

namespace Impulse.SharedFramework.Services;

public interface IUnitOfWork : IDisposable
{
    Task<T> LoadAsync<T>(Guid id) where T : IHaveId, new();

    Task<List<T>> LoadAllAsync<T>() where T : IHaveId, new();

    Task StoreAsync<T>(T t) where T : IHaveId, new();

    Task DeleteAsync<T>(T t) where T : IHaveId, new();

    Task SaveChangesAsync();
}
