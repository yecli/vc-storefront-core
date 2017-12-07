using VirtoCommerce.Storefront.Model.Common.Events;

namespace VirtoCommerce.Storefront.Model.Security.Events
{
    public class UserRegisteredEvent : DomainEvent
    {
        public UserRegisteredEvent(WorkContext workContext, User user, Register register)
        {
            WorkContext = workContext;
            User = user;
            Registration = register;
        }

        public WorkContext WorkContext { get; set; }
        public User User { get; set; }
        public Register  Registration { get; set; }
    }
}
