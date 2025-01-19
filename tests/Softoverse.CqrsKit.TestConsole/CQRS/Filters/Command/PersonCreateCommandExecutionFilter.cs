using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Filters;
using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Utility;
using Softoverse.CqrsKit.Services;
using Softoverse.CqrsKit.TestConsole.CQRS.Events.Command;
using Softoverse.CqrsKit.TestConsole.Models;

namespace Softoverse.CqrsKit.TestConsole.CQRS.Filters.Command
{
    [ScopedLifetime]
    public class PersonCreateCommandExecutionFilter : CommandExecutionFilterBase<PersonCreateCommand, Person>
    {
        public override Task<Result<Person>> OnExecutingAsync(CqrsContext context, CancellationToken ct = default)
        {
            Console.WriteLine($"Method Call: {UtilityHelper.GetFormattedTypeName(this.GetType())}.{nameof (this.OnExecutingAsync)} - (Custom)");
            return ResponseDefaults.DefaultResponse<Person>();
        }

        public override Task<Result<Person>> OnExecutedAsync(CqrsContext context, CancellationToken ct = default)
        {
            Console.WriteLine($"Method Call: {UtilityHelper.GetFormattedTypeName(this.GetType())}.{nameof (this.OnExecutedAsync)} - (Custom)");
            return ResponseDefaults.DefaultResponse<Person>();
        }
    }
}