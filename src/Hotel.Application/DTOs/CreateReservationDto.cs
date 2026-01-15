namespace Hotel.Application.DTOs;

public class CreateReservationDto
{
    public int GuestId { get; set; }
    public int RoomId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}