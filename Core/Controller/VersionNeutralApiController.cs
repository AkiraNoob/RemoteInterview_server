using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Core.Controller;

[Route("api/[controller]")]
[ApiVersionNeutral]
public class VersionNeutralApiController : BaseApiController
{
}