using MeterReadingApi.Models;
using System.Collections.Concurrent;

namespace MeterReadingApi.Repositories
{
    public class MeterReadingsRepository : IMeterReadingsRepository
    {
        private readonly ConcurrentDictionary<int, ConcurrentBag<MeterReading>> meterReadings = new();

        public IEnumerable<MeterReading> GetForAccount(int accountId)
        {
            this.meterReadings.TryGetValue(accountId, out var accountMeterReadings);

            return accountMeterReadings ?? Enumerable.Empty<MeterReading>();
        }

        public void Add(MeterReading meterReading)
        {
            var accountMeterReadings = this.meterReadings.GetOrAdd(meterReading.AccountId, (k) => new ConcurrentBag<MeterReading>());
            accountMeterReadings.Add(meterReading);
        }
    }
}
