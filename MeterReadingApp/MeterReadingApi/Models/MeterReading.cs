namespace MeterReadingApi.Models
{
    public record MeterReading
    {
        public int AccountId { get; init; }

        public DateTime Date { get; init; }

        public string Reading { get; init; } = String.Empty;
    }
}
