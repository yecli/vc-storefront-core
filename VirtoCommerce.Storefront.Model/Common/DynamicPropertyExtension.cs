using System;
using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.Storefront.Model.Common
{
    public static class DynamicPropertiesExtensions
    {
        public static void SetDynamicPropertySingleValue(this IList<DynamicProperty> properties, string propertyName, string valueType, Language language, string value)
        {
            var property = properties.FirstOrDefault(v => v.Name.EqualsInvariant(propertyName) && v.Values != null);
            if(property == null)
            {
                property = new Model.DynamicProperty { Name = propertyName, ValueType = "ShortText" };
                properties.Add(property);
            }
            property.Values = new List<LocalizedString> { new LocalizedString(Language.InvariantLanguage, value) };          
        }

        public static string GetDynamicPropertyValue(this IEnumerable<DynamicProperty> properties, string propertyName, Language language = null)
        {
            string retVal = null;

            language = language ?? Language.InvariantLanguage;

            if (properties != null)
            {
                var property = properties.FirstOrDefault(v => string.Equals(v.Name, propertyName, StringComparison.OrdinalIgnoreCase) && v.Values != null);

                if (property != null)
                {
                    retVal = property.Values.Where(x => x.Language.Equals(language)).Select(x => x.Value).FirstOrDefault();
                }
            }

            return retVal;
        }

        public static DynamicPropertyDictionaryItem GetDynamicPropertyDictValue(this IEnumerable<DynamicProperty> properties, string propertyName)
        {
            var retVal = new DynamicPropertyDictionaryItem();

            if (properties != null)
            {
                var property = properties.FirstOrDefault(v => string.Equals(v.Name, propertyName, StringComparison.OrdinalIgnoreCase) && v.IsDictionary && v.Values != null);

                if (property != null)
                {
                    retVal = property.DictionaryValues.FirstOrDefault();
                }
            }

            return retVal;
        }

        public static string[] GetDynamicPropertyArrayValues(this IEnumerable<DynamicProperty> properties, string propertyName)
        {
            var result = new string[] { };

            if (properties != null)
            {
                var property = properties.FirstOrDefault(v => string.Equals(v.Name, propertyName, StringComparison.OrdinalIgnoreCase) && v.IsArray && v.Values != null);

                if (property != null)
                {
                    result = property.Values.Select(v => v.Value).ToArray();
                }
            }

            return result;
        }
    }
}
