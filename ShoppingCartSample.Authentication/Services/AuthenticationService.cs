using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Host.SystemWeb;
using Microsoft.Owin.Security;
using ShoppingCartSample.Authentication.Models;

namespace ShoppingCartSample.Authentication.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().Authentication;
            }
        }


        public async Task<SignInStatus> PasswordSignInAsync(string email, string password, bool rememberMe, bool shouldLockout)
        {
            return await SignInManager.PasswordSignInAsync(email, password, rememberMe, shouldLockout);
        }

        public async Task SignInAsync(ApplicationUser user, bool isPersistent, bool rememberBrowser)
        {
            await SignInManager.SignInAsync(user, isPersistent, rememberBrowser);
        }

        public void SignIn(ClaimsIdentity identity)
        {
            AuthenticationManager.SignIn(identity);
        }

        public void SignOut(string defaultAuthenticationType)
        {
            AuthenticationManager.SignOut(defaultAuthenticationType);            
        }
    }
}
