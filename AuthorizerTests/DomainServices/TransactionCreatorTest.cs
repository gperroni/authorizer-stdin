using Authorizer.DomainServices;
using Authorizer.Models;
using Authorizer.Properties;
using Authorizer.Repositories.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;

namespace AuthorizerTests.DomainServices
{
    [TestClass]
    public class TransactionCreatorTest
    {
        private Mock<IAccountRepository> AccountRepository { get; set; }
        private TransactionCreator TransactionCreator { get; set; }
        [TestInitialize]
        public void Init()
        {
            AccountRepository = new Mock<IAccountRepository>();
            TransactionCreator = new TransactionCreator(AccountRepository.Object);
        }

        [TestMethod]
        public void ShouldCreateTransactionCorrectly()
        {
            //Arrenge
            var account = new Account(true, 100);
            var transaction = new Transaction(20, "Mechant 1", DateTime.Now);
            AccountRepository.Setup(q => q.Get()).Returns(account);

            //Action
            var savedAccount = TransactionCreator.Execute(transaction);

            //Assert
            Assert.AreEqual(savedAccount.GetErrors().Count(), 0);
            Assert.AreEqual(savedAccount.Amount, 80);
            Assert.AreEqual(savedAccount.Transactions.Count(), 1);
            AccountRepository.Verify(q => q.Update(account), Times.Once);
        }

        [TestMethod]
        public void ShouldCreateTransactionsCorrectlyIfHasAmount()
        {
            //Arrenge
            var account = new Account(true, 100);
            account.AddTransaction(new Transaction(50, "Mechant 1", DateTime.Now));
            var transaction = new Transaction(20, "Mechant 2", DateTime.Now);
            AccountRepository.Setup(q => q.Get()).Returns(account);

            //Action
            var savedAccount = TransactionCreator.Execute(transaction);

            //Assert
            Assert.AreEqual(savedAccount.GetErrors().Count(), 0);
            Assert.AreEqual(savedAccount.Amount, 30);
            Assert.AreEqual(savedAccount.Transactions.Count(), 2);
            AccountRepository.Verify(q => q.Update(account), Times.Once);
        }

        [TestMethod]
        public void ShouldNotCreateTransactionsBecauseOfAmount()
        {
            //Arrenge
            var account = new Account(true, 100);
            account.AddTransaction(new Transaction(90, "Mechant 1", DateTime.Now));
            var transaction = new Transaction(20, "Mechant 2", DateTime.Now);
            AccountRepository.Setup(q => q.Get()).Returns(account);

            //Action
            var savedAccount = TransactionCreator.Execute(transaction);

            //Assert
            var errors = savedAccount.GetErrors();
            Assert.AreEqual(errors.Count(), 1);
            Assert.AreEqual(errors.First(), Resources.INSUFFICIENT_LIMIT);
            Assert.AreEqual(savedAccount.Amount, 10);
            Assert.AreEqual(savedAccount.Transactions.Count(), 1);
            AccountRepository.Verify(q => q.Update(account), Times.AtMostOnce);
        }

        [TestMethod]
        public void ShouldNotCreateTransactionsIfCardNotActive()
        {
            //Arrenge
            var account = new Account(false, 100);
            var transaction = new Transaction(150, "Mechant 1", DateTime.Now);
            AccountRepository.Setup(q => q.Get()).Returns(account);

            //Action
            var savedAccount = TransactionCreator.Execute(transaction);

            //Assert
            var errors = savedAccount.GetErrors();
            Assert.AreEqual(errors.Count(), 2);
            Assert.IsTrue(errors.Contains(Resources.INSUFFICIENT_LIMIT));
            Assert.IsTrue(errors.Contains(Resources.CARD_NOT_ACTIVE));
            Assert.AreEqual(savedAccount.Amount, 100);
            Assert.AreEqual(savedAccount.Transactions.Count(), 0);
            AccountRepository.Verify(q => q.Update(account), Times.AtMostOnce);
        }

