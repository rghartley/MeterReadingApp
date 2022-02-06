namespace MeterReadingApi.Services
{
    public interface IMeterReadingsService
    {
        (int successfulReadings, int failedReadings) ProcessMeterReadings(MemoryStream meterReadingsStream);
    }
}
