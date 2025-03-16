using Softoverse.CqrsKit.Models.Abstraction;

namespace Softoverse.CqrsKit.Models.Command;

public class Command<TRequest>(TRequest payload) : ICommand
    where TRequest : notnull
{
    public TRequest Payload { get; set; } = payload;
}