        [TestMethod]
        public void ShouldNotCreateTransactionsIfAccountNotInitialized()
        {
            //Arrenge
            var account = new Account(false, 100);
            var transaction = new Transaction(50, "Mechant 1", DateTime.Now);

            //Action
            var savedAccount = TransactionCreator.Execute(transaction);

            //Assert
            var errors = savedAccount.GetErrors();
            Assert.AreEqual(errors.Count(), 1);
            Assert.IsTrue(errors.Contains("account-not-initialized"));
            AccountRepository.Verify(q => q.Update(account), Times.Never);

        }

        [TestMethod]
        public void ShouldCreateTransactionsIfDifferentTimes()
        {
            //Arrenge
            var account = new Account(true, 100);
            var dataCorrente = DateTime.Now;
            account.AddTransaction(new Transaction(20, "Mechant 1", dataCorrente.AddMinutes(-1)));
            account.AddTransaction(new Transaction(10, "Mechant 1", dataCorrente));
            account.AddTransaction(new Transaction(15, "Mechant 1", dataCorrente));
            var newTransaction = new Transaction(55, "Mechant 1", dataCorrente.AddMinutes(1));
            AccountRepository.Setup(q => q.Get()).Returns(account);

            //Action
            var savedAccount = TransactionCreator.Execute(newTransaction);

            //Assert
            Assert.AreEqual(savedAccount.Transactions.Count(), 4);
            Assert.AreEqual(savedAccount.Amount, 0);
            AccountRepository.Verify(q => q.Update(account), Times.Once);
        }

        [TestMethod]
        public void ShouldNotCreateTransactionsIfTimesInTwoMintue()
        {
            //Arrenge
            var account = new Account(true, 100);
            var dataCorrente = DateTime.Now;
            account.AddTransaction(new Transaction(20, "Mechant 1", dataCorrente.AddMinutes(-1)));
            account.AddTransaction(new Transaction(10, "Mechant 1", dataCorrente.AddMinutes(-1)));
            account.AddTransaction(new Transaction(15, "Mechant 1", dataCorrente));
            var newTransaction = new Transaction(55, "Mechant 1", dataCorrente);
            AccountRepository.Setup(q => q.Get()).Returns(account);

            //Action
            var savedAccount = TransactionCreator.Execute(newTransaction);

            //Assert
            var errors = savedAccount.GetErrors();
            Assert.AreEqual(errors.Count(), 1);
            Assert.IsTrue(errors.Contains("high-frequency-small-interval"));
            Assert.AreEqual(savedAccount.Amount, 55);
            AccountRepository.Verify(q => q.Update(account), Times.AtMost(3));
        }

        [TestMethod]
        public void ShouldNotCreateTransactionsIfEqualsInLastTwoMinutes()
        {
            //Arrenge
            var account = new Account(true, 100);
            var dataCorrente = DateTime.Now;
            account.AddTransaction(new Transaction(05, "Mechant 1", dataCorrente.AddMinutes(-1)));
            account.AddTransaction(new Transaction(10, "Mechant 1", dataCorrente.AddMinutes(-1)));
            account.AddTransaction(new Transaction(15, "Mechant 1", dataCorrente));
            var newTransaction = new Transaction(05, "Mechant 1", dataCorrente);
            AccountRepository.Setup(q => q.Get()).Returns(account);

            //Action
            var savedAccount = TransactionCreator.Execute(newTransaction);

            //Assert
            Assert.AreEqual(savedAccount.GetErrors().Count(), 2);
            Assert.IsTrue(savedAccount.GetErrors().Contains(Resources.HIGH_FREQUENCY_SMALL_INTERVAL));
            Assert.IsTrue(savedAccount.GetErrors().Contains(Resources.DOUBLED_TRANSACTION));
            Assert.AreEqual(savedAccount.Amount, 70);
            AccountRepository.Verify(q => q.Update(account), Times.AtMost(3));
        }
    }
}
