using System.Threading.Tasks;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Services.Models;

namespace Cynosura.Studio.Core.Services
{
    public interface IUserService
    {        
        Task<User> GetUserAsync(int id);
        Task<PageModel<User>> GetUsersAsync(int? pageIndex = null, int? pageSize = null);
        Task<int> CreateUserAsync(UserCreateModel model);
        Task UpdateUserAsync(int id, UserUpdateModel model);
        Task DeleteUserAsync(int id);
    }
}
