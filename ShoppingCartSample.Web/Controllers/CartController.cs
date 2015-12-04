using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ShoppingCartSample.Authentication.Services;
using ShoppingCartSample.Domain.Exceptions;
using ShoppingCartSample.Domain.Models;
using ShoppingCartSample.Logic.Services;
using ShoppingCartSample.ViewModels;

namespace ShoppingCartSample.Controllers
{
    [Authorize]
    [RoutePrefix("cart")]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IUserService _userService;

        public CartController(ICartService cartService, IUserService userService)
        {
            _cartService = cartService;
            _userService = userService;
        }
        
        [HttpGet]
        [Route("")]
        // GET: Cart
        public ActionResult GetCart()
        {
            string userId = User.Identity.GetUserId();
            //var user = _userService.FindById(userId);

            Cart cart;

            try
            {
                cart = _cartService.GetByUserId(userId);
            }
            catch (CartNotFoundException)
            {
                cart = _cartService.Create(userId);
            }

            return Json(cart, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPut]
        [Route("")]
        public ActionResult AddOrder(AddOrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userId = User.Identity.GetUserId();            

            try
            {
                var order = _cartService.AddOrder(userId, model.ProductID, model.Quantity);
                Response.StatusCode = (int) HttpStatusCode.Created;
                return Json(new {id = order.ID, subTotal = order.SubTotal}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete]
        [Route("")]
        public ActionResult RemoveOrder(RemoveOrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userId = User.Identity.GetUserId();

            try
            {
                _cartService.RemoveOrder(userId, model.OrderId);
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            catch (CartNotFoundException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "cart");
            }
            catch (OrderNotFoundException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "order");
            }      
        }

        [HttpPost]
        [Route("")]
        public ActionResult UpdateOrder(UpdateOrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userId = User.Identity.GetUserId();

            try
            {
                _cartService.UpdateOrder(userId, model.OrderId, model.NewQuantity);
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (CartNotFoundException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "cart");
            }
            catch (OrderNotFoundException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "order");
            }
            catch (InvalidStockUpdateException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "stock");
            }
        }

        [HttpPost]
        [Route("checkout")]
        public ActionResult ProcessCheckout(ProcessCheckoutViewModel model)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                _cartService.ProcessCheckout(userId);
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (CartNotFoundException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "cart");
            }            
        }

        [HttpPost]
        [Route("confirm")]
        public ActionResult ConfirmCheckout()
        {
            string userId = User.Identity.GetUserId();
            var user = _userService.FindById(userId);

            if (user.IsTemporary)
            {
                //redirect to registration page
                Response.StatusCode = (int)HttpStatusCode.PaymentRequired;
                return Json(new { id = userId }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                var cart = _cartService.GetByUserId(userId);
                return Json(cart, JsonRequestBehavior.AllowGet);
            }
            catch (CartNotFoundException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            catch (InvalidStockUpdateException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        [Route("transfer")]
        //This only happens in case the user already has a cart when signing in from a temporary user.
        //Otherwise this happens automatically when the user logs in. (Check AccountController)
        public ActionResult ConfirmCartTransfer(TransferCartViewModel model)
        {
            string temporaryUserId = model.TemporaryUserId;
            string currentUserId = User.Identity.GetUserId();

            try
            {
                _cartService.Transfer(temporaryUserId, currentUserId, overwriteCart: true);
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (UserNotFoundException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "user");
            }
            catch (CartNotFoundException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "cart");
            }
        }

        //Exception message being returned for debug purposes only. :)
        protected override void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Response.StatusDescription = "An internal error has occurred.";
                Response.Write(filterContext.Exception.Message);                
            }
        }
    }
}