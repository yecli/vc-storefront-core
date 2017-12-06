using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Common.Events;
using VirtoCommerce.Storefront.Model.Customer.Services;
using VirtoCommerce.Storefront.Model.Wholesaler.Events;

namespace VirtoCommerce.Storefront.Domain.Wholesaler.Handlers
{
    public class DeliveryAgreementEventsHandler : IEventHandler<ConfirmDeliveryAgreementEvent>, IEventHandler<SendDeliveryAgreementEvent>
    {
        private readonly IMemberService _memberService;
        public DeliveryAgreementEventsHandler(IMemberService memberService)
        {
            _memberService = memberService;
        }

        public Task Handle(ConfirmDeliveryAgreementEvent message)
        {
            //Nothing todo:
            return Task.CompletedTask;
        }

        public Task Handle(SendDeliveryAgreementEvent message)
        {
            //TODO: Send email notification
            return Task.CompletedTask;
        }
    }
}
