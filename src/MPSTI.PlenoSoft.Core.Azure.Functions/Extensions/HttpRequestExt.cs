using Microsoft.AspNetCore.Http;
using MPSTI.PlenoSoft.Core.Extensions.Providers;
using System;
using System.Linq;

namespace MPSTI.PlenoSoft.Core.Azure.Functions.Extensions
{
    public static class HttpRequestExt
    {
        public static TValue GetFromQueryString<TValue>(this HttpRequest request) where TValue : new()
        {
            var properties = typeof(TValue).GetProperties();
            var result = new TValue();
            foreach (var parameter in request.Query)
            {
                var property = properties.FirstOrDefault(p => p.Name == parameter.Key);
                if (property != null && property.CanWrite)
                {
                    var stringValue = parameter.Value.FirstOrDefault();

                    var value = property.PropertyType.IsEnum
                        ? Enum.Parse(property.PropertyType, stringValue)
                        : Convert.ChangeType(stringValue, property.PropertyType, FormatProviders.enUS);

                    property.SetValue(result, value);
                }
            }
            return result;
        }
    }
}