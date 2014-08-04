using Church.Common.Service;

namespace Church.Components.Core
{
    public interface IChurchService : IService
    {
        Model.Church GetById(int churchId);
        void Add(Model.Church church);
        void Update(Model.Church church);

    }
}
