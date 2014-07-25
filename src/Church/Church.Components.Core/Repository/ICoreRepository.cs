namespace Church.Components.Core.Repository
{
    public interface ICoreRepository
    {
        Model.Core.Church GetById(int churchId);
        void Add(Model.Core.Church church);
    }
}
