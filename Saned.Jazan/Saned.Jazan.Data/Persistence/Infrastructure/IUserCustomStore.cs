using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Saned.Jazan.Data.Core.Models;

namespace Saned.Jazan.Data.Persistence.Infrastructure {
    public interface IUserCustomStore<TUser> : IUserStore<TUser>, IDisposable where TUser : ApplicationUser, IUser<string> {
        Task<TUser> FindByPhoneNumberAsync( string phoneNumber );

    }
}