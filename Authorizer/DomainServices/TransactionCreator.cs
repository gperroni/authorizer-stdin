using Authorizer.Models;
using Authorizer.Properties;
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
                return new Account(false, 0, Resources.ACCOUNT_NOT_INITIALIZED);

            account.AddTransaction(transaction);
            AccountRepository.Update(account);
            return account;
        }
    }
}
