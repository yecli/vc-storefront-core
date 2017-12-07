using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Order;
using VirtoCommerce.Storefront.Model.Quote;
using VirtoCommerce.Storefront.Model.Subscriptions;

namespace VirtoCommerce.Storefront.Model.Customer
{
    /// <summary>
    /// Represent customer information structure 
    /// </summary>
    public partial class Contact : Member
    {
        public string FullName { get; set; }
        /// <summary>
        /// Returns the first name of the customer.
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Returns the last name of the customer.
        /// </summary>
        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public string License
        {
            get
            {
                return DynamicProperties.GetDynamicPropertyValue("License", null);
            }
            set
            {
                DynamicProperties.SetDynamicPropertySingleValue("License", "ShortText", Language.InvariantLanguage, value);
            }
        }

        public string TaxId
        {
            get
            {
                return DynamicProperties.GetDynamicPropertyValue("TaxId", null);
            }
            set
            {
                DynamicProperties.SetDynamicPropertySingleValue("TaxId", "ShortText", Language.InvariantLanguage, value);
            }
        }
        public string Phone { get; set; }
        public string Outlet
        {
            get
            {
                return DynamicProperties.GetDynamicPropertyValue("Outlet", null);
            }
            set
            {
                DynamicProperties.SetDynamicPropertySingleValue("Outlet", "ShortText", Language.InvariantLanguage, value);
            }
        }

        public string TimeZone { get; set; }
        public string DefaultLanguage { get; set; }

        public Address DefaultBillingAddress { get; set; }
        public Address DefaultShippingAddress { get; set; }


        /// <summary>
        /// Returns true if the customer accepts marketing, returns false if the customer does not.
        /// </summary>
        public bool AcceptsMarketing { get; set; }

        /// <summary>
        /// Returns the default customer_address.
        /// </summary>
        public Address DefaultAddress { get; set; }

        public IList<string> Organizations { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public IMutablePagedList<CustomerOrder> Orders { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public IMutablePagedList<QuoteRequest> QuoteRequests { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public IMutablePagedList<Subscription> Subscriptions { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public System.Collections.Generic.IList<Wholesaler.Wholesaler> Wholesalers { get; set; }

    }
}
