using Softoverse.CqrsKit.Abstractions.Filters;
using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Models;
using Softoverse.CqrsKit.Models.Utility;
using Softoverse.CqrsKit.Services;
using Softoverse.CqrsKit.TestConsole.CQRS.Events.Command;
using Softoverse.CqrsKit.TestConsole.Models;

namespace Softoverse.CqrsKit.TestConsole.CQRS.Filters.Command
{
    [ScopedLifetime]
    public class PersonCreateCommandExecutionFilter : CommandExecutionFilterBase<PersonCreateCommand, Person>
    {
        public override Task<Result<Person>> OnExecutingAsync(PersonCreateCommand command, CqrsContext context, CancellationToken ct = default)
        {
            Console.WriteLine($"Method Call: {UtilityHelper.GetFormattedTypeName(this.GetType())}.{nameof (this.OnExecutingAsync)} - (Custom)");
            return ResultDefaults.DefaultResult<Person>();
        }

        public override Task<Result<Person>> OnExecutedAsync(PersonCreateCommand command, CqrsContext context, CancellationToken ct = default)
        {
            Console.WriteLine($"Method Call: {UtilityHelper.GetFormattedTypeName(this.GetType())}.{nameof (this.OnExecutedAsync)} - (Custom)");
            return ResultDefaults.DefaultResult<Person>();
        }
    }

    [ScopedLifetime]
    public class PersonSecondCreateCommandExecutionFilter : CommandExecutionFilterBase<PersonCreateCommand, Person>
    {
        public override Task<Result<Person>> OnExecutingAsync(PersonCreateCommand command, CqrsContext context, CancellationToken ct = default)
        {
            Console.WriteLine($"Method Call: {UtilityHelper.GetFormattedTypeName(this.GetType())}.{nameof (this.OnExecutingAsync)} - (Custom)");
            return ResultDefaults.DefaultResult<Person>();
        }

        public override Task<Result<Person>> OnExecutedAsync(PersonCreateCommand command, CqrsContext context, CancellationToken ct = default)
        {
            Console.WriteLine($"Method Call: {UtilityHelper.GetFormattedTypeName(this.GetType())}.{nameof (this.OnExecutedAsync)} - (Custom)");
            return ResultDefaults.DefaultResult<Person>();
        }
    }
}