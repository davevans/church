using System.Collections.Generic;
using System.Threading.Tasks;
using Church.Common.Structures;
using Church.Components.Account.Model;

namespace Church.Components.Account.Repository
{
    public interface IUserRepository
    {
        Task<Result<IEnumerable<User>>> GetAllActiveUsersAsync();
        Task<Result<User>> AddUserAsync(User userToAdd);
    }
}