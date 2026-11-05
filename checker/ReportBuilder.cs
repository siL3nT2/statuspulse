public static class ReportBuilder
{
    public static LatestReport Build(DateTime generatedAt, List<SiteCheck> checks)
    {
        var sites = new List<SiteStatus>();

        foreach (var item in checks)
        {
            sites.Add(new SiteStatus(
                item.Site.Id,
                item.Site.Name,
                item.Site.Url,
                StatusRules.GetStatus(item.Result),
                item.Result));
        }

        return new LatestReport(generatedAt, sites);
    }
}