using Xunit;

public class UptimeSummaryTests
{
    private static readonly DateTime Now =
        new DateTime(2026, 11, 5, 12, 0, 0, DateTimeKind.Utc);

    [Fact]
    public void Summarize_UsesSeparateWindows()
    {
        var history = new List<CheckResult>
        {
            new CheckResult(Now.AddDays(-3), false, null, null, "timeout"),
            new CheckResult(Now.AddHours(-1), true, 200, 100, null),
        };

        var summary = Uptime.Summarize(history, Now);

        Assert.Equal(100.0, summary.Last24Hours.GetValueOrDefault());
        Assert.Equal(50.0, summary.Last7Days.GetValueOrDefault());
        Assert.Equal(50.0, summary.Last30Days.GetValueOrDefault());
    }
}