using SFML.Graphics;
using SFML.System;
using System;
using System.Windows;

namespace Impulse.SFML.Viewer.Viewer
{
    /// <summary>
    /// Interaction logic for SfmlViewerView.xaml
    /// </summary>
    public class SfmlViewerView : ViewerView
    {
        public SfmlViewerView()
        {
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            base.OnInitialized(e);

            ViewModel.Initialize(this.Handle);

            this.isLoaded = true;
            Loaded -= OnLoaded;
        }

        public SfmlViewerViewModel ViewModel => (SfmlViewerViewModel)this.DataContext;

        private bool isLoaded = false;

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            if (!isLoaded)
            {
                return;
            }

            base.OnRenderSizeChanged(sizeInfo);

            var fWidth = (float)sizeInfo.NewSize.Width;
            var fHeight= (float)sizeInfo.NewSize.Height;

            this.ViewModel.RenderWindow.SetView(new View(
                new Vector2f(fWidth / 2, fHeight / 2),
                new Vector2f(fWidth, fHeight)));
        }

        protected override void Render(IntPtr _)
        {
            this.ViewModel.OnRender();
        }
    }
}