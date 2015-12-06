using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ShoppingCartSample.Authentication.Models;
using ShoppingCartSample.Authentication.Services;

namespace ShoppingCartSample.Controllers
{    
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;

        //Entry point for SPA.
        public HomeController(IUserService userService, IAuthenticationService authenticationService)
        {
            _userService = userService;
            _authenticationService = authenticationService;
        }

        public async Task<ActionResult> Index()
        {
            if (!Request.IsAuthenticated)
            {
                var temporaryEmail = "temporary-user@" + Guid.NewGuid() + ".com";
                var user = new ApplicationUser
                {
                    UserName = temporaryEmail,
                    Email = temporaryEmail,
                    IsTemporary = true
                };

                var result = await _userService.CreateTemporaryAsync(user);

                if (result.Succeeded)
                {
                    await _authenticationService.SignInAsync(user, false, false);
                    ViewData["IsTemporaryUser"] = true;
                }
                else
                {
                    //handle cart storage. use session?
                }
            }
            else
            {
                string userId = _userService.GetUserId();
                ViewData["IsTemporaryUser"] = _userService.IsTemporary(userId);
            }

            //loads the partial            
            return View();
        }
    }
}
