using var client = new HttpClient();
client.Timeout = TimeSpan.FromSeconds(10);

string path = Path.Combine(AppContext.BaseDirectory, "sites.json");
var sites = await SiteConfig.LoadAsync(path);

foreach (var site in sites)
{
    var result = await RetryPolicy.CheckWithRetryAsync(
        () => SiteChecker.CheckAsync(client, site.Url),
        TimeSpan.FromSeconds(5));

    Console.WriteLine($"{site.Name} ({site.Url}) -> {StatusRules.GetStatus(result)} ({result})");
}