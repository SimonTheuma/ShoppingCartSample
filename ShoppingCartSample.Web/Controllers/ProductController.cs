using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ShoppingCartSample.Attributes;
using ShoppingCartSample.Domain.Exceptions;
using ShoppingCartSample.Domain.Models.UserActions;
using ShoppingCartSample.Logic.Services;

namespace ShoppingCartSample.Controllers
{    
    [RoutePrefix("products")]    
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IAuditService _auditService;

        public ProductController(IProductService productService, IAuditService auditService)
        {
            _productService = productService;
            _auditService = auditService;
        }
        
        [HttpGet]       
        [Route("")]
        //Get all products.
        public ActionResult GetAllProducts()
        {
            try
            {
                var products = _productService.GetAll();
                return Json(products, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error getting products: " + ex.Message);
            }            
        }

        [HttpGet]
        [Route("{productId}")]
        //Get a product by id
        public ActionResult GetProductById(int productId)
        {
            try
            {
                string userId = User.Identity.GetUserId();
                var product = _productService.GetById(productId);

                _auditService.LogUserAction(new UserSawProduct(userId, productId));

                return Json(product, JsonRequestBehavior.AllowGet);
            }
            catch (ProductNotFoundException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Product not found.");
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error getting product: " + ex.Message);
            }
        }
    }
}