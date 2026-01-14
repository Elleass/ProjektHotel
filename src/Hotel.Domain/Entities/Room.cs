namespace Hotel.Domain.Entities;

public class Room : BaseEntity
{
    public int RoomNumber { get; private set; }
    public string Type { get; private set; } // np. "Standard", "Deluxe"
    public decimal Price { get; private set; }
    public bool IsAvailable { get; private set; }

    public Room(int roomNumber, string type, decimal price)
    {
        RoomNumber = roomNumber;
        Type = type;
        Price = price;
        IsAvailable = true;
    }

    public void ChangePrice(decimal newPrice)
    {
        if (newPrice <= 0) throw new ArgumentException("Price must be greater than zero");
        Price = newPrice;
    }
}