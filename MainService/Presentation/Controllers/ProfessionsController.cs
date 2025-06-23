using MainService.Application.Slices.ProfessionSlice.DTOs;
using MainService.Application.Slices.ProfessionSlice.Requests;
using Microsoft.AspNetCore.Mvc;

namespace MainService.Presentation.Controllers;

public class ProfessionsController : VersionedApiController
{
    [HttpPost("")]
    public async Task<IActionResult> AddProfessionsAsync([FromBody] AddProfessionsRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request, cancellationToken);
        return Ok();
    }

    [HttpGet("")]
    public async Task<ICollection<ProfessionDTO>> GetAllProfessionsAsync(CancellationToken cancellationToken)
    {
       return await Mediator.Send(new GetAllProfessionsRequest(), cancellationToken);
    }


    [HttpPut("${recruitmentId:guid}")]
    public async Task<IActionResult> UpdateProfessionAsync(Guid recruitmentId, [FromBody] UpdateProfessionDTO request, CancellationToken cancellationToken)
    {
        await Mediator.Send(new UpdateProfessionRequest(recruitmentId, request), cancellationToken);
        return Ok();
    }


    [HttpDelete("${recruitmentId:guid}")]
    public async Task<IActionResult> DeleteProfessionAsync(Guid recruitmentId, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteProfessionRequest(recruitmentId), cancellationToken);
        return Ok();
    }
}
