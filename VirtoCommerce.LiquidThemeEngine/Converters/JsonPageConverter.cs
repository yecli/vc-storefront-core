using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.LiquidThemeEngine.Objects;
using VirtoCommerce.Storefront.Model.Common;
using StorefrontModel = VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class JsonPageConverter
    {
        public static JsonPage ToShopifyModel(this StorefrontModel.JsonPage.JsonPageDefinition jsonPage)
        {
            var converter = new ShopifyModelConverter();
            return converter.ToLiquidCmsPage(jsonPage);
        }
    }

    public partial class ShopifyModelConverter
    {
        public virtual JsonPage ToLiquidCmsPage(StorefrontModel.JsonPage.JsonPageDefinition jsonPage)
        {
            var result = new JsonPage()
            {
                Settings = jsonPage.Settings,
                Blocks = new List<IDictionary<string, object>>()
            };

            foreach(IDictionary<string, object> block in jsonPage.Blocks)
            {
                MetafieldsCollection collection = new MetafieldsCollection(string.Empty, block);
                result.Blocks.Add(collection);
            }

            return result;
        }
    }
}
