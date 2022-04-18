// <copyright file="MonoGameViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Windows;
using Impulse.Shared.ReactiveUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Impulse.Mono.Viewer.MonoGameWrapper;

public class MonoGameViewModel : ReactiveScreen, IMonoGameViewModel
{
    public MonoGameViewModel()
    {
    }

    public IGraphicsDeviceService GraphicsDeviceService { get; set; }

    protected GraphicsDevice GraphicsDevice => GraphicsDeviceService?.GraphicsDevice;

    protected MonoGameServiceProvider Services { get; private set; }

    protected ContentManager Content { get; set; }

    public void Dispose()
    {
        Content?.Dispose();
    }

    public virtual void Initialize()
    {
        Services = new MonoGameServiceProvider();
        Services.AddService(GraphicsDeviceService);
        Content = new ContentManager(Services) { RootDirectory = "Content" };
    }

    public virtual void LoadContent() { }

    public virtual void UnloadContent() { }

    public virtual void Update(GameTime gameTime) { }

    public virtual void Draw(GameTime gameTime) { }

    public virtual void OnActivated(object sender, EventArgs args) { }

    public virtual void OnDeactivated(object sender, EventArgs args) { }

    public virtual void OnExiting(object sender, EventArgs args) { }

    public virtual void SizeChanged(object sender, SizeChangedEventArgs args) { }
}
