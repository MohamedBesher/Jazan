using Saned.Jazan.Data.Core.Dtos;
using Saned.Jazan.Data.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saned.Jazan.Data.Core.Repositories
{
    public interface INewsRepository : IBaseRepository<News> , IDisposable
    {
        Task<SingleNewsDto> GetSingleNews(int NewsId);
        List<ViewNewsDto> GetNews(NewsRequestDto status);
        IQueryable<News> GetAll();
        News GetNewsById(int newsId);
        int SaveNews(News news);
        int UpdateNews(News news);
        int DeleteNews(int id);
        Task<int> AddNews(InsertNewsDto news);
        Task<int> RemoveNews(int NewsId);
        Task<int> UpdateNews(UpdateNewsDto news);
        Task<int> SetImageAsDefault(int ImageId);
        //Task<int> AddImageToNews(string Url, bool isDefault, int NewsId);
        Task<List<Dtos.ViewNewsDto>> GetNewsList(NewsRequestDto status);
        Task<List<NewsImageDto>> GetNewsImages(int NewsId);
 
    }
}
