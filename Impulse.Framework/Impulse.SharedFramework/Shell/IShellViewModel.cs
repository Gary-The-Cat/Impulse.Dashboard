// <copyright file="IShellViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Impulse.SharedFramework.Application;

namespace Impulse.SharedFramework.Shell;

public interface IShellViewModel
{
    IApplication ActiveApplication { get; set; }
}
