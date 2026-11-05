using var client = new HttpClient();
client.Timeout = TimeSpan.FromSeconds(10);

string outputFolder = args.Length > 0 ? args[0] : "data";

string path = Path.Combine(AppContext.BaseDirectory, "sites.json");
var sites = await SiteConfig.LoadAsync(path);

var checks = new List<SiteCheck>();

foreach (var site in sites)
{
    var result = await RetryPolicy.CheckWithRetryAsync(
        () => SiteChecker.CheckAsync(client, site.Url),
        TimeSpan.FromSeconds(5));

    var history = await HistoryStore.LoadAsync(outputFolder, site.Id);
    history = History.AppendAndTrim(history, result, DateTime.UtcNow);
    await HistoryStore.SaveAsync(outputFolder, site.Id, history);

    checks.Add(new SiteCheck(site, result));
    Console.WriteLine($"{site.Name} -> {StatusRules.GetStatus(result)} ({history.Count} checks in history)");
}

var report = ReportBuilder.Build(DateTime.UtcNow, checks);
await ReportWriter.WriteLatestAsync(report, outputFolder);

Console.WriteLine($"Wrote latest.json to {Path.GetFullPath(outputFolder)}");