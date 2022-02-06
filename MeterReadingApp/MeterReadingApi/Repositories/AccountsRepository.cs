using MeterReadingApi.Common.Result;
using MeterReadingApi.Models;
using System.Collections.Concurrent;

namespace MeterReadingApi.Repositories
{
    public class AccountsRepository : IAccountsRepository
    {
        private readonly ConcurrentDictionary<int, Account> accounts = new();

        public AccountsRepository()
        {
            this.SeedDatabase();
        }

        public Result<Account> Get(int id)
        {
            this.accounts.TryGetValue(id, out var account);

            return account is null
                ? new NotFoundResult<Account>()
                : new SuccessfulResult<Account>(account);
        }

        private void SeedDatabase()
        {
            // The link to the test accounts spreadsheet was missing in the e-mail.
            // Therefore I'm creating a range of accounts that will include all the accounts in the meter reading spreadsheet.
            // I've excluded account 8766 from the seeding, so unknown accounts can be tested for.
            for (var index = 1234; index < 8766; index++)
            {
                var account = new Account
                {
                    Id = index
                };

                this.accounts.TryAdd(index, account);
            }
        }
    }
}
