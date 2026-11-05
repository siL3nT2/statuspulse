using System.Text.Json;

public static class ReportWriter
{
    private static readonly JsonSerializerOptions Options =
        new JsonSerializerOptions(JsonSerializerDefaults.Web) { WriteIndented = true };

    public static string ToJson(LatestReport report)
    {
        return JsonSerializer.Serialize(report, Options);
    }

    public static async Task WriteLatestAsync(LatestReport report, string folder)
    {
        Directory.CreateDirectory(folder);
        string path = Path.Combine(folder, "latest.json");
        await File.WriteAllTextAsync(path, ToJson(report));
    }
}