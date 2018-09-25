using Saned.Jazan.Data.Core.Dtos;
using Saned.Jazan.Data.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saned.Jazan.Data.Core.Repositories
{
    public interface IAdvertisementRepository : IBaseRepository<Advertisement>, IDisposable
    {
        Task<List<Advertisement_SelectById_Result>> SelectById(int id);
        Advertisement SelectAdId(int id);
        int DeleteAd(int id);
        Task<List<AdvertisementDto>> SelectPaged(AdvertisementParam advertisementComplex);

        Task<List<AdvertisementDto>> SelectMainBannerAdvertisement();
         
        IQueryable<AdvertisementsViewModel> GetAll_Select(string keyword);


        Task<Advertisement_SelectSummaryCount_Result> SelectAdvertisementSummaryCount(int? catgeoryId);
        IQueryable<Advertisement> GetAll();
        IQueryable<AdvertisementsViewModel> GetAll(int featureId);

        Advertisement SelectAdIdwithImages(int id);


        int GetSentNotificationCountById(int id);

        void DeleteAdvertisementCommentsRatingViewsAndNotifications(int id);


    }
}
