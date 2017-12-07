using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using VirtoCommerce.Storefront.AutoRestClients.CustomerModuleApi;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Common.Events;
using VirtoCommerce.Storefront.Model.Customer;
using VirtoCommerce.Storefront.Model.Order.Services;
using VirtoCommerce.Storefront.Model.Quote.Services;
using VirtoCommerce.Storefront.Model.Subscriptions.Services;
using VirtoCommerce.Storefront.Model.Wholesaler;
using VirtoCommerce.Storefront.Model.Wholesaler.Events;

namespace VirtoCommerce.Storefront.Domain.Wholesaler
{
    public class WholesalerMemberService : MemberService, IEventHandler<ConfirmDeliveryAgreementEvent>, IEventHandler<SendDeliveryAgreementEvent>
    {
        private static readonly ConcurrentDictionary<string, List<Model.Wholesaler.Wholesaler>> _customerWholesalers = new ConcurrentDictionary<string, List<Model.Wholesaler.Wholesaler>>();

        private readonly ICustomerModule _customerApi;
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly IStorefrontUrlBuilder _urlBuilder;
        public WholesalerMemberService(IWorkContextAccessor workContextAccessor, ICustomerModule customerApi, ICustomerOrderService orderService,
            IQuoteService quoteService, ISubscriptionService subscriptionService, IMemoryCache memoryCache, IStorefrontUrlBuilder urlBuilder)
            : base(customerApi, orderService, quoteService, subscriptionService, memoryCache)
        {
            _customerApi = customerApi;
            _workContextAccessor = workContextAccessor;
            _urlBuilder = urlBuilder;
        }

        public override async Task<Contact> GetContactByIdAsync(string contactId)
        {
            var workContext = _workContextAccessor.WorkContext;

            var result = await base.GetContactByIdAsync(contactId);

            if (!_customerWholesalers.TryGetValue(result.Id, out var customerWholesalers))
            {
                _customerWholesalers[result.Id] = customerWholesalers = new List<Model.Wholesaler.Wholesaler>();

                var allStores = _workContextAccessor.WorkContext.AllStores;
                var organizations = _customerApi.ListOrganizations();
                foreach (var store in allStores)
                {
                    var organization = organizations.FirstOrDefault(x => x.Name.EqualsInvariant(store.Id));
                    if (organization != null)
                    {
                        var wholesaler = customerWholesalers.FirstOrDefault(x => x.Id == store.Id);
                        if (wholesaler == null)
                        {
                            wholesaler = new Model.Wholesaler.Wholesaler
                            {
                                Id = store.Id,
                                Logo = organization.DynamicProperties.Select(x => x.ToDynamicProperty()).GetDynamicPropertyValue("logo"),
                                Email = organization.Emails?.FirstOrDefault() ?? store.Email,
                                Phone = organization.Phones?.FirstOrDefault(),
                                Description = organization?.Description,
                                Name = organization.Name,
                                Address = store.PrimaryFullfilmentCenter?.Address,
                                Url = _urlBuilder.ToAppAbsolute("~/", store, store.DefaultLanguage)
                            };
                            var agreement = new DeliveryAgreementRequest
                            {
                                Id = $"{ wholesaler.Id }-{ contactId }-AGR",
                                Wholesaler = wholesaler,
                                Status = DeliveryAgreementStatus.NotSent
                            };
                            wholesaler.AgreementRequest = agreement;
                            customerWholesalers.Add(wholesaler);
                        }
                    }
                }
            }
            result.Wholesalers = customerWholesalers;
            return result;

        }

        public Task Handle(ConfirmDeliveryAgreementEvent message)
        {
            var agreement = _customerWholesalers.SelectMany(x => x.Value).Select(x => x.AgreementRequest).FirstOrDefault(x => x.Id == message.DeliveryAgreementId);
            if (agreement != null)
            {
                agreement.ConfirmedDate = DateTime.UtcNow;
                agreement.Status = DeliveryAgreementStatus.Confirmed;
            }
            return Task.CompletedTask;
        }

        public Task Handle(SendDeliveryAgreementEvent message)
        {
            var agreement = _customerWholesalers.SelectMany(x => x.Value).Select(x => x.AgreementRequest).FirstOrDefault(x => x == message.DeliveryAgreement);
            if (agreement != null)
            {
                agreement.SentDate = DateTime.UtcNow;
                agreement.Status = DeliveryAgreementStatus.Sent;
            }
            //TODO: Send email notification
            return Task.CompletedTask;
        }
    }
}
