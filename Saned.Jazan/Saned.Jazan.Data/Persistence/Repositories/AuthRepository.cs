using Microsoft.AspNet.Identity;
using Saned.Jazan.Data.Core.Dtos;
using Saned.Jazan.Data.Core.Models;
using Saned.Jazan.Data.Core.Repositories;
using Saned.Jazan.Data.Persistence.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;


namespace Saned.Jazan.Data.Persistence.Repositories
{
    public enum EmailType
    {
        EmailConfirmation = 1,
        ForgetPassword = 2,
    }
    public class AuthRepository : IDisposable //: BaseRepository<ApplicationUser>, IAuthRepository
    {
        private readonly ApplicationUserManagerImpl _userManager;
        public ApplicationDbContext _context { get; set; }
        public AuthRepository()
        {
            _userManager = new ApplicationUserManagerImpl();
            _context = new ApplicationDbContext();
        }

        #region user

        public async Task<IdentityResult> RegisterUser(RegisterDataDto data)
        {
            var user = GetApplicationUser(data.Name, data.MobileNumber, data.Email, data.UserName, "user", true, true, "New");

            var result = await _userManager.CreateAsync(user, data.Password);
            if (result.Succeeded)
            {
                await AddRoleToUser(user.Id, "User");
                await SendEmailConfirmation(user);
            }
            return result;
        }

        private object GetApplicationUser(string name, string mobileNumber, string email, object userName, string v1, bool v2, bool v3, string v4)
        {
            throw new NotImplementedException();
        }

        public async Task ForgetPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            await SendPasswordResetToken(user);

        }
        public async Task<IdentityResult> ResetPassword(string userId = "", string code = "", string newPassword = "")
        {
            IdentityResult result = await _userManager.ResetPasswordAsync(userId, code, newPassword);
            return result;
        }
        public async Task<ApplicationUser> FindUser(string userName, string password)
        {
            ApplicationUser user = await _userManager.FindAsync(userName, password);
            return user;
        }

        public async Task<ApplicationUser> FindUserByUserName(string userName)
        {
            ApplicationUser user = await _userManager.Users.SingleOrDefaultAsync(x => x.UserName == userName);
            return user;
        }

