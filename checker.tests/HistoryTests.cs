using Xunit;

public class HistoryTests
{
    private static readonly DateTime Now =
        new DateTime(2026, 11, 5, 12, 0, 0, DateTimeKind.Utc);

    private static CheckResult CheckAt(DateTime time) =>
        new CheckResult(time, true, 200, 100, null);

    [Fact]
    public void AppendAndTrim_AddsNewestCheck()
    {
        var existing = new List<CheckResult> { CheckAt(Now.AddMinutes(-30)) };

        var result = History.AppendAndTrim(existing, CheckAt(Now), Now);

        Assert.Equal(2, result.Count);
        Assert.Equal(Now, result[1].Time);
    }

    [Fact]
    public void AppendAndTrim_RemovesChecksOlderThanThirtyDays()
    {
        var existing = new List<CheckResult>
        {
            CheckAt(Now.AddDays(-31)),
            CheckAt(Now.AddDays(-1)),
        };

        var result = History.AppendAndTrim(existing, CheckAt(Now), Now);

        Assert.Equal(2, result.Count);
        Assert.Equal(Now.AddDays(-1), result[0].Time);
    }

    [Fact]
    public void AppendAndTrim_KeepsCheckExactlyAtCutoff()
    {
        var existing = new List<CheckResult> { CheckAt(Now.AddDays(-30)) };

        var result = History.AppendAndTrim(existing, CheckAt(Now), Now);

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void HistoryJson_RoundTrips()
    {
        var history = new List<CheckResult>
        {
            new CheckResult(Now, true, 200, 245, null),
            new CheckResult(Now.AddMinutes(30), false, null, null, "timeout"),
        };

        var parsed = HistoryStore.Parse(HistoryStore.ToJson(history));

        Assert.Equal(2, parsed.Count);
        Assert.Equal(history[0], parsed[0]);
        Assert.Equal("timeout", parsed[1].Error);
    }
}