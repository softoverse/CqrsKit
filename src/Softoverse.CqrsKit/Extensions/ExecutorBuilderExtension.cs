using Microsoft.Extensions.DependencyInjection;

using Softoverse.CqrsKit.Abstractions.Handlers;
using Softoverse.CqrsKit.Models.Abstraction;

namespace Softoverse.CqrsKit.Extensions;

public static class CommandHandlerBuilderExtension
{
    public static ICommandHandler<TCommand, TResponse> GetCommandHandler<TCommand, TResponse>(this IServiceProvider serviceProvider) where TCommand : ICommand
    {
        ICommandHandler<TCommand, TResponse> handler = serviceProvider.GetRequiredService<ICommandHandler<TCommand, TResponse>>();

        if (handler is null)
        {
            throw new Exception(GetExceptionMessage(typeof(TCommand).Name));
        }

        return handler;
    }

    public static IQueryHandler<TQuery, TResponse> GetQueryHandler<TQuery, TResponse>(this IServiceProvider serviceProvider) where TQuery : IQuery
    {
        IQueryHandler<TQuery, TResponse> handler = serviceProvider.GetRequiredService<IQueryHandler<TQuery, TResponse>>();

        if (handler is null)
        {
            throw new Exception(GetExceptionMessage(typeof(TQuery).Name));
        }

        return handler;
    }

    public static IApprovalFlowHandler<TCommand, TResponse> GetApprovalFlowHandler<TCommand, TResponse>(this IServiceProvider serviceProvider) where TCommand : ICommand
    {
        try
        {
            IApprovalFlowHandler<TCommand, TResponse>? handler = serviceProvider.GetService<IApprovalFlowHandler<TCommand, TResponse>>();

            if (handler is null)
            {
                throw new Exception(GetExceptionMessage(typeof(TCommand).Name));
            }

            return handler;
        }
        catch (Exception ex)
        {
            return null!;
        }
    }

    private static string GetExceptionMessage(string commandName)
    {
        return $"No handler for {commandName}";
    }
}