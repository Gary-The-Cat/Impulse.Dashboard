// <copyright file="ToastViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Caliburn.Micro;

namespace Impulse.SharedFramework.ToastNotifications
{
    public class ToastViewModel : Screen
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public ToastType Type { get; set; }
    }
}
