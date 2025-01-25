namespace Softoverse.CqrsKit.TestConsole.Utility;

public static class ConsoleColor
{
    public static readonly string NewLine = Environment.NewLine; // shortcut
    public static readonly string Normal = Console.IsOutputRedirected ? "" : "\x1b[39m";
    public static readonly string Red = Console.IsOutputRedirected ? "" : "\x1b[91m";
    public static readonly string Green = Console.IsOutputRedirected ? "" : "\x1b[92m";
    public static readonly string Yellow = Console.IsOutputRedirected ? "" : "\x1b[93m";
    public static readonly string Blue = Console.IsOutputRedirected ? "" : "\x1b[94m";
    public static readonly string Magenta = Console.IsOutputRedirected ? "" : "\x1b[95m";
    public static readonly string Cyan = Console.IsOutputRedirected ? "" : "\x1b[96m";
    public static readonly string Grey = Console.IsOutputRedirected ? "" : "\x1b[97m";
    public static readonly string Bold = Console.IsOutputRedirected ? "" : "\x1b[1m";
    public static readonly string NoBold = Console.IsOutputRedirected ? "" : "\x1b[22m";
    public static readonly string Underline = Console.IsOutputRedirected ? "" : "\x1b[4m";
    public static readonly string NoUnderline = Console.IsOutputRedirected ? "" : "\x1b[24m";
    public static readonly string Reverse = Console.IsOutputRedirected ? "" : "\x1b[7m";
    public static readonly string Noreverse = Console.IsOutputRedirected ? "" : "\x1b[27m";
    //errorLogMessageBuilder.AppendLine($"{RED}Url:{NORMAL} {currentUrl}");
}