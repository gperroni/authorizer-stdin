using Authorizer.ApplicationServices;
using Authorizer.CrossCutting;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace Authorizer
{
    class Program
    {
        static void Main(string[] args)
        {
            //var textRead = new StringReader(
            //    @"{""account"": {""active-card"": true, ""available-limit"": 100}}" +
            //    Environment.NewLine +
            //    @"{""transaction"": {""merchant"": ""Burger King"", ""amount"": 20, ""time"":""2019-02-13T09:58:15.010Z""}}" +
            //    Environment.NewLine +
            //    @"{""transaction"": {""merchant"": ""Burger King"", ""amount"": 30, ""time"":""2019-02-13T09:59:27.000Z""}}" +
            //    Environment.NewLine +
            //    @"{""transaction"": {""merchant"": ""Burger King"", ""amount"": 50, ""time"":""2019-02-13T09:59:31.000Z""}}" +
            //    Environment.NewLine +
            //    @"{""transaction"": {""merchant"": ""Burger King"", ""amount"": 20, ""time"":""2019-02-13T10:00:15.011Z""}}");
            

            DependencyContainer.Initialize();

            var reader = Console.In;
            //var reader = textRead;
            var output = new CommandExecuter(reader).Execute();

            output.ForEachAsync(q =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(q));
            });
        }
    }
}
