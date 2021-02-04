using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authorizer.CrossCutting
{
    public static class JTokenExtensions
    {
        public static T GetTypedProperty<T>(this JToken jToken, string propertyName)
        {
            return (T)Convert.ChangeType(jToken[propertyName].ToString(), typeof(T));
        }
    }
}
