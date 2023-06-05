// <copyright file="MonoViewerView.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using Impulse.Viewer.Mono.MonoGameWrapper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Impulse.Viewer.Mono.ViewerControl;

public class MonoViewerView : ContentControl, IDisposable
{
    private static readonly MonoGameGraphicsDeviceService GraphicsDeviceService = new MonoGameGraphicsDeviceService();
    private readonly GameTime gameTime = new GameTime();
    private readonly Stopwatch stopwatch = new Stopwatch();

    private int instanceCount;
    private IMonoGameViewModel viewModel;
    private D3DImage direct3DImage;
    private RenderTarget2D renderTarget;
    private SharpDX.Direct3D9.Texture renderTargetD3D9;
    private bool isFirstLoad = true;
    private bool isInitialized;

    public MonoViewerView()
    {
        if (DesignerProperties.GetIsInDesignMode(this))
        {
            return;
        }

        instanceCount++;
        Loaded += OnLoaded;
        DataContextChanged += (sender, args) =>
        {
            viewModel = args.NewValue as IMonoGameViewModel;

            if (viewModel != null)
            {
                viewModel.GraphicsDeviceService = GraphicsDeviceService;
            }
        };
        SizeChanged += (sender, args) => viewModel?.SizeChanged(sender, args);
    }

    ~MonoViewerView()
    {
        Dispose(false);
    }

    public static GraphicsDevice GraphicsDevice => GraphicsDeviceService?.GraphicsDevice;

    public bool IsDisposed { get; private set; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected override void OnGotFocus(RoutedEventArgs e)
    {
        viewModel?.OnActivated(this, EventArgs.Empty);
        base.OnGotFocus(e);
    }

    protected override void OnLostFocus(RoutedEventArgs e)
    {
        viewModel?.OnDeactivated(this, EventArgs.Empty);
        base.OnLostFocus(e);
    }

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        base.OnRenderSizeChanged(sizeInfo);

        // sometimes OnRenderSizeChanged happens before OnLoaded.
        Start();
        ResetBackBufferReference();
    }

    private void Dispose(bool disposing)
    {
        if (IsDisposed)
        {
            return;
        }

        viewModel?.Dispose();
        renderTarget?.Dispose();
        renderTargetD3D9?.Dispose();
        instanceCount--;

        if (instanceCount <= 0)
        {
            GraphicsDeviceService?.Dispose();
        }

        IsDisposed = true;
    }

    private void Start()
    {
        if (isInitialized)
        {
            return;
        }

        if (Application.Current.MainWindow == null)
        {
            throw new InvalidOperationException("The application must have a MainWindow");
        }

        // :TODO: This logic should not be handled by this window but rather the parent that embeds it.
        Application.Current.MainWindow.Closing += (sender, args) => viewModel?.OnExiting(this, EventArgs.Empty);

        direct3DImage = new D3DImage();

        AddChild(new Image { Source = direct3DImage, Stretch = Stretch.None });

        // _direct3DImage.IsFrontBufferAvailableChanged += OnDirect3DImageIsFrontBufferAvailableChanged;
        renderTarget = CreateRenderTarget();
        CompositionTarget.Rendering += OnRender;
        stopwatch.Start();
        isInitialized = true;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        Start();

        if (isFirstLoad)
        {
            GraphicsDeviceService.StartDirect3D(Application.Current.MainWindow);
            viewModel?.Initialize();
            viewModel?.LoadContent();
            isFirstLoad = false;
        }
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        viewModel?.UnloadContent();

        if (GraphicsDeviceService != null)
        {
            CompositionTarget.Rendering -= OnRender;
            ResetBackBufferReference();
            GraphicsDeviceService.DeviceResetting -= OnGraphicsDeviceServiceDeviceResetting;
        }
    }

    private void OnGraphicsDeviceServiceDeviceResetting(object sender, EventArgs e)
    {
        ResetBackBufferReference();
    }

    private void ResetBackBufferReference()
    {
        if (DesignerProperties.GetIsInDesignMode(this))
        {
            return;
        }

        if (renderTarget != null)
        {
            renderTarget.Dispose();
            renderTarget = null;
        }

        if (renderTargetD3D9 != null)
        {
            renderTargetD3D9.Dispose();
            renderTargetD3D9 = null;
        }

        direct3DImage.Lock();
        direct3DImage.SetBackBuffer(D3DResourceType.IDirect3DSurface9, IntPtr.Zero);
        direct3DImage.Unlock();
    }

