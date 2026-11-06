public static class Uptime
{
    public static double? Percentage(List<CheckResult> history, DateTime now, TimeSpan window)
    {
        var start = now - window;
        var inWindow = history.Where(check => check.Time >= start).ToList();

        if (inWindow.Count == 0)
        {
            return null;
        }

        int successful = inWindow.Count(check => check.Ok);
        return Math.Round(100.0 * successful / inWindow.Count, 2);
    }

    public static UptimeSummary Summarize(List<CheckResult> history, DateTime now)
    {
        return new UptimeSummary(
            Percentage(history, now, TimeSpan.FromHours(24)),
            Percentage(history, now, TimeSpan.FromDays(7)),
            Percentage(history, now, TimeSpan.FromDays(30)));
    }
}