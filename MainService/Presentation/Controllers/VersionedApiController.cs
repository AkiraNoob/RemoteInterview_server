using Microsoft.AspNetCore.Mvc;

namespace MainService.Presentation.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
public class VersionedApiController : BaseApiController
{
}