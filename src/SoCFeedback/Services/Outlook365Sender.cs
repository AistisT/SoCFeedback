using zxm.MailKit.Abstractions;

namespace SoCFeedback.Services
{
    public class Outlook365Sender:Sender
    {
        public Outlook365Sender(string userName, string password)
            : base(
                new MailServerOptions
                {
                    UserName = userName,
                    Password = password,
                    Host = "smtp.office365.com",
                    Port = 587
                })
        {
        }
    }
}
