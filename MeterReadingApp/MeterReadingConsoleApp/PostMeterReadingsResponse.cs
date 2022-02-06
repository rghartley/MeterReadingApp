namespace MeterReadingConsoleApp
{
    internal record PostMeterReadingsResponse
    {
        public int SuccessfulReadings { get; init; }

        public int FailedReadings { get; init; }
    }
}
