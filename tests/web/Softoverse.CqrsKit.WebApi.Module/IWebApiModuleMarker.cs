using Microsoft.AspNetCore.Mvc.Filters;

using Softoverse.CqrsKit.WebApi.DataAccess;

namespace Softoverse.CqrsKit.WebApi.Module;

public interface IWebApiModuleMarker;

public class CustomAuthorize : ActionFilterAttribute
{
}