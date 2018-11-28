using System;
using System.Collections.Generic;
using System.Text;
using DotLiquid;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    public class CustomerReview : Drop
    {
        public string AuthorNickname { get; set; }
        public string Content { get; set; }
        public bool? IsActive { get; set; }
        public string ProductId { get; set; }
        public int? Rating { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
