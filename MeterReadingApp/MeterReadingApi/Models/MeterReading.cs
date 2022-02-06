namespace MeterReadingApi.Models
{
    public record MeterReading
    {
        public int AccountId { get; init; }

        public DateTimeOffset Date { get; init; }

        public int Reading { get; init; }
    }
}
