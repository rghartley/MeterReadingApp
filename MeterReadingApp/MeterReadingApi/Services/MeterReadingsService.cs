using MeterReadingApi.Common.Result;
using MeterReadingApi.Models;
using MeterReadingApi.Repositories;
using System.Text;

namespace MeterReadingApi.Services
{
    public class MeterReadingsService : IMeterReadingsService
    {
        private readonly IAccountsRepository accountsRepository;
        private readonly IMeterReadingsRepository meterReadingsRepository;

        public MeterReadingsService(IAccountsRepository accountsRepository, IMeterReadingsRepository meterReadingsRepository)
        {
            this.accountsRepository = accountsRepository;
            this.meterReadingsRepository = meterReadingsRepository;
        }

        public (int successfulReadings, int failedReadings) ProcessMeterReadings(MemoryStream meterReadingsStream)
        {
            var meterReadings = this.ParseMeterReadingsFile(meterReadingsStream);

            var successfulReadings = 0;
            var failedReadings = 0;

            foreach (var meterReading in meterReadings)
            {
                if (IsMeterReadingValid(meterReading))
                {
                    this.meterReadingsRepository.Add(meterReading);
                    successfulReadings++;
                }
                else
                {
                    failedReadings++;
                }
            }

            return (successfulReadings, failedReadings);
        }

        private IEnumerable<MeterReading> ParseMeterReadingsFile(MemoryStream meterReadingsStream)
        {
            var meterReadings = new List<MeterReading>();
            var meterReadingsFile = Encoding.UTF8.GetString(meterReadingsStream.ToArray());
            using var reader = new StringReader(meterReadingsFile);
            reader.ReadLine();

            while (true)
            {
                var line = reader.ReadLine();

                if (line is null)
                {
                    break;
                }

                var lineFields = line.Split(",");

                var meterReading = new MeterReading
                {
                    AccountId = Convert.ToInt32(lineFields[0]),
                    Date = DateTime.Parse(lineFields[1]),
                    Reading = lineFields[2]
                };

                meterReadings.Add(meterReading);
            }

            return meterReadings;
        }

        private bool IsMeterReadingValid(MeterReading meterReading)
        {
            if (string.IsNullOrWhiteSpace(meterReading.Reading) || meterReading.Reading.Length > 5)
            {
                return false;
            }

            if (!meterReading.Reading.All(char.IsDigit))
            {
                return false;
            }

            var account = this.accountsRepository.Get(meterReading.AccountId);

            if (account is NotFoundResult<Account>)
            {
                return false;
            }

            var existingMeterReadings = this.meterReadingsRepository.GetForAccount(meterReading.AccountId);

            return existingMeterReadings.All(e => e.Date < meterReading.Date);
        }
    }
}
