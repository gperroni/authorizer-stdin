using Authorizer.Models;
using Authorizer.Repositories.Interfaces;

namespace Authorizer.DomainServices
{
    public class TransactionCreator
    {
        private IAccountRepository AccountRepository { get; set; }
        public TransactionCreator(IAccountRepository accountRepository)
        {
            AccountRepository = accountRepository;
        }
        public Account Execute(Transaction transaction)
        {
            var account = AccountRepository.Get();

            if (account == null)
                return new Account(false, 0, "account-not-initialized");

            account.AddTransaction(transaction);
            AccountRepository.Update(account);
            return account;
        }
    }
}
