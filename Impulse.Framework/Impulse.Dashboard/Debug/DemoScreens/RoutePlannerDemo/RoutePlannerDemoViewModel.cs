// <copyright file="RoutePlannerDemoViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Impulse.Shared.Datastructures;
using Impulse.Shared.Services;
using Impulse.SharedFramework.Controls.AddressCompletion;
using Impulse.SharedFramework.Services;
using Impulse.SharedFramework.Services.Layout;
using Impulse.SharedFramework.ToastNotifications;
using Ninject;
using ReactiveUI;

namespace Impulse.Dashboard.Debug.DemoScreens.RoutePlannerDemo
{
    public class RoutePlannerDemoViewModel : DocumentBase
    {
        private readonly IGoogleApiService googleApiService;
        private readonly IDialogService dialogService;

        public RoutePlannerDemoViewModel(
            IKernel kernel,
            IGoogleApiService googleApiService,
            IDialogService dialogService) : base(kernel)
        {
            // Set the documents display name
            DisplayName = "Route Planner Demo";

            // Grab a reference to our service
            this.googleApiService = googleApiService;
            this.dialogService = dialogService;

            // Initialize the suggested addresses list, slightly faster than new'ing on update
            Addresses = new ObservableCollection<Address>();

            this.OptimizeCommand = ReactiveCommand.Create(() => this.OptimizeAsync());
            this.RemoveCommand = ReactiveCommand.Create(() => Addresses.Remove(SelectedAddress));

            // The current address value has been changed, update the suggested addresses
            StartLocationAutoComplete = kernel.Get<AddressCompletionViewModel>();
            StartLocationAutoComplete.WhenAnyValue(i => i.SelectedSuggestedAddress).Subscribe(address =>
            {
                StartAddress = address;
            });

            EndLocationAutoComplete = kernel.Get<AddressCompletionViewModel>();
            EndLocationAutoComplete.WhenAnyValue(i => i.SelectedSuggestedAddress).Subscribe(address =>
            {
                EndAddress = address;
            });

            StopLocationAutoComplete = kernel.Get<AddressCompletionViewModel>();
            StopLocationAutoComplete.WhenAnyValue(i => i.SelectedSuggestedAddress)
                .Where(a => a != null)
                .Subscribe(async suggestedAddresses =>
                {
                    await AddAsync(suggestedAddresses);

                    StopLocationAutoComplete.CurrentAddress = null;
                });
        }

        public AddressCompletionViewModel StartLocationAutoComplete { get; set; }

        public AddressCompletionViewModel StopLocationAutoComplete { get; set; }

        public AddressCompletionViewModel EndLocationAutoComplete { get; set; }

        // Address Entry
        public Address StartAddress { get; set; }

        public Address EndAddress { get; set; }

        public Address SelectedAddress { get; set; }

        public bool IsAddressSelected => SelectedAddress != null;

        public ICommand OptimizeCommand { get; set; }

        public ICommand RemoveCommand { get; set; }

        public ObservableCollection<Address> Addresses { get; set; }

        public bool IsBusy { get; set; }

        private async Task AddAsync(Address address)
        {
            if (await ValidateAddress(address))
            {
                Addresses.Add(address);
            }
        }

        private async Task OptimizeAsync()
        {
            IsBusy = true;

            var tempAddresses = Addresses.ToList();
            tempAddresses.AddRange(new[] { StartAddress, EndAddress });

            var travelCosts = await GetTravelCostLookup(tempAddresses.Select(a => a.PlaceId).Distinct());

            double GetTravelCost((string, string) key)
            {
                return travelCosts[key];
            }

            var solver = new RoutePlanner(
                StartAddress.PlaceId,
                EndAddress.PlaceId,
                Addresses.Select(a => a.PlaceId).ToList(),
                GetTravelCost);

            solver.ReportProgress = ReportProgress;

            await solver.Solve();

            IsBusy = false;
        }

        private void ReportProgress(double value)
        {
            dialogService.ShowProgressMessage("Best Fitness: " + value.ToString("0.00"));
        }

        private async Task<Dictionary<(string, string), double>> GetTravelCostLookup(IEnumerable<string> addresses)
        {
            var lookup = new Dictionary<(string, string), double>();

            foreach (var addressA in addresses)
            {
                foreach (var addressB in addresses)
                {
                    if (addressA.Equals(addressB))
                    {
                        continue;
                    }

                    var directions = await googleApiService.GetDirectionInformationAsync(addressA, addressB, Impulse.Shared.Enums.WaypointMode.PLACE_ID);
                    if (!directions.Any())
                    {
                        throw new ArgumentException("No valid directions found.");
                    }

                    var key = (addressA, addressB);
                    lookup[key] = directions.First().Duration;
                }
            }

            return lookup;
        }

        private async Task<bool> ValidateAddress(Address currentAddress)
        {
            if (Addresses.Contains(currentAddress))
            {
                dialogService.ShowToast("Address already added", ToastType.Error);
                return false;
            }

            return true;
        }

        private async Task<IEnumerable<Address>> CalculateSuggestedAddresses(string userInput)
        {
            return await googleApiService.ListPlacePredictionAddressesAsync(userInput);
        }
    }
}