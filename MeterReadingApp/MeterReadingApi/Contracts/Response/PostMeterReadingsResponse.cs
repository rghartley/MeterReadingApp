namespace MeterReadingApi.Contracts.Response
{
    public record PostMeterReadingsResponse
    {
        public int SuccessfulReadings { get; init; }

        public int FailedReadings { get; init; }
    }
}
