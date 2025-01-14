using CqrsKit.Model.Abstraction;

namespace CqrsKit.Model.Command;

public class Command<TRequest>(TRequest payload) : ICommand
    where TRequest : notnull
{
    public TRequest Payload { get; set; } = payload;
}