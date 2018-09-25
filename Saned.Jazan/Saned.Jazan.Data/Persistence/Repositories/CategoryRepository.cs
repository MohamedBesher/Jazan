using Saned.Jazan.Data.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saned.Jazan.Data.Core.Dtos;
using System.Data.SqlClient;
using System.Data;
using System.Data.Entity;
using Saned.Jazan.Data.Core.Models;

namespace Saned.Jazan.Data.Persistence.Repositories
{

    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository, IDisposable
    {

        public CategoryRepository(ApplicationDbContext _context) : base(_context)
        {
        }

        public async Task<CategoryDtos> Single(int CategoryId, int LanguageId = 0)
        {
            try
            {
                var config = "Categories_Select_Single ".GenerateParameterArray("CategoryId".KVP(CategoryId), "LanguageId".KVP(LanguageId));
                return await _context.Database.SqlQuery<CategoryDtos>(config.Item2, config.Item1).SingleAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Category> GetAll()
        {
            return All().Where(x => x.ParentId == 0).ToList();
        }

        public List<CategoryView> GetAllCategory(int? parentId)
        {
            var parentIdParamter = (parentId == null) ? new SqlParameter("@ParentId", DBNull.Value) : new SqlParameter("@ParentId", parentId);
            return _context.Database.SqlQuery<CategoryView>("Categories_SelectParentAndChild @ParentId", parentIdParamter).ToList();
        }
        public IQueryable<CategoryView> GetSubCategories(int? parentId)
        {


            var subcategories = from m in _context.Categories
                join category in _context.Categories on m.ParentId equals category.CategoryId
                where m.ParentId != 0 && (parentId == 0 || m.ParentId == parentId)
                select new CategoryView
                {
                    CategoryNameAr = m.CategoryNameAr,
                    CategoryImage = m.CategoryImage,
                    CategoryId = m.CategoryId,
                    ParentId = m.ParentId,
                    ParentName = category.CategoryNameAr
                };

            return subcategories.AsNoTracking().OrderByDescending(u => u.CategoryNameAr);     
        }

        public IQueryable<CategoryView> GetMainCategories()
        {
            return
                _context.Categories.Where(u => u.ParentId == 0)
                    .AsNoTracking()
                    .Select(u => new CategoryView()
                    {
                        CategoryNameAr = u.CategoryNameAr,
                        CategoryImage = u.CategoryImage,
                        CategoryId = u.CategoryId

                    }).OrderByDescending(u=>u.CategoryNameAr);
        }




        public List<Category> GetAll(int catId)
        {
            return All().Where(x => x.ParentId == catId).ToList();
        }



        public List<Category> GetAllSub()
        {
            return All().Where(x => x.ParentId != 0).ToList();
        }
        public async Task<int> AddCategory(AddCategoryDto category)
        {
            try
            {
                if (IsCategoryExist(category.Name).Result)
                {
                    throw new Exception("This Name is Exist Try another Name");
                }
                string ImageName = category.CategoryImageBase64.SaveImage("Category", category.ImageExtention);
                string namearVal = category.Name;
                string nameEnVal = null;
                if (category.LanguageId == 1)
                {
                    nameEnVal = category.Name;
                    namearVal = null;
                }
                var config = "Category_Insert ".GenerateParameterArray("CategoryNameAr".KVP(namearVal), "CategoryNameEn".KVP(nameEnVal), "CategoryImage".KVP(ImageName), "parentId".KVP(category.ParentId), "Status".KVP(0), "CreateDate".KVP(DateTime.Now));
                return await _context.Database.ExecuteSqlCommandAsync(config.Item2, config.Item1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<List<CategoryDtos>> GetCategoryChields(int ParentId, CategoriesRequest request)
        {
            var config = "Category_Chields_Select_Paged ".GenerateParameterArray(
                "NameFilter".KVP(request.NameFilter),
                 "PageSize".KVP(request.PageSize),
                 "PageNumber".KVP(request.PageNumber),
                 "parentId".KVP(ParentId),
                 "LanguageId".KVP(request.LanguageId)
                 );
            return await _context.Database.SqlQuery<CategoryDtos>(config.Item2, config.Item1).ToListAsync();
        }

        public async Task<List<CategoryDtos>> GetParentCategories(CategoriesRequest request)
        {
            try
            {
                var config = "Categories_Select_Paged ".GenerateParameterArray("NameFilter".KVP(request.NameFilter),
                    "PageSize".KVP(request.PageSize), "PageNumber".KVP(request.PageNumber), "LanguageId".KVP(request.LanguageId));
                return await _context.Database.SqlQuery<CategoryDtos>(config.Item2, config.Item1).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> IsCategoryExist(string Name, int LanguageId = 0, int OwnerId = 0)
        {
            try
            {
                var config = "Category_Name_Exist ".GenerateParameterArray("CategoryName".KVP(Name), "CategoryId".KVP(OwnerId), "LanguageId".KVP(LanguageId));
                var res = await _context.Database.SqlQuery<int>(config.Item2, config.Item1).SingleAsync();
                return res > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int SaveCategory(Category category)
        {
            _context.Categories.Add(category);
            return _context.SaveChanges();
        }

        public Category GetCategoryById(int id)
        {
            var firstOrDefault = _context.Categories.FirstOrDefault(z => z.CategoryId == id);
            return firstOrDefault;
        }

        public async Task<int> RemoveCategory(int CategoryId)
        {
            //Category_Delete
            try
            {
                var config = "Category_Delete ".GenerateParameterArray("CategoryId".KVP(CategoryId));
                return await _context.Database.ExecuteSqlCommandAsync(config.Item2, config.Item1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int UpdateCategory(Category category)
        {
            return _context.SaveChanges();
        }

        public int DeleteCategory(int id)
        {
            var news = _context.Categories.Find(id);
            if (news != null) _context.Categories.Remove(news);
            return _context.SaveChanges();
        }

        public bool IsRelatedWith(int id)
        {
            var bo = _context.
               Categories.
               Include(p => p.Advertisements).
               SingleOrDefault(p => p.CategoryId == id);
            bool adex = bo?.Advertisements != null && bo.Advertisements.Count > 0;
            if (adex)
            {
                return true;
            }
            var cat = _context.Categories.Where(p => p.ParentId == id).ToList().Count > 0;
            if (cat)
                return true;


            return false;
        }

        public async Task<int> UpdateCategory(UpdateCategoryDto category)
        {
            try
            {
                if (IsCategoryExist(category.Name, category.CategoryId).Result)
                {
                    throw new Exception("This Name is Exist Try another Name");
                }
                var res = await Single(category.CategoryId);
                string ImageName = res.ImageUrl;
                if (!string.IsNullOrEmpty(category.NewCategoryImageBase64))
                {
                    ImageName = category.NewCategoryImageBase64.SaveImage("Category", category.ImageExt);
                }
                var config = "Category_Update ".GenerateParameterArray("CategoryId".KVP(category.CategoryId),
                    "CategoryName".KVP(category.Name), "CategoryImage".KVP(ImageName), "Status".KVP(0));
                return await _context.Database.ExecuteSqlCommandAsync(config.Item2, config.Item1);
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
