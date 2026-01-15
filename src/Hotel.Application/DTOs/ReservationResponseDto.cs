namespace Hotel.Application.DTOs;

public class ReservationResponseDto
{
    public int Id { get; set; }
    public int GuestId { get; set; }
    public string GuestName { get; set; } = string.Empty; 
    public int RoomId { get; set; }
    public int RoomNumber { get; set; } 
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime CreatedAt { get; set; }
}