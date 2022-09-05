using System.Threading;
using System.Threading.Tasks;
using CinemaBookingSystem.Common.Mailer.Models;

namespace CinemaBookingSystem.Common.Mailer.Abstractions
{
    public interface IMailService
    {
        Task<bool> SendAsync(MailData mailData, CancellationToken ct);
        string GetEmailTemplate<T>(string emailTemplate, T emailTemplateModel);
    }
}
