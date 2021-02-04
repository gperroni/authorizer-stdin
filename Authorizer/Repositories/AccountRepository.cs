using Authorizer.Models;
using Authorizer.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authorizer.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IMemoryCache MemoryCache;
        private const string CACHE_NAME = "account";
        public AccountRepository(IMemoryCache memoryCache)
        {
            MemoryCache = memoryCache;
        }

        public void Create(Account account)
        {
            SetCache(account);
        }

        public Account Get()
        {
            var savedAccount = MemoryCache.Get<Account>(CACHE_NAME);
            savedAccount?.CleanErrors();
            return savedAccount;
        }

        public void Update(Account account)
        {
            SetCache(account);
        }

        private void SetCache(Account account)
        {
            MemoryCache.Set(CACHE_NAME, account);
        }
    }
}
