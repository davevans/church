using System.Threading.Tasks;
using Church.Common.Structures;

namespace Church.Components.Core.Repository
{
    public interface IChurchRepository
    {
        Task<Model.Church> GetByIdAsync(int churchId);
        Task<Result<Model.Church>> TryAddAsync(Model.Church church);
        Task<Result<Model.Church>> TryUpdateAsync(Model.Church church);
    }
}
