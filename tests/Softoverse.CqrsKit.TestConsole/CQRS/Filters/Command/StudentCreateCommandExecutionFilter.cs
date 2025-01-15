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
    public class StudentCreateCommandExecutionFilter : CommandExecutionFilterBase<StudentCreateCommand, Student>
    {
        public override Task<Result<Student>> OnExecutingAsync(CqrsContext context, CancellationToken ct = default)
        {
            Console.WriteLine($"Method Call: {UtilityHelper.GetFormattedTypeName(this.GetType())}.{nameof (this.OnExecutingAsync)} - (Custom)");
            return ResponseDefaults.DefaultResponse<Student>();
        }

        public override Task<Result<Student>> OnExecutedAsync(CqrsContext context, CancellationToken ct = default)
        {
            Console.WriteLine($"Method Call: {UtilityHelper.GetFormattedTypeName(this.GetType())}.{nameof (this.OnExecutedAsync)} - (Custom)");
            return ResponseDefaults.DefaultResponse<Student>();
        }
    }
}