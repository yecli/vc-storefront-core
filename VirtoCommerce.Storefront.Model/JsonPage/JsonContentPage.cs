using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VirtoCommerce.Storefront.Model.StaticContent;
using VirtoCommerce.Storefront.Model.JsonPage;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.JsonPage
{
    public class JsonContentPage : ContentPage
    {
        public JsonPageDefinition JsonPage { get; set; }

        public JsonContentPage()
            : base()
        {
            JsonPage = new JsonPageDefinition();
        }
    }
}
