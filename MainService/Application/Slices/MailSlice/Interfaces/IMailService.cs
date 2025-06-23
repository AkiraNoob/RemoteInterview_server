using MainService.Application.Slices.MailSlice.Requests;

namespace MainService.Application.Slices.MailSlice.Interfaces;

public interface IMailService 
{
    public Task SendEmailAsync(MailRequest request, CancellationToken cancellationToken = default);
}
