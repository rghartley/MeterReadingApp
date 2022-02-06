using MeterReadingApi.Common.Result;
using MeterReadingApi.Models;

namespace MeterReadingApi.Repositories
{
    public interface IAccountsRepository
    {
        Result<Account> Get(int id);
    }
}
