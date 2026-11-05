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

    var now = DateTime.UtcNow;
    string day = Uptime.Format(Uptime.Percentage(history, now, TimeSpan.FromHours(24)));
    string week = Uptime.Format(Uptime.Percentage(history, now, TimeSpan.FromDays(7)));
    string month = Uptime.Format(Uptime.Percentage(history, now, TimeSpan.FromDays(30)));

    checks.Add(new SiteCheck(site, result));
    Console.WriteLine($"{site.Name} -> {StatusRules.GetStatus(result)} | uptime 24h: {day}, 7d: {week}, 30d: {month}");
}

var report = ReportBuilder.Build(DateTime.UtcNow, checks);
await ReportWriter.WriteLatestAsync(report, outputFolder);

Console.WriteLine($"Wrote latest.json to {Path.GetFullPath(outputFolder)}");