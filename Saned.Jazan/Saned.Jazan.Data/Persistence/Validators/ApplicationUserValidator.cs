using Microsoft.AspNet.Identity;
using Saned.Jazan.Data.Core.Models;
using Saned.Jazan.Data.Persistence.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Saned.Jazan.Data.Persistence.Validators
{
    public class ApplicationUserValidator<TUser> : UserValidator<TUser>
        where TUser : ApplicationUser
    {
        public bool PhoneIsRequire { get; set; }
        private ApplicationUserManager<TUser> Manager { get; }
        public ApplicationUserValidator(ApplicationUserManager<TUser> manager) : base(manager)
        {
            Manager = manager;
        }

        public override async Task<IdentityResult> ValidateAsync(TUser item)
        {
            IdentityResult baseResult = await base.ValidateAsync(item);
            List<string> errors = new List<string>(baseResult.Errors);

            if (Manager != null)
            {
                var otherAccount = await Manager.FindByPhoneNumberUserManagerAsync(item.PhoneNumber);
                if (otherAccount != null && otherAccount.Id != item.Id)
                {
                    string errorMsg = "Phone Number '" + item.PhoneNumber + "' is already taken.";
                    errors.Add(errorMsg);
                }
                //await this.ValidateUserName(item, errors);

            }
            return errors.Any()
                ? IdentityResult.Failed(errors.ToArray())
                : IdentityResult.Success;
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
    }
}
