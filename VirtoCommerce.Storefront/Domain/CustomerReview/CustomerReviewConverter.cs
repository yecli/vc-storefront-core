using VirtoCommerce.Storefront.Model.CustomerReviews;

namespace VirtoCommerce.Storefront.Domain.CustomerReview
{
    public static class CustomerReviewConverter
    {
        public static Model.CustomerReviews.CustomerReview ToCustomerReview(this AutoRestClients.TestCustomModule.WebModuleApi.Models.CustomerReview dto)
        {
            var result = new Model.CustomerReviews.CustomerReview()
            {
                AuthorNickname = dto.AuthorNickname,
                Content = dto.Content,
                CreatedBy = dto.CreatedBy,
                CreatedDate = dto.CreatedDate,
                Id = dto.Id,
                IsActive = dto.IsActive,
                ModifiedBy = dto.ModifiedBy,
                ModifiedDate = dto.ModifiedDate,
                ProductId = dto.ProductId,
                Rating = dto.Rating                 
            };
            return result;
        }

        public static AutoRestClients.TestCustomModule.WebModuleApi.Models.CustomerReviewSearchCriteria ToSearchCriteriaDto(this CustomerReviewSearchCriteria criteria)
        {
            var result = new AutoRestClients.TestCustomModule.WebModuleApi.Models.CustomerReviewSearchCriteria()
            {
                IsActive = criteria.IsActive,
                ProductIds = criteria.ProductIds,
                HasRating = criteria.HasRating,

                Skip = criteria.Start,
                Take = criteria.PageSize,
                Sort = criteria.Sort,
            };
            return result;
        }
    }
}
