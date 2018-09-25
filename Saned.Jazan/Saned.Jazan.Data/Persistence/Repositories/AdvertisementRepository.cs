using Saned.Jazan.Data.Core.Models;
using Saned.Jazan.Data.Core.Repositories;
using System;

using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saned.Common.Notification.Model;
using Saned.Jazan.Data.Core.Dtos;
using Z.EntityFramework.Plus;

namespace Saned.Jazan.Data.Persistence.Repositories
{

    public class AdvertisementRepository : BaseRepository<Advertisement>, IAdvertisementRepository
    {
        public async Task<Advertisement_SelectSummaryCount_Result> SelectAdvertisementSummaryCount(int? catgeoryId)
        {
            var catgeoryIdParamter = catgeoryId.HasValue ? new SqlParameter("@CategoryId", catgeoryId) : new SqlParameter("@CategoryId", DBNull.Value);
            var result = await _context.Database.SqlQuery<Advertisement_SelectSummaryCount_Result>
                ("Advertisement_SelectSummaryCount @CategoryId", catgeoryIdParamter).FirstOrDefaultAsync();
            return result;
        }

        public IQueryable<Advertisement> GetAll()
        {

            //return All().ToList();
            return _context.Advertisements.Include(u => u.Category).Include(u => u.Package).AsNoTracking();
        }
        public IQueryable<AdvertisementsViewModel> GetAll(int notificatianFeatureId)
        {
            //var ll = _context.Advertisements
            //    .Include("Category")
            //    .Include("Package")
            //    .IncludeFilter(u=>u.AdvertisementFeatures.Where(t => t.FeatureId == 7)).AsNoTracking();



            return _context.Advertisements
                .Include("Category")
                .Include("Package")
                .Include("AdvertisementFeatures")
                .Select(c => new AdvertisementsViewModel()
                {

                    Id = c.Id,
                    Name = c.Name,
                    UserId=c.CreatedBy,
                    CategoryId = c.CategoryId,
                    ImageUrl = c.ImageUrl,
                    PackageId = c.PackageId,
                    IsApproved = c.IsApproved,
                    CreatedOn = c.CreatedOn,
                    Category = c.Category,
                    Package = c.Package,
                    AdvertisementFeatures = c.AdvertisementFeatures.Where(u => u.FeatureId == 7).ToList()
                    //include any other fields needed here
                }).AsNoTracking();
            //var dddd = dd.ToList();
            // var ll = _context.Advertisements.Include("Category").Include("Package").Include("AdvertisementFeatures").Where(u=>u.AdvertisementFeatures.Any(t=>t.FeatureId== notificatianFeatureId)).AsNoTracking();
            // return dd;
        }


        public IQueryable<AdvertisementsViewModel> GetAll_Select(string keyword)
        {

            return _context.Advertisements
                .Include("Category").Where(u => u.IsApproved && (string.IsNullOrEmpty(keyword) || u.Name.Contains(keyword)))
                 .Select(c => new AdvertisementsViewModel()
                 {

                     Id = c.Id,
                     Name = c.Name,
                     CategoryId = c.CategoryId,
                     ImageUrl = c.ImageUrl,
                     PackageId = c.PackageId,
                     IsApproved = c.IsApproved,
                     CreatedOn = c.CreatedOn,
                     Category = c.Category,
                     //include any other fields needed here
                 }).AsNoTracking();
        }

        public async Task<List<Advertisement_SelectById_Result>> SelectById(int id)
        {
            var idParamter = new SqlParameter("Id", id);
            var result = await _context.Database.SqlQuery<Advertisement_SelectById_Result>("Advertisement_SelectById @Id", idParamter).ToListAsync();
            return result;
        }

