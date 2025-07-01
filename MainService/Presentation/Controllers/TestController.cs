using MainService.Application.Slices.MailSlice.Interfaces;
using MainService.Application.Slices.MailSlice.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MainService.Presentation.Controllers;

public class TestController(IMailService mailService) : VersionedApiController
{
    [AllowAnonymous]
    [HttpPost("send-email")]
    [EndpointDescription("Sends an email using the configured mail service.")]
    public async Task<IActionResult> SendEmailAsync([FromBody] SendTestingMailRequest request, CancellationToken cancellationToken = default)
    {
        await mailService.SendEmailAsync(new MailRequest([request.To], request.Subject, request.Content), cancellationToken);

        return Ok();
    }
}
