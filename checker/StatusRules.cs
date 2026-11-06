public static class StatusRules
{
    public static string GetStatus(CheckResult result)
    {
        if (!result.Ok)
        {
            return "down";
        }

        if (result.Ms >= 3000)
        {
            return "degraded";
        }

        return "up";
    }
}