namespace Church.Components.Core
{
    public interface IChurchService
    {
        Model.Church GetById(int churchId);
        void Add(Model.Church church);
    }
}
