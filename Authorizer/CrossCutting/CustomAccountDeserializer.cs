using Authorizer.CrossCutting.Extensions;
using Authorizer.Models;
using Newtonsoft.Json.Linq;

namespace Authorizer.CrossCutting
{
    public class CustomAccountDeserializer : CustomDeserializer<Account>
    {
        public override Account BuildObject(JObject jObject)
        {

            return new Account(
                jObject["account"].GetTypedProperty<bool>("active-card"),
                jObject["account"].GetTypedProperty<int>("available-limit"));
        }
    }
}
