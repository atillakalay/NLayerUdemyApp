using NLayer.Core.UnitOfWorks;

namespace NLayer.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void commit()
        {
            _context.SaveChanges();
        }
    }
}
