using Saned.Jazan.Data.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using Saned.Jazan.Data.Core.Models;
using System.Data.SqlClient;
using Saned.Jazan.Data.Core.Dtos;

namespace Saned.Jazan.Data.Persistence.Repositories
{
    public class PackageRepository : BaseRepository<Package>, IPackageRepository, IDisposable
    {
        //private readonly ApplicationDbContext _context;
        public PackageRepository(ApplicationDbContext _context) : base(_context)
        {
            //_context = new ApplicationDbContext();
        }

        public async Task<List<Advertisements_Packages_SelectAll_Result>> SelectAllPackages()
        {
            return await _context.Database.SqlQuery<Advertisements_Packages_SelectAll_Result>("Packages_SelectAll").ToListAsync();
        }

        public async Task<List<PackageFeaturesDto>> SelectById(int id)
        {
            var idParamter = new SqlParameter("Id", id);
            return await _context.Database.SqlQuery<PackageFeaturesDto>("Features_SelectByPackageId @Id", idParamter).ToListAsync();
        }

        public async Task<Package> UpdatePackage(Package package)
        {
            try
            {
                var oldPackage = await _context.Packages.FindAsync(package.Id);

                oldPackage.UpdatedBy = package.UpdatedBy;
                oldPackage.UpdatedOn = DateTime.Now;
                oldPackage.ArabicName = package.ArabicName;
                oldPackage.EnglishName = package.EnglishName;
                oldPackage.Period = package.Period;
                oldPackage.Price = package.Price;

                await _context.SaveChangesAsync();

                return oldPackage;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<bool> DeletePackage(int id)
        {
            try
            {
                var oldPackage = await _context.Packages.FindAsync(id);
                _context.Packages.Remove(oldPackage);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> InsertPackage(Package package)
        {
            try
            {
                _context.Packages.Add(package);
                await _context.SaveChangesAsync();
                return package.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<PackageDto>> SelectPagedPackages(PackageParamterDto packageParamterDto)
        {
            var nameParamter = string.IsNullOrEmpty(packageParamterDto.Name) ? new SqlParameter("Name", DBNull.Value) : new SqlParameter("Name", packageParamterDto.Name);
            var periodParamter = !packageParamterDto.Period.HasValue ? new SqlParameter("Period", DBNull.Value) : new SqlParameter("Period", packageParamterDto.Period.Value);
            var priceParamter = !packageParamterDto.Price.HasValue ? new SqlParameter("Price", DBNull.Value) : new SqlParameter("Price", packageParamterDto.Price.Value);

            var pageSize = new SqlParameter("PageSize", packageParamterDto.PageSize);

            var pageNumber = new SqlParameter("PageNumber", packageParamterDto.PageNumber);

            return await _context.Database.SqlQuery<PackageDto>("Packages_SelectPaging @PageNumber,@PageSize,@Name,@Period,@Price").ToListAsync();
        }

        public List<Package> GetAll()
        {
           return All().AsNoTracking().ToList();
        }

        public Package SelectByIdwithFeatures(int id)
        {

            return _context.Packages.Include(u => u.PackageFeatures.Select(x => x.Feature)).FirstOrDefault(u => u.Id == id);

        }


        public Package SelectPackageById(int id)
        {
            return _context.Packages.FirstOrDefault(u => u.Id == id);
        }
        #region IDisposable Support
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

        

        #endregion



    }
}
