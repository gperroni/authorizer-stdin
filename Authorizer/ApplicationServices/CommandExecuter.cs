using Authorizer.CrossCutting;
using Authorizer.DomainServices;
using Authorizer.Models;
using Authorizer.Repositories.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Authorizer.ApplicationServices
{
    public class CommandExecuter
    {
        private TextReader File { get; set; }

        public CommandExecuter(TextReader file)
        {
            File = file;
        }

        public async IAsyncEnumerable<Account> Execute()
        {
            Func<Account> command;
            while ((command = await GetCommand()) != null)
            {
                yield return command();
            }
        }

        private async Task<Func<Account>> GetCommand()
        {
            try
            {
                var fileLine = await File.ReadLineAsync();
                var accountRepository = DependencyContainer.GetService<IAccountRepository>();

                if (RegexHelper.ComandAccount.IsMatch(fileLine))
                {
                    var account = JsonConvert.DeserializeObject<Account>(fileLine, new CustomAccountDeserializer());
                    return () => new AccountCreator(accountRepository).Save(account);
                }

                if (RegexHelper.CommandTransaction.IsMatch(fileLine))
                {
                    var transaction = JsonConvert.DeserializeObject<Transaction>(fileLine, new CustomTransactionDeserializer());
                    return () => new TransactionCreator(accountRepository).Execute(transaction);

                }

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
