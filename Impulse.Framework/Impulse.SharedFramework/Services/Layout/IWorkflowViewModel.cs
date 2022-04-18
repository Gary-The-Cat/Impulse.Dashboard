// <copyright file="IWorkflowViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Impulse.SharedFramework.Services.Layout;

public interface IWorkflowViewModel
{
    T GetValue<T>(string propertyName);
}
