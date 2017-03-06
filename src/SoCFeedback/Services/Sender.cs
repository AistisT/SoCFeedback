using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using zxm.MailKit;
using zxm.MailKit.Abstractions;

namespace SoCFeedback.Services
{
    public class Sender : MailSender
    {
        public Sender(MailServerOptions options) : base(options)
        {
        }
        public new Task SendEmailAsync(string to, string subject, string message)
        {
            return SendEmailAsync(new List<MailAddress> { new MailAddress { Address = to } }, subject, message);
        }

        private new Task SendEmailAsync(IEnumerable<MailAddress> tos, string subject, string message)
        {
            return SendEmailAsync(tos, null, subject, message);
        }

        private new async Task SendEmailAsync(IEnumerable<MailAddress> to, IEnumerable<MailAddress> bcc, string subject, string message)
        {
            if (to == null)
            {
                throw new ArgumentNullException(nameof(to));
            }

            if (to.Count() == 0)
            {
                throw new Exception("At least has one to mail address");
            }

            var emailMessage = new MimeMessage();

            emailMessage.From.Add(MailServerOptions.From == null
                ? new MailboxAddress(MailServerOptions.UserName, MailServerOptions.UserName)
                : new MailboxAddress(string.IsNullOrEmpty(MailServerOptions.From.DisplayName) ? MailServerOptions.From.Address : MailServerOptions.From.DisplayName, MailServerOptions.From.Address));
            foreach (var t in to)
            {
                emailMessage.To.Add(new MailboxAddress(string.IsNullOrEmpty(t.Address) ? t.Address : t.DisplayName, t.Address));
            }
            if (bcc != null)
            {
                foreach (var t in bcc)
                {
                    emailMessage.To.Add(new MailboxAddress(string.IsNullOrEmpty(t.Address) ? t.Address : t.DisplayName, t.Address));
                }
            }
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("html") { Text = message };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(MailServerOptions.Host, MailServerOptions.Port, false);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(MailServerOptions.UserName, MailServerOptions.Password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
