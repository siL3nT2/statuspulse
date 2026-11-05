using Xunit;

public class SiteConfigValidationTests
{
    [Fact]
    public void GoodSite_IsAccepted()
    {
        var sites = new List<Site> { new Site("my-site-1", "Test", "https://example.com") };

        SiteConfig.Validate(sites);
    }

    [Theory]
    [InlineData("../evil")]
    [InlineData("..\\evil")]
    [InlineData("Has Space")]
    [InlineData("UPPER")]
    [InlineData("")]
    public void BadIds_AreRejected(string id)
    {
        var sites = new List<Site> { new Site(id, "Test", "https://example.com") };

        Assert.Throws<InvalidOperationException>(() => SiteConfig.Validate(sites));
    }

    [Fact]
    public void DuplicateIds_AreRejected()
    {
        var sites = new List<Site>
        {
            new Site("same", "One", "https://example.com"),
            new Site("same", "Two", "https://example.org"),
        };

        Assert.Throws<InvalidOperationException>(() => SiteConfig.Validate(sites));
    }

    [Theory]
    [InlineData("file:///C:/secret.txt")]
    [InlineData("ftp://example.com")]
    [InlineData("not a url")]
    public void BadUrls_AreRejected(string url)
    {
        var sites = new List<Site> { new Site("ok", "Test", url) };

        Assert.Throws<InvalidOperationException>(() => SiteConfig.Validate(sites));
    }
}