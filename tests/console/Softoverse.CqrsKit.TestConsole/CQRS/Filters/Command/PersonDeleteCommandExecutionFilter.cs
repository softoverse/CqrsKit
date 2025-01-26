using Softoverse.CqrsKit.Abstraction.Filters;
using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Utility;
using Softoverse.CqrsKit.Services;
using Softoverse.CqrsKit.TestConsole.CQRS.Events.Command;

namespace Softoverse.CqrsKit.TestConsole.CQRS.Filters.Command
{
    [ScopedLifetime]
    public class PersonDeleteCommandExecutionFilter : CommandExecutionFilterBase<PersonDeleteCommand, Guid>
    {
        public override Task<Result<Guid>> OnExecutingAsync(CqrsContext context, CancellationToken ct = default)
        {
            Console.WriteLine($"Method Call: {UtilityHelper.GetFormattedTypeName(this.GetType())}.{nameof (this.OnExecutingAsync)} - (Custom)");
            return ResultDefaults.DefaultResult<Guid>();
        }

        public override Task<Result<Guid>> OnExecutedAsync(CqrsContext context, CancellationToken ct = default)
        {
            Console.WriteLine($"Method Call: {UtilityHelper.GetFormattedTypeName(this.GetType())}.{nameof (this.OnExecutedAsync)} - (Custom)");
            return ResultDefaults.DefaultResult<Guid>();
        }
    }
}