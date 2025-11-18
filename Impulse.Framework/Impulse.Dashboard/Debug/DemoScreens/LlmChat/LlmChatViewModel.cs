using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using Impulse.Shared.Enums;
using Impulse.SharedFramework.Services;
using Impulse.SharedFramework.Services.Layout;
using Impulse.SharedFramework.Services.Llm;

namespace Impulse.Dashboard.Debug.DemoScreens.LlmChat;

public class LlmChatViewModel : DocumentBase
{
    private readonly ILlmBrokerService brokerService;
    private readonly IDialogService dialogService;
    private readonly ObservableCollection<LlmSession> sessions = new();
    private readonly ObservableCollection<LlmSessionMessage> activeMessages = new();
    private readonly IReadOnlyList<LlmProvider> providers;
    private LlmSession? selectedSession;
    private string selectedSessionTitle = string.Empty;
    private string messageText = string.Empty;
    private string statusMessage = "Idle";
    private bool isSending;
    private bool hasLoaded;

    public event EventHandler? MessagesUpdated;

    public LlmChatViewModel(ILlmBrokerService brokerService, IDialogService dialogService)
    {
        this.brokerService = brokerService;
        this.dialogService = dialogService;
        DisplayName = "LLM Chat";
        providers = Enum.GetValues(typeof(LlmProvider)).Cast<LlmProvider>().ToList();
        SelectedProvider = providers.FirstOrDefault();
        brokerService.SessionChanged += BrokerServiceOnSessionChanged;
    }

    public ObservableCollection<LlmSession> Sessions => sessions;

    public ObservableCollection<LlmSessionMessage> ActiveMessages => activeMessages;

    public IReadOnlyList<LlmProvider> Providers => providers;

    public LlmProvider SelectedProvider { get; set; }

    public LlmSession? SelectedSession
    {
        get => selectedSession;
        set
        {
            if (selectedSession == value)
            {
                return;
            }

            selectedSession = value;
            UpdateSelectedSessionTitle(selectedSession?.Title ?? string.Empty, force: true);
            RefreshActiveMessages();
            NotifyOfPropertyChange(() => SelectedSession);
            NotifyOfPropertyChange(() => HasSelectedSession);
            NotifyOfPropertyChange(() => CanSendMessage);
            NotifyOfPropertyChange(() => CanSaveSessionTitle);
            NotifyOfPropertyChange(() => CanDeleteSession);
        }
    }

    public bool HasSelectedSession => SelectedSession != null;

    public string SelectedSessionTitle
    {
        get => selectedSessionTitle;
        set
        {
            var sanitized = value ?? string.Empty;
            UpdateSelectedSessionTitle(sanitized, force: false);
        }
    }

    public bool CanSaveSessionTitle => HasSelectedSession && !string.IsNullOrWhiteSpace(SelectedSessionTitle);

    public string MessageText
    {
        get => messageText;
        set
        {
            var sanitized = value ?? string.Empty;
            if (messageText == sanitized)
            {
                return;
            }

            messageText = sanitized;
            NotifyOfPropertyChange(() => MessageText);
            NotifyOfPropertyChange(() => CanSendMessage);
        }
    }

    public bool IsSending
    {
        get => isSending;
        set
        {
            if (isSending == value)
            {
                return;
            }

            isSending = value;
            NotifyOfPropertyChange(() => IsSending);
            NotifyOfPropertyChange(() => CanSendMessage);
        }
    }

    public bool CanSendMessage => HasSelectedSession && !IsSending && !string.IsNullOrWhiteSpace(MessageText);

    public string StatusMessage
    {
        get => statusMessage;
        set
        {
            var sanitized = value ?? string.Empty;
            if (statusMessage == sanitized)
            {
                return;
            }

            statusMessage = sanitized;
            NotifyOfPropertyChange(() => StatusMessage);
        }
    }

    public bool CanDeleteSession => HasSelectedSession;

    protected override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        await base.OnActivateAsync(cancellationToken);

        if (hasLoaded)
        {
            return;
        }

