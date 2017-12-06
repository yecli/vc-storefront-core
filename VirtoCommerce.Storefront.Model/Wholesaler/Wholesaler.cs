using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.Wholesaler
{
    public class Wholesaler : Entity
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
        public Address Address { get; set; }
        public bool IsActive { get; set; }
        public DeliveryAgreementRequest AgreementRequest { get; set; }
    }
}
