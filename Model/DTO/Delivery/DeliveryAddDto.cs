using System.ComponentModel.DataAnnotations;

namespace Model.DTO.Delivery;

public record DeliveryAddDto(
    [Required] string Name,
    [Required] string Description,
    [Required] string Category,
    [Required] LocationDto StartLocation,
    [Required] int StartLocationRegion,
    [Required] LocationDto EndLocation,
    [Required] int EndLocationRegion,
    [Required] List<string> Recipients,
    [Required] bool IsFragile
);