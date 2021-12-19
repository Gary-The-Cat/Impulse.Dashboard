// <copyright file="ViewerViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using Caliburn.Micro;
using Impulse.Renderer;
using Impulse.Shared.Viewer;
using Impulse.SharedFramework.Viewer;
using Veldrid;

namespace Impulse.Viewer.ViewerControl
{
    public class ViewerViewModel : Screen, IViewer
    {
        private readonly Stopwatch stopwatch;
        private readonly Stopwatch cameraWatch;
        private readonly Queue<int> frameRenderTimes;

        private GraphicsDevice device;
        private CommandList commandList;
        private BasicMeshRenderer renderer;
        private List<Mesh> meshes;
        private bool isActive;

        public ViewerViewModel()
        {
            stopwatch = new Stopwatch();
            cameraWatch = Stopwatch.StartNew();
            frameRenderTimes = new Queue<int>();
            meshes = new List<Mesh>();
        }

        public Action<string> SetFrameTime { get; set; }

        public string ModelFilePath { get; set; }

        public void OnSelected()
        {
            isActive = true;
        }

        public void OnDeselected()
        {
            isActive = false;
        }

        public void SetVertexColour(float sliderValue)
        {
        }

        public void AddMeshFromVertices(MeshVertex[] vertices)
        {
            uint[] indices = { 0, 1, 2 };
            meshes.Add(new Mesh(device, vertices, indices));
        }

        public void OnClosing()
        {
            var visual = (ViewerView)GetView();

            visual.OnClosing();
        }

        internal void Initialize()
        {
            var (width, height) = GetVisualSize();
            var visual = (ViewerView)GetView();
            var mainModule = typeof(ViewerView).Module;
            var hinstance = Marshal.GetHINSTANCE(mainModule);

            GraphicsDeviceOptions options = new GraphicsDeviceOptions
            {
                PreferStandardClipSpaceYDirection = true,
                PreferDepthRangeZeroToOne = true,
                SwapchainDepthFormat = Veldrid.PixelFormat.R16_UNorm
            };

            SwapchainDescription description = new SwapchainDescription(
                SwapchainSource.CreateWin32(visual.Hwnd, hinstance),
                width,
                height,
                options.SwapchainDepthFormat,
                options.SyncToVerticalBlank);

            device = GraphicsDevice.CreateD3D11(options, description);
            commandList = device.ResourceFactory.CreateCommandList();
            renderer = new BasicMeshRenderer(device);

            var currentDirectory = Directory.GetCurrentDirectory();
            var filePath = !string.IsNullOrEmpty(ModelFilePath)
                ? ModelFilePath
                : currentDirectory + "\\Models\\RD.Model.obj";

            // meshes = MeshFactory.ImportModel(device, filePath);
            meshes.Add(MeshFactory.CreateBox(device));
            meshes.Last().Transform.TranslateZ(2.5f);
            meshes.Add(MeshFactory.CreateBox(device));
            meshes.Last().Transform.TranslateZ(-2.5f);
            meshes.Add(MeshFactory.CreateBox(device));
            meshes.Last().Transform.TranslateX(2.5f);
            meshes.Add(MeshFactory.CreateBox(device));
            meshes.Last().Transform.TranslateX(-2.5f);

            OnSelected();
        }

        internal void OnResize()
        {
            // :TODO: When resize is added to the renderer interface, call it here.
            var size = GetVisualSize();
        }

        internal void Render()
        {
            UpdateFrameTime();

            double elapsed = cameraWatch.Elapsed.TotalSeconds;
            float x = (float)(5 * Math.Sin(elapsed));
            float z = (float)(5 * Math.Cos(elapsed));
            renderer.Camera.Position = new Vector3(x, 2, z);
            renderer.Camera.LookAt(Vector3.Zero);

            commandList.Begin();

            commandList.SetFramebuffer(device.SwapchainFramebuffer);
            commandList.ClearColorTarget(0, RgbaFloat.LightGrey);
            commandList.ClearDepthStencil(1f);

            renderer.Enable(commandList);
            meshes.ForEach(mesh => renderer.Draw(commandList, mesh));

            commandList.End();

            device.SubmitCommands(commandList);
            device.SwapBuffers();
            device.WaitForIdle();
        }

        private void OnRender(object sender, EventArgs e)
        {
            if (!isActive)
            {
                return;
            }

            Render();
        }

        // TODO replace this with a FrameCount class (scott)
        private void UpdateFrameTime()
        {
            // Get the current frametime
            var frameRenderTime = (int)stopwatch.ElapsedMilliseconds;

            // Add it to our stack (frame rates fluctuate way to much at idle and need to be averaged to get a good approximation)
            frameRenderTimes.Enqueue(frameRenderTime);

            // Formats the average for the last 30 frames to e.g. 16.15
            SetFrameTime?.Invoke(((float)frameRenderTimes.Average(t => t)).ToString("0.##"));

            // only keep the most recent 30 frames
            if (frameRenderTimes.Count > 30)
            {
                frameRenderTimes.Dequeue();
            }

            stopwatch.Restart();
        }

        private double GetDpiScale()
        {
            PresentationSource source = PresentationSource.FromVisual((Visual)this.GetView());
            return source.CompositionTarget.TransformToDevice.M11;
        }

        private (uint width, uint height) GetVisualSize()
        {
            var visual = (Win32HwndControl)GetView();
            double dpiScale = GetDpiScale();
            uint width = (uint)(visual.ActualWidth < 0 ? 0 : Math.Ceiling(visual.ActualWidth * dpiScale));
            uint height = (uint)(visual.ActualHeight < 0 ? 0 : Math.Ceiling(visual.ActualHeight * dpiScale));

            return (width, height);
        }
    }
}
