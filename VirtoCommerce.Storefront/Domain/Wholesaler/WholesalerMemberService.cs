using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using PagedList.Core;
using VirtoCommerce.Storefront.AutoRestClients.CustomerModuleApi;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Customer;
using VirtoCommerce.Storefront.Model.Wholesaler;
using VirtoCommerce.Storefront.Model.Order.Services;
using VirtoCommerce.Storefront.Model.Quote.Services;
using VirtoCommerce.Storefront.Model.Subscriptions.Services;

namespace VirtoCommerce.Storefront.Domain.Wholesaler
{
    public class WholesalerMemberService : MemberService
    {
        private static readonly ConcurrentDictionary<VirtoCommerce.Storefront.Model.Customer.Contact, List<DeliveryAgreementRequest>> _agreementRequests = new ConcurrentDictionary<VirtoCommerce.Storefront.Model.Customer.Contact, List<DeliveryAgreementRequest>>();

        private readonly ICustomerModule _customerApi;
        private readonly IWorkContextAccessor _workContextAccessor;
        public WholesalerMemberService(IWorkContextAccessor workContextAccessor, ICustomerModule customerApi, ICustomerOrderService orderService,
            IQuoteService quoteService, ISubscriptionService subscriptionService, IMemoryCache memoryCache)
            : base(customerApi, orderService, quoteService, subscriptionService, memoryCache)
        {
            _customerApi = customerApi;
            _workContextAccessor = workContextAccessor;
        }
    
        public override async Task<Contact> GetContactByIdAsync(string contactId)
        {
            var workContext = _workContextAccessor.WorkContext;

            var result = await base.GetContactByIdAsync(contactId);
            Func<int, int, IEnumerable<SortInfo>, IPagedList<Model.Wholesaler.Wholesaler>> getter = (pageNumber, pageSize, sortInfos) =>
            {
                var wholesalers = new List<Model.Wholesaler.Wholesaler>();
                var allStores = _workContextAccessor.WorkContext.AllStores;
                var organizations = _customerApi.ListOrganizations();
                foreach (var store in allStores)
                {
                    var organization = organizations.FirstOrDefault(x => x.Name.EqualsInvariant(store.Id));
                    var wholesaler = new Model.Wholesaler.Wholesaler
                    {
                        Id = store.Id,
                        Email = organization?.Emails?.FirstOrDefault() ?? store.Email,
                        Phone = organization?.Phones?.FirstOrDefault(),
                        Description = organization?.Description,
                        Name = organization.Name,
                        Address = store.PrimaryFullfilmentCenter?.Address,
                    };
                    if (!_agreementRequests.TryGetValue(workContext.CurrentUser.Contact.Value, out var aggrements))
                    {
                        _agreementRequests[workContext.CurrentUser.Contact.Value] = aggrements = new List<DeliveryAgreementRequest>();
                    }
                    var existAgreement = aggrements.FirstOrDefault(x => x.Wholesaler == wholesaler);
                    if(existAgreement == null)
                    {
                        existAgreement = new DeliveryAgreementRequest
                        {
                            Wholesaler = wholesaler,
                            Status = DeliveryAgreementStatus.NotSent
                        };
                        aggrements.Add(existAgreement);
                    }
                    wholesaler.AgreementRequest = existAgreement;
                    wholesalers.Add(wholesaler);
                }
                return new StaticPagedList<Model.Wholesaler.Wholesaler>(wholesalers, pageNumber, pageSize, wholesalers.Count());
            };
            result.Wholesalers = new MutablePagedList<Model.Wholesaler.Wholesaler>(getter, 1,  20);
            return result;

        }

    }
}
