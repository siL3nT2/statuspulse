using Xunit;

public class ReportBuilderTests
{
    private static readonly DateTime Now =
        new DateTime(2026, 11, 5, 12, 0, 0, DateTimeKind.Utc);

    private static CheckResult Up(DateTime time) =>
        new CheckResult(time, true, 200, 120, null);

    private static CheckResult Down(DateTime time) =>
        new CheckResult(time, false, null, null, "timeout");

    [Fact]
    public void Build_SetsStateForEachSite()
    {
        var upCheck = Up(Now);
        var downCheck = Down(Now);

        var checks = new List<SiteCheck>
        {
            new SiteCheck(new Site("a", "Site A", "https://a.example"), upCheck, new List<CheckResult> { upCheck }),
            new SiteCheck(new Site("b", "Site B", "https://b.example"), downCheck, new List<CheckResult> { downCheck }),
        };

        var report = ReportBuilder.Build(Now, checks);

        Assert.Equal(2, report.Sites.Count);
        Assert.Equal("up", report.Sites[0].State);
        Assert.Equal("down", report.Sites[1].State);
    }

    [Fact]
    public void Build_IncludesUptimeFromHistory()
    {
        var latest = Up(Now);
        var history = new List<CheckResult>
        {
            Down(Now.AddHours(-2)),
            Up(Now.AddHours(-1)),
            latest,
        };

        var checks = new List<SiteCheck>
        {
            new SiteCheck(new Site("a", "Site A", "https://a.example"), latest, history),
        };

        var report = ReportBuilder.Build(Now, checks);

        Assert.Equal(66.67, report.Sites[0].Uptime.Last24Hours.GetValueOrDefault(), 2);
    }

    [Fact]
    public void ToJson_UsesCamelCaseNames()
    {
        var report = new LatestReport(Now, new List<SiteStatus>());

        string json = ReportWriter.ToJson(report);

        Assert.Contains("\"generatedAt\"", json);
        Assert.Contains("\"sites\"", json);
    }
}