using Microsoft.AspNetCore.Mvc;

namespace StudioManager.API.Base;

[Route("[controller]")]
[ApiController]
//[Authorize(Policy = "AuthorizedUser")]
public abstract class CoreController : ControllerBase;