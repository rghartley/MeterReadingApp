using FakeItEasy;
using MeterReadingApi.Common.Result;
using MeterReadingApi.Models;
using MeterReadingApi.Repositories;
using MeterReadingApi.Services;
using System;
using System.IO;
using System.Text;
using Xunit;

namespace MeterReadingApi.Tests
{
    public class When_A_Meter_Readings_File_Is_Processed
    {
        private readonly int successfulReadings;
        private readonly int failedReadings;
        private readonly IMeterReadingsRepository meterReadingsRepository;

        public When_A_Meter_Readings_File_Is_Processed()
        {
            var accountsRepository = A.Fake<IAccountsRepository>();
            A.CallTo(() => accountsRepository.Get(1234)).Returns(new NotFoundResult<Account>());

            this.meterReadingsRepository = A.Fake<IMeterReadingsRepository>();
            A.CallTo(() => this.meterReadingsRepository.GetForAccount(4321)).Returns(new[] { new MeterReading { Date = DateTime.MaxValue } });
            
            var meterReadingsService = new MeterReadingsService(accountsRepository, meterReadingsRepository);

            using var meterReadingsStream = this.BuildMockedMeterReadings();
            (var successfulReadings, var failedReadings) = meterReadingsService.ProcessMeterReadings(meterReadingsStream);

            this.successfulReadings = successfulReadings;
            this.failedReadings = failedReadings;
        }

        private MemoryStream BuildMockedMeterReadings()
        {
            const string SuccessfulReading = "2344,22/04/2019 09:24,1002";
            const string InvalidReadingLengnth = "2344,23/04/2019 09:24,99999999";
            const string NonNumericReading = "2344,24/04/2019 09:24,xxxx";
            const string UnknownAccount = "1234,22/04/2019 09:24,1002";
            const string OlderReading = "4321,22/04/2019 09:24,1002";

            var meterReadingsCsv = new StringBuilder();
            meterReadingsCsv.AppendLine("AccountId,MeterReadingDateTime,MeterReadValue");
            meterReadingsCsv.AppendLine(SuccessfulReading);
            meterReadingsCsv.AppendLine(InvalidReadingLengnth);
            meterReadingsCsv.AppendLine(NonNumericReading);
            meterReadingsCsv.AppendLine(UnknownAccount);
            meterReadingsCsv.AppendLine(OlderReading);

            var meterReadingsBytes = Encoding.UTF8.GetBytes(meterReadingsCsv.ToString());
            
            return new MemoryStream(meterReadingsBytes);
        }

        [Fact]
        public void Then_The_Number_Of_Successful_Readings_Is_Returned()
        {
            Assert.Equal(1, this.successfulReadings);
        }

        [Fact]
        public void Then_The_Number_Of_Failed_Readings_Is_Returned()
        {
            Assert.Equal(4, this.failedReadings);
        }

        [Fact]
        public void Then_Successful_Readings_Are_Persisted()
        {
            A.CallTo(() => this.meterReadingsRepository.Add(A<MeterReading>.That.Matches(
                m => m.AccountId == 2344 &&
                m.Date == new DateTime(2019, 4, 22, 9, 24, 0) &&
                m.Reading == "1002")))
                .MustHaveHappenedOnceExactly();
        }
    }
}