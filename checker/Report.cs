public record UptimeSummary(double? Last24Hours, double? Last7Days, double? Last30Days);

public record SiteStatus(
    string Id,
    string Name,
    string Url,
    string State,
    CheckResult Check,
    UptimeSummary Uptime);

public record LatestReport(DateTime GeneratedAt, List<SiteStatus> Sites);