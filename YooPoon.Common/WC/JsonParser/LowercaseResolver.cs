using Newtonsoft.Json.Serialization;

namespace YooPoon.Common.WC.JsonParser
{
    public class LowercaseResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return propertyName.ToLower();
        }
    }
}