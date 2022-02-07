namespace MeterReadingApi.Models
{
    public record Account
    {
        public int Id { get; init; }

        public string FirstName { get; init; } = string.Empty;

        public string LastName { get; init; } = string.Empty;
    }
}
