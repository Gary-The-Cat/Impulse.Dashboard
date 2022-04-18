// <copyright file="HwndMouseState.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Windows;
using System.Windows.Input;

namespace Impulse.SharedFramework.Viewer.Controls;

public class HwndMouseState
{
    public MouseButtonState LeftButton { get; set; }

    public MouseButtonState RightButton { get; set; }

    public MouseButtonState MiddleButton { get; set; }

    public MouseButtonState X1Button { get; set; }

    public MouseButtonState X2Button { get; set; }

    public Point ScreenPosition { get; set; }
}
