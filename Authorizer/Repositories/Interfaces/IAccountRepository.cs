using Authorizer.Models;

namespace Authorizer.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        void Create(Account account);
        Account Get();
        void Update(Account account);
    }
}
