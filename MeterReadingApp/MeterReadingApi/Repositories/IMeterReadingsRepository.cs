using MeterReadingApi.Models;

namespace MeterReadingApi.Repositories
{
    public interface IMeterReadingsRepository
    {
        IEnumerable<MeterReading> GetForAccount(int accountId);

        void Add(MeterReading meterReading);
    }
}
