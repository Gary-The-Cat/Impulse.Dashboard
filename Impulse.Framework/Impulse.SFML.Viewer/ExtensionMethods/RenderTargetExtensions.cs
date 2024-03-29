﻿using SFML.Graphics;
using SFML.System;

namespace Impulse.SFML.Viewer.ExtensionMethods;

public static class RenderTargetExtensions
{
    public static void Draw(this RenderTarget target, Texture texture, IntRect region, Vector2f position, float rotation = 0.0f, float scale = 1.0f)
    {
        Sprite sprite = new Sprite(texture, region);
        Draw(target, sprite, position, rotation, scale);
    }

    public static void Draw(this RenderTarget target, Texture texture, Vector2f position, float rotation = 0.0f, float scale = 1.0f)
    {
        IntRect region = new IntRect(0, 0, (int)texture.Size.X, (int)texture.Size.X);
        Draw(target, texture, region, position, rotation, scale);
    }

    public static void Draw(this RenderTarget target, Sprite sprite, Vector2f position, float rotation = 0.0f, float scale = 1.0f)
    {
        sprite.Position = position;
        sprite.Rotation = rotation;
        sprite.Scale = new Vector2f(scale, scale);
        target.Draw(sprite);
    }

    public static void DrawString(this RenderTarget target, Text text, bool centred = true)
    {
        if (centred)
        {
            var size = text.GetLocalBounds();
            text.Origin = new Vector2f(size.Width / 2, size.Height / 2);
        }

        target.Draw(text);
    }
}
