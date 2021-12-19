// <copyright file="ISerializationService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;

namespace Impulse.SharedFramework.Services
{
    public interface ISerializationService
    {
        IUnitOfWork GetUnitOfWork();

        IUnitOfWork GetUnitOfWork(string xmlFileSource);

        void GetDefaultSource(Func<string> getSource);
    }
}
