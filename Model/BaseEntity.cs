using System.ComponentModel.DataAnnotations;

namespace Model;

public class BaseEntity
{
    [Key] public Guid Id { get; set; } = Guid.NewGuid();
}