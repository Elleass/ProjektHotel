namespace Hotel.Application.DTOs;

public class RoomResponseDto
{
    public int Id { get; set; }
    public int RoomNumber { get; set; }
    public string Type { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; }
}