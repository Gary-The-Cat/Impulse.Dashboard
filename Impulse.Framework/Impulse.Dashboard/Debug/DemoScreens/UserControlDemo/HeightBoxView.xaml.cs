// <copyright file="HeightBoxView.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Windows.Controls;

namespace Impulse.Dashboard.Debug.DemoScreens.UserControlDemo
{
    /// <summary>
    /// Interaction logic for HeightBox.xaml
    /// </summary>
    public partial class HeightBoxView : UserControl
    {
        public HeightBoxView()
        {
            InitializeComponent();

            HeightBoxViewModel viewModel = new HeightBoxViewModel();
            viewModel.Name = "Taylor";
            viewModel.HeightValue = 165;

            HeightBoxViewModel lukesViewModel = new HeightBoxViewModel();
            lukesViewModel.Name = "Luke";
            lukesViewModel.HeightValue = 175;

            DataContext = lukesViewModel;
        }
    }
}
