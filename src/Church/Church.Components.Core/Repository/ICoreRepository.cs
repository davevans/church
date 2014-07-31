namespace Church.Components.Core.Repository
{
    public interface ICoreRepository
    {
        Model.Church GetById(int churchId);
        void Add(Model.Church church);
    }
}
