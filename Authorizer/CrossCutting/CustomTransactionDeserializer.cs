using Authorizer.CrossCutting.Extensions;
using Authorizer.Models;
using Newtonsoft.Json.Linq;

namespace Authorizer.CrossCutting
{
    public class CustomTransactionDeserializer : CustomDeserializer<Transaction>
    {
        public override Transaction BuildObject(JObject jObject)
        {
            return new Transaction(
                jObject["transaction"].GetTypedProperty<int>("amount"),
                jObject["transaction"].GetTypedProperty<string>("merchant"),
                jObject["transaction"].ToDateTimeWithMiliseconds("time"));
        }
    }
}