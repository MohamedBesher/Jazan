using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Saned.Jazan.Data.Core.Models;
using Saned.Jazan.Data.Persistence;
using Saned.Jazan.Data.Core.Repositories;
using Saned.Jazan.Api.Models;
using Saned.Jazan.Data.Persistence.Repositories;
using Saned.Jazan.Data.Core.Dtos;
using Microsoft.AspNet.Identity;

namespace Saned.Jazan.Api.Controllers
{
    [RoutePrefix("api/Advertisements")]
    public class AdvertisementsController : BasicController
    {
        private IAdvertisementRepository IAdvertisementRepository;
        private IAdvertisementImageRepository IAdvertisementImageRepository;
        private IAdvertisementFeatureRepository IAdvertisementFeatureRepository;
        private IPackageRepository IPackageRepository;
        private ICulturalCompetitionQuestionSponsorsRepository ICulturalCompetitionQuestionSponsorsRepository;
        public AdvertisementsController(IAdvertisementRepository IAdvertisementRepository,
                                        IAdvertisementImageRepository IAdvertisementImageRepository,
                                        IAdvertisementFeatureRepository IAdvertisementFeatureRepository,
                                        IPackageRepository IPackageRepository,
                                        ICulturalCompetitionQuestionSponsorsRepository ICulturalCompetitionQuestionSponsorsRepository)
        {
            this.IAdvertisementRepository = IAdvertisementRepository;
            this.IAdvertisementImageRepository = IAdvertisementImageRepository;
            this.IAdvertisementFeatureRepository = IAdvertisementFeatureRepository;
            this.IPackageRepository = IPackageRepository;
            this.ICulturalCompetitionQuestionSponsorsRepository = ICulturalCompetitionQuestionSponsorsRepository;
        }


        [HttpPost]
        [Route("GetPagedAdvertisement")]
        public async Task<IHttpActionResult> GetAdvertisements(AdvertisementParam advertisementParam)
        {
            //advertisementParam.PageSize = (int)pageSize;

            var result = await IAdvertisementRepository.SelectPaged(advertisementParam);

            var firstBanner = await IAdvertisementRepository.SelectPaged(new AdvertisementParam()
            {
                FeatureId = 2,
                CategoryId = advertisementParam.CategoryId,
                PageNumber = advertisementParam.PageNumber,
                PageSize = 1,

            });

            return Ok(new
            {
                advertisements = result,
                banner = firstBanner != null ? firstBanner.FirstOrDefault() : null
            });
        }

        [HttpGet]
        [Route("CalculateDynamicPageSize/{categoryId}")]
        public async Task<IHttpActionResult> CalculateDynamicPageSize(int categoryId)
        {
            double totalNumberOfAds = 0;
            double totalNumberOfBannersBetween = 1;
            var summary = await IAdvertisementRepository.SelectAdvertisementSummaryCount(categoryId);
            totalNumberOfAds = summary.OverallCount;
            totalNumberOfBannersBetween = summary.BetweenSectionsOverAllCount;


            var pageSize = totalNumberOfBannersBetween != 0 ?
                Math.Ceiling(totalNumberOfAds / totalNumberOfBannersBetween) : totalNumberOfAds;

            return Ok(pageSize);
        }

        [HttpPost]
        [Route("GetMainBannerAdvertisements")]
        public async Task<IHttpActionResult> GetMainBannerAdvertisements(AdvertisementParam advertisementParam)
        {
            
            var result = await IAdvertisementRepository.SelectMainBannerAdvertisement();
            return Ok(result);
        }


