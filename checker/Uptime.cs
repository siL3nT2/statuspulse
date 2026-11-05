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
        return Math.Round(100 * (successful / inWindow.Count), 2);
    }

    public static string Format(double? value) =>
        value is null ? "no data" : $"{value}%";
}