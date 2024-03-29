﻿using PropertyChanged;

namespace Impulse.Shared.ReactiveUI;

public class ChangableItem : ReactiveViewModelBase
{
    [DoNotSetChanged]
    public virtual bool IsChanged { get; set; }

    [DoNotSetChanged]
    public bool IsLoaded { get; set; }

    [DoNotSetChanged]
    public bool IsDirty => IsChanged && IsLoaded;

    public void AcceptChanges()
    {
        this.IsLoaded = true;
        this.IsChanged = false;
    }
}
