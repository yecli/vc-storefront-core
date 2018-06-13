using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.JsonPage;

namespace VirtoCommerce.Storefront.JsonConverters
{
    public class JsonPageJsonConverter : JsonConverter
    {
        private readonly IWorkContextAccessor _workContextAccessor;

        public JsonPageJsonConverter(IWorkContextAccessor workContextAccessor)
        {
            _workContextAccessor = workContextAccessor;
        }

        public JsonPageJsonConverter()
        {
        }


        public override bool CanWrite { get { return false; } }
        public override bool CanRead { get { return true; } }

        public override bool CanConvert(Type objectType)
        {
            return typeof(JsonPageDefinition).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var blocks = JArray.Load(reader);

            var retVal = new JsonPageDefinition();

            retVal.Blocks = ReadBlocks(retVal, blocks);

            return retVal;
        }

        private List<IDictionary<string, object>> ReadBlocks(JsonPageDefinition cmsPage, JToken jblocks)
        {
            var retVal = new List<IDictionary<string, object>>();
            var index = 0;

            foreach (var jblock in jblocks.Children())
            {
                var type = String.Empty;

                var dictBlock = jblock.ToObject<Dictionary<string, object>>().ToDictionary(x => x.Key, x => x.Value);

                if (dictBlock.ContainsKey("type"))
                {
                    type = jblock["type"].Value<string>();
                }

                if (type == "settings")
                {
                    var dict = jblock.ToObject<Dictionary<string, object>>().ToDictionary(x => x.Key, x => x.Value);

                    if (!dict.ContainsKey("id"))
                    {
                        dict.Add("id", Guid.NewGuid().ToString("N"));
                    }
                    cmsPage.Settings = new DefaultableDictionary(dict, null);
                }
                else
                {
                    var dict = jblock.ToObject<Dictionary<string, object>>().ToDictionary(x => x.Key, x => x.Value);

                    if (!dict.ContainsKey("id"))
                    {
                        dict.Add("id", Guid.NewGuid().ToString("N"));
                    }

                    if (dict.ContainsKey("blocks"))
                    {
                        var innerBlocks = dict["blocks"];

                        dict["blocks"] = ReadBlocks(cmsPage, (JToken)innerBlocks);
                    }

                    if (dict.ContainsKey("images"))
                    {
                        var inner = dict["images"];

                        dict["images"] = ReadBlocks(cmsPage, (JToken)inner);
                    }

                    if (dict.ContainsKey("columns"))
                    {
                        var inner = dict["columns"];

                        dict["columns"] = ReadBlocks(cmsPage, (JToken)inner);
                    }

                    retVal.Add(new Dictionary<string, object>(dict));
                }

                index++;
            }

            return retVal;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
