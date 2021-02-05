using Authorizer.Models;
using Authorizer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

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
            account.FailIf(() => savedAccount != null, "account-already-initialized");

            if (account.Valid)
                AccountRepository.Create(account);

            return account;
        }
    }
}
