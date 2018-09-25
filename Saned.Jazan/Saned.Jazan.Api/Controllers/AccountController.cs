using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Linq;
using Saned.Jazan.Api.Dtos;
using Saned.Jazan.Api.Models;
using Saned.Jazan.Api.Results;
using Saned.Jazan.Data.Core;
using Saned.Jazan.Data.Core.Dtos;
using Saned.Jazan.Data.Core.Models;
using Saned.Jazan.Data.Persistence;
using Saned.Jazan.Data.Persistence.Repositories;
using Saned.Jazan.Api.Controllers;
using Saned.Jazan.Api;

namespace Saned.Jazan.AuthenticationApi.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : BasicController
    {

        private readonly AuthRepository _repo = null;
        //readonly IUnitOfWorkAsync _unitOfWork;

        public AccountController()
        {
            _repo = new AuthRepository();
            //_unitOfWork = new UnitOfWorkAsync();
        }
        private IAuthenticationManager Authentication => Request.GetOwinContext().Authentication;

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(ClientViewModel userViewModel)
        {
            if (ModelState == null)
                return BadRequest(ModelState);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            // Check Email
            var userbyemail = await _repo.FindUser(userViewModel.Email);
            if (userbyemail != null)
            {
                ModelState.AddModelError("Email", "Email Must Be Unique");
                return BadRequest(ModelState);
            }

            var bo = AutoMapper.Mapper.Map<ClientViewModel, RegisterDataDto>(userViewModel);

            IdentityResult result = await _repo.RegisterUser(bo);

            if (result == IdentityResult.Success)
            {
                Data.Core.Models.ApplicationUser user = await _repo.FindUser(userViewModel.UserName, userViewModel.Password);
                //await _repo.Complete();
                return Ok(user.Id);
            }
            else
            {
                IHttpActionResult errorResult = GetErrorResult(result);
                return errorResult;
            }
        }



        [AllowAnonymous]
        [Route("ReSendConfirmationCode/{id}")]
        public async Task<IHttpActionResult> ReSendConfirmationCode(string id)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(id))
                    return BadRequest("User Id Not Send");

                var user = await _repo.FindUserByUserId(id);
                if (user == null)
                    return BadRequest("User Not Found");
                if (user.EmailConfirmed == true)
                    return Ok("Email Is Confirmed");
                await _repo.ReSendEmailConfirmation(user);
                return Ok("Email Is SendConfrim");

            }
            catch (Exception ex)
            {

                string msg = ex.GetaAllMessages();
                return BadRequest("ReSendConfirmationCode " + msg);
            }
        }


        public IHttpActionResult SignOut()
        {
            Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return Ok();
        }

        [AllowAnonymous]
        [Route("ConfirmEmail")]
        public async Task<IHttpActionResult> ConfirmEmail(ConfirmViewModel confirmViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            IdentityResult result = await _repo.ConfirmEmail(confirmViewModel.UserId, confirmViewModel.Code);

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }
            return Ok();
        }

        [AllowAnonymous]
        [Route("ResetPassword")]
        public async Task<IHttpActionResult> ResetPassword(ResetPasswordViewModel resetPasswordView)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _repo.FindUser(resetPasswordView.Email);
            if (user == null)
            {
                ModelState.AddModelError("Email", "Not Found");
                return BadRequest(ModelState);
            }



            IdentityResult result = await _repo.ResetPassword(user.Id, resetPasswordView.Code, resetPasswordView.Password);

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }
            return Ok();
        }

        [AllowAnonymous]
        [Route("ForgetPassword")]
        public async Task<IHttpActionResult> ForgetPassword(ForgotPasswordViewModel forgotPasswordViewModel)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _repo.FindUser(forgotPasswordViewModel.Email);
            if (user == null)
            {
                ModelState.AddModelError("Email", "Not Found");
                return BadRequest(ModelState);
            }
            await _repo.ForgetPassword(forgotPasswordViewModel.Email);
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repo.Dispose();
            }

            base.Dispose(disposing);
        }



        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var id = await GetUserByUserName();

            IdentityResult result = await _repo.ChangePassword(id, changePasswordViewModel.OldPassword, changePasswordViewModel.NewPassword);

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }
            return Ok();
        }
        private async Task<string> GetUserByUserName()
        {
            string userName = User.Identity.GetUserName();
            string userId = await _repo.FindUserIdByUserName(userName);
            return userId;
        }
        #region Soical

        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            string redirectUri = string.Empty;

            if (error != null)
            {
                return BadRequest(Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            var redirectUriValidationResult = ValidateClientAndRedirectUri(this.Request, ref redirectUri);

            if (!string.IsNullOrWhiteSpace(redirectUriValidationResult))
            {
                return BadRequest(redirectUriValidationResult);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            IdentityUser user = await _repo.FindAsync(new UserLoginInfo(externalLogin.LoginProvider, externalLogin.ProviderKey));

            bool hasRegistered = user != null;

            redirectUri = string.Format("{0}#external_access_token={1}&provider={2}&haslocalaccount={3}&external_user_name={4}",
                                            redirectUri,
                                            externalLogin.ExternalAccessToken,
                                            externalLogin.LoginProvider,
                                            hasRegistered.ToString(),
                                            externalLogin.UserName);

            return Redirect(redirectUri);

        }


        // POST api/Account/RegisterExternal
        [AllowAnonymous]
        [Route("RegisterExternalbck")]
        public async Task<IHttpActionResult> RegisterExternalbck(RegisterExternalBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var verifiedAccessToken = await VerifyExternalAccessToken(model.Provider, model.ExternalAccessToken);
            if (verifiedAccessToken == null)
            {
                return BadRequest("Invalid Provider or External Access Token");
            }

            Data.Core.Models.ApplicationUser user = await _repo.FindAsync(new UserLoginInfo(model.Provider, verifiedAccessToken.user_id));

            bool hasRegistered = user != null;

            if (hasRegistered)
            {
                return BadRequest("External user is already registered");
            }

            user = new Data.Core.Models.ApplicationUser() { UserName = model.UserName };

            IdentityResult result = await _repo.CreateAsync(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            var info = new ExternalLoginInfo()
            {
                DefaultUserName = model.UserName,
                Login = new UserLoginInfo(model.Provider, verifiedAccessToken.user_id)
            };

            result = await _repo.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            //generate access token response
            var accessTokenResponse = GenerateLocalAccessTokenResponse(model.UserName);

            return Ok(accessTokenResponse);
        }

        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Data.Core.Models.ApplicationUser user;
            //if (model.Provider != "Twitter")
            //{
            //    var verifiedAccessToken = await VerifyExternalAccessToken(model.Provider, model.ExternalAccessToken);

            //    if (verifiedAccessToken == null)
            //    {
            //        return BadRequest("Invalid Provider or External Access Token"+model.ExternalAccessToken +"provider"+ model.Provider );
            //    }
            //    model.UserId = verifiedAccessToken.user_id;
            //    user = await _repo.FindAsync(new UserLoginInfo(model.Provider, model.UserId));
            //}
            //else
            {
                user = await _repo.FindAsync(new UserLoginInfo(model.Provider, model.UserId));
            }




            bool hasRegistered = user != null;

            if (hasRegistered)
            {
                // return BadRequest("External user is already registered");
                if (user.IsApprove.HasValue && !user.IsApprove.Value)
                {
                    //ModelState.AddModelError("blocked_user", "3-Wating To Approve");

                    return BadRequest("تم ايقاف حسابك من فضلك اتصل بادارة التطبيق");
                }
                else
                {
                    var token = GenerateLocalAccessTokenResponseUpdate(user);
                    return Ok(token);
                }
               
            }

            if (string.IsNullOrWhiteSpace(model.Name))
                model.Name = model.Provider + "User";
            user = new Data.Core.Models.ApplicationUser()
            {
                UserName = model.UserName,
                Name = model.Name,


            };

            IdentityResult result = await _repo.CreateAsync(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            var info = new ExternalLoginInfo()
            {
                DefaultUserName = model.UserName,
                Login = new UserLoginInfo(model.Provider, model.UserId)
            };

            result = await _repo.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            //generate access token response
            user = await _repo.FindAsync(new UserLoginInfo(model.Provider, model.UserId));
            var accessTokenResponse = GenerateLocalAccessTokenResponseUpdate(user);

            return Ok(accessTokenResponse);
        }

        [AllowAnonymous]
        [Route("RegisterExternal2")]
        public async Task<IHttpActionResult> RegisterExternal2(RegisterExternalBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var verifiedAccessToken = await VerifyExternalAccessToken(model.Provider, model.ExternalAccessToken);
            if (verifiedAccessToken == null)
            {
                return BadRequest("Invalid Provider or External Access Token");
            }

            Data.Core.Models.ApplicationUser user = await _repo.FindAsync(new UserLoginInfo(model.Provider, verifiedAccessToken.user_id));

            bool hasRegistered = user != null;

            if (hasRegistered)
            {
                // return BadRequest("External user is already registered");
                var token = GenerateLocalAccessTokenResponseUpdate(user);
                return Ok(token);
            }

            user = new Data.Core.Models.ApplicationUser() { UserName = model.UserName, Name = model.Provider + "User" };

            IdentityResult result = await _repo.CreateAsync(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            var info = new ExternalLoginInfo()
            {
                DefaultUserName = model.UserName,
                Login = new UserLoginInfo(model.Provider, verifiedAccessToken.user_id)
            };

            result = await _repo.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            //generate access token response
            user = await _repo.FindAsync(new UserLoginInfo(model.Provider, verifiedAccessToken.user_id));
            var accessTokenResponse = GenerateLocalAccessTokenResponseUpdate(user);

            return Ok(accessTokenResponse);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ObtainLocalAccessToken")]
        public async Task<IHttpActionResult> ObtainLocalAccessToken(string provider, string externalAccessToken)
        {

            if (string.IsNullOrWhiteSpace(provider) || string.IsNullOrWhiteSpace(externalAccessToken))
            {
                return BadRequest("Provider or external access token is not sent");
            }

            var verifiedAccessToken = await VerifyExternalAccessToken(provider, externalAccessToken);
            if (verifiedAccessToken == null)
            {
                return BadRequest("Invalid Provider or External Access Token");
            }

            IdentityUser user = await _repo.FindAsync(new UserLoginInfo(provider, verifiedAccessToken.user_id));

            bool hasRegistered = user != null;

            if (!hasRegistered)
            {
                return BadRequest("External user is not registered");
            }

            //generate access token response
            var accessTokenResponse = GenerateLocalAccessTokenResponse(user.UserName);

            return Ok(accessTokenResponse);

        }

        #endregion

        #region Helpers



        private string ValidateClientAndRedirectUri(HttpRequestMessage request, ref string redirectUriOutput)
        {

            Uri redirectUri;

            var redirectUriString = GetQueryString(Request, "redirect_uri");

            if (string.IsNullOrWhiteSpace(redirectUriString))
            {
                return "redirect_uri is required";
            }

            bool validUri = Uri.TryCreate(redirectUriString, UriKind.Absolute, out redirectUri);

            if (!validUri)
            {
                return "redirect_uri is invalid";
            }

            var clientId = GetQueryString(Request, "client_id");

            if (string.IsNullOrWhiteSpace(clientId))
            {
                return "client_Id is required";
            }

            var client = _repo.FindClient(clientId);

            if (client == null)
            {
                return string.Format("Client_id '{0}' is not registered in the system.", clientId);
            }

            if (!string.Equals(client.AllowedOrigin, redirectUri.GetLeftPart(UriPartial.Authority), StringComparison.OrdinalIgnoreCase))
            {
                return string.Format("The given URL is not allowed by Client_id '{0}' configuration.", clientId);
            }

            redirectUriOutput = redirectUri.AbsoluteUri;

            return string.Empty;

        }

        private string GetQueryString(HttpRequestMessage request, string key)
        {
            var queryStrings = request.GetQueryNameValuePairs();

            if (queryStrings == null) return null;

            var match = queryStrings.FirstOrDefault(keyValue => string.Compare(keyValue.Key, key, true) == 0);

            if (string.IsNullOrEmpty(match.Value)) return null;

            return match.Value;
        }

        private async Task<ParsedExternalAccessToken> VerifyExternalAccessToken(string provider, string accessToken)
        {
            ParsedExternalAccessToken parsedToken = null;

            var verifyTokenEndPoint = "";

            if (provider == "Facebook")
            {
                //You can get it from here: https://developers.facebook.com/tools/accesstoken/
                //More about debug_tokn here: http://stackoverflow.com/questions/16641083/how-does-one-get-the-app-access-token-for-debug-token-inspection-on-facebook
                var appToken = "1820592408154775|xnwZKF0jSCgz3UCJ0-F7G65cdi4";
                verifyTokenEndPoint = string.Format("https://graph.facebook.com/debug_token?input_token={0}&access_token={1}", accessToken, appToken);
            }
            else if (provider == "Google")
            {
                verifyTokenEndPoint = string.Format("https://www.googleapis.com/oauth2/v1/tokeninfo?access_token={0}", accessToken);
            }
            else
            {
                return null;
            }

            var client = new HttpClient();
            var uri = new Uri(verifyTokenEndPoint);
            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                dynamic jObj = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content);

                parsedToken = new ParsedExternalAccessToken();

                if (provider == "Facebook")
                {
                    parsedToken.user_id = jObj["data"]["user_id"];
                    parsedToken.app_id = jObj["data"]["app_id"];

                    if (!string.Equals(Startup.facebookAuthOptions.AppId, parsedToken.app_id, StringComparison.OrdinalIgnoreCase))
                    {
                        return null;
                    }
                }
                else if (provider == "Google")
                {
                    parsedToken.user_id = jObj["user_id"];
                    parsedToken.app_id = jObj["audience"];

                    if (!string.Equals(Startup.googleAuthOptions.ClientId, parsedToken.app_id, StringComparison.OrdinalIgnoreCase))
                    {
                        return null;
                    }

                }

            }

            return parsedToken;
        }

        private JObject GenerateLocalAccessTokenResponse(string userName)
        {

            var tokenExpiration = TimeSpan.FromDays(1);

            ClaimsIdentity identity = new ClaimsIdentity(OAuthDefaults.AuthenticationType);

            identity.AddClaim(new Claim(ClaimTypes.Name, userName));
            identity.AddClaim(new Claim(ClaimTypes.Role, "User"));

            var props = new AuthenticationProperties()
            {
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.Add(tokenExpiration),
            };

            var ticket = new AuthenticationTicket(identity, props);

            var accessToken = Startup.OAuthBearerOptions.AccessTokenFormat.Protect(ticket);

            JObject tokenResponse = new JObject(
                new JProperty("userName", userName),
                new JProperty("access_token", accessToken),
                new JProperty("token_type", "bearer"),
                new JProperty("expires_in", tokenExpiration.TotalSeconds.ToString()),
                new JProperty(".issued", ticket.Properties.IssuedUtc.ToString()),
                new JProperty(".expires", ticket.Properties.ExpiresUtc.ToString())
                );

            return tokenResponse;
        }

        private JObject GenerateLocalAccessTokenResponseUpdate(Data.Core.Models.ApplicationUser user)
        {

            var tokenExpiration = TimeSpan.FromDays(1);

            ClaimsIdentity identity = new ClaimsIdentity(OAuthDefaults.AuthenticationType);

            identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            identity.AddClaim(new Claim("sub", user.UserName));
            using (AuthRepository repo = new AuthRepository())
            {
                var userRoles = repo.GetRoles(user.Id);
                foreach (var role in userRoles)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, role));
                }
            }
            var props = new AuthenticationProperties()
            {
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.Add(tokenExpiration),
            };

            var ticket = new AuthenticationTicket(identity, props);

            var accessToken = Startup.OAuthBearerOptions.AccessTokenFormat.Protect(ticket);

            JObject tokenResponse = new JObject(
                new JProperty("userName", user.UserName),
                new JProperty("access_token", accessToken),
                new JProperty("token_type", "bearer"),
                new JProperty("PhotoUrl", user.PhotoUrl ?? string.Empty),
                new JProperty("expires_in", tokenExpiration.TotalSeconds.ToString()),
                new JProperty(".issued", ticket.Properties.IssuedUtc.ToString()),
                new JProperty(".expires", ticket.Properties.ExpiresUtc.ToString())
                );

            return tokenResponse;
        }

        #endregion
    }
}
