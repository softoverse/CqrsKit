﻿using Softoverse.CqrsKit.Abstractions.Filters;
using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Models;
using Softoverse.CqrsKit.Models.Utility;
using Softoverse.CqrsKit.Services;
using Softoverse.CqrsKit.TestConsole.CQRS.Events.Command;

namespace Softoverse.CqrsKit.TestConsole.CQRS.Filters.ApprovalFlow;

[ScopedLifetime]
public class PersonDeleteCommandApprovalFlowExecutionFilter : ApprovalFlowExecutionFilterBase<PersonDeleteCommand, Guid>
{
    public override Task<Result<Guid>> OnExecutingAsync(PersonDeleteCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {UtilityHelper.GetFormattedTypeName(this.GetType())}.{nameof (this.OnExecutingAsync)} - (Custom)");
        return ResultDefaults.DefaultResult<Guid>();
    }

    public override Task<Result<Guid>> ExecuteAsync(PersonDeleteCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {UtilityHelper.GetFormattedTypeName(this.GetType())}.{nameof (this.ExecuteAsync)} - (Custom)");
        return ResultDefaults.DefaultResult<Guid>();
    }

    public override Task<Result<Guid>> OnExecutedAsync(PersonDeleteCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {UtilityHelper.GetFormattedTypeName(this.GetType())}.{nameof (this.OnExecutedAsync)} - (Custom)");
        return ResultDefaults.DefaultResult<Guid>();
    }
}