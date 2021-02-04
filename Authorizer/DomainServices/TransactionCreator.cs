using Authorizer.Models;
using Authorizer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

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
            var account = AccountRepository.Get() ?? new Account(false, 0);
            account.FailIf(() => !account.IsInitialized, "account-not-initialized");

            // Aqui provavelmente depende do retorno do EMAIL
            if (!account.Valid)
                return account;

            account.AddTransaction(transaction);
            AccountRepository.Update(account);
            return account;
        }
    }
}
