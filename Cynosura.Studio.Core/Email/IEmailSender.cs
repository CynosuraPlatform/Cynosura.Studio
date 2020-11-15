using System.Threading.Tasks;
using Cynosura.Studio.Core.Email.Models;

namespace Cynosura.Studio.Core.Email
{
    public interface IEmailSender
    {
        Task SendAsync(EmailModel model);
    }
}
