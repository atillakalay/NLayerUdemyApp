namespace NLayer.Core.UnitOfWorks
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
        void commit();
    }
}