    private RenderTarget2D CreateRenderTarget()
    {
        var actualWidth = (int)ActualWidth;
        var actualHeight = (int)ActualHeight;

        if (actualWidth == 0 || actualHeight == 0)
        {
            return null;
        }

        if (GraphicsDevice == null)
        {
            return null;
        }

        var renderTarget = new RenderTarget2D(
            GraphicsDevice,
            actualWidth,
            actualHeight,
            false,
            SurfaceFormat.Bgra32,
            DepthFormat.Depth24Stencil8,
            1,
            RenderTargetUsage.PlatformContents,
            true);

        var handle = renderTarget.GetSharedHandle();

        if (handle == IntPtr.Zero)
        {
            throw new ArgumentException("Handle could not be retrieved");
        }

        renderTargetD3D9 = new SharpDX.Direct3D9.Texture(
            GraphicsDeviceService.Direct3DDevice,
            renderTarget.Width,
            renderTarget.Height,
            1,
            SharpDX.Direct3D9.Usage.RenderTarget,
            SharpDX.Direct3D9.Format.A8R8G8B8,
            SharpDX.Direct3D9.Pool.Default,
            ref handle);

        using (var surface = renderTargetD3D9.GetSurfaceLevel(0))
        {
            direct3DImage.Lock();
            direct3DImage.SetBackBuffer(D3DResourceType.IDirect3DSurface9, surface.NativePointer);
            direct3DImage.Unlock();
        }

        return renderTarget;
    }

    private void OnRender(object sender, EventArgs e)
    {
        gameTime.ElapsedGameTime = stopwatch.Elapsed;
        gameTime.TotalGameTime += gameTime.ElapsedGameTime;
        stopwatch.Restart();

        if (CanBeginDraw())
        {
            try
            {
                direct3DImage.Lock();

                if (renderTarget == null)
                {
                    renderTarget = CreateRenderTarget();
                }

                if (renderTarget != null)
                {
                    GraphicsDevice.SetRenderTarget(renderTarget);
                    SetViewport();

                    viewModel?.Update(gameTime);
                    viewModel?.Draw(gameTime);

                    GraphicsDevice.Flush();
                    direct3DImage.AddDirtyRect(new Int32Rect(0, 0, (int)ActualWidth, (int)ActualHeight));
                }
            }
            finally
            {
                direct3DImage.Unlock();
                GraphicsDevice.SetRenderTarget(null);
            }
        }
    }

    private bool CanBeginDraw()
    {
        // If we have no graphics device, we must be running in the designer.
        if (GraphicsDeviceService == null)
        {
            return false;
        }

        if (!direct3DImage.IsFrontBufferAvailable)
        {
            return false;
        }

        // Make sure the graphics device is big enough, and is not lost.
        if (!HandleDeviceReset())
        {
            return false;
        }

        return true;
    }

    private void SetViewport()
    {
        // Many GraphicsDeviceControl instances can be sharing the same
        // GraphicsDevice. The device backbuffer will be resized to fit the
        // largest of these controls. But what if we are currently drawing
        // a smaller control? To avoid unwanted stretching, we set the
        // viewport to only use the top left portion of the full backbuffer.
        var width = Math.Max(1, (int)ActualWidth);
        var height = Math.Max(1, (int)ActualHeight);
        GraphicsDevice.Viewport = new Viewport(0, 0, width, height);
    }

    private bool HandleDeviceReset()
    {
        if (GraphicsDevice == null)
        {
            return false;
        }

        var deviceNeedsReset = false;

        switch (GraphicsDevice.GraphicsDeviceStatus)
        {
            case GraphicsDeviceStatus.Lost:
                // If the graphics device is lost, we cannot use it at all.
                return false;

            case GraphicsDeviceStatus.NotReset:
                // If device is in the not-reset state, we should try to reset it.
                deviceNeedsReset = true;
                break;
        }

        if (deviceNeedsReset)
        {
            GraphicsDeviceService.ResetDevice((int)ActualWidth, (int)ActualHeight);
            return false;
        }

        return true;
    }
}
