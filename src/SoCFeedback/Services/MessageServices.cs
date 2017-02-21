using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using zxm.MailKit.Outlook365;

namespace SoCFeedback.Services
{
    public class AuthMessageSender : IEmailSender
    {
        public AuthMessageSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public Task SendEmailAsync(string email, string subject, string message)
        {
            var mailsender = new Outlook365Sender("ataraskevicius@dundee.ac.uk", "XinyueWuXinyueWu");
            return mailsender.SendEmailAsync(email, subject, message);
        }
    }
}