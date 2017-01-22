using System.Threading.Tasks;

namespace SoCFeedback.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}