using Xunit;

public class SiteConfigTests
{
    [Fact]
    public void ValidJson_ReturnsSites()
    {
        string json = """
            [
              { "id": "portfolio", "name": "Portfolio", "url": "https://example.com" },
              { "id": "second", "name": "Second", "url": "https://example.org" }
            ]
            """;

        var sites = SiteConfig.Parse(json);

        Assert.Equal(2, sites.Count);
        Assert.Equal("portfolio", sites[0].Id);
        Assert.Equal("https://example.org", sites[1].Url);
    }

    [Fact]
    public void EmptyList_Throws()
    {
        Assert.Throws<InvalidOperationException>(() => SiteConfig.Parse("[]"));
    }
}