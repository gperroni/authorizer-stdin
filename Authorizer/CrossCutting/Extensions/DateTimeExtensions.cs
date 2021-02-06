using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authorizer.CrossCutting.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToStringWithMilliseconds(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fff");
        }

    }
}
