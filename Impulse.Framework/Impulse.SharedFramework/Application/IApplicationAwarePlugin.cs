// <copyright file="IApplicationAwarePlugin.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Threading.Tasks;

namespace Impulse.SharedFramework.Application;

public interface IApplicationAwarePlugin
{
    Task OnActiveApplicationChanged(IApplication activeApplication);
}
