using Microsoft.AspNetCore.Mvc;
using WebStore.Services;

namespace WebStore.Controllers.Api;

[ApiController]
[Route(WebAddresses.Console)]
public class ConsoleApiController : ControllerBase
{
    [HttpGet("clear")]
    public void Clear() => Console.Clear();
    [HttpGet("write")]
    public void WriteLine(string str) => Console.WriteLine(str);
}