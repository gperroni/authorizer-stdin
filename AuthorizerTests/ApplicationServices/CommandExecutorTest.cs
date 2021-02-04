using Authorizer.ApplicationServices;
using Authorizer.CrossCutting;
using AuthorizerTests.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizerTests.ApplicationServices
{
    [TestClass]
    public class CommandExecutorTest
    {
        [TestInitialize]
        public void Init()
        {
            DependencyContainer.Initialize();
        }

        [TestMethod]
        public void ShouldCreateAccountCommantCorrectly()
        {
            //Arrenge
            var textFile = new CommandFileBuilder()
                .AddAccountCommand(true, 10)
                .Build();

            //Action
            var accountReturns = new CommandExecuter(textFile).Execute();

            //Assert
            Assert.AreEqual(accountReturns.CountAsync().Result, 1);
        }

        [TestMethod]
        public async Task ShouldCreateTransferCommantCorrectly()
        {
            //Arrenge
            var textFile = new CommandFileBuilder()
                .AddTransactionCommand("", 0, DateTime.Now)
                .Build();

            //Action
            var accountReturns = await new CommandExecuter(textFile).Execute().ToListAsync();

            //Assert

            Assert.AreEqual(accountReturns.Count(), 1);
            Assert.AreEqual(accountReturns.First().ActiveCard, false);
        }

        [TestMethod]
        public void ShouldNotCreateAnyCommand()
        {
            //Arrenge
            var textFile = new CommandFileBuilder()
                .AddCustomLine("test")
                .Build();

            //Action
            var accountReturns = new CommandExecuter(textFile).Execute();

            //Assert
            Assert.AreEqual(accountReturns.CountAsync().Result, 0);
        }

        [TestMethod]
        public void ShouldReturnTwoLinesOfCommands()
        {
            //Arrenge
            var textFile = new CommandFileBuilder()
                .AddAccountCommand(false, 0)
                .AddTransactionCommand("", 0, DateTime.Now)
                .Build();

            //Action
            var accountReturns = new CommandExecuter(textFile).Execute();

            //Assert

            Assert.AreEqual(accountReturns.CountAsync().Result, 2);
        }

    }
}
