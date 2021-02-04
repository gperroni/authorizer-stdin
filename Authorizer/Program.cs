using Authorizer.CrossCutting;
using Authorizer.Services;
using System;
using System.IO;
using System.Linq;

namespace Authorizer
{
    class Program
    {
        static void Main(string[] args)
        {
            var textRead = new StringReader(
                @"{""account"": {""active-card"": true, ""available-limit"": 100}}" +
                Environment.NewLine +
                @"{""transaction"": {""merchant"": ""Burger King"", ""amount"": 20, ""time"":""2019-02-13T09:59:00.000Z""}}" +
                Environment.NewLine +
                @"{""transaction"": {""merchant"": ""Burger King"", ""amount"": 1, ""time"":""2019-02-13T09:59:00.000Z""}}" +
                Environment.NewLine +
                @"{""transaction"": {""merchant"": ""Burger King"", ""amount"": 100, ""time"":""2019-02-13T09:59:00.000Z""}}" +
                Environment.NewLine +
                @"{""transaction"": {""merchant"": ""Burger King"", ""amount"": 2, ""time"":""2019-02-13T09:59:00.000Z""}}" +
                Environment.NewLine +
                @"{""transaction"": {""merchant"": ""Burger King"", ""amount"": 1, ""time"":""2019-02-13T10:00:00.000Z""}}");


            DependencyContainer.Initialize();

            //var reader = Console.In;
            var reader = textRead;
            var output = new CommandExecuter(reader).Execute();

            output.ForEachAsync(q =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(q));
            });
        }
    }
}
