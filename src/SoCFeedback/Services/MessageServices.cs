﻿using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using zxm.MailKit.Outlook365;

namespace SoCFeedback.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender
    {
        public AuthMessageSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public Task SendEmailAsync(string email, string subject, string message)
        {
            // Plug in your email service here to send an email.

            var mailsender = new Outlook365MailSender("ataraskevicius@dundee.ac.uk", "XinyueWuXinyueWu");
            // mailsender.SendEmailAsync(email, subject, message);
            //var myMessage = new SendGrid.SendGridMessage();
            //myMessage.AddTo(email);
            //myMessage.From = new System.Net.Mail.MailAddress("ataraskevicius@dundee.ac.uk", "Aistis Taraskevicius");
            //myMessage.Subject = subject;
            //myMessage.Text = message;
            //myMessage.Html = message;
            //var credentials = new System.Net.NetworkCredential(
            //    Options.SendGridUser,
            //    Options.SendGridKey);
            //// Create a Web transport for sending email.
            //var transportWeb = new SendGrid.Web(credentials);
            return mailsender.SendEmailAsync(email, subject, message);
        }
    }
}