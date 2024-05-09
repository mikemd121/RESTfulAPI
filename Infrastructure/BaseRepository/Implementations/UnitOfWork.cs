using Core;
using System.Linq.Expressions;

namespace Infrastructure
{
    public  class UnitOfWork : IUnitOfWork
    {
        #region constructors
        private CoreWebAppDbContext _context;
        public UnitOfWork(CoreWebAppDbContext context)
        {
            this._context = context;
        }
        #endregion

        #region private-fields
        private bool disposed;
        private readonly CoreWebAppDbContext context;

        #endregion

        #region Methods

        private void getMethod()
        {
            Expression<Func<Insurance, bool>> filter = f => f.InsuranceRate == 4545;
            var  dsd= _context.Insurances.AsQueryable().Where(filter);
        }

        public IBaseRepository<T> GetRepository<T>() where T : class
        {
            var repositoryInstance = new BaseRepository<T>(_context);
            return repositoryInstance;
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() > 0;
        }

        #endregion

        #region Disposing

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
                if (disposing)
                    _context.Dispose();
            disposed = true;
        }
        #endregion
    }
}
