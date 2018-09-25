using Saned.Jazan.Data.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saned.Jazan.Data.Core.Repositories;
using Saned.Jazan.Data.Persistence.Infrastructure;
using Saned.Jazan.Data.Persistence.Repositories;

namespace Saned.Jazan.Data.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDALContext context;

        public UnitOfWork(IDALContext dal)
        {
            context = dal;
        }

        public void Commit()
        {
            context.Commit();
        }

        public Task<int> CommitAsync()
        {
           return context.CommitAsync();
        }

        public void Dispose()
        {
            if (context != null)
                context.Dispose();
        }
    }
}
