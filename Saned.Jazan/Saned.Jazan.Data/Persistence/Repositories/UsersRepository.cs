using Saned.Jazan.Data.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Saned.Jazan.Data.Core.Dtos;
using Saned.Jazan.Data.Core.Models;
using System.Linq;
using System.Web.Security;
using Z.EntityFramework.Plus;

namespace Saned.Jazan.Data.Persistence.Repositories
{
    public class UsersRepository : BaseRepository<ApplicationUser>, IUsersRepository, IDisposable
    {
        //private readonly ApplicationDbContext _context;
        public UsersRepository(ApplicationDbContext _context) : base(_context)
        {
            // _context = new ApplicationDbContext();
        }

        #region Helper
        KeyValuePair<string, object> KVP(string k, object v)
        {
            return new KeyValuePair<string, object>(k, v);
        }
        #endregion
        
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IQueryable<ApplicationUser> GetAllUsers()
        {
           

            string roleId = _context.Roles.FirstOrDefault(u => u.Name == "User")?.Id;
            return _context.Users.Where(m => m.Roles.Any(r => r.RoleId == roleId)).AsNoTracking(); ;

        }

        public ApplicationUser GetUserbyId(string id)
        {
            return _context.Users.FirstOrDefault(u=>u.Id==id);          
        }

        public IQueryable<Advertisement> GetAdvertisementbyUserId(string id)
        {
            return _context.Advertisements.Include(u => u.Category).Include(u => u.Package).Where(u=>u.CreatedBy==id);
        }

        public IQueryable<TouristVisit> GetTouristVisitsbyUserId(string id)
        {
           
            return _context.TouristVisits.Where(u => u.CreatedBy == id);
        }

        public void Update()
        {
            _context.SaveChanges();
        }

        public void DeleteUser(ApplicationUser selected)
        {
            _context.Users.Remove(selected);
            _context.SaveChanges();
        }
    }
}
