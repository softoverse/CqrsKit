using System.Text;

namespace Softoverse.CqrsKit.WebApi.Middlewares;

public class DocumentationAuthorizeMiddleware : IMiddleware
{

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var configuration = context.RequestServices.GetRequiredService<IConfiguration>();

        if (context.Request.Path.StartsWithSegments("/swagger") || context.Request.Path.StartsWithSegments("/scalar"))
        {
            string basicHeader = context.Request.Headers.Authorization.ToString();

            if (string.IsNullOrEmpty(basicHeader))
            {
                context.Response.StatusCode = 401;
                context.Response.Headers.WWWAuthenticate = "Basic realm=\"http://localhost:5001\"";
                return;
            }

            if (basicHeader.Contains(' '))
            {
                basicHeader = basicHeader.Split(' ')[1];
            }
            var decodedString = Encoding.UTF8.GetString(Convert.FromBase64String(basicHeader));

            // splitting decodeAuthToken using ':'
            var splitText = decodedString.Split([':']);
            var isValidBasicHeader = splitText[0] == configuration["JWT:ClientId"] && splitText[1] == configuration["JWT:ClientSecret"];

            if (!isValidBasicHeader)
            {
                context.Response.StatusCode = 401;
                context.Response.Headers.WWWAuthenticate = "Basic realm=\"http://localhost:5001\"";
                return;
            }
        }

        await next(context);
    }
}