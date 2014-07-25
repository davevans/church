namespace Church.Components.Core
{
    public interface IChurchService
    {
        Model.Core.Church GetById(int churchId);
        void Add(Model.Core.Church church);
    }
}
