using Authorizer.CrossCutting;
using Authorizer.CrossCutting.Extensions;
using Authorizer.Models;
using AuthorizerTests.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;

namespace AuthorizerTests.CrossCutting
{
    [TestClass]
    public class CustomTransactionDeserializerTest
    {
        [TestMethod]
        public void ShouldDeserializeTransactionCorrectly()
        {
            //Arrenge
            var merchant = "MERCHANT 1";
            var currentDate = DateTime.Now;
            var textFile = new CommandFileBuilder()
               .AddTransactionCommand(merchant, 100, currentDate)
               .Build();

            //Action
            var transaction = JsonConvert.DeserializeObject<Transaction>(textFile.ReadLine(), new CustomTransactionDeserializer());

            //Assert
            Assert.AreEqual(transaction.Amount, 100);
            Assert.AreEqual(transaction.Merchant, merchant);
            Assert.AreEqual(transaction.Time.ToStringWithMilliseconds(), currentDate.ToStringWithMilliseconds());
        }
    }
}