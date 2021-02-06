using Authorizer.Models;
using Authorizer.Properties;
using Authorizer.Repositories.Interfaces;

namespace Authorizer.DomainServices
{
    public class AccountCreator
    {
        private IAccountRepository AccountRepository { get; set; }

        public AccountCreator(IAccountRepository accountRepository)
        {
            AccountRepository = accountRepository;
        }

        public Account Save(Account account)
        {
            var savedAccount = AccountRepository.Get();
            account.FailIf(() => savedAccount != null, Resources.ACCOUNT_ALREADY_INITIALIZED);

            if (account.Valid)
                AccountRepository.Create(account);

            return account;
        }
    }
}
