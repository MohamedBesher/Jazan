using Saned.Jazan.Data.Core.Dtos;
using Saned.Jazan.Data.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saned.Jazan.Data.Core.Repositories
{
    public interface ICategoryRepository : IBaseRepository<Category>, IDisposable
    {
        Task<CategoryDtos> Single(int CategoryId , int LanguageId = 0);
        Task<List<CategoryDtos>> GetParentCategories(CategoriesRequest request);
        Task<List<CategoryDtos>> GetCategoryChields(int ParentId, CategoriesRequest request);
        List<Category> GetAll();
        List<CategoryView> GetAllCategory(int? parentId);
        Task<int> AddCategory(AddCategoryDto category);
        Task<int> UpdateCategory(UpdateCategoryDto category);
        Task<int> RemoveCategory(int CategoryId);
        Task<bool> IsCategoryExist(string Name, int LanguageId = 0, int OwnerId = 0);
        int SaveCategory(Category category);
        Category GetCategoryById(int id);
        int UpdateCategory(Category category);
        int DeleteCategory(int id);
        bool IsRelatedWith(int id);
        List<Category> GetAll(int catId);
        List<Category> GetAllSub();
        IQueryable<CategoryView> GetMainCategories();
        IQueryable<CategoryView> GetSubCategories(int? parentId);
    }
}
