using Softoverse.CqrsKit.Model.Abstraction;

namespace Softoverse.CqrsKit.Model.Command;

public class Command<TRequest>(TRequest payload) : ICommand
    where TRequest : notnull
{
    public TRequest Payload { get; set; } = payload;
}