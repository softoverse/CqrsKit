using Microsoft.Extensions.DependencyInjection;

using Softoverse.CqrsKit.Abstractions.Builders;
using Softoverse.CqrsKit.Abstractions.Executors;
using Softoverse.CqrsKit.Abstractions.Filters;
using Softoverse.CqrsKit.Abstractions.Services;
using Softoverse.CqrsKit.Executors;
using Softoverse.CqrsKit.Models.Entity;
using Softoverse.CqrsKit.Models.Utility;

namespace Softoverse.CqrsKit.Builders;

internal sealed class ApprovalFlowExecutorBuilder<T> : IApprovalFlowExecutorBuilder<T> where T : BaseApprovalFlowPendingTask
{
    private readonly IServiceProvider _services;

    private string _approvalFlowPendingTaskId;

    private readonly CqrsContext _context;

    private ApprovalFlowExecutorBuilder()
    {
    }

    private ApprovalFlowExecutorBuilder(IServiceProvider services)
    {
        _services = services;
        _context = CqrsContext.New(_services);
    }

    public static IApprovalFlowExecutorBuilder<T> Initialize(IServiceProvider services)
    {
        return new ApprovalFlowExecutorBuilder<T>(services);
    }

    public IApprovalFlowExecutorBuilder<T> WithId(string id)
    {
        _approvalFlowPendingTaskId = id;
        return this;
    }

    public IApprovalFlowExecutorBuilder<T> WithItems(IDictionary<object, object?> items)
    {
        _context.Items = items;
        return this;
    }

    public IApprovalFlowExecutor<T> Build()
    {
        if (string.IsNullOrEmpty(_approvalFlowPendingTaskId)) throw new Exception("Approval Flow Id cannot be empty");

        var approvalFlowService = _services.GetRequiredService<IApprovalFlowService>();
        var approvalFlowAcceptFilter = _services.GetRequiredService<IApprovalFlowAcceptFilter>();
        var approvalFlowRejectFilter = _services.GetRequiredService<IApprovalFlowRejectFilter>();

        return new ApprovalFlowExecutor<T>()
        {
            Services = _services,
            ApprovalFlowService = approvalFlowService,
            ApprovalFlowAcceptFilter = approvalFlowAcceptFilter,
            ApprovalFlowRejectFilter = approvalFlowRejectFilter,
            ApprovalFlowPendingTaskId = _approvalFlowPendingTaskId,
            Context = _context,
        };
    }
}