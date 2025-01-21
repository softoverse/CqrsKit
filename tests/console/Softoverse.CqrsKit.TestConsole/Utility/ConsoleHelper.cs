namespace Softoverse.CqrsKit.TestConsole.Utility;

public static class ConsoleHelper
{
    public static void PrintErrors(IDictionary<string, string[]>? errors)
    {
        Console.ForegroundColor = System.ConsoleColor.Red;

        Console.WriteLine("\nError Occured!\nErrors: \n");
        Console.WriteLine("");

        errors ??= new Dictionary<string, string[]>();

        foreach (var keyValuePair in errors)
        {
            Console.WriteLine(keyValuePair.Key + ":\n\t" + string.Join("\n\t", keyValuePair.Value.Select(x => x)));
        }
        Console.ResetColor();

        throw new Exception($"{ConsoleColor.Red}Errors found!{ConsoleColor.Normal}");
    }

    public static void PrintBlock(string value, char fillBy = ' ', int length = 70, string startsWith = "", string endsWith = "")
    {
        length -= value.Length;
        string block = string.Empty;
        for (int i = 0; i < length / 2; i++)
        {
            block += fillBy;
        }

        Console.WriteLine($"{startsWith}{block} {value} {block}{endsWith}");
    }

    public static void StartBlock()
    {
        PrintBlock($"Start", '=', startsWith: ConsoleColor.Magenta, endsWith: ConsoleColor.Normal);
    }

    public static void EndBlock()
    {
        PrintBlock("End", '=', startsWith: ConsoleColor.Magenta, endsWith: $"\n\n{ConsoleColor.Normal}");
    }
}