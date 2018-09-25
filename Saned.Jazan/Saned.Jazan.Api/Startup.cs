using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using System.Web.Http;
using Saned.Jazan.Api;
using Saned.Jazan.Api.Providers;

// the “assembly” attribute which states which class to fire on start-up
[assembly: OwinStartup(typeof(Startup))]
namespace Saned.Jazan.Api
{
    public class Startup
    {
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }
        public static GoogleOAuth2AuthenticationOptions googleAuthOptions { get; private set; }
        public static FacebookAuthenticationOptions facebookAuthOptions { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"> this parameter will be supplied by the host at run-time. 
        /// This “app” parameter is an interface which will be used to compose the application for our Owin server.</param>
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            ConfigureOAuth(app);

            WebApiConfig.Register(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }
        public void ConfigureOAuth(IAppBuilder app)
        {


            //use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);
            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();

            OAuthAuthorizationServerOptions oAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(3650),
                Provider = new SimpleAuthorizationServerProvider(),
                RefreshTokenProvider = new SimpleRefreshTokenProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(OAuthBearerOptions);
            googleAuthOptions = new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "234204074170-kckf6bflfb8ldcpoabmsu54u2duu07c1.apps.googleusercontent.com",
                ClientSecret = "8ORKGtS5ssblxP2YRbR9YCAh",
                Provider = new GoogleAuthProvider()
            };
            app.UseGoogleAuthentication(googleAuthOptions);

            //Configure Facebook External Login
            facebookAuthOptions = new FacebookAuthenticationOptions()
            {
                AppId = "1820592408154775",
                AppSecret = "b7462964fd82166d280b87028caeabf9",
                Provider = new FacebookAuthProvider()
            };
            app.UseFacebookAuthentication(facebookAuthOptions);

        }
    }
}
