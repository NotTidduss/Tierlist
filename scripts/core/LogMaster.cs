using Godot;

public static class LogMaster
{
    public static void LogInfo(string message, string sourceClass = "", string sourceFunction = "")
        => GD.PrintRich(GetLogString(message, sourceClass, sourceFunction, "green"));
    public static void LogWarning(string message, string sourceClass = "", string sourceFunction = "")
        => GD.PrintRich(GetLogString(message, sourceClass, sourceFunction, "yellow"));
    public static void LogError(string message, string sourceClass = "", string sourceFunction = "")
    {
        string errorMessage = $"[pulse][color=red]{message}[/color][/pulse]";
        GD.PrintRich(GetLogString(errorMessage, sourceClass, sourceFunction, "red"));
        GD.PushError(message);
    }

    private static string GetLogString(string msg, string srcClass, string srcFnc, string color)
    {
        string m1 = $"{(string.IsNullOrWhiteSpace(srcClass) ? "" : $"[color={color}][b]{srcClass}[/b][/color] - ")}";
        string m2 = $"{(string.IsNullOrWhiteSpace(srcFnc) ? "" : $"[color={color}][u]{srcFnc}[/u][/color]: ")}";
        return $"{m1}{m2}{msg}";
    }
}
