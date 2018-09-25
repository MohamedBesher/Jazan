using Saned.Jazan.Data.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saned.Jazan.Data.Core.Models;
using System.Data.SqlClient;

namespace Saned.Jazan.Data.Persistence.Repositories
{
    public class FeatureRepository : BaseRepository<Feature>, IFeatureRepository
    {
        //private readonly ApplicationDbContext _context;
        public FeatureRepository(ApplicationDbContext _context) : base(_context)
        {
            //  _context = new ApplicationDbContext();
        }


        public async Task<List<Feature>> SelectAllFeatures()
        {
            try
            {
                return await _context.Database.SqlQuery<Feature>("Features_SelectAll").ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Feature> UpdateFeature(Feature feature)
        {
            try
            {
                var oldPackage = await _context.Features.FindAsync(feature.Id);              
                oldPackage.ArabicName = feature.ArabicName;
                oldPackage.EnglishName = feature.EnglishName;              
                await _context.SaveChangesAsync();

                return oldPackage;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public Feature SelectById(int id)
        {
            return _context.Features.FirstOrDefault(u => u.Id == id);
        }
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

       
    }
}
