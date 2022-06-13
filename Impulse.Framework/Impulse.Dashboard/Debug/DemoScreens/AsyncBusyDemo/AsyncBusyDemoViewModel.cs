// <copyright file="AsyncBusyDemoViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Impulse.SharedFramework.Services.Layout;
using ReactiveUI;

namespace Impulse.Dashboard.Debug.DemoScreens.AsyncBusyDemo;

public class AsyncBusyDemoViewModel : DocumentBase
{
    private CancellationTokenSource cancellaitonTokenSource;

    public AsyncBusyDemoViewModel()
    {
        DisplayName = "Async Busy Demo";

        SetBusyCommand = ReactiveCommand.Create(SetBusy);

        cancellaitonTokenSource = new CancellationTokenSource();
    }

    public bool IsBusy { get; set; }

    public int PercentComplete { get; set; }

    public ICommand SetBusyCommand { get; set; }

    private Task PerformLongAction { get; set; }

    public async Task SetBusy()
    {
        // Cancel the previous task
        cancellaitonTokenSource.Cancel();

        // Wait for the previous task to be cancelled
        if (PerformLongAction != null)
        {
            await PerformLongAction;
        }

        // Create a new cancellation token source
        cancellaitonTokenSource = new CancellationTokenSource();

        // Fire a new long running action
        PerformLongAction = GetLongRunningTask();
    }

    public void SetNotBusy()
    {
        cancellaitonTokenSource.Cancel();
        IsBusy = false;
    }

    public Task GetLongRunningTask()
    {
        return Task.Run(
            async () =>
        {
            IsBusy = true;

            for (int percent = 0; percent < 100; percent++)
            {
                // Ensure that cancellation has not been requested
                if (cancellaitonTokenSource.IsCancellationRequested)
                {
                    break;
                }

                // Put this task to sleep for 100ms - also allow the UI to update.
                await Task.Delay(100);

                // Update the percentage we have completed.
                PercentComplete = percent;
            }

            IsBusy = false;
        }, cancellaitonTokenSource.Token);
    }
}
