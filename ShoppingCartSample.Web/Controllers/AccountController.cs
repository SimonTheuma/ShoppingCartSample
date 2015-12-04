using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ShoppingCartSample.Authentication.Models;
using ShoppingCartSample.Authentication.Services;
using ShoppingCartSample.Domain.Exceptions;
using ShoppingCartSample.Logic.Services;
using ShoppingCartSample.Models;

namespace ShoppingCartSample.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly ICartService _cartService;

        public AccountController(IAuthenticationService authenticationService, IUserService userService, ICartService cartService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
            _cartService = cartService;
        }        

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]        
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return Json(model, JsonRequestBehavior.DenyGet);
            }

            //check if user is already "logged in" with a temporary user.
            var userId = User.Identity.GetUserId();
            var wasAnonymous = _userService.IsTemporary(userId);

            if (wasAnonymous)
            {
                _authenticationService.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await _authenticationService.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    {
                        //if user was previously anonymous, try transferring cart.
                        if (wasAnonymous)
                        {
                            try
                            {
                                if (!_cartService.HasEmptyCart(userId))
                                {
                                    var newUser = await _userService.FindByNameAsync(model.Email);
                                    var newUserId = newUser.Id;                                
                                    _cartService.Transfer(userId, newUserId);
                                }
                            }
                            catch (UserAlreadyHasCartException)
                            {
                                return new HttpStatusCodeResult(HttpStatusCode.Conflict);
                            }
                        }

                        return new HttpStatusCodeResult(HttpStatusCode.OK);
                    }                    
                case SignInStatus.LockedOut:
                    return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        
        //
        // POST: /Account/Register
        [HttpPost]     
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var oldUserId = User.Identity.GetUserId();
                var wasAnonymous = _userService.IsTemporary(oldUserId);

                if (wasAnonymous)
                {
                    _authenticationService.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                }

                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userService.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {                   
                    await _authenticationService.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    try
                    {
                        if (!_cartService.HasEmptyCart(oldUserId))
                        {                            
                            _cartService.Transfer(oldUserId, user.Id);
                        }
                    }                    
                    catch (Exception)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                    }

                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(model, JsonRequestBehavior.DenyGet);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var result = await _userService.ConfirmEmailAsync(userId, code);

            var resultCode = result.Succeeded ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            return new HttpStatusCodeResult(resultCode);            
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.FindByNameAsync(model.Email);
                if (user == null || !(await _userService.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(model, JsonRequestBehavior.DenyGet);
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return Json(model, JsonRequestBehavior.DenyGet);
            }
            var user = await _userService.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            var result = await _userService.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }

            AddErrors(result);
            Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        public async Task<ActionResult> LogOut()
        {
            _authenticationService.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            var temporaryEmail = "temporary-user@" + Guid.NewGuid() + ".com";
            var user = new ApplicationUser
            {
                UserName = temporaryEmail,
                Email = temporaryEmail,
                IsTemporary = true
            };

            //create a temporary user again
            var result = await _userService.CreateTemporaryAsync(user);

            if (result.Succeeded)
            {
                await _authenticationService.SignInAsync(user, false, false);
                ViewData["IsTemporaryUser"] = true;
            }            

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }    

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";       

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
}
