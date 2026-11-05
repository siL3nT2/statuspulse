public record SiteStatus(string Id, string Name, string Url, string State, CheckResult Check);

public record LatestReport(DateTime GeneratedAt, List<SiteStatus> Sites);