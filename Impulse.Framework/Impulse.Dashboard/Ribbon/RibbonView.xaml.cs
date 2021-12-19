// <copyright file="RibbonView.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Windows.Controls;

namespace Impulse.Dashboard.Ribbon
{
    /// <summary>
    /// Interaction logic for RibbonView.xaml
    /// </summary>
    public partial class RibbonView : UserControl
    {
        public RibbonView()
        {
            InitializeComponent();
        }

        public Fluent.Ribbon FluentRibbon => this.MainRibbon;
    }
}
