// <copyright file="AddressCompletionViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Impulse.Shared.Datastructures;
using Impulse.Shared.Services;
using Impulse.Shared.ReactiveUI;
using ReactiveUI;

namespace Impulse.SharedFramework.Controls.AddressCompletion;

public class AddressCompletionViewModel : ReactiveScreen
{
    private IGoogleApiService googleApiService;

    public AddressCompletionViewModel(IGoogleApiService googleApiService)
    {
        this.googleApiService = googleApiService;

        SuggestedAddresses = new ObservableCollection<Address>();

        this.WhenAnyValue(i => i.CurrentAddress).Where(a => a != null).Subscribe(async address =>
        {
            await UpdateSuggestedAddresses(address);
        });

        this.WhenAnyValue(i => i.SelectedSuggestedAddress).Subscribe(SelectedSuggestedAddress =>
        {
            if (SelectedSuggestedAddress != null)
            {
                CurrentAddress = SelectedSuggestedAddress.FormattedAddress;
                CurrentAddressId = SelectedSuggestedAddress.PlaceId;
            }
        });
    }

    public ObservableCollection<Address> SuggestedAddresses { get; set; }

    public string CurrentAddress { get; set; }

    public string CurrentAddressId { get; set; }

    public Address SelectedSuggestedAddress { get; set; }

    public bool IsListBoxVisible =>
        !SuggestedAddresses.Any(a => a.FormattedAddress.Equals(CurrentAddress)) &&
        CurrentAddress != null;

    public async Task<bool> IsAddressValid()
    {
        var suggestions = await CalculateSuggestedAddresses(CurrentAddress);
        return suggestions.Any(s => s.Equals(CurrentAddress));
    }

    private async Task UpdateSuggestedAddresses(string address)
    {
        var tempSuggestedAddresses = await CalculateSuggestedAddresses(address);

        foreach (var suggestedAddress in SuggestedAddresses.ToList())
        {
            if (!tempSuggestedAddresses.Any(a => a.PlaceId.Equals(suggestedAddress.PlaceId)))
            {
                SuggestedAddresses.Remove(suggestedAddress);
            }
        }

        // Remove old
        foreach (var tempAddress in tempSuggestedAddresses)
        {
            if (!SuggestedAddresses.Any(a => a.PlaceId.Equals(tempAddress.PlaceId)))
            {
                SuggestedAddresses.Add(tempAddress);
            }
        }
    }

    private async Task<IEnumerable<Address>> CalculateSuggestedAddresses(string userInput)
    {
        return await googleApiService.ListPlacePredictionAddressesAsync(userInput);
    }
}
