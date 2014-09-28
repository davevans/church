using System.Threading.Tasks;
using Church.Common.Service;

namespace Church.Components.Core
{
    public interface IChurchService : IService
    {
        
        Task<Model.Church> GetByIdAsync(int churchId);
        Task<Model.Church> AddAsync(Model.Church church);
        Task<Model.Church> UpdateAsync(Model.Church church);
        Task ArchiveAsync(Model.Church church);
    }
}
