using System.Threading.Tasks;

namespace SoCFeedback.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}