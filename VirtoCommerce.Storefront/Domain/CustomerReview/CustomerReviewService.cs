using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using PagedList.Core;
using VirtoCommerce.Storefront.AutoRestClients.TestCustomModule.WebModuleApi;
using VirtoCommerce.Storefront.Extensions;
using VirtoCommerce.Storefront.Infrastructure;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Caching;
using VirtoCommerce.Storefront.Model.Common.Caching;
using VirtoCommerce.Storefront.Model.CustomerReviews;

namespace VirtoCommerce.Storefront.Domain.CustomerReview
{
    public class CustomerReviewService : ICustomerReviewService
    {
        private readonly ICustomerReviews _customerReviewsApi;
        private readonly IApiChangesWatcher _apiChangesWatcher;
        private readonly IStorefrontMemoryCache _memoryCache;
        private readonly IWorkContextAccessor _workContextAccessor;

        public CustomerReviewService(ICustomerReviews customerReviewsApi, IApiChangesWatcher apiChangesWatcher, IStorefrontMemoryCache memoryCache, IWorkContextAccessor workContextAccessor)
        {
            _customerReviewsApi = customerReviewsApi;
            _apiChangesWatcher = apiChangesWatcher;
            _memoryCache = memoryCache;
            _workContextAccessor = workContextAccessor;
        }

        public IPagedList<Model.CustomerReviews.CustomerReview> GetCustomerReviews(CustomerReviewSearchCriteria criteria)
        {
            return GetCustomerReviewsAsync(criteria).GetAwaiter().GetResult();
        }

        public async Task<IPagedList<Model.CustomerReviews.CustomerReview>> GetCustomerReviewsAsync(CustomerReviewSearchCriteria criteria)
        {
            var workContext = _workContextAccessor.WorkContext;
            var cacheKey = CacheKey.With(GetType(), nameof(GetCustomerReviewsAsync), criteria.GetCacheKey(), workContext.CurrentLanguage.CultureName);
            return await _memoryCache.GetOrCreateExclusiveAsync(cacheKey, async (cacheEntry) => {
                cacheEntry.AddExpirationToken(CustomerReviewCacheRegion.CreateChangeToken());
                cacheEntry.AddExpirationToken(_apiChangesWatcher.CreateChangeToken());

                var result = await _customerReviewsApi.SearchCustomerReviewsAsync(criteria.ToSearchCriteriaDto());
                return new StaticPagedList<Model.CustomerReviews.CustomerReview>(result.Results.Select(x => x.ToCustomerReview()), criteria.PageNumber, criteria.PageSize, result.TotalCount.Value);
            });
        }

        public ProductRating GetProductRating(string productId)
        {
            return GetProductRatingAsync(productId).GetAwaiter().GetResult();
        }

        public async Task<ProductRating> GetProductRatingAsync(string productId)
        {
            var workContext = _workContextAccessor.WorkContext;
            var cacheKey = CacheKey.With(GetType(), nameof(GetProductRatingAsync), productId, workContext.CurrentLanguage.CultureName);
            return await _memoryCache.GetOrCreateExclusiveAsync(cacheKey, async (cacheEntry) => {
                cacheEntry.AddExpirationToken(CustomerReviewCacheRegion.CreateChangeToken());
                cacheEntry.AddExpirationToken(_apiChangesWatcher.CreateChangeToken());

                var result = await _customerReviewsApi.GetProductRatingAsync(productId);
                return new ProductRating() {
                    ProductId = productId,
                    Rating = (decimal?)result.RatingValue,
                };
            });
        }
    }
}
