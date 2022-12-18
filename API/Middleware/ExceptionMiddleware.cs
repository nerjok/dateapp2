using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Middleware
{
  public class ExceptionMiddleware
  {
    public readonly RequestDelegate next;
    private readonly ILogger<ExceptionMiddleware> logger;
    private readonly IHostEnvironment env;
    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
      this.env = env;
      this.logger = logger;
      this.next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
      try
      {
        await next(httpContext);
      }
      catch (System.Exception ex)
      {

        logger.LogError(ex, ex.Message);
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;

        var response = env.IsDevelopment()

        ? new errors.ApiException(httpContext.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
        :
        new errors.ApiException(httpContext.Response.StatusCode, ex.Message, "Internal Server error");

        var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
        var json = JsonSerializer.Serialize(response, options);
        await httpContext.Response.WriteAsync(json);

      }
    }
  }
}