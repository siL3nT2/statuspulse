public static class History
{
    public static List<CheckResult> AppendAndTrim(
        List<CheckResult> existing,
        CheckResult newest,
        DateTime now,
        int keepDays = 30)
    {
        var cutoff = now.AddDays(-keepDays);

        var all = new List<CheckResult>(existing);
        all.Add(newest);

        return all.Where(check => check.Time >= cutoff).ToList();
    }
}