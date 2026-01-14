namespace Hotel.Domain.Entities;

public class Reservation : BaseEntity
{
    public int GuestId { get; private set; }
    public virtual Guest Guest { get; private set; } = null!;
    
    public int RoomId { get; private set; }
    public virtual Room Room { get; private set; } = null!;
    
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Reservation(int guestId, int roomId, DateTime startDate, DateTime endDate)
    {
        if (endDate <= startDate) throw new ArgumentException("End date must be after start date");

        GuestId = guestId;
        RoomId = roomId;
        StartDate = startDate;
        EndDate = endDate;
        CreatedAt = DateTime.UtcNow;
    }
}