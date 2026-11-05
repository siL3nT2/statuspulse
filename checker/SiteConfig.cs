using System.Text.Json;
using System.Text.RegularExpressions;

public static class SiteConfig
{
    private static readonly JsonSerializerOptions Options =
        new JsonSerializerOptions(JsonSerializerDefaults.Web);

    public static List<Site> Parse(string json)
    {
        var sites = JsonSerializer.Deserialize<List<Site>>(json, Options);

        if (sites is null || sites.Count == 0)
        {
            throw new InvalidOperationException("No sites found in the config.");
        }

        Validate(sites);
        return sites;
    }

    public static void Validate(List<Site> sites)
    {
        var seen = new HashSet<string>();

        foreach (var site in sites)
        {
            if (!Regex.IsMatch(site.Id ?? "", "^[a-z0-9-]{1,40}$"))
            {
                throw new InvalidOperationException(
                    $"Invalid site id '{site.Id}'. Use 1-40 lowercase letters, digits or hyphens.");
            }

            if (!seen.Add(site.Id))
            {
                throw new InvalidOperationException($"Duplicate site id '{site.Id}'.");
            }

            bool validUrl =
                Uri.TryCreate(site.Url, UriKind.Absolute, out var uri)
                && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);

            if (!validUrl)
            {
                throw new InvalidOperationException(
                    $"Invalid URL for site '{site.Id}'. Only http and https addresses are allowed.");
            }
        }
    }

    public static async Task<List<Site>> LoadAsync(string path)
    {
        string json = await File.ReadAllTextAsync(path);
        return Parse(json);
    }
}