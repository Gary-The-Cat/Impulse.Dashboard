// <copyright file="IShellViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Impulse.Shared.Application;

namespace Impulse.SharedFramework.Shell;

public interface IShellViewModel
{
    IApplication ActiveApplication { get; set; }
}
