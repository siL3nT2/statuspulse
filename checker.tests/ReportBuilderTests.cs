using Xunit;

public class ReportBuilderTests
{
    [Fact]
    public void Build_SetsStateForEachSite()
    {
        var checks = new List<SiteCheck>
        {
            new SiteCheck(
                new Site("a", "Site A", "https://a.example"),
                new CheckResult(DateTime.UtcNow, true, 200, 120, null)),
            new SiteCheck(
                new Site("b", "Site B", "https://b.example"),
                new CheckResult(DateTime.UtcNow, false, null, null, "timeout")),
        };

        var report = ReportBuilder.Build(DateTime.UtcNow, checks);

        Assert.Equal(2, report.Sites.Count);
        Assert.Equal("up", report.Sites[0].State);
        Assert.Equal("down", report.Sites[1].State);
    }

    [Fact]
    public void ToJson_UsesCamelCaseNames()
    {
        var report = new LatestReport(DateTime.UtcNow, new List<SiteStatus>());

        string json = ReportWriter.ToJson(report);

        Assert.Contains("\"generatedAt\"", json);
        Assert.Contains("\"sites\"", json);
    }
}