using Softoverse.CqrsKit.Models.Utility;

namespace Softoverse.CqrsKit.Models.Errors;

public static class CommandErrors
{
    public static CqrsError NotFound(string name) => CqrsError.Create("Command.NotFound",
                                                                      string.IsNullOrEmpty(name) ? "Command not found." : $"Command with name '{name}' was not found.");
}