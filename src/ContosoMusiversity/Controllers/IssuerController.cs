using Microsoft.AspNetCore.Mvc;
using MicrosoftEntra.VerifiedId.Client;

namespace ContosoMusiversity.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IssuerController : ControllerBase
{
    private readonly ILogger<IssuerController> logger;

    public IssuerController(ILogger<IssuerController> logger, RequestClient requestClient)
    {
        this.logger = logger;
    }

    [HttpGet]
    public string Get()
    {
        return "ok";
    }
}