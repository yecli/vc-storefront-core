using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.StaticContent;
using System.Threading.Tasks;
using VirtoCommerce.LiquidThemeEngine;
using VirtoCommerce.Storefront.Model.JsonPage;
using VirtoCommerce.Storefront.JsonConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VirtoCommerce.Storefront.Controllers.Api
{
    public class BlockData
    {
        public string block { get; set; }
    }

    public class ApiJsonPageController : StorefrontControllerBase
    {
        readonly private ILiquidThemeEngine _liquidThemeEngine = null;

        public ApiJsonPageController(IWorkContextAccessor workContextAccessor, IStorefrontUrlBuilder urlBuilder, ILiquidThemeEngine themeEngine)
            : base(workContextAccessor, urlBuilder)
        {
            _liquidThemeEngine = themeEngine ?? throw new ArgumentNullException(nameof(themeEngine));
        }

        // GET: storefrontapi/jsonPage/getBlockHtml
        [HttpPost]
        public async Task<ActionResult> GetBlockHtml([FromBody] BlockData data)
        {
            JsonPageDefinition jsonPage = JsonConvert.DeserializeObject<JsonPageDefinition>(data.block, new JsonPageJsonConverter());

            //var shopifyContext = WorkContext..ToShopifyModel(_urlBuilder);
            var parameters = new Dictionary<string, object>();
            parameters.Add("block", jsonPage.Blocks[0]);

            var type = String.Empty;

            if (jsonPage.Blocks[0].ContainsKey("type"))
            {
                type = jsonPage.Blocks[0]["type"].ToString();
            }

            var retVal = String.Empty;

            if (type != String.Empty)
            {
                retVal = _liquidThemeEngine.RenderTemplateByName(type, parameters);
            }

            return Json(new { html = retVal });
        }
    }
}
