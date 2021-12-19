// <copyright file="WorkflowViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using Impulse.Dashboard.Services.Workflow.WorkflowTabBadge;
using Impulse.SharedFramework.Attributes;
using Impulse.SharedFramework.ExtensionMethods;
using Impulse.SharedFramework.Services.Layout;

namespace Impulse.Dashboard.Services.Workflow
{
    public class WorkflowViewModel : Conductor<WorkflowTabBase>.Collection.OneActive, IWorkflowViewModel
    {
        private readonly WorkflowTabBase[] workflowTabs;

        public WorkflowViewModel(params WorkflowTabBase[] workflowTabs)
        {
            this.workflowTabs = workflowTabs;
            Badges = new ObservableCollection<WorkflowTabBadgeViewModel>();

            Items.AddRange(workflowTabs);
            ActiveItem = workflowTabs.First();
        }

        public ObservableCollection<WorkflowTabBadgeViewModel> Badges { get; set; }

        public uint Width { get; set; } = 600;

        public uint Height { get; set; } = 450;

        public override string DisplayName
        {
            get => ActiveItem.DisplayName;
            set => value = string.Empty;
        }

        public T GetValue<T>(string propertyName)
        {
            foreach (var workflowTab in workflowTabs)
            {
                // Loop over each of the properties in this type that are flagged as export
                foreach (var property in workflowTab.GetType().GetProperties()
                    .Where(p => Attribute.IsDefined(p, typeof(ExportProperty))))
                {
                    // If the name and type match, get the value from the tab and return it
                    if (property.Name.Equals(propertyName) && property.PropertyType == typeof(T))
                    {
                        return workflowTab.GetPropertyValue<T>(propertyName);
                    }
                }
            }

            return default;
        }

        public void NextClick()
        {
            var preChangeIndex = Items.IndexOf(ActiveItem);
            int postChangeIndex = preChangeIndex + 1;

            if (postChangeIndex >= Items.Count)
            {
                TryClose(true);
            }
            else
            {
                ActiveItem = Items[postChangeIndex];
                SetProgress(preChangeIndex, postChangeIndex);
            }
        }

        public void BackClick()
        {
            var preChangeIndex = Items.IndexOf(ActiveItem);
            int postChangeIndex = preChangeIndex - 1;

            if (postChangeIndex < 0)
            {
                TryClose(false);
            }
            else
            {
                ActiveItem = Items[postChangeIndex];
                SetProgress(preChangeIndex, postChangeIndex);
            }
        }

        public void SetProgress(int preChangeIndex, int postChangeIndex)
        {
            Badges[preChangeIndex].IsSelected = false;
            Badges[postChangeIndex].IsSelected = true;

            NotifyOfPropertyChange(nameof(DisplayName));
        }
    }
}