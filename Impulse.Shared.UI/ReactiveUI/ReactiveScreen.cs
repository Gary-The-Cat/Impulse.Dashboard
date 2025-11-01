// <copyright file="ReactiveScreen.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.Windows.Input;
using Caliburn.Micro;
using PropertyChanged;

namespace Impulse.Shared.ReactiveUI;

public abstract class ReactiveScreen : Screen, IChangeTracking
{
    public ReactiveScreen()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; set; }

    [DoNotSetChanged]
    public virtual bool IsChanged { get; set; }

    [DoNotSetChanged]
    public bool IsLoaded { get; set; }

    [DoNotSetChanged]
    public bool IsDirty => IsChanged && IsLoaded;

    [DoNotSetChanged]
    public override string DisplayName { get; set; }

    [DoNotSetChanged]
    public ICommand SaveCommand { get; set; }

    [DoNotSetChanged]
    public ICommand LoadCommand { get; set; }

    [DoNotSetChanged]
    public bool IsSelected { get; set; }

    public virtual void AcceptChanges()
    {
        this.IsLoaded = true;
        this.IsChanged = false;
    }

    public void SaveAsync()
    {
        this.AcceptChanges();
    }

    protected override async Task OnActivateAsync(CancellationToken ct)
    {
        await base.OnActivateAsync(ct);

        if (!IsLoaded)
        {
            LoadCommand?.Execute(null);
        }
    }

    protected override async Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
    {
        await base.OnDeactivateAsync(close, cancellationToken);

        if (IsLoaded && IsChanged)
        {
            SaveCommand?.Execute(null);
        }
    }
}
