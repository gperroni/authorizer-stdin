using Newtonsoft.Json.Linq;
using System;

namespace Authorizer.CrossCutting.Extensions
{
    public static class JTokenExtensions
    {
        public static T GetTypedProperty<T>(this JToken jToken, string propertyName)
        {
            return (T)Convert.ChangeType(jToken[propertyName].ToString(), typeof(T));
        }

        public static DateTime ToDateTimeWithMiliseconds(this JToken jToken, string propertyName)
        {
            return (DateTime)jToken[propertyName];
        }
    }
}
