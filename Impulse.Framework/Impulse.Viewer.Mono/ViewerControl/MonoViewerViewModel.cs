﻿// <copyright file="MonoViewerViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Impulse.Viewer.Mono.MonoGameWrapper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Impulse.Viewer.Mono.ViewerControl;

public class MonoViewerViewModel : MonoGameViewModel
{
    private Renderer renderer;

    public MonoViewerViewModel()
    {
    }

    public override void Initialize()
    {
        renderer = new Renderer(GraphicsDevice);
        base.Initialize();
    }

    public override void LoadContent()
    {
        var model = Content.Load<Model>("Icosphere");
        renderer.AddItem(model);
    }

    public override void Update(GameTime gameTime)
    {
        renderer.OnResize(GraphicsDevice.Viewport.AspectRatio);

        renderer.Update(gameTime.ElapsedGameTime.Milliseconds / 1000f);

        base.Update(gameTime);
    }

    public void Draw()
    {
        renderer.Draw();
    }

    public override void Draw(GameTime gameTime)
    {
        renderer.Draw();

        base.Draw(gameTime);
    }

    public void OnResize()
    {
        renderer.OnResize(GraphicsDevice.Viewport.AspectRatio);
    }
}
