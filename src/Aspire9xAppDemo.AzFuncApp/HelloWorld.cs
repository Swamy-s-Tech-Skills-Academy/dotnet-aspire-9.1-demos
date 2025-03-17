using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Aspire9xAppDemo.AzFuncApp;

public class HelloWorld(ILogger<HelloWorld> logger)
{
    private readonly ILogger<HelloWorld> _logger = logger;

    [Function("Function1")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        return new OkObjectResult("Welcome to Azure Functions!");
    }
}
