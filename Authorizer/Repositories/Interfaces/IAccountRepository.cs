using Authorizer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authorizer.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        void Create(Account account);
        Account Get();
        void Update(Account account);
    }
}
