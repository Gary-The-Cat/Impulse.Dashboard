using System;

namespace Impulse.SharedFramework.Services.Llm;

public sealed class LlmSessionChangedEventArgs : EventArgs
{
    public LlmSessionChangedEventArgs(LlmSessionChangeReason reason, LlmSession session)
    {
        Reason = reason;
        Session = session;
    }

    public LlmSessionChangeReason Reason { get; }

    public LlmSession Session { get; }
}

public enum LlmSessionChangeReason
{
    Created,
    Updated,
    Deleted,
}
