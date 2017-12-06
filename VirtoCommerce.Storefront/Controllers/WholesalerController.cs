using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Common.Events;
using VirtoCommerce.Storefront.Model.Wholesaler.Events;

namespace VirtoCommerce.Storefront.Controllers
{
    public class WholesalerController : StorefrontControllerBase
    {
        private readonly IEventPublisher _publisher;
        public WholesalerController(IWorkContextAccessor workContextAccesor, IStorefrontUrlBuilder urlBuilder, IEventPublisher publisher)
              : base(workContextAccesor, urlBuilder)
        {
            _publisher = publisher;
        }

        // GET: /wholesalers/landing
        [HttpGet]
        public ActionResult Landing()
        {
            return View("wholesaler/landing");
        }

        // GET: /wholesalers/agreements/{agreementId}/confirm
        [HttpGet]
        public ActionResult ConfirmDeliveryAggrement(string agreementId)
        {
            _publisher.Publish(new ConfirmDeliveryAgreementEvent(WorkContext, agreementId));
            return View("wholesaler/agreementConfirmed");
        }  

    }
}
