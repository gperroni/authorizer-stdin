using System.Text.RegularExpressions;

namespace Authorizer.CrossCutting
{
    public class RegexHelper
    {
        public static Regex ComandAccount = new Regex(@"{""account"":");

        public static Regex CommandTransaction = new Regex(@"{""transaction"":");
    }
}
