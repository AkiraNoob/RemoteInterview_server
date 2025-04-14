using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace MainService.Presentation.Controllers;

[Route("api/[controller]")]
[ApiVersionNeutral]
public class VersionNeutralApiController : BaseApiController
{
}