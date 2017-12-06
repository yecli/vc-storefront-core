using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Stores;
using System.Linq;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Middleware
{
    public class ActiveWholesalerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWorkContextAccessor _workContextAccessor;
        public ActiveWholesalerMiddleware(RequestDelegate next, IWorkContextAccessor workContextAccessor)
        {
            _next = next;
            _workContextAccessor = workContextAccessor;
        }

        public async Task Invoke(HttpContext context)
        {
            var workContext = _workContextAccessor.WorkContext;
            if (workContext != null && workContext.CurrentUser != null && workContext.CurrentUser.IsRegisteredUser && workContext.CurrentUser.Contact.Value != null)
            {
                workContext.CurrentWholesaler =  workContext.CurrentUser.Contact.Value.Wholesalers.FirstOrDefault(x => x.IsActive);         
            }

            await _next(context);
        }
    }

}
