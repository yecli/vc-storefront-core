using VirtoCommerce.Storefront.Model.Customer;

namespace VirtoCommerce.Storefront.Model.Security
{
    public partial class Register : Contact
    {        
        public string UserName { get; set; }
        public string Password { get; set; }

    }
}
