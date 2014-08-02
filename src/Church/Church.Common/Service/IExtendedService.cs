namespace Church.Common.Service
{
    public interface IExtendedService : IService
    {
        void PreStart();
        void PreStop();
    }
}
