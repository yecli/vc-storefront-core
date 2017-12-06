using VirtoCommerce.Storefront.Model.Common.Events;
using VirtoCommerce.Storefront.Model.Customer;

namespace VirtoCommerce.Storefront.Model.Wholesaler.Events
{
    public class SendDeliveryAgreementEvent : DomainEvent
    {
        public SendDeliveryAgreementEvent(WorkContext workContext, DeliveryAgreementRequest deliveryAgreement)
        {
            WorkContext = workContext;
            DeliveryAgreement = deliveryAgreement;
        }
        public WorkContext WorkContext { get; set; }
        public DeliveryAgreementRequest DeliveryAgreement { get; set; }
        
    }
}