        [Route("{id}/{userId?}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAdvertisement(int id, string userId = "")
        {
            List<AdvertisementDto> result = new List<AdvertisementDto>();
            if (string.IsNullOrEmpty(userId))
            {
                result = await IAdvertisementRepository.SelectPaged(new AdvertisementParam()
                {
                    AdvertisementId = id,
                    PageNumber = 1,
                    PageSize = 1
                });
            }
            else
            {
                result = await IAdvertisementRepository.SelectPaged(new AdvertisementParam()
                {
                    AdvertisementId = id,
                    PageNumber = 1,
                    PageSize = 1,
                    UserId = userId
                });
            }


            if (result != null && result.Count > 0)
            {
                var advertisement = result.FirstOrDefault();
                var imagesList = !string.IsNullOrEmpty(advertisement.Images) ?
                    advertisement.Images.Split(',').ToList().Select(x => x.Trim())
                    .Select(x =>
                    new
                    {
                        id = x.Split(':')[1],
                        imageUrl = x.Split(':')[0]
                    }) : null;

                return Ok(new
                {
                    advertisement.AdvertisementId,
                    advertisement.AdvertisementImageUrl,
                    advertisement.AdvertisementName,
                    advertisement.CategoryId,
                    advertisement.CityName,
                    advertisement.CreatedById,
                    advertisement.CreatedByUserName,
                    advertisement.Description,
                    advertisement.Email,
                    advertisement.FaceBook,
                    advertisement.Features,
                    Images = imagesList,
                    advertisement.Instagram,
                    advertisement.IsApproved,
                    advertisement.Latitude,
                    advertisement.Longitude,
                    advertisement.Mobile,
                    advertisement.OverallCount,
                    advertisement.PackageId,
                    advertisement.PackageName,
                    advertisement.Rating,
                    advertisement.Snapchat,
                    advertisement.Twitter,
                    advertisement.WebSite,
                    advertisement.WorkingHours,
                    advertisement.StartDate,
                    advertisement.EndDate,
                    advertisement.Views,
                    advertisement.CategoryName

                });
            }
            else
            {
                return Ok();
            }
        }

        [Route("Add")]
        [ResponseType(typeof(Advertisement))]
        [Authorize]
        public async Task<IHttpActionResult> PostAdvertisement(Saned.Jazan.Api.Models.AdvertisementViewModel advertisementViewModel)
        {
            var advertisement = AutoMapper.Mapper.Map<Saned.Jazan.Api.Models.AdvertisementViewModel, Advertisement>(advertisementViewModel);

            string userName = User.Identity.GetUserName();
            Data.Core.Models.ApplicationUser u = await GetApplicationUser(userName);


            var package = IPackageRepository.SelectPackageById(advertisement.PackageId);


            advertisement.StartDate = DateTime.Now;
            advertisement.EndDate = DateTime.Now.AddMonths(package.Period);

            advertisement.CreatedOn = DateTime.Now;
            advertisement.CreatedBy = u.Id;
            advertisement.ImageUrl = advertisementViewModel.MainImageBase64.SaveImage("Advertisement", advertisementViewModel.MainImageExtension);
            advertisement.IsApproved = false;
            advertisement.IsActive = true;


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            advertisement = await IAdvertisementRepository.CreateAsync(advertisement);

            var packagesFeatures = await IPackageRepository.SelectById(advertisement.PackageId);

            foreach (var item in packagesFeatures)
            {
                var advertisementFeature = new AdvertisementFeature()
                {
                    AdvertisementId = advertisement.Id,
                    FeatureId = item.FeatureId,
                    Period = item.PackageFeaturePeriod,
                    Quantity = item.PackageFeatureQuantity,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMonths(package.Period)
                };

                await IAdvertisementFeatureRepository.CreateAsync(advertisementFeature);
            }



            if (advertisementViewModel.AdvertisementImages != null)
            {
                foreach (var item in advertisementViewModel.AdvertisementImages)
                {
                    var advertisementImage = new AdvertisementImage()
                    {
                        AdvertisementId = advertisement.Id,
                        ImageUrl = item.Base64.SaveImage("Advertisement", item.ImageExtension)
                    };

                    await IAdvertisementImageRepository.CreateAsync(advertisementImage);
                }
            }

            return Ok(advertisement);
        }

        [HttpDelete]
        [Route("DeleteAdvertisement/{id}")]
        [Authorize]
        public async Task<IHttpActionResult> DeleteAdvertisement(int id)
        {
            await IAdvertisementImageRepository.DeleteAsync(x => x.AdvertisementId == id);
            await IAdvertisementFeatureRepository.DeleteAsync(x => x.AdvertisementId == id);
            await ICulturalCompetitionQuestionSponsorsRepository.DeleteAsync(x => x.AdvertisementId == id);
            var result = await IAdvertisementRepository.DeleteAsync(x => x.Id == id);
            IAdvertisementRepository.DeleteAdvertisementCommentsRatingViewsAndNotifications(id);
            return Ok(result);
        }


        [HttpDelete]
        [Route("DeleteImageAdvertisement/{id}")]
        [Authorize]
        public async Task<IHttpActionResult> DeleteImageAdvertisement(int id)
        {
            var result = await IAdvertisementImageRepository.DeleteAsync(x => x.Id == id);
            return Ok(result);
        }

        [Route("Edit")]
        [ResponseType(typeof(Advertisement))]
        [Authorize]
        public async Task<IHttpActionResult> EditAdvertisement(Saned.Jazan.Api.Models.AdvertisementViewModel advertisementViewModel)
        {
            // user cannot update category , package and features 

            var advertisement = IAdvertisementRepository.Find(x => x.Id == advertisementViewModel.Id);

            if (advertisement != null)
            {
                string userName = User.Identity.GetUserName();
                Data.Core.Models.ApplicationUser u = await GetApplicationUser(userName);

                advertisement.UpdatedOn = DateTime.Now;
                advertisement.UpdatedBy = u.Id;

                if (!string.IsNullOrEmpty(advertisementViewModel.MainImageBase64) && !string.IsNullOrEmpty(advertisementViewModel.MainImageExtension))
                {
                    advertisement.ImageUrl = advertisementViewModel.MainImageBase64.SaveImage("Advertisement", advertisementViewModel.MainImageExtension);
                }

                advertisement.Name = advertisementViewModel.Name;
                advertisement.CityName = advertisementViewModel.CityName;
                advertisement.Longitude = advertisementViewModel.Longitude;
                advertisement.Latitude = advertisementViewModel.Latitude;
                advertisement.WorkingHours = advertisementViewModel.WorkingHours;
                advertisement.Description = advertisementViewModel.Description;

                advertisement.Mobile = advertisementViewModel.Mobile;
                advertisement.Email = advertisementViewModel.Email;
                advertisement.WebSite = advertisementViewModel.WebSite;
                advertisement.FaceBook = advertisementViewModel.FaceBook;
                advertisement.Instagram = advertisementViewModel.Instagram;
                advertisement.Snapchat = advertisementViewModel.Snapchat;
                advertisement.Twitter = advertisementViewModel.Twitter;

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await IAdvertisementRepository.UpdateAsync(advertisement);

                if (advertisementViewModel.AdvertisementImages != null)
                {
                    foreach (var item in advertisementViewModel.AdvertisementImages)
                    {
                        var advertisementImage = new AdvertisementImage()
                        {
                            AdvertisementId = advertisement.Id,
                            ImageUrl = item.Base64.SaveImage("Advertisement", item.ImageExtension)
                        };

                        await IAdvertisementImageRepository.CreateAsync(advertisementImage);
                    }
                }

                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }


        [HttpGet]
        [Route("SelectMaximumImagesCount/{advertisementId}")]
        public IHttpActionResult SelectMaximumImagesCount(int advertisementId)
        {
            var limitedImagesCount = IAdvertisementFeatureRepository.All().FirstOrDefault(x => x.AdvertisementId == advertisementId &&
             x.FeatureId == 15);

            if (limitedImagesCount != null)
            {
                var allreadyAddedImagesCount = IAdvertisementImageRepository.All().Where(x => x.AdvertisementId == advertisementId).Count();
                return Ok(limitedImagesCount.Quantity.HasValue ? limitedImagesCount.Quantity.Value - allreadyAddedImagesCount : 0);
            }
            else
            {
                return Ok(new { });
            }
        }


    }
}