using System;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Customer;

namespace VirtoCommerce.Storefront.Model.Wholesaler
{
    public class DeliveryAgreementRequest : ValueObject
    {
        public Wholesaler Wholesaler { get; set; }
        public DateTime SentDate { get; set; }
        public DateTime? ConfirmedDate { get; set; }
        public DateTime? RejectedDate { get; set; }
        public DeliveryAgreementStatus Status { get; set; }
    }
}
