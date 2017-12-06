using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Common.Events;
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

        // POST: storefrontapi/wholesaler/{wholesalerId}/agreement/confirm
        [HttpPost]
        public ActionResult ConfirmDeliveryAggrement(string wholesalerId)
        {
            var wholesaler = WorkContext.CurrentUser?.Contact?.Value?.Wholesalers?.FirstOrDefault(x => x.Id == wholesalerId);
            if (wholesaler != null)
            {
                wholesaler.AgreementRequest.ConfirmedDate = DateTime.UtcNow;
                wholesaler.AgreementRequest.Status = Model.Wholesaler.DeliveryAgreementStatus.Confirmed;
                _publisher.Publish(new ConfirmDeliveryAgreementEvent(WorkContext, WorkContext.CurrentUser.Contact.Value, wholesaler.AgreementRequest));
            }
            return NoContent();
        }

        // POST: storefrontapi/wholesaler/{wholesalerId}/agreement/send
        [HttpPost]
        public ActionResult SendDeliveryAggrement(string wholesalerId)
        {
            var wholesaler = WorkContext.CurrentUser?.Contact?.Value?.Wholesalers?.FirstOrDefault(x => x.Id == wholesalerId);
            if (wholesaler != null)
            {
                wholesaler.AgreementRequest.SentDate = DateTime.UtcNow;
                wholesaler.AgreementRequest.Status = Model.Wholesaler.DeliveryAgreementStatus.Sent;
                _publisher.Publish(new SendDeliveryAgreementEvent(WorkContext, WorkContext.CurrentUser.Contact.Value, wholesaler.AgreementRequest));
            }
            return NoContent();
        }

        // GET: storefrontapi/wholesalers
        [HttpGet]
        public ActionResult GetWholesalers()
        {
           var result =  WorkContext.CurrentUser?.Contact?.Value?.Wholesalers;
           return Json(result);
            
        }
    }
}
