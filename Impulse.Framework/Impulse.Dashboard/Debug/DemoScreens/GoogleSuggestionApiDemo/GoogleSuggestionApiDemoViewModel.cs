// <copyright file="GoogleSuggestionApiDemoViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Impulse.Shared.Datastructures;
using Impulse.Shared.Services;
using Impulse.SharedFramework.Services.Layout;
using Ninject;
using ReactiveUI;

namespace Impulse.Dashboard.Debug.DemoScreens.GoogleSuggestionApiDemo;

public class GoogleSuggestionApiDemoViewModel : DocumentBase
{
    private readonly IGoogleApiService googleApiService;

    public GoogleSuggestionApiDemoViewModel(IKernel kernel, IGoogleApiService googleApiService) : base(kernel)
    {
        // Set the documents display name
        DisplayName = "Google Api Demo";

        // Grab a reference to our service
        this.googleApiService = googleApiService;

        ClickCommand = ReactiveCommand.Create(async () =>
        {
            var suggestions = await GetSuggestedAddress(CurrentAddress);
            SuggestedAddress = suggestions.Any() ? suggestions.First().FormattedAddress : string.Empty;
        });
    }

    public string CurrentAddress { get; set; }

    public string SuggestedAddress { get; set; }

    public ICommand ClickCommand { get; set; }

    private async Task<IEnumerable<Address>> GetSuggestedAddress(string userInput)
    {
        return await googleApiService.ListSearchAddressesAsync(userInput);
    }
}
