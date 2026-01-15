using System.ComponentModel.DataAnnotations;

namespace Hotel.Domain.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; }

    [ConcurrencyCheck]
    public string Version { get; set; } = Guid.NewGuid().ToString();
}