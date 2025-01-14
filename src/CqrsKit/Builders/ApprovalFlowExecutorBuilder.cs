using CqrsKit.Executors;

using CqrsKit.Abstraction.Builders;
using CqrsKit.Abstraction.Executors;
using CqrsKit.Abstraction.Filters;
using CqrsKit.Abstraction.Services;

using CqrsKit.Model.Entity;
using CqrsKit.Model.Utility;

using Microsoft.Extensions.DependencyInjection;

namespace CqrsKit.Builders;

internal sealed class ApprovalFlowExecutorBuilder<T> : IApprovalFlowExecutorBuilder<T> where T : BaseApprovalFlowPendingTask
{
    private readonly IServiceProvider _services;

    private string _approvalFlowPendingTaskId;

    private readonly CqrsContext _context = new CqrsContext();

    private ApprovalFlowExecutorBuilder()
    {
    }

    private ApprovalFlowExecutorBuilder(IServiceProvider services)
    {
        _services = services;
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