using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Saned.Jazan.Data.Persistence.Infrastructure
{
    public class UserValidatorLocalizedAr<TUser> : IIdentityValidator<TUser> where TUser : class, IUser
    {
        public bool AllowOnlyAlphanumericUserNames { get; set; }

        private UserManager<TUser> Manager { get; set; }

        public UserValidatorLocalizedAr(UserManager<TUser> manager)
        {
            if (manager == null)
                throw new ArgumentNullException("manager");
            this.AllowOnlyAlphanumericUserNames = true;
            this.Manager = manager;
        }

        private async Task ValidateUserName(TUser user, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(user.UserName))
                errors.Add("يجب تحديد اسم المستخدم.");
            else if (this.AllowOnlyAlphanumericUserNames && !Regex.IsMatch(user.UserName, "^[A-Za-z0-9]+$"))
            {
                errors.Add("أسماء المستخدمين يمكن أن تحتوي على أحرف وأرقام فقط.");
            }
            else
            {
                TUser owner = await this.Manager.FindByNameAsync(user.UserName);
                if (!(owner == null) && owner.Id != user.Id)
                    errors.Add("هناك بالفعل حساب البريد الإلكتروني ");
            }
        }

        public async Task<IdentityResult> ValidateAsync(TUser item)
        {
            if ((object)item == null)
                throw new ArgumentNullException("entity");
            List<string> errors = new List<string>();
            await this.ValidateUserName(item, errors);
            return errors.Count <= 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray());
        }
    }
}