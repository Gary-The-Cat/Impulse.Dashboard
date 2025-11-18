using System;
using System.Windows;
using System.Windows.Controls;

namespace Impulse.Dashboard.Debug.DemoScreens.LlmChat;

public partial class LlmChatView : UserControl
{
    public LlmChatView()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (e.OldValue is LlmChatViewModel oldViewModel)
        {
            oldViewModel.MessagesUpdated -= OnMessagesUpdated;
        }

        if (e.NewValue is LlmChatViewModel newViewModel)
        {
            newViewModel.MessagesUpdated += OnMessagesUpdated;
            ScrollToBottom();
        }
    }

    private void OnMessagesUpdated(object? sender, EventArgs e)
    {
        ScrollToBottom();
    }

    private void ScrollToBottom()
    {
        ChatScrollViewer?.ScrollToEnd();
    }
}
