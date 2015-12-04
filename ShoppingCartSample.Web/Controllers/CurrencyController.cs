using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ShoppingCartSample.Authentication.Services;
using ShoppingCartSample.Domain.Exceptions;
using ShoppingCartSample.Logic.Services;

namespace ShoppingCartSample.Controllers
{
    [Authorize]
    [RoutePrefix("currencies")]
    public class CurrencyController : Controller
    {        
        private readonly ICurrencyService _currencyService;
        private readonly IUserService _userService;

        public CurrencyController(ICurrencyService currencyService, IUserService userService)
        {
            _currencyService = currencyService;
            _userService = userService;
        }

        [HttpGet]
        [Route("available")]
        public ActionResult GetAvailableCurrencies()
        {
            var currencies = _currencyService.GetAvailableCurrencies();
            return Json(currencies.OrderBy(x => x.Symbol), JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        [Route("change")]
        public ActionResult ChangeCurrency(string oldCurrencyCode, string newCurrencyCode)
        {           
            try
            {
                var newSymbol = _currencyService.GetCurrencySymbol(newCurrencyCode);
                var newPriceModifier = _currencyService.GetNewPriceModifier(oldCurrencyCode, newCurrencyCode);

                //TODO: update user's default currency?                
                return Json(new {Symbol = newSymbol, PriceModifier = newPriceModifier}, JsonRequestBehavior.AllowGet);
            }
            catch (CurrencyNotFoundException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid currency.");
            }
            catch (CurrencyConversionException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error converting currencies.");
            }
        }       
    }
}