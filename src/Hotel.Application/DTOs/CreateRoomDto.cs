namespace Hotel.Application.DTOs;

public class CreateRoomDto
{
    public int RoomNumber { get; set; }
    public string Type { get; set; } = string.Empty;
    public decimal Price { get; set; }
}