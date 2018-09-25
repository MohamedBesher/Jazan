using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Saned.Jazan.Data.Core.Models;
using Saned.Jazan.Data.Persistence.Providers;

namespace Saned.Jazan.Data.Persistence.Infrastructure {
    public class ApplicationUserManagerImpl : ApplicationUserManager<ApplicationUser> {
        public ApplicationUserManagerImpl( ) : base( new ApplicationUserStoreImpl( ) ) {
            this.UserTokenProvider = new ApplicationTokenProvider<ApplicationUser>( );

        }


        public static ApplicationUserManagerImpl Create()
        {
            return new ApplicationUserManagerImpl();
        }
    }
}