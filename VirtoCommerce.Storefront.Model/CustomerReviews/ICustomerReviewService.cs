using System.Threading.Tasks;
using PagedList.Core;

namespace VirtoCommerce.Storefront.Model.CustomerReviews
{
    public interface ICustomerReviewService
    {
        IPagedList<CustomerReview> GetCustomerReviews(CustomerReviewSearchCriteria criteria);
        Task<IPagedList<CustomerReview>> GetCustomerReviewsAsync(CustomerReviewSearchCriteria criteria);

        ProductRating GetProductRating(string productId);
        Task<ProductRating> GetProductRatingAsync(string productId);
    }
}
