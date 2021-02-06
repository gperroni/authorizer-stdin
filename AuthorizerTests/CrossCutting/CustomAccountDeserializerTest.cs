using Authorizer.CrossCutting;
using Authorizer.Models;
using AuthorizerTests.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace AuthorizerTests.CrossCutting
{
    [TestClass]
    public class CustomAccountDeserializerTest
    {
        [TestMethod]
        public void ShouldDeserializeAccountCorrectly()
        {
            //Arrenge
            var textFile = new CommandFileBuilder()
               .AddAccountCommand(true, 100)
               .Build();

            //Action
            var account = JsonConvert.DeserializeObject<Account>(textFile.ReadLine(), new CustomAccountDeserializer());

            //Assert
            Assert.AreEqual(account.Amount, 100);
            Assert.IsTrue(account.ActiveCard);
        }
    }
}
