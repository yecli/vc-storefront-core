using VirtoCommerce.Storefront.Model.Common.Events;

namespace VirtoCommerce.Storefront.Model.Wholesaler.Events
{
    public class ConfirmDeliveryAgreementEvent : DomainEvent
    {
        public ConfirmDeliveryAgreementEvent(WorkContext workContext, string deliveryAgreementId)
        {
            WorkContext = workContext;
            DeliveryAgreementId = deliveryAgreementId;
        }
        public WorkContext WorkContext { get; set; }
        public string DeliveryAgreementId { get; set; }
        
    }
}
