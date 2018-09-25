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
namespace Saned.Jazan.Data.Persistence.Repositories
{
    public class NewsRepository : BaseRepository<News>, INewsRepository, IDisposable
    {
        //private readonly ApplicationDbContext _context;
        public NewsRepository(ApplicationDbContext _context) : base(_context)
        {
            // _context = new ApplicationDbContext();
        }

        #region Helper
        KeyValuePair<string, object> KVP(string k, object v)
        {
            return new KeyValuePair<string, object>(k, v);
        }
        #endregion

        public List<ViewNewsDto> GetNews(NewsRequestDto model)
        {
            try
            {
                var res = (_context.Database.SqlQuery<ViewNewsDto>("EXEC News_Select_Paged  @PageNumber,@PageSize",
                 new SqlParameter("PageNumber", SqlDbType.Int) { Value = model.PageNumber },
                  new SqlParameter("PageSize", SqlDbType.Int) { Value = model.PageSize }
                ).ToList());
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public IQueryable<News> GetAll()
        {
            return All().Include(s => s.NewsImages).OrderBy(z => z.PublishingDate);
        }

        public News GetNewsById(int newsId)
        {
            var firstOrDefault = _context.NewsSet.Include(s => s.NewsImages).FirstOrDefault(z => z.Id == newsId);
            return firstOrDefault;
        }

        public int SaveNews(News news)
        {
            try
            {
                _context.NewsSet.Add(news);
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            //  return  Create(news);
        }

        public int UpdateNews(News news)
        {
            return _context.SaveChanges();
        }

        public int DeleteNews(int id)
        {
            var news = _context.NewsSet.Find(id);
            if (news != null) _context.NewsSet.Remove(news);
            return _context.SaveChanges();
        }

        public async Task<int> AddNews(InsertNewsDto news)
        {
            try
            {
                string concatenatedImages = "";
                int length = news.ImagesBase64s.Count;
                for (int i = 0; i < length; i++)
                {
                    concatenatedImages += $"{news.ImagesBase64s[i].SaveImage("News", news.ImageExtentions[i])}-{((i == news.DefaultImageIndex) ? '1' : '0')}";
                    concatenatedImages += (i == length - 1) ? "" : ",";
                }
                var config = "News_Insert ".GenerateParameterArray(KVP("Title", news.Title), KVP("Description", news.Description), KVP("concatenatedImages", concatenatedImages));
                return await _context.Database.ExecuteSqlCommandAsync(config.Item2, config.Item1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<NewsImageDto>> GetNewsImages(int NewsId)
        {
            try
            {
                var config = "News_Select_Image ".GenerateParameterArray(KVP("NewsId", NewsId));
                return await _context.Database.SqlQuery<NewsImageDto>(config.Item2, config.Item1).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<ViewNewsDto>> GetNewsList(NewsRequestDto status)
        {
            try
            {
                var config = "News_Select_Paged ".GenerateParameterArray(KVP("PageNumber", status.PageNumber),
                    KVP("PageSize", status.PageSize), KVP("DateFilter", status.DateFilter),
                    KVP("TitleFilter", status.TitleFilter), KVP("DetailFilter", status.DetailFilter));
                return await _context.Database.SqlQuery<ViewNewsDto>(config.Item2, config.Item1).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> RemoveNews(int NewsId)
        {
            //DeleteNews
            try
            {
                var config = "News_Delete ".GenerateParameterArray(KVP("NewsId", NewsId));
                return await _context.Database.ExecuteSqlCommandAsync(config.Item2, config.Item1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> SetImageAsDefault(int ImageId)
        {
            try
            {
                var config = "News_Image_SetAsDefault ".GenerateParameterArray(KVP("ImageID", ImageId));
                return await _context.Database.ExecuteSqlCommandAsync(config.Item2, config.Item1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> UpdateNews(UpdateNewsDto news)
        {
            try
            {
                string concatenatedImages = null;
                if (news.NewImagesBase64s != null)
                {
                    int length = news.NewImagesBase64s.Count;
                    for (int i = 0; i < length; i++)
                    {
                        concatenatedImages += $"{news.NewImagesBase64s[i].SaveImage("News", news.NewImageExtentions[i])}-{((i == news.DefaultImageIndex) ? '1' : '0')}";
                        concatenatedImages += (i == length - 1) ? "" : ",";
                    }
                }
                string tobeDeleted = null;
                if (news.ImagesToDelete != null)
                {
                    int length2 = news.ImagesToDelete.Count;
                    for (int i = 0; i < length2; i++)
                    {
                        tobeDeleted += $"{news.ImagesToDelete[i]}";
                        tobeDeleted += (i == length2 - 1) ? "" : ",";
                    }
                }

                var config = "News_Update ".GenerateParameterArray(KVP("NewsId", news.NewsId), KVP("Title", news.Title), KVP("Description", news.Description),
                    KVP("concatenatedNewImages", concatenatedImages), KVP("deletedImages", tobeDeleted));
                return await _context.Database.ExecuteSqlCommandAsync(config.Item2, config.Item1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<SingleNewsDto> GetSingleNews(int NewsId)
        {
            try
            {
                var config = "News_Select_Single ".GenerateParameterArray(KVP("NewsId", NewsId));
                var result = await _context.Database.SqlQuery<SingleNews>(config.Item2, config.Item1).FirstOrDefaultAsync();
                result.NewsId = NewsId;
                List<string> imageslst = new List<string>();
                int imageInd = 0;
                var images = await this.GetNewsImages(NewsId);
                for (int i = 0; i < images.Count; i++)
                {
                    if (images[i].IsDefault)
                    {
                        imageInd = i;
                    }
                    imageslst.Add(images[i].ImagePath);
                }

                return new SingleNewsDto
                {
                    Title = result.Title,
                    Details = result.Details,
                    NewsId = result.NewsId,
                    DefaultIndex = imageInd,
                    Images = imageslst,
                    PublishingDate = result.PublishingDate
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
