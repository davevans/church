using Church.Common.Structures;

namespace Church.Components.Core.Repository
{
    public interface IChurchRepository
    {
        Model.Church GetById(int churchId);
        bool TryAdd(Model.Church church, out Error error);
    }
}
