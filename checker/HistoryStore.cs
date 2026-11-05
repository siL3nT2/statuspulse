using System.Text.Json;

public static class HistoryStore
{
    private static readonly JsonSerializerOptions Options =
        new JsonSerializerOptions(JsonSerializerDefaults.Web) { WriteIndented = true };

    public static string ToJson(List<CheckResult> history)
    {
        return JsonSerializer.Serialize(history, Options);
    }

    public static List<CheckResult> Parse(string json)
    {
        return JsonSerializer.Deserialize<List<CheckResult>>(json, Options)
            ?? new List<CheckResult>();
    }

    public static async Task<List<CheckResult>> LoadAsync(string folder, string siteId)
    {
        string path = Path.Combine(folder, "history", $"{siteId}.json");

        if (!File.Exists(path))
        {
            return new List<CheckResult>();
        }

        string json = await File.ReadAllTextAsync(path);
        return Parse(json);
    }

    public static async Task SaveAsync(string folder, string siteId, List<CheckResult> history)
    {
        string historyFolder = Path.Combine(folder, "history");
        Directory.CreateDirectory(historyFolder);

        string path = Path.Combine(historyFolder, $"{siteId}.json");
        await File.WriteAllTextAsync(path, ToJson(history));
    }
}