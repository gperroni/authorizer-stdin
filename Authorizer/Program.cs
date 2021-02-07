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
            DependencyContainer.Initialize();

            var reader = Console.In;
            var output = new CommandExecuter(reader).Execute();

            output.ForEachAsync(q =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(q));
            });
        }
    }
}
