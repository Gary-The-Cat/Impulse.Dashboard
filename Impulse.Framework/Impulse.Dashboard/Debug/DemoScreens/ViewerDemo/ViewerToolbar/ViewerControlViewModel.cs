// <copyright file="ViewerControlViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.IO;
using Impulse.SharedFramework.Services.Layout;
using Impulse.Viewer.ViewerControl;
using Microsoft.Win32;
using PropertyChanged;
using ReactiveUI;

namespace Impulse.Dashboard.Debug.DemoScreens.ViewerDemo.ViewerToolbar;

public class ViewerControlViewModel : ToolWindowBase
{
    private readonly ViewerViewModel viewer;

    public ViewerControlViewModel(
        ViewerViewModel viewer)
    {
        DisplayName = "Viewer Toolbar";
        this.viewer = viewer;
        SliderValue = 1f;

        Placement = Impulse.Shared.Enums.ToolWindowPlacement.Right;

        this.viewer.SetFrameTime = s =>
        {
            FrameTime = $"{s}ms";
        };

        this.WhenAnyValue(i => i.SliderValue).Subscribe(value =>
        {
            this.viewer.SetVertexColour((float)value);
        });
    }

    public double SliderValue { get; set; }

    [DoNotSetChanged]
    public string FrameTime { get; set; }

    public void LoadNewObjFile()
    {
        var openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Obj files (*.obj)|*.obj|All files (*.*)|*.*";
        openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();

        // If the user has selected a file
        if (openFileDialog.ShowDialog() == true && openFileDialog.CheckFileExists)
        {
            // Pass the file to the viewer to place and render
        }
    }
}
