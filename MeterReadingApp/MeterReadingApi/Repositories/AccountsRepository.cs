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
            var filePath = Path.Combine(AppContext.BaseDirectory, "SampleData", "Test_Accounts.csv");
            var accounts = File.ReadAllLines(filePath);

            for (var index = 1; index < accounts.Length; index++)
            {
                var accountFields = accounts[index].Split(",");
                var accountId = accountFields[0];
                var firstName = accountFields[1];
                var lastName = accountFields[2];

                var account = new Account
                {
                    Id = Convert.ToInt32(accountId),
                    FirstName = firstName,
                    LastName = lastName
                };

                this.accounts.TryAdd(account.Id, account);
            }
        }
    }
}
