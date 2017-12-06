using VirtoCommerce.Storefront.Model.Common.Events;
using VirtoCommerce.Storefront.Model.Customer;

namespace VirtoCommerce.Storefront.Model.Wholesaler.Events
{
    public class SendDeliveryAgreementEvent : DomainEvent
    {
        public SendDeliveryAgreementEvent(WorkContext workContext, Contact contact, DeliveryAgreementRequest deliveryAgreement)
        {
            WorkContext = workContext;
            Contact = contact;
            DeliveryAgreement = deliveryAgreement;
        }
        public WorkContext WorkContext { get; set; }
        public Contact Contact { get; set; }
        public DeliveryAgreementRequest DeliveryAgreement { get; set; }
        
    }
}
