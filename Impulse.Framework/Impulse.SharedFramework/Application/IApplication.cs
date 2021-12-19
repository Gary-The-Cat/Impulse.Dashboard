// <copyright file="IApplication.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;

namespace Impulse.SharedFramework.Application
{
    public interface IApplication
    {
        string DisplayName { get; }

        Uri Icon { get; }

        Task LaunchApplication();

        Task Initialize();

        Task OnClose();
    }
}
