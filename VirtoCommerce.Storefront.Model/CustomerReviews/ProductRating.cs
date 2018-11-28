using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.CustomerReviews
{
    public class ProductRating : ValueObject
    {
        public string ProductId { get; set; }
        public decimal? Rating { get; set; }
    }
}
