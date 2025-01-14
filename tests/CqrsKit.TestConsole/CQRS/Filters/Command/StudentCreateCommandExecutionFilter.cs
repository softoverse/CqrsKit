using CqrsKit.Attributes;
using CqrsKit.Filters;
using CqrsKit.Model;
using CqrsKit.Model.Utility;
using CqrsKit.Services;
using CqrsKit.TestConsole.CQRS.Events.Command;
using CqrsKit.TestConsole.Models;

namespace CqrsKit.TestConsole.CQRS.Filters.Command
{
    [ScopedLifetime]
    public class StudentCreateCommandExecutionFilter : CommandExecutionFilterBase<StudentCreateCommand, Student>
    {
        public override Task<Response<Student>> OnExecutingAsync(CqrsContext context, CancellationToken ct = default)
        {
            Console.WriteLine($"Method Call: {UtilityHelper.GetFormattedTypeName(this.GetType())}.{nameof (this.OnExecutingAsync)} - (Custom)");
            return ResponseDefaults.DefaultResponse<Student>();
        }

        public override Task<Response<Student>> OnExecutedAsync(CqrsContext context, CancellationToken ct = default)
        {
            Console.WriteLine($"Method Call: {UtilityHelper.GetFormattedTypeName(this.GetType())}.{nameof (this.OnExecutedAsync)} - (Custom)");
            return ResponseDefaults.DefaultResponse<Student>();
        }
    }
}