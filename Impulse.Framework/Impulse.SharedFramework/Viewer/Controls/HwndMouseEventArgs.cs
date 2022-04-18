// <copyright file="HwndMouseEventArgs.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Windows;
using System.Windows.Input;

namespace Impulse.SharedFramework.Viewer.Controls;

public class HwndMouseEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HwndMouseEventArgs"/> class.
    /// </summary>
    /// <param name="state">The state from which to initialize the properties.</param>
    public HwndMouseEventArgs(HwndMouseState state)
    {
        LeftButton = state.LeftButton;
        RightButton = state.RightButton;
        MiddleButton = state.MiddleButton;
        X1Button = state.X1Button;
        X2Button = state.X2Button;
        ScreenPosition = state.ScreenPosition;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HwndMouseEventArgs"/> class.
    /// </summary>
    /// <param name="state">The state from which to initialize the properties.</param>
    /// <param name="mouseWheelDelta">The mouse wheel rotation delta.</param>
    /// <param name="mouseHWheelDelta">The horizontal mouse wheel delta.</param>
    public HwndMouseEventArgs(HwndMouseState state, int mouseWheelDelta, int mouseHWheelDelta)
        : this(state)
    {
        WheelDelta = mouseWheelDelta;
        HorizontalWheelDelta = mouseHWheelDelta;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HwndMouseEventArgs"/> class.
    /// </summary>
    /// <param name="state">The state from which to initialize the properties.</param>
    /// <param name="doubleClickButton">The button that was double clicked.</param>
    public HwndMouseEventArgs(HwndMouseState state, MouseButton doubleClickButton)
        : this(state)
    {
        DoubleClickButton = doubleClickButton;
    }

    /// <summary>
    /// Gets the state of the left mouse button.
    /// </summary>
    public MouseButtonState LeftButton { get; private set; }

    /// <summary>
    /// Gets the state of the right mouse button.
    /// </summary>
    public MouseButtonState RightButton { get; private set; }

    /// <summary>
    /// Gets the state of the middle mouse button.
    /// </summary>
    public MouseButtonState MiddleButton { get; private set; }

    /// <summary>
    /// Gets the state of the first extra mouse button.
    /// </summary>
    public MouseButtonState X1Button { get; private set; }

    /// <summary>
    /// Gets the state of the second extra mouse button.
    /// </summary>
    public MouseButtonState X2Button { get; private set; }

    /// <summary>
    /// Gets the button that was double clicked.
    /// </summary>
    public MouseButton? DoubleClickButton { get; private set; }

    /// <summary>
    /// Gets the mouse wheel delta.
    /// </summary>
    public int WheelDelta { get; private set; }

    /// <summary>
    /// Gets the horizontal mouse wheel delta.
    /// </summary>
    public int HorizontalWheelDelta { get; private set; }

    /// <summary>
    /// Gets the position of the mouse in screen coordinates.
    /// </summary>
    public Point ScreenPosition { get; private set; }

    public Point GetPosition(UIElement relativeTo)
    {
        return relativeTo.PointFromScreen(ScreenPosition);
    }
}
