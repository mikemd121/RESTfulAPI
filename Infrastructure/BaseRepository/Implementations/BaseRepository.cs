using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure
{
    public class BaseRepository<T> : IBaseRepository<T>
           where T : class
    {
        internal readonly CoreWebAppDbContext sanaDbContext;
        internal DbSet<T> _dbSet;
        DbContextOptionsBuilder<CoreWebAppDbContext> _optionsBuilder;
        public BaseRepository()
        {
            sanaDbContext = sanaDbContext = new CoreWebAppDbContext(_optionsBuilder.Options);
            _dbSet = sanaDbContext.Set<T>();
        }

        public BaseRepository(CoreWebAppDbContext _sanaDbContext)
        {
            sanaDbContext = _sanaDbContext;
            _dbSet = _sanaDbContext.Set<T>();
        }

        public bool Exists(object primaryKey)
        {
            return _dbSet.Find(primaryKey) == null ? false : true;
        }

        public virtual IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "", int start = 0, int length = 0)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);

            foreach (var includeProperty in includeProperties.Split
                (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty);

            if (orderBy != null)
                query = orderBy(query);

            if (length != 0)
                query = query.Skip(start).Take(length);

            return query.ToList();
        }

        public virtual int Count(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);

            foreach (var includeProperty in includeProperties.Split
                (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty);

            if (orderBy != null)
                return orderBy(query).Count();
            return query.Count();
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.AsEnumerable().ToList();
        }

        public virtual void Insert(T entity)
        {
            dynamic obj = _dbSet.Add(entity);
        }

        public virtual void Update(T entity)
        {
            _dbSet.Attach(entity);
            sanaDbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            if (sanaDbContext.Entry(entity).State == EntityState.Detached)
                _dbSet.Attach(entity);
            _dbSet.Remove(entity);
        }

        public void Delete(object primaryKey)
        {
            var entityToDelete = _dbSet.Find(primaryKey);
            Delete(entityToDelete);
            _dbSet.Remove(entityToDelete);
        }


        public T GetById(object primaryKey)
        {
            var dbResult = _dbSet.Find(primaryKey);
            return dbResult;
        }

        //public void Delete(Expression<Func<T, bool>> filter)
        //{
        //    sanaDbContext.BulkDelete(filter);
        //}


    }
}