        hasLoaded = true;
        await LoadSessionsAsync(cancellationToken);
    }

    protected override async Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
    {
        await base.OnDeactivateAsync(close, cancellationToken);

        if (close)
        {
            brokerService.SessionChanged -= BrokerServiceOnSessionChanged;
        }
    }

    public async Task CreateSession()
    {
        try
        {
            var session = await brokerService.CreateSessionAsync(new LlmSessionCreationOptions
            {
                Title = $"Chat {DateTimeOffset.Now:HH:mm}"
            }, CancellationToken.None).ConfigureAwait(false);

            sessions.Insert(0, session);
            SelectedSession = session;
            StatusMessage = $"Created session '{session.Title}'.";
        }
        catch (Exception ex)
        {
            dialogService.ShowError("LLM Chat", ex.Message);
            StatusMessage = $"Failed to create session: {ex.Message}";
        }
    }

    public async Task SaveSelectedSessionTitle()
    {
        if (!HasSelectedSession)
        {
            return;
        }

        var newTitle = (SelectedSessionTitle ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(newTitle))
        {
            await dialogService.ShowWarning("LLM Chat", "Title cannot be empty.");
            return;
        }

        try
        {
            await brokerService.RenameSessionAsync(SelectedSession!.Id, newTitle, CancellationToken.None).ConfigureAwait(false);
            UpdateSelectedSessionTitle(newTitle, force: true);
            StatusMessage = $"Renamed session to '{newTitle}'.";
        }
        catch (Exception ex)
        {
            dialogService.ShowError("LLM Chat", ex.Message);
            StatusMessage = $"Failed to rename session: {ex.Message}";
        }
    }

    public async Task DeleteSelectedSession()
    {
        if (!HasSelectedSession)
        {
            return;
        }

        var result = await dialogService.ShowConfirmation("Delete session", $"Delete '{SelectedSession!.Title}'?");
        if (result != DialogResult.Yes)
        {
            return;
        }

        try
        {
            var sessionId = SelectedSession!.Id;
            await brokerService.DeleteSessionAsync(sessionId, CancellationToken.None).ConfigureAwait(false);
            StatusMessage = "Session deleted.";
        }
        catch (Exception ex)
        {
            dialogService.ShowError("LLM Chat", ex.Message);
            StatusMessage = $"Failed to delete session: {ex.Message}";
        }
    }

    public async Task SendMessage()
    {
        if (!CanSendMessage)
        {
            return;
        }

        IsSending = true;
        StatusMessage = $"Sending via {SelectedProvider}...";

        try
        {
            var request = new LlmRequest(
                SelectedSession!.Id,
                new LlmMessage(LlmMessageRole.User, MessageText.Trim()),
                new LlmRequestOptions
                {
                    Provider = SelectedProvider,
                    Stream = false,
                });

            await brokerService.SendAsync(request, CancellationToken.None).ConfigureAwait(false);
            MessageText = string.Empty;
            StatusMessage = $"Response received from {SelectedProvider}.";
        }
        catch (Exception ex)
        {
            dialogService.ShowError("LLM Chat", ex.Message);
            StatusMessage = $"Request failed: {ex.Message}";
        }
        finally
        {
            IsSending = false;
        }
    }

    private async Task LoadSessionsAsync(CancellationToken cancellationToken)
    {
        try
        {
            var existing = await brokerService.GetSessionsAsync(cancellationToken).ConfigureAwait(false);
            Execute.OnUIThread(() =>
            {
                sessions.Clear();
                foreach (var session in existing.OrderByDescending(s => s.LastUpdatedUtc))
                {
                    sessions.Add(session);
                }

                if (sessions.Count > 0)
                {
                    SelectedSession ??= sessions.First();
                }
            });
        }
        catch (Exception ex)
        {
            dialogService.ShowError("LLM Chat", ex.Message);
            StatusMessage = $"Failed to load sessions: {ex.Message}";
        }
    }

    private void BrokerServiceOnSessionChanged(object? sender, LlmSessionChangedEventArgs e)
    {
        Execute.OnUIThread(() =>
        {
            switch (e.Reason)
            {
                case LlmSessionChangeReason.Deleted:
                    RemoveSession(e.Session.Id);
                    break;
                default:
                    UpsertSession(e.Session);
                    break;
            }
        });
    }

    private void UpsertSession(LlmSession session)
    {
        var existing = sessions.FirstOrDefault(s => s.Id == session.Id);
        if (existing != null)
        {
            UpdateSession(existing, session);
            session = existing;
        }
        else
        {
            sessions.Add(session);
        }

        SortSessionsByLastUpdated();

        if (SelectedSession?.Id == session.Id)
        {
            UpdateSelectedSessionTitle(session.Title ?? string.Empty, force: true);
            RefreshActiveMessages();
            NotifyOfPropertyChange(() => SelectedSession);
            NotifyOfPropertyChange(() => CanSendMessage);
        }
    }

    private void UpdateSession(LlmSession target, LlmSession source)
    {
        target.Title = source.Title;
        target.Description = source.Description;
        target.CreatedAtUtc = source.CreatedAtUtc;
        target.LastUpdatedUtc = source.LastUpdatedUtc;
        target.Metadata = source.Metadata;
        target.Messages = source.Messages;
    }

    private void RemoveSession(Guid sessionId)
    {
        var existing = sessions.FirstOrDefault(s => s.Id == sessionId);
        if (existing != null)
        {
            sessions.Remove(existing);
        }

        if (SelectedSession?.Id == sessionId)
        {
            SelectedSession = sessions.FirstOrDefault();
        }
    }

    private void RefreshActiveMessages()
    {
        activeMessages.Clear();

        if (SelectedSession?.Messages != null)
        {
            foreach (var message in SelectedSession.Messages)
            {
                activeMessages.Add(message);
            }
        }

        OnMessagesUpdated();
    }

    private void SortSessionsByLastUpdated()
    {
        var ordered = sessions.OrderByDescending(s => s.LastUpdatedUtc).ToList();
        for (var targetIndex = 0; targetIndex < ordered.Count; targetIndex++)
        {
            var item = ordered[targetIndex];
            var currentIndex = sessions.IndexOf(item);
            if (currentIndex >= 0 && currentIndex != targetIndex)
            {
                sessions.Move(currentIndex, targetIndex);
            }
        }
    }

    private void UpdateSelectedSessionTitle(string value, bool force)
    {
        if (!force && selectedSessionTitle == value)
        {
            return;
        }

        selectedSessionTitle = value;
        NotifyOfPropertyChange(() => SelectedSessionTitle);
        NotifyOfPropertyChange(() => CanSaveSessionTitle);
    }

    private void OnMessagesUpdated()
    {
        MessagesUpdated?.Invoke(this, EventArgs.Empty);
    }
}
