using Hotel.Domain.Common;
namespace Hotel.Domain.Entities;

public class Room : ISoftDelete
{
    public int Id { get; private set; }
    public int RoomNumber { get; private set; }
    public string Type { get; private set; }
    public decimal Price { get; private set; }
    public bool IsAvailable { get; private set; }
    public int? HotelId { get; private set; }
    
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    public Room(int roomNumber, string type, decimal price)
    {
        RoomNumber = roomNumber;
        Type = type;
        Price = price;
        IsAvailable = true;
        IsDeleted = false;
    }
    public void ChangePrice(decimal newPrice)
    {
        if (newPrice <= 0) throw new ArgumentException("Price must be greater than zero");
        Price = newPrice;
    }
}