using Saned.Jazan.Data.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Saned.Jazan.Data.Persistence.Repositories
{
    public class BaseRepository<TObject> : IBaseRepository<TObject>
        where TObject : class

    {
        private bool shareContext = false;

        public BaseRepository()
        {
            _context = new ApplicationDbContext();
        }

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
            shareContext = true;
        }

        protected ApplicationDbContext _context = null;

        protected DbSet<TObject> DbSet
        {
            get
            {
                return _context.Set<TObject>();
            }
        }

        public void Dispose()
        {
            if (shareContext && (_context != null))
                _context.Dispose();
        }

        public virtual IQueryable<TObject> All()
        {
            return DbSet.AsQueryable();
        }

        public virtual IQueryable<TObject> Filter(Expression<Func<TObject, bool>> predicate)
        {
            return DbSet.Where(predicate).AsQueryable<TObject>();
        }

        public virtual IQueryable<TObject> Filter(Expression<Func<TObject, bool>> filter, out int total, int index = 0, int size = 50)
        {
            int skipCount = index * size;
            var _resetSet = filter != null ? DbSet.Where(filter).AsQueryable() : DbSet.AsQueryable();
            _resetSet = skipCount == 0 ? _resetSet.Take(size) : _resetSet.Skip(skipCount).Take(size);
            total = _resetSet.Count();
            return _resetSet.AsQueryable();
        }

        public bool Contains(Expression<Func<TObject, bool>> predicate)
        {
            return DbSet.Count(predicate) > 0;
        }

        public virtual TObject Find(params object[] keys)
        {
            return DbSet.Find(keys);
        }

        public virtual TObject Find(Expression<Func<TObject, bool>> predicate)
        {
            return DbSet.FirstOrDefault(predicate);
        }

        public virtual TObject Create(TObject TObject)
        {
            var newEntry = DbSet.Add(TObject);
            if (!shareContext)
                _context.SaveChanges();
            return newEntry;
        }

        public virtual async Task<TObject> CreateAsync(TObject TObject)
        {
            var newEntry = DbSet.Add(TObject);
            if (!shareContext)
              await  _context.SaveChangesAsync();
            return newEntry;
        }

        public virtual int Count
        {
            get
            {
                return DbSet.Count();
            }
        }

        public virtual int Delete(TObject TObject)
        {
            DbSet.Remove(TObject);
            if (!shareContext)
                return _context.SaveChanges();
            return 0;
        }

        public virtual int Update(TObject TObject)
        {
            var entry = _context.Entry(TObject);
            DbSet.Attach(TObject);
            entry.State = EntityState.Modified;
            if (!shareContext)
                return _context.SaveChanges();
            return 0;
        }

        public virtual int Delete(Expression<Func<TObject, bool>> predicate)
        {
            var objects = Filter(predicate);
            foreach (var obj in objects)
                DbSet.Remove(obj);
            if (!shareContext)
                return _context.SaveChanges();
            return 0;
        }

        public async Task<int> DeleteAsync(Expression<Func<TObject, bool>> predicate)
        {
            var objects = Filter(predicate);
            foreach (var obj in objects)
                DbSet.Remove(obj);
            if (!shareContext)
                return await _context.SaveChangesAsync();
            return 0;
        }

        public async Task<int> UpdateAsync(TObject TObject)
        {
            var entry = _context.Entry(TObject);
            DbSet.Attach(TObject);
            entry.State = EntityState.Modified;
            if (!shareContext)
                return await _context.SaveChangesAsync();
            return 0;
        }
    }
}
