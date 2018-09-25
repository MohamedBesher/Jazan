using Saned.Jazan.Data.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Saned.Jazan.Data.Core.Dtos;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Saned.Jazan.Data.Core.Repositories
{
    public interface IAuthRepository : IBaseRepository<ApplicationUser>
    {
        Task<ApplicationUser> FindUser(string email);
        Task<IdentityResult> RegisterUser(RegisterDataDto bo);
        Task<ApplicationUser> FindUser(string userName, string password);
        Task<int> Complete();
        Task<ApplicationUser> FindUserByUserId(string id);
        Task ReSendEmailConfirmation(ApplicationUser user);
        Task ForgetPassword(string email);
        Task<IdentityResult> ConfirmEmail(string userId, string code);
        Task<IdentityResult> ResetPassword(string id, string code, string password);
        Client FindClient(string clientId);
        Task<ApplicationUser> FindAsync(UserLoginInfo loginInfo);
        IList<string> GetRoles(string userId);
    }
}
