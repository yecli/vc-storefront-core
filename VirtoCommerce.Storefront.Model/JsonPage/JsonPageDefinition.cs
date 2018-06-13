using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.JsonPage
{
    public class JsonPageDefinition
    {
        public DefaultableDictionary Settings { get; set; }
        public List<IDictionary<string, object>> Blocks { set; get; }

        public JsonPageDefinition()
        {
            Settings = new DefaultableDictionary(null);
            Blocks = new List<IDictionary<string, object>>();
        }
    }
}
