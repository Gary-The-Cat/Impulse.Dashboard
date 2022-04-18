// <copyright file="GoogleApiDemoViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Impulse.Shared.Datastructures;
using Impulse.Shared.Services;
using Impulse.SharedFramework.Services.Layout;
using Ninject;
using ReactiveUI;

namespace Impulse.Dashboard.Debug.DemoScreens.GoogleApiDemo;

public class GoogleApiDemoViewModel : DocumentBase
{
    private readonly IGoogleApiService googleApiService;

    public GoogleApiDemoViewModel(IKernel kernel, IGoogleApiService googleApiService) : base(kernel)
    {
        // Set the documents display name
        DisplayName = "Google Api Demo";

        // Grab a reference to our service
        this.googleApiService = googleApiService;

        // Initialize the suggested addresses list, slightly faster than new'ing on update
        SuggestedAddresses = new ObservableCollection<string>();

        // The current address value has been changed, update the suggested addresses
        this.WhenAnyValue(i => i.CurrentAddress).Subscribe(async _ =>
        {
            var suggestions = await CalculateSuggestedAddresses(CurrentAddress);

            SuggestedAddresses.Clear();
            foreach (var suggestion in suggestions)
            {
                SuggestedAddresses.Add(suggestion.FormattedAddress);
            }
        });
    }

    public ObservableCollection<string> SuggestedAddresses { get; set; }

    public string CurrentAddress { get; set; }

    private async Task<IEnumerable<Address>> CalculateSuggestedAddresses(string userInput)
    {
        return await googleApiService.ListPlacePredictionAddressesAsync(userInput);
    }
}
