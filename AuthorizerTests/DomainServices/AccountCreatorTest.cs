using Authorizer.DomainServices;
using Authorizer.Models;
using Authorizer.Repositories.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;

namespace AuthorizerTests.DomainServices
{
    [TestClass]
    public class AccountCreatorTest
    {
        private Mock<IAccountRepository> AccountRepository { get; set; }
        private AccountCreator AccountCreator { get; set; }


        [TestInitialize]
        public void Init()
        {
            AccountRepository = new Mock<IAccountRepository>();
            AccountCreator = new AccountCreator(AccountRepository.Object);
        }

        [TestMethod]
        public void ShoulCreateAccountCorrectly()
        {
            // Arrenge
            var account = new Account(true, 100);
            AccountRepository.Setup(q => q.Get());
            AccountRepository.Setup(q => q.Create(account));

            // Action
            var savedAccount = AccountCreator.Save(account);

            // Assert
            Assert.AreEqual(savedAccount, account);
            AccountRepository.Verify(q => q.Create(account), Times.Once);
        }

        [TestMethod]
        public void ShouldNotCreateAccountBecauseAccountAlreadyCreated()
        {
            // Arrenge
            var account = new Account(true, 100);
            AccountRepository.Setup(q => q.Get()).Returns(new Account());
            AccountRepository.Setup(q => q.Create(account));

            // Action
            var savedAccount = AccountCreator.Save(account);

            // Assert
            Assert.AreEqual(savedAccount.GetErrors().Count(), 1);
            Assert.AreEqual(savedAccount.GetErrors().First(), "account-already-initialized");
            AccountRepository.Verify(q => q.Create(account), Times.Never);
        }

    }
}
