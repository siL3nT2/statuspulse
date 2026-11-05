using Xunit;

public class UptimeTests
{
    private static readonly DateTime Now =
        new DateTime(2026, 11, 5, 12, 0, 0, DateTimeKind.Utc);

    private static readonly TimeSpan Day = TimeSpan.FromHours(24);

    private static CheckResult Check(DateTime time, bool ok) =>
        new CheckResult(time, ok, ok ? 200 : null, ok ? 100 : null, ok ? null : "timeout");

    [Fact]
    public void AllSuccessful_Is100()
    {
        var history = new List<CheckResult>
        {
            Check(Now.AddHours(-2), true),
            Check(Now.AddHours(-1), true),
        };

        Assert.Equal(100.0, Uptime.Percentage(history, Now, Day).GetValueOrDefault());
    }

    [Fact]
    public void OneSuccessOutOfFour_Is25()
    {
        var history = new List<CheckResult>
        {
            Check(Now.AddHours(-4), true),
            Check(Now.AddHours(-3), false),
            Check(Now.AddHours(-2), false),
            Check(Now.AddHours(-1), false),
        };

        Assert.Equal(25.0, Uptime.Percentage(history, Now, Day).GetValueOrDefault());
    }

    [Fact]
    public void TwoSuccessesOutOfThree_RoundsToTwoDecimals()
    {
        var history = new List<CheckResult>
        {
            Check(Now.AddHours(-3), true),
            Check(Now.AddHours(-2), true),
            Check(Now.AddHours(-1), false),
        };

        Assert.Equal(66.67, Uptime.Percentage(history, Now, Day).GetValueOrDefault(), 2);
    }

    [Fact]
    public void NoChecksInWindow_ReturnsNull()
    {
        var history = new List<CheckResult> { Check(Now.AddDays(-3), true) };

        Assert.Null(Uptime.Percentage(history, Now, Day));
    }

    [Fact]
    public void ChecksOutsideTheWindow_AreIgnored()
    {
        var history = new List<CheckResult>
        {
            Check(Now.AddDays(-2), false),
            Check(Now.AddHours(-1), true),
        };

        Assert.Equal(100.0, Uptime.Percentage(history, Now, Day).GetValueOrDefault());
    }
}