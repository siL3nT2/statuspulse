using System.Text.Json;

public static class SiteConfig
{
    private static readonly JsonSerializerOptions Options =
    new JsonSerializerOptions(JsonSerializerDefaults.Web);

    public static List<Site> Parse(string json)
    {
        var sites = JsonSerializer.Deserialize<List<Site>>(json, Options);

        if (sites is null || sites.Count == 0)
        {
            throw new InvalidOperationException("No sites found in configuration.");
        }

        return sites;
    }

    public static async Task<List<Site>> LoadAsync(string path)
    {
        string json = await File.ReadAllTextAsync(path);
        return Parse(json);
    }
}