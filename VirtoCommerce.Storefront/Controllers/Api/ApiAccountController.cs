using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PagedList.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.AutoRestClients.PlatformModuleApi.Models;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Customer.Services;
using VirtoCommerce.Storefront.Model.Customer;
using VirtoCommerce.Storefront.Model.Quote;
using VirtoCommerce.Storefront.Model.Security;
using VirtoCommerce.Storefront.Domain.Security;
using VirtoCommerce.Storefront.Model.Common.Events;
using VirtoCommerce.Storefront.Model.Security.Events;

namespace VirtoCommerce.Storefront.Controllers.Api
{
    public class ApiAccountController : StorefrontControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IEventPublisher _publisher;
        private readonly IMemberService _memberService;
        public ApiAccountController(IWorkContextAccessor workContextAccessor, IStorefrontUrlBuilder urlBuilder, SignInManager<User> signInManager, IMemberService memberService, IEventPublisher publisher)
            : base(workContextAccessor, urlBuilder)
        {
            _signInManager = signInManager;
            _memberService = memberService;
            _publisher = publisher;
        }

        // POST: storefrontapi/account/register
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register([FromBody] Register formModel)
        {
            var user = formModel.ToUser();
            user.StoreId = WorkContext.CurrentStore.Id;

            var result = await _signInManager.UserManager.CreateAsync(user, formModel.Password);
            if (result.Succeeded == true)
            {
                user = await _signInManager.UserManager.FindByNameAsync(user.UserName);
                await _publisher.Publish(new UserRegisteredEvent(WorkContext, user, formModel));
                await _signInManager.SignInAsync(user, isPersistent: true);
                await _publisher.Publish(new UserLoginEvent(WorkContext, user));
                return Json(new { RedirectUrl = UrlBuilder.ToAppAbsolute("~/account#/wholesalers") });
            }

            return Json(new { Errors = result.Errors });
        }


        // GET: storefrontapi/account
        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetCurrentCustomer()
        {       
            return Json(WorkContext.CurrentUser);
        }

        // GET: storefrontapi/account/quotes
        [HttpGet]
        public ActionResult GetCustomerQuotes(int pageNumber, int pageSize, IEnumerable<SortInfo> sortInfos)
        {
            if (WorkContext.CurrentUser.IsRegisteredUser)
            {
                var entries = WorkContext.CurrentUser?.Contact?.Value?.QuoteRequests;
                if (entries != null)
                {
                    entries.Slice(pageNumber, pageSize, sortInfos);
                    var retVal = new StaticPagedList<QuoteRequest>(entries.Select(x => x), entries);

                    return Json(new
                    {
                        Results = retVal,
                        TotalCount = retVal.TotalItemCount
                    });
                }
            }
            return NoContent();
        }

        // POST: storefrontapi/account
        [HttpPost]
        public async Task<ActionResult> UpdateAccount([FromBody] ContactUpdateInfo updateInfo)
        {
            await _memberService.UpdateContactAsync(WorkContext.CurrentUser.ContactId, updateInfo);

            return Ok();
        }

        // POST: storefrontapi/account/password
        [HttpPost]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePassword formModel)
        {
            var changePassword = new ChangePasswordInfo
            {
                OldPassword = formModel.OldPassword,
                NewPassword = formModel.NewPassword,
            };

            var result = await _signInManager.UserManager.ChangePasswordAsync(WorkContext.CurrentUser, formModel.OldPassword, formModel.NewPassword);

            return Json(new { Succeeded = result.Succeeded, Errors = result.Errors.Select(x => x.Description) });
        }

        // POST: storefrontapi/account/addresses
        [HttpPost]
        public async Task<ActionResult> UpdateAddresses([FromBody] IList<Address> addresses)
        {
            await _memberService.UpdateContactAddressesAsync(WorkContext.CurrentUser.ContactId, addresses);

            return Ok();
        }
    }
}
