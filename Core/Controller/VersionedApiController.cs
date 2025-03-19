using Core.Controller;
using Microsoft.AspNetCore.Mvc;

namespace Core.Controller;

[Route("api/v{version:apiVersion}/[controller]")]
public class VersionedApiController : BaseApiController
{
}