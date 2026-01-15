namespace Hotel.Domain.ValueObjects;

public record DateRange(DateTime StartDate, DateTime EndDate)
{
    public bool OverlapsWith(DateRange other)
    {
        return StartDate < other.EndDate && EndDate > other.StartDate;
    }

    public bool IsValid()
    {
        return EndDate > StartDate;
    }
}