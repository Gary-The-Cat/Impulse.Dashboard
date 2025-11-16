namespace Impulse.Dashboard.Tests.Logging;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using Impulse.Logging.Contracts;
using Impulse.Logging.Domain.Services;
using Impulse.Shared.Interfaces;

public sealed class LogServiceTests
{
    [Fact]
    public async Task LogInfo_ShouldCacheRecordAndAppendLogEntry()
    {
        using var harness = new LogServiceHarness();
        var timestamp = new DateTime(2024, 05, 01, 12, 30, 0, DateTimeKind.Utc);
        harness.SetCurrentTime(timestamp);

        await harness.Service.LogInfo("Dashboard booted");

        var cachedRecords = harness.Service.GetLogRecordsForCricicality(Criticality.Info).ToList();
        cachedRecords.Should().ContainSingle();
        cachedRecords[0].Message.Should().Be("Dashboard booted");
        cachedRecords[0].Timestamp.Should().Be(timestamp);

        var logLines = harness.ReadLogLines();
        logLines.Should().ContainSingle();
        logLines[0].Should().Contain("[INFO] Dashboard booted");
        logLines[0].Should().StartWith(timestamp.ToString("O"));
    }

    [Fact]
    public async Task DeleteRecordsAsync_ShouldRemoveMatchingRecordsFromCache()
    {
        using var harness = new LogServiceHarness();
        harness.SetCurrentTime(new DateTime(2024, 05, 01, 12, 30, 0, DateTimeKind.Utc));
        await harness.Service.LogWarning("Keep me");
        harness.SetCurrentTime(new DateTime(2024, 05, 01, 12, 31, 0, DateTimeKind.Utc));
        await harness.Service.LogError("Remove me");

        var allRecords = harness.Service.GetLogRecordsForCricicality(Criticality.Info).ToList();
        allRecords.Should().HaveCount(2);
        var removeTarget = allRecords.Single(record => record.Message == "Remove me");

        await harness.Service.DeleteRecordsAsync(new[] { removeTarget.Id });

        var remaining = harness.Service.GetLogRecordsForCricicality(Criticality.Info).ToList();
        remaining.Should().ContainSingle(record => record.Message == "Keep me");
    }

    [Fact]
    public async Task Subscribe_ShouldReplayExistingRecordsAndPushNewOnes()
    {
        using var harness = new LogServiceHarness();
        harness.SetCurrentTime(new DateTime(2024, 05, 01, 12, 30, 0, DateTimeKind.Utc));
        await harness.Service.LogInfo("Seed record");

        var observer = new TestObserver();
        var subscription = harness.Service.Subscribe(observer);
        observer.Records.Should().ContainSingle(record => record.Message == "Seed record");

        harness.SetCurrentTime(new DateTime(2024, 05, 01, 12, 31, 0, DateTimeKind.Utc));
        await harness.Service.LogError("New record");

        observer.Records.Should().HaveCount(2);
        observer.Records.Last().Message.Should().Be("New record");

        subscription.Dispose();
        harness.SetCurrentTime(new DateTime(2024, 05, 01, 12, 32, 0, DateTimeKind.Utc));
        await harness.Service.LogWarning("Ignored after dispose");

        observer.Records.Should().HaveCount(2);
    }

    private sealed class LogServiceHarness : IDisposable
    {
        private static readonly FieldInfo LogFilePathField = typeof(LogService)
            .GetField("logFilePath", BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("Could not access logFilePath field");

        private readonly FakeDateTimeProvider dateTimeProvider = new();
        private readonly string logFilePath = Path.Combine(Path.GetTempPath(), $"LogServiceTests_{Guid.NewGuid():N}.log");

        public LogServiceHarness()
        {
            if (File.Exists(logFilePath))
            {
                File.Delete(logFilePath);
            }

            Service = new LogService(dateTimeProvider);
            LogFilePathField.SetValue(Service, logFilePath);
        }

        public LogService Service { get; }

        public void SetCurrentTime(DateTime now) => dateTimeProvider.Set(now);

        public string[] ReadLogLines() => File.Exists(logFilePath)
            ? File.ReadAllLines(logFilePath)
            : Array.Empty<string>();

        public void Dispose()
        {
            if (File.Exists(logFilePath))
            {
                File.Delete(logFilePath);
            }
        }
    }

    private sealed class FakeDateTimeProvider : IDateTimeProvider
    {
        private DateTime current = DateTime.UtcNow;

        public DateTime Now => current;

        public void Set(DateTime value) => current = value;
    }

    private sealed class TestObserver : IObserver<LogRecordBase>
    {
        public List<LogRecordBase> Records { get; } = new();

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(LogRecordBase value)
        {
            Records.Add(value);
        }
    }
}
