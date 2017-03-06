using System;
using System.Threading.Tasks;

namespace SoCFeedback.Services
{
    public class AuthMessageSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var mailsender = new Outlook365Sender("doc-feedback@dundee.ac.uk", "faymmimerwdkmg");
                await mailsender.SendEmailAsync(email, subject, message);
            }
            catch (Exception e)
            {

                var mailsender = new Outlook365Sender("doc-request@dundee.ac.uk", "dcbdmjhbdmvhmj");
                 #pragma warning disable 4014
                 mailsender.SendEmailAsync(email, subject, message);
                #pragma warning restore 4014
            }
        }
    }
}