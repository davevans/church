using System.Threading.Tasks;

namespace Church.Components.Account
{
    public interface IUserService
    {
        Task<Model.User> AddUserAsync(Model.User user);
        Task<Model.User> AddUserFromPerson(long personId, string plainTextPassword);
    }
}