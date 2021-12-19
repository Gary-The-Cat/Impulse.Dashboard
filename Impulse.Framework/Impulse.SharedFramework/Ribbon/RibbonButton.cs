// <copyright file="RibbonButton.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Impulse.Shared.ReactiveUI;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Impulse.SharedFramework.Ribbon
{
    public class RibbonButton : ReactiveViewModelBase
    {
        private ReactiveScreen context;

        public RibbonButton()
        {
        }

        public string Title { get; set; } = "Not Set";

        public string Id { get; set; }

        public string Icon => IsEnabled ? EnabledIcon : DisabledIcon;

        public string DisabledIcon { get; set; }

        public string EnabledIcon { get; set; }

        public bool IsEnabled { get; set; } = true;

        public string EnabledPropertyName { get; set; }

        public ReactiveScreen Context
        {
            get => context;
            set
            {
                context = value;
                Context.PropertyChanged += EnabledCallback;
            }
        }

        public Action Callback { get; set; } = () => { };

        public ICommand Command { get; set; }

        public Func<object, bool> PropertyConverter { get; set; } =
            o =>
            {
                if (bool.TryParse(o.ToString(), out var result))
                {
                    return result;
                }

                return false;
            };

        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        private void EnabledCallback(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(EnabledPropertyName))
            {
                var value = PropertyConverter(GetPropValue(sender, e.PropertyName));
                IsEnabled = value;
            }
        }
    }
}
