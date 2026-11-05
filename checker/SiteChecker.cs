using System.Diagnostics;

public static class SiteChecker
{
    public static async Task<CheckResult> CheckAsync(HttpClient client, string url)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            var response = await client.GetAsync(url);
            stopwatch.Stop();

            int status = (int)response.StatusCode;
            bool ok = status >= 200 && status < 400;
            string? error = ok ? null : $"http_{status}";
            return new CheckResult(DateTime.UtcNow, ok, status, (int)stopwatch.ElapsedMilliseconds, error);
        }
        catch (TaskCanceledException)
        {
            return new CheckResult(DateTime.UtcNow, false, null, null, "timeout");
        }
        catch (HttpRequestException)
        {
            return new CheckResult(DateTime.UtcNow, false, null, null, "network");
        }
    }
}