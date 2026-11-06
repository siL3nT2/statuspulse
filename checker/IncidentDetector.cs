public static class IncidentDetector
{
    public static List<Incident> Detect(string siteId, List<CheckResult> history)
    {
        var incidents = new List<Incident>();
        DateTime? start = null;

        foreach (var check in history.OrderBy(item => item.Time))
        {
            if (!check.Ok && start is null)
            {
                start = check.Time;
            }
            else if (check.Ok && start is not null)
            {
                incidents.Add(new Incident(siteId, start.Value, check.Time));
                start = null;
            }
        }

        if (start is not null)
        {
            incidents.Add(new Incident(siteId, start.Value, null));
        }

        return incidents;
    }
}