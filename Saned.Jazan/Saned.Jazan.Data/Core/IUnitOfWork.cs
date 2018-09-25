using Saned.Common.Comments.Repository;
using Saned.Common.Notification.Repository;
using Saned.Jazan.Data.Core.Repositories;
using Saned.Jazan.Data.Persistence;
using Saned.Jazan.Data.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saned.Jazan.Data.Core
{
    public interface IDALContext : IUnitOfWork
    {
        IPackageRepository IPackageRepository { get; }
        IFeatureRepository IFeatureRepository { get; }
        INewsRepository INewsRepository { get; }
        IUsersRepository IUsersRepository { get; }
        ICategoryRepository ICategoryRepository { get; }
        IEmailSettingRepository IEmailSettingRepository { get; }
        ICommentRepositoryAsync ICommentRepositoryAsync { get; }
        INotificationsRepositoryAsync INotificationsRepositoryAsync { get; }
    }

    public interface IUnitOfWork : IDisposable
    {
        void Commit();

        Task<int> CommitAsync();
    }

    public class DALContext : IDALContext
    {
        private ApplicationDbContext dbContext;
        private IPackageRepository _IPackageRepository;
        private IFeatureRepository _IFeatureRepository;
        private INewsRepository _INewsRepository;
        private IUsersRepository _IUsersRepository;
        private ICategoryRepository _ICategoryRepository;
        private IEmailSettingRepository _IEmailSettingRepository;
        private ICommentRepositoryAsync _ICommentRepositoryAsync;
        private INotificationsRepositoryAsync _INotificationsRepositoryAsync;

        public DALContext()
        {
            dbContext = new ApplicationDbContext();
        }

        public ICommentRepositoryAsync ICommentRepositoryAsync
        {
            get
            {
                if (_ICommentRepositoryAsync == null)
                {
                    _ICommentRepositoryAsync = new CommentRepositoryAsync(dbContext.Database.Connection.ConnectionString);
                }
                return _ICommentRepositoryAsync;
            }
        }

        public IPackageRepository IPackageRepository
        {
            get
            {
                if (_IPackageRepository == null)
                    _IPackageRepository = new PackageRepository(dbContext);
                return _IPackageRepository;
            }
        }

        public IFeatureRepository IFeatureRepository
        {
            get
            {
                if (_IFeatureRepository == null)
                    _IFeatureRepository = new FeatureRepository(dbContext);
                return _IFeatureRepository;
            }
        }

        public INewsRepository INewsRepository
        {
            get
            {
                if (_INewsRepository == null)
                    _INewsRepository = new NewsRepository(dbContext);
                return _INewsRepository;
            }
        }
        public IUsersRepository IUsersRepository
        {
            get
            {
                if (_IUsersRepository == null)
                    _IUsersRepository = new UsersRepository(dbContext);
                return _IUsersRepository;
            }
        }

        public ICategoryRepository ICategoryRepository
        {
            get
            {
                if (_ICategoryRepository == null)
                    _ICategoryRepository = new CategoryRepository(dbContext);
                return _ICategoryRepository;
            }


        }

        public IEmailSettingRepository IEmailSettingRepository
        {
            get
            {
                if (_IEmailSettingRepository == null)
                    _IEmailSettingRepository = new EmailSettingRepository();
                return _IEmailSettingRepository;
            }


        }

        public INotificationsRepositoryAsync INotificationsRepositoryAsync
        {
            get
            {
                if (_INotificationsRepositoryAsync == null)
                {
                    _INotificationsRepositoryAsync = new NotificationRepositoryAsync(dbContext.Database.Connection.ConnectionString);
                }
                return _INotificationsRepositoryAsync;
            }
        }

        public void Commit()
        {
            dbContext.SaveChanges();
        }

        public async Task<int> CommitAsync()
        {
            return await dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            if (dbContext != null)
                dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