        public async Task<bool> IsEmailConfirme(string userId)
        {
            return await _userManager.IsEmailConfirmedAsync(userId);
        }
        public async Task<bool> IsUserArchieve(string userId)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == userId);
            return user.IsDeleted != null && user.IsDeleted.Value;
        }

        public async Task<ApplicationUser> FindUser(string email)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            return user;
        }

        public ApplicationUser FindUserbyEmail(string email)
        {
            ApplicationUser user = _userManager.FindByEmail(email);
            return user;
        }

        public async Task<ApplicationUser> FindByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<IdentityResult> ConfirmEmail(string userId, string code)
        {
            IdentityResult result = await this._userManager.ConfirmEmailAsync(userId, code);
            return result;
        }

        public IList<string> GetRoles(string userId)
        {
            IList<string> lst = _userManager.GetRoles(userId);
            return lst;
        }

        private EmailSetting GetEmailMessage(string toName, string code, string messageTemplate)
        {
            EmailSetting emailSettings = GetEmailSetting(messageTemplate);
            emailSettings.MessageBodyAr = emailSettings.MessageBodyAr.Replace("@FullName", toName);
            emailSettings.MessageBodyAr = emailSettings.MessageBodyAr.Replace("@code", "Code=" + code);

            return emailSettings;
        }
        private ApplicationUser GetApplicationUser(string name, string phone, string email, string userName, string role = "user", bool isApprove = false, bool isSelfAdded = false, string status = "New")
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = userName,
                PhoneNumber = phone,
                Email = email,
                Name = name,
                IsApprove = isApprove,
                IsSelfAdded = isSelfAdded
            };
            return user;
        }

        private async Task AddRoleToUser(string userId, string role)
        {
            await _userManager.AddToRoleAsync(userId, role);
        }

        private async Task SendEmailConfirmation(ApplicationUser user)
        {

            var code = await GenerateToken(user.Id, EmailType.EmailConfirmation);
            await SendEmail(user, code, EmailType.EmailConfirmation.GetHashCode().ToString());
        }
        public async Task ReSendEmailConfirmation(ApplicationUser user)
        {

            var code = await GenerateToken(user.Id, EmailType.EmailConfirmation);
            await SendEmail(user, code, EmailType.EmailConfirmation.GetHashCode().ToString());
        }
        private async Task SendPasswordResetToken(ApplicationUser user)
        {
            var code = await GenerateToken(user.Id, EmailType.ForgetPassword);
            await SendEmail(user, code, EmailType.ForgetPassword.GetHashCode().ToString());
        }
        private async Task<string> GenerateToken(string userId, EmailType emailType)
        {
            switch (emailType)
            {
                case EmailType.EmailConfirmation:
                    return await _userManager.GenerateEmailConfirmationTokenAsync(userId);
                case EmailType.ForgetPassword:
                    return await _userManager.GeneratePasswordResetTokenAsync(userId);
                default:
                    return "";
            }

        }
        private async Task SendEmail(ApplicationUser user, string code, string messageTemplate)
        {
            EmailSetting emailMessage = GetEmailMessage(user.UserName, code, messageTemplate);
            await _userManager.SendEmailAsync(user.Id, messageTemplate, emailMessage.MessageBodyAr);
        }

        public async Task<IdentityResult> ChangePassword(string userId, string oldPassword, string newPassword)
        {
            IdentityResult result = await _userManager.ChangePasswordAsync(userId, oldPassword, newPassword);
            return result;
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        #endregion

        #region token



        public async Task<bool> AddRefreshToken(RefreshToken token)
        {

            var existingToken =
                _context.RefreshTokens
                    .SingleOrDefault(r => r.Subject == token.Subject && r.ClientId == token.ClientId);

            if (existingToken != null)
            {
                var result = await RemoveRefreshToken(existingToken);
            }

            _context.RefreshTokens.Add(token);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _context.RefreshTokens.FindAsync(refreshTokenId);

            if (refreshToken != null)
            {
                _context.RefreshTokens.Remove(refreshToken);
                return await _context.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Remove(refreshToken);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _context.RefreshTokens.FindAsync(refreshTokenId);

            return refreshToken;
        }

        public List<RefreshToken> GetAllRefreshTokens()
        {
            return _context.RefreshTokens.ToList();
        }

        #endregion

        #region Soical
        public async Task<ApplicationUser> FindAsync(UserLoginInfo loginInfo)
        {
            ApplicationUser user = await _userManager.FindAsync(loginInfo);

            return user;
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user)
        {
            user.IsApprove = true;
            user.SoicalMediaId = "1";
            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                await AddRoleToUser(user.Id, "User");
            }

            return result;
        }

        public async Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login)
        {
            var result = await _userManager.AddLoginAsync(userId, login);

            return result;
        }
        #endregion

        public EmailSetting GetEmailSetting(string type)
        {
            return _context.EmailSettings.SingleOrDefault(u => u.EmailSettingType == type.Trim());
        }

        public async Task<ApplicationUser> FindUserByUserId(string id)
        {
            ApplicationUser user = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == id);
            return user;
        }

        public async Task<IdentityResult> UpdateUser(ApplicationUser applicationUser)
        {
            return await _userManager.UpdateAsync(applicationUser);
        }

        public Client FindClient(string clientId)
        {
            var client = _context.Clients.Find(clientId);

            return client;
        }

        public async Task<string> FindUserIdByUserName(string userName)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            return user.Id;
        }

        public List<ApplicationUser> GetAll()
        {
            return _userManager.Users.Where(x => x.IsApprove.Value).ToList();
        }

        public void Dispose()
        {

        }



    }
}
