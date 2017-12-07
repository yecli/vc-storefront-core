using System.Linq;

namespace VirtoCommerce.Storefront.Domain
{
    public static class WholesalerWorkContextBuilderExtensions
    {
        public static void WithWholesaler(this IWorkContextBuilder builder)
        {
            var workContext = builder.WorkContext;
            if (workContext != null && workContext.CurrentUser != null && workContext.CurrentUser.IsRegisteredUser && workContext.CurrentUser.Contact.Value != null)
            {
                workContext.CurrentWholesaler = workContext.CurrentUser.Contact.Value.Wholesalers.FirstOrDefault(x => x.IsActive);
            }
        }
    }
}
