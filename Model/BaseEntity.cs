using System.ComponentModel.DataAnnotations;

namespace Model;

public abstract class BaseEntity
{
    [Key] public Guid Id { get; set; } = Guid.NewGuid();
}