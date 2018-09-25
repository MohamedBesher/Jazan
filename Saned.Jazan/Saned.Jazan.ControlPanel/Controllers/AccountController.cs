using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Saned.Jazan.ControlPanel.Extensions;
using Saned.Jazan.ControlPanel.ViewModels;
using Saned.Jazan.Data.Core.Models;
using Saned.Jazan.Data.Persistence.Infrastructure;
using Saned.Jazan.Data.Persistence.Repositories;

namespace Saned.Jazan.ControlPanel.Controllers
{
    [System.Web.Mvc.Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManagerImpl _userManager;
        private readonly AuthRepository _repo = null;

        public AccountController()
        {
            _repo = new AuthRepository();
        }

        public AccountController(ApplicationUserManagerImpl userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _repo = new AuthRepository();
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManagerImpl UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManagerImpl>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);


            var result = Task.Run(
                       () =>
                          UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword)).Result;
            if (result.Succeeded)
            {
                var user = Task.Run(
                       () =>
                         UserManager.FindByIdAsync(User.Identity.GetUserId())).Result;
              
                if (user != null)
                {
                    Task.Run(
                      () => SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false));
                }
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                bool incorrectPassword = result.Errors.Contains("Incorrect password.");
                if (incorrectPassword)
                {
                    IdentityResult x = new IdentityResult("كلمة السر غير صحيحة");
                    AddErrors(x);
                    return View(model).Error("كلمة السر غير صحيحة");
                }
                else
                {
                    AddErrors(result);
                    return View(model).Error(result.Errors.ToString());
                }
            }


        }
        //
        // GET: /Account/Login
        [System.Web.Mvc.AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        //
        // POST: /Account/Login

        [System.Web.Mvc.AllowAnonymous]
        [System.Web.Mvc.HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
           
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                default:
                    ModelState.AddModelError("", "فشل تسجيل الدخول: اسم المستخدم غير معروف أو كلمة المرور غير صالحة");
                    return View(model);
            }
            //var user = await _repo.FindUser(model.UserName, model.Password);
            //if (user == null)
            //{
            //    ModelState.AddModelError("", "فشل تسجيل الدخول: اسم المستخدم غير معروف أو كلمة المرور غير صالحة");
            //    return View(model);
            //}
            //else
            //{

            //    return RedirectToLocal(returnUrl);
            //}
        }

        //
        // GET: /Account/VerifyCode

        [System.Web.Mvc.AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

      


        //[ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }


        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }

    public class ErrorController : Controller
    {
        public ViewResult Index()
        {
            return View("Error");
        }
        public ViewResult NotFound()
        {
            Response.StatusCode = 404;  //you may want to set this to 200
            return View("NotFound");
        }
    }
}