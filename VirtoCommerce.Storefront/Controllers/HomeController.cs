using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.Storefront.Models;
using VirtoCommerce.Storefront.Infrastructure;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.Storefront.Controllers
{
    public class HomeController : StorefrontControllerBase
    {
        public HomeController(IWorkContextAccessor workContextAccessor, IStorefrontUrlBuilder urlBuilder)
            : base(workContextAccessor, urlBuilder)
        {     
        }

        public IActionResult Index()
        {
            if(!WorkContext.CurrentUser.IsRegisteredUser)
            {
                var defaultStore = WorkContext.AllStores.FirstOrDefault(x => x.Id == "Default");
                return base.Redirect(UrlBuilder.ToAppAbsolute("~/wholesalers/landing", defaultStore, defaultStore.DefaultLanguage));
            }
            else if (WorkContext.CurrentWholesaler == null)
            {
                return StoreFrontRedirect("~/account#/wholesalers");
            }
            else 
            {
                return View("index");
            }
        }

        [StorefrontRoute("about")]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        
    }
}
