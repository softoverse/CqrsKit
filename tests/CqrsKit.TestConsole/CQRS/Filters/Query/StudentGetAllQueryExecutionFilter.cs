using CqrsKit.Attributes;
using CqrsKit.Filters;
using CqrsKit.Model;
using CqrsKit.Model.Utility;
using CqrsKit.Services;
using CqrsKit.TestConsole.CQRS.Events.Query;
using CqrsKit.TestConsole.Models;

namespace CqrsKit.TestConsole.CQRS.Filters.Query;

[ScopedLifetime]
public class StudentGetAllQueryExecutionFilter : QueryExecutionFilterBase<StudentGetAllQuery, List<Student>>
{
    public override Task<Response<List<Student>>> OnExecutingAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {UtilityHelper.GetFormattedTypeName(this.GetType())}.{nameof (this.OnExecutingAsync)} - (Custom)");
        return ResponseDefaults.DefaultResponse<List<Student>>();
    }

    public override Task<Response<List<Student>>> OnExecutedAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {UtilityHelper.GetFormattedTypeName(this.GetType())}.{nameof (this.OnExecutingAsync)} - (Custom)");
        return ResponseDefaults.DefaultResponse<List<Student>>();
    }
}