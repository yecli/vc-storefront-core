using VirtoCommerce.LiquidThemeEngine.Objects;
using storefrontModel = VirtoCommerce.Storefront.Model.CustomerReviews;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class CustomerReviewConverter
    {
        public static CustomerReview ToShopifyModel(this storefrontModel.CustomerReview customerReview)
        {
            var result = new CustomerReview
            {
                ProductId = customerReview.ProductId,
                AuthorNickname = customerReview.AuthorNickname,
                Content = customerReview.Content,
                CreatedDate = customerReview.CreatedDate,
                IsActive = customerReview.IsActive,
                Rating = customerReview.Rating,
            };
            return result;
        }
    }
}
