namespace Hotel.Domain.Entities;

public class Hotel : BaseEntity
{
    public string Name { get; private set; }
    public string Address { get; private set; }
    public ICollection<Room> Rooms { get; private set; } = new List<Room>();

    public Hotel(string name, string address)
    {
        Name = name;
        Address = address;
    }
}