        public Advertisement SelectAdId(int id)
        {
            return Find(id);
        }
        public int DeleteAd(int id)
        {

            // AdvertisementFeatures
            //AdvertisementImages
            //CulturalCompetitionQuestionSponsors
            var ad = _context.Advertisements.Find(id);
            if (ad != null) _context.Advertisements.Remove(ad);
            DeleteNotificationsByAdsId(id);
            return _context.SaveChanges();
        }
        public async Task<List<AdvertisementDto>> SelectPaged(AdvertisementParam advertisementParam)
        {

            var categoryIdParam = advertisementParam.CategoryId.HasValue ?
                             new SqlParameter("CategoryId", advertisementParam.CategoryId) :
                             new SqlParameter("CategoryId", DBNull.Value);

            var userIdParam = !string.IsNullOrEmpty(advertisementParam.UserId) ?
                 new SqlParameter("UserId", advertisementParam.UserId) :
                 new SqlParameter("UserId", DBNull.Value);

            var featureIdsParam = advertisementParam.FeatureId.HasValue ?
                            new SqlParameter("FeatureIds", advertisementParam.FeatureId.Value) :
                            new SqlParameter("FeatureIds", DBNull.Value);

            var advertisementIdParam = advertisementParam.AdvertisementId.HasValue ?
                            new SqlParameter("AdvertisementId", advertisementParam.AdvertisementId.Value) :
                            new SqlParameter("AdvertisementId", DBNull.Value);

            var isApprovedParam = advertisementParam.IsApproved.HasValue ?
                            new SqlParameter("IsApproved", advertisementParam.IsApproved.Value) :
                            new SqlParameter("IsApproved", DBNull.Value);

            var pageSizeParam = new SqlParameter("PageSize", advertisementParam.PageSize);
            var pageNumberParam = new SqlParameter("PageNumber", advertisementParam.PageNumber);

            return await _context.Database.SqlQuery<AdvertisementDto>("Advertisement_SelectPaging   @PageNumber , @PageSize , @CategoryId , @UserId , @FeatureIds , @AdvertisementId",
                                              pageNumberParam, pageSizeParam, categoryIdParam, userIdParam, featureIdsParam, advertisementIdParam).ToListAsync();

        }

        public async Task<List<AdvertisementDto>> SelectMainBannerAdvertisement()
        {
            return await _context.Database.SqlQuery<AdvertisementDto>("SelectMainBannerAdvertisement").ToListAsync();
        }

        public Advertisement SelectAdIdwithImages(int id)
        {
            return _context.Advertisements.Include(u => u.AdvertisementImages).Include(u => u.Category).Include(u => u.Package).Include(u => u.CreatedByUser).FirstOrDefault(u => u.Id == id);
        }

        public int GetSentNotificationCountById(int id)
        {
            string query = "SELECT Count(n.Id) FROM dbo.Notifications n WHERE n.RelatedId = " + id;
            var results = _context.Database.SqlQuery<int>(query).FirstOrDefault();
            return results;
        }

        public bool DeleteNotificationsByAdsId(int id)
        {
            try
            {
                string query = "DELETE FROM dbo.Notifications  WHERE RelatedTypeId=1 and RelatedId = " + id;
                var results = _context.Database.SqlQuery<int>(query).FirstOrDefault();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }


        public void DeleteAdvertisementCommentsRatingViewsAndNotifications(int id)
        {
            string query = @"delete from comments where commentTypeId = 1 and relatedid = @relatedid
                             delete from views where relatedTypeId = 1 and  relatedid = @relatedid
                             delete from RatingUserDetails where ratinguserid in 
                             (select id from RatingUsers where relatedid = @relatedid and relatedType = 1)
                             delete from RatingUsers where relatedid = @relatedid and relatedType = 1
                             delete from NotificationLogs where NotificationMessageId in (select id from Notifications where relatedid = @relatedid and relatedType = 1 )
                             delete from Notifications where relatedid = @relatedid and relatedType = 1 ";

            _context.Database.SqlQuery<object>(query, new SqlParameter("@relatedid", id)).FirstOrDefault();

        }


    }
}
