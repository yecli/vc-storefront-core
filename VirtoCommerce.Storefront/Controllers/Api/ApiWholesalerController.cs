using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Common.Events;
using VirtoCommerce.Storefront.Model.Wholesaler;
using VirtoCommerce.Storefront.Model.Wholesaler.Events;

namespace VirtoCommerce.Storefront.Controllers.Api
{
    public class ApiWholesalerController : StorefrontControllerBase
    {
        private readonly IEventPublisher _publisher;
        public ApiWholesalerController(IWorkContextAccessor workContextAccessor, IStorefrontUrlBuilder urlBuilder, IEventPublisher publisher)
            : base(workContextAccessor, urlBuilder)
        {
            _publisher = publisher;
        }


        // POST: storefrontapi/wholesalers/{wholesalerId}/agreement/send
        [HttpPost]
        public ActionResult SendDeliveryAggrement([FromBody] DeliveryAgreementRequest agreement)
        {
            _publisher.Publish(new SendDeliveryAgreementEvent(WorkContext, agreement));            
            return NoContent();
        }
        // GET storefrontapi/wholesalers/{wholesalerId}/select
        [HttpGet]
        public ActionResult SelectWholesaler(string wholesalerId)
        {
            var wholesalers = WorkContext.CurrentUser?.Contact?.Value?.Wholesalers;
            foreach (var activeWholesaler in wholesalers.Where(x => x.IsActive))
            {
                activeWholesaler.IsActive = false;
            }
            var wholesaler = wholesalers.FirstOrDefault(x => x.Id == wholesalerId);
            if (wholesaler != null)
            {
                wholesaler.IsActive = true;
            }
            return Ok();
        }

        // GET: storefrontapi/wholesalers
        [HttpGet]
        public ActionResult GetWholesalers()
        {
           var result =  WorkContext.CurrentUser?.Contact?.Value?.Wholesalers.ToArray();
           return Json(result);
            
        }
    }
}
