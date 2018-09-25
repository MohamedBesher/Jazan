using Microsoft.AspNet.Identity;
using Saned.Jazan.Api.Models;
using Saned.Jazan.Data.Core.Dtos;
using Saned.Jazan.Data.Core.Models;
using Saned.Jazan.Data.Core.Repositories;
using Saned.Jazan.Data.Persistence.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Saned.Jazan.Api.Controllers
{
    [RoutePrefix("api/TouristVisit")]
    public class TouristVisitController : BasicController
    {
        private ITouristVisitRepository ITouristVisitRepository;
        private ITouristVisitImageRepository ITouristVisitImageRepository;
        private IAdvertisementRepository IAdvertisementRepository;

        public TouristVisitController
            (
            ITouristVisitRepository ITouristVisitRepository,
            ITouristVisitImageRepository ITouristVisitImageRepository,
            IAdvertisementRepository IAdvertisementRepository
            )
        {
            this.ITouristVisitRepository = ITouristVisitRepository;
            this.ITouristVisitImageRepository = ITouristVisitImageRepository;
            this.IAdvertisementRepository = IAdvertisementRepository;
        }

        [HttpPost]
        [Route("GetPaged")]
        public async Task<IHttpActionResult> GetPaged(TouristVisitGetPagedParam TouristVisitGetPagedParam)
        {
            var result = await ITouristVisitRepository.SelectPaged(TouristVisitGetPagedParam.PageSize, TouristVisitGetPagedParam.PageNumber, TouristVisitGetPagedParam.UserId, TouristVisitGetPagedParam.IsApproved);
            var firstBanner = await IAdvertisementRepository.SelectPaged(new AdvertisementParam()
            {
                FeatureId = 2,
                PageNumber = TouristVisitGetPagedParam.PageNumber,
                PageSize = 1
            });

            return Ok(new
            {
                touristVisits = result,
                banner = firstBanner

            });
        }

        [Route("CalculateDynamicPageSize")]
        public async Task<IHttpActionResult> CalculateDynamicPageSize()
        {
            double totalNumberOfAds = 0;
            double totalNumberOfBannersBetween = 1;
            var summary = await ITouristVisitRepository.SelectTouristVisitSummaryCount();
            totalNumberOfAds = summary.OverallCount;
            totalNumberOfBannersBetween = summary.BetweenSectionsOverAllCount;

            var pageSize = totalNumberOfBannersBetween != 0 ?
                Math.Ceiling(totalNumberOfAds / totalNumberOfBannersBetween) : totalNumberOfAds;

            return Ok(pageSize);
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<IHttpActionResult> GetById(int id)
        {
            var touristVisit = await ITouristVisitRepository.SelectById(id);


            if (touristVisit != null && touristVisit.FirstOrDefault() != null)
            {
                var result = touristVisit.FirstOrDefault();

                return Ok(new
                {
                    result.Id,
                    result.CreatedBy,
                    result.CreatedOn,
                    result.Description,
                    ImagesUrl = touristVisit.Where(x => x.MediaType == MediaType.Image).Select(x => new { x.MediaUrl, x.TouristVisitImageId }),
                    result.Latitude,
                    result.Longitude,
                    result.Name,
                    result.UserName,
                    result.CityName,
                    result.Rating,
                    result.VisitDate,
                    result.ImageUrl,
                    result.IsApproved,
                    YouTubeUrls = touristVisit.Where(x => x.MediaType == MediaType.YouTube).Select(x => new { x.MediaUrl, x.TouristVisitImageId })
                });
            }
            else
            {
                return NotFound();
            }

        }

        [HttpPost]
        [Authorize]
        [Route("Add")]
        public async Task<IHttpActionResult> Add(TouristVisitViewModel touristVisitViewModel)
        {
            var touristVisit = AutoMapper.Mapper.Map<TouristVisitViewModel, TouristVisit>(touristVisitViewModel);

            string userName = User.Identity.GetUserName();
            Data.Core.Models.ApplicationUser u = await GetApplicationUser(userName);

            touristVisit.CreatedOn = DateTime.Now;
            touristVisit.CreatedBy = u.Id;
            touristVisit.ImageUrl = touristVisitViewModel.MainImageBase64.SaveImage("TouristVisit", touristVisitViewModel.MainImageExtension);


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            touristVisit = await ITouristVisitRepository.CreateAsync(touristVisit);

            if (touristVisitViewModel.Images != null)
            {
                foreach (var item in touristVisitViewModel.Images)
                {
                    var touristVisitImage = new TouristVisitImage()
                    {
                        TouristVisitId = touristVisit.Id,
                        ImageUrl = item.Base64.SaveImage("Advertisement", item.ImageExtension),
                        MediaType = MediaType.Image
                    };

                    await ITouristVisitImageRepository.CreateAsync(touristVisitImage);
                }
            }

            if (touristVisitViewModel.YouTubeUrls != null)
            {
                foreach (var item in touristVisitViewModel.YouTubeUrls)
                {
                    var touristVisitImage = new TouristVisitImage()
                    {
                        TouristVisitId = touristVisit.Id,
                        ImageUrl = item,
                        MediaType = MediaType.YouTube
                    };

                    await ITouristVisitImageRepository.CreateAsync(touristVisitImage);
                }
            }


            return Ok(touristVisit.Id);

        }

        [HttpPost]
        [Authorize]
        [Route("Edit")]
        public async Task<IHttpActionResult> Edit(TouristVisitViewModel touristVisitViewModel)
        {
            var touristVisit = ITouristVisitRepository.Find(x => x.Id == touristVisitViewModel.Id);

            if (touristVisit != null)
            {


                string userName = User.Identity.GetUserName();
                Data.Core.Models.ApplicationUser u = await GetApplicationUser(userName);

                touristVisit.UpdatedOn = DateTime.Now;
                touristVisit.UpdatedBy = u.Id;

                if (!string.IsNullOrEmpty(touristVisitViewModel.MainImageBase64)
                    && !string.IsNullOrEmpty(touristVisitViewModel.MainImageExtension))
                {
                    touristVisit.ImageUrl = touristVisitViewModel.MainImageBase64.SaveImage("TouristVisit", touristVisitViewModel.MainImageExtension);
                }

                touristVisit.CityName = touristVisitViewModel.CityName;
                touristVisit.Description = touristVisitViewModel.Description;
                touristVisit.Latitude = touristVisitViewModel.Latitude;
                touristVisit.Longitude = touristVisitViewModel.Longitude;
                touristVisit.Name = touristVisitViewModel.Name;
                touristVisit.VisitDate = touristVisitViewModel.VisitDate;

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await ITouristVisitRepository.UpdateAsync(touristVisit);

                if (touristVisitViewModel.Images != null)
                {
                    foreach (var item in touristVisitViewModel.Images)
                    {
                        var touristVisitImage = new TouristVisitImage()
                        {
                            TouristVisitId = touristVisit.Id,
                            ImageUrl = item.Base64.SaveImage("Advertisement", item.ImageExtension),
                            MediaType = MediaType.Image
                        };

                        await ITouristVisitImageRepository.CreateAsync(touristVisitImage);
                    }
                }

                if (touristVisitViewModel.YouTubeUrls != null)
                {
                    foreach (var item in touristVisitViewModel.YouTubeUrls)
                    {
                        var touristVisitImage = new TouristVisitImage()
                        {
                            TouristVisitId = touristVisit.Id,
                            ImageUrl = item,
                            MediaType = MediaType.YouTube
                        };

                        await ITouristVisitImageRepository.CreateAsync(touristVisitImage);
                    }
                }

                return Ok(result);
            }
            else
            {
                return NotFound();
            }

        }

        [HttpDelete]
        [Authorize]
        [Route("DeleteTouristVisitImage/{id}")]
        public async Task<IHttpActionResult> DeleteTouristVisitImage(int id)
        {
            var result = await ITouristVisitImageRepository.DeleteAsync(x => x.Id == id);
            return Ok(result);
        }

        [HttpDelete]
        [Authorize]
        [Route("Delete/{id}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            await ITouristVisitImageRepository.DeleteAsync(x => x.TouristVisitId == id);
            var result = await ITouristVisitRepository.DeleteAsync(x => x.Id == id);
            return Ok(result);
        }
    }
}
