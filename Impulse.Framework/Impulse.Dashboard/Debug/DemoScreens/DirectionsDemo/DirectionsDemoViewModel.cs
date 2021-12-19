// <copyright file="DirectionsDemoViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using DynamicData;
using Impulse.Shared.Datastructures;
using Impulse.Shared.Enums;
using Impulse.Shared.Services;
using Impulse.SharedFramework.Reactive;
using ReactiveUI;

namespace Impulse.Dashboard.Debug.DemoScreens.DirectionsDemo
{
    public class DirectionsDemoViewModel : ReactiveScreen
    {
        private IGoogleApiService googleApiService;

        public DirectionsDemoViewModel(IGoogleApiService googleApiService)
        {
            DisplayName = "Directions Demo";

            this.googleApiService = googleApiService;

            SuggestedFirstAddresses = new ObservableCollection<string>();
            SuggestedSecondAddresses = new ObservableCollection<string>();
            SpokenDirections = new ObservableCollection<string>();

            GoCommand = ReactiveCommand.Create(() => GoAsync());

            this.WhenAnyValue(i => i.FirstAddress).Subscribe(async address =>
            {
                await UpdateSuggestions(FirstAddress, SuggestedFirstAddresses);
            });

            this.WhenAnyValue(i => i.SecondAddress).Subscribe(async address =>
            {
                await UpdateSuggestions(SecondAddress, SuggestedSecondAddresses);
            });

            this.WhenAnyValue(i => i.SelectedFirstAddress).Subscribe(async SelectedSuggestedAddress =>
            {
                if (!string.IsNullOrWhiteSpace(SelectedSuggestedAddress))
                {
                    FirstAddress = SelectedSuggestedAddress;
                    SuggestedFirstAddresses.Clear();
                }
            });

            this.WhenAnyValue(i => i.SelectedSecondAddress).Subscribe(async SelectedSuggestedAddress =>
            {
                if (!string.IsNullOrWhiteSpace(SelectedSuggestedAddress))
                {
                    SecondAddress = SelectedSuggestedAddress;
                    SuggestedSecondAddresses.Clear();
                }
            });
        }

        public ObservableCollection<string> SuggestedFirstAddresses { get; set; }

        public ObservableCollection<string> SpokenDirections { get; set; }

        public string SelectedFirstAddress { get; set; }

        public string FirstAddress { get; set; }

        public ObservableCollection<string> SuggestedSecondAddresses { get; set; }

        public string SelectedSecondAddress { get; set; }

        public string SecondAddress { get; set; }

        public ICommand GoCommand { get; set; }

        public bool AreFirstSuggestionsVisible =>
            !SuggestedFirstAddresses.Contains(FirstAddress) &&
            !string.IsNullOrEmpty(FirstAddress);

        public bool AreSecondSuggestionsVisible =>
            !SuggestedSecondAddresses.Contains(SecondAddress) &&
            !string.IsNullOrEmpty(SecondAddress);

        private async Task UpdateSuggestions(string address, ObservableCollection<string> addresses)
        {
            var suggestions = await CalculateSuggestedAddresses(address);

            addresses.Clear();
            foreach (var suggestion in suggestions)
            {
                addresses.Add(suggestion.FormattedAddress);
            }
        }

        private async Task<IEnumerable<Address>> CalculateSuggestedAddresses(string userInput)
        {
            return await googleApiService.ListPlacePredictionAddressesAsync(userInput);
        }

        private async Task GoAsync()
        {
            var stopwatch = Stopwatch.StartNew();

            if (!string.IsNullOrWhiteSpace(FirstAddress) && !string.IsNullOrWhiteSpace(SecondAddress))
            {
                var waypoints = new List<string> { };

                var settings = this.googleApiService.GetDirectionsApiSettings();
                settings.OptimiseWaypoints = true;
                this.googleApiService.SetDirectionsApiSettings(settings);

                var directions = await googleApiService.GetSpokenDirectionsAsync(
                    FirstAddress,
                    SecondAddress,
                    WaypointMode.ADDRESS,
                    waypoints);

                this.SpokenDirections.Clear();
                this.SpokenDirections.AddRange(directions);
            }

            stopwatch.Stop();
        }
    }
}
