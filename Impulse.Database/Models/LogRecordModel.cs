﻿using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Impulse.Repository.Models;

[DataContract]
public record LogRecordModel
{
    public int Id { get; init; }

    public string Message { get; init; }

    public string? StackTrace { get; init; }

    public int Criticality { get; init; }

    public long TimeStampTicks { get; init; }
